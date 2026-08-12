using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SpiceLogic.HtmlEditor.Abstractions.Entities;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.Extensions;
using SpiceLogic.HtmlEditor.WPF.Models.Services;
using Color = System.Drawing.Color;
using ColorConverter = System.Drawing.ColorConverter;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class TablePropertiesDialog
/// </summary>
public partial class TablePropertiesDialog : ITableDialog
{
    /// <summary>
    /// The _element
    /// </summary>
    private TableElement _element;

    /// <summary>
    /// The _table cell dialog
    /// </summary>
    /// <summary>
    /// The _background picture URL
    /// </summary>
    private string _backgroundPictureUrl = string.Empty;

    private readonly List<string> _propertiesAffected =
    [
        "Rows",
        "Columns",
        "Width",
        "Height",
        "Caption",
        "BorderWidth",
        "CellPadding",
        "CellSpacing",
        "BorderColor",
        "BorderAttr",
        "BorderStyle",
        "BgColor",
        "BorderCollapse",
        "SummaryDescription",
        "ID",
        "Name",
        "CssClassName",
        "BackGround"
    ];

    /// <summary>
    /// The dialog service
    /// </summary>
    private readonly IDialogService _dialogService;


    /// <summary>
    /// Initializes a new instance of the <see cref="TablePropertiesDialog" /> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public TablePropertiesDialog(IDialogService dialogService)
    {
        _dialogService = dialogService;

        InitializeComponent();
    }

    /// <summary>
    /// Handles the Loaded event of the TablePropertiesDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void TablePropertiesDialog_Loaded(object sender, RoutedEventArgs e)
    {
        BtnCellProperties.Visibility = this.GetOrInitCellElement() != null ? Visibility.Visible : Visibility.Hidden;
        TxtId.Visibility = Visibility.Collapsed;
        TxtName.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    public TableElement Element
    {
        get
        {
            _element ??= new TableElement(null);

            _element.ResetValues(this._propertiesAffected);

            _element.Rows = (int)NumRows.Value;
            _element.Columns = (int)NumCols.Value;

            if (ChkCellPadding.IsChecked == true)
                _element.CellPadding = (int)NumCellPadding.Value;
            if (ChkCellSpacing.IsChecked == true)
                _element.CellSpacing = (int)NumCellSpacing.Value;

            if (ChkWidth.IsChecked == true && CmbWidthUnit.SelectedItem != null)
            {
                _element.Width = Convert.ToInt32(TxtWidth.Text.Trim());
                _element.WidthUnit = CmbWidthUnit.Text;
            }

            if (ChkHeight.IsChecked == true && CmbHeightUnit.SelectedItem != null)
            {
                _element.Height = Convert.ToInt32(TxtHeight.Text.Trim());
                _element.HeightUnit = CmbHeightUnit.Text;
            }

            if (ChkCaption.IsChecked == true)
                _element.Caption = TxtCaption.Text.Trim();

            if (ChkBorderWidth.IsChecked == true)
            {
                if (ChkBorderStyle.IsChecked == true && CmbBorderStyle.SelectedItem != null)
                {
                    _element.BorderStyle = CmbBorderStyle.Text;
                    _element.BorderWidth = (int)NumBorderWidth.Value;
                }
                else
                    _element.BorderAttr = (int)NumBorderWidth.Value;
            }

            if (LnkButtonBorderColor.IsEnabled && ChkBorderColor.IsChecked == true)
            {
                if (TxtBorderColor.Background is SolidColorBrush brush)
                {
                    _element.BorderColor = WpfColorTranslator.ToHtml(brush.Color);
                }
            }

            if (ChkBgColor.IsChecked == true)
            {
                if (TxtBgColor.Background is SolidColorBrush brush)
                {
                    _element.BgColor = WpfColorTranslator.ToHtml(brush.Color);
                }
            }

            _element.SummaryDescription = TxtSummaryDescription.Text;
            _element.BorderCollapse = ChkBorderCollapse.IsEnabled && ChkBorderCollapse.IsChecked == true;
            _element.Id = TxtId.Text.Trim();
            _element.Name = TxtName.Text.Trim();
            _element.CssClassName = TxtClassName.Text.Trim();
            _element.CssStyle = TxtCss.Text.Trim();

            if (ChkBackgroundPicture.IsChecked == true)
                _element.BackGround = ImgBackgroundPicture.Source.ToString();

            _element.BorderToAll = ChkBorderToAll.IsChecked == true;

            return _element;
        }
        set
        {
            _element = value;
            if (_element == null)
                return;

            // every element→UI assignment is gated on the
            // element carrying data, so design-time defaults survive otherwise.
            if (_element.Rows > 0)
                NumRows.Value = _element.Rows;
            if (_element.Columns > 0)
                NumCols.Value = _element.Columns;

            if (_element.Width.HasValue)
            {
                ChkWidth.IsChecked = true;
                TxtWidth.Text = _element.Width.Value.ToString(CultureInfo.InvariantCulture);
                if (_element.WidthUnit != null)
                    CmbWidthUnit.SelectedItem = _element.WidthUnit;
            }

            if (_element.Height.HasValue)
            {
                ChkHeight.IsChecked = true;
                TxtHeight.Text = _element.Height.Value.ToString(CultureInfo.InvariantCulture);
                if (_element.HeightUnit != null)
                    CmbHeightUnit.SelectedItem = _element.HeightUnit;
            }

            if (!string.IsNullOrEmpty(_element.Caption))
            {
                ChkCaption.IsChecked = true;
                TxtCaption.Text = _element.Caption;
            }

            if (_element.BorderWidth.HasValue || _element.BorderAttr.HasValue)
                ChkBorderWidth.IsChecked = true;

            if (!string.IsNullOrEmpty(_element.BorderStyle))
            {
                ChkBorderStyle.IsChecked = true;
                CmbBorderStyle.IsEnabled = true;
                ChkBorderCollapse.IsEnabled = true;
                ChkBorderColor.IsEnabled = true;
                CmbBorderStyle.Text = _element.BorderStyle;
                if (ChkBorderWidth.IsChecked == true)
                    NumBorderWidth.Value = _element.BorderWidth ?? 0;
            }
            else if (ChkBorderWidth.IsChecked == true)
            {
                NumBorderWidth.Value = _element.BorderAttr ?? (_element.BorderWidth ?? 0);
            }

            if (_element.CellPadding.HasValue)
            {
                ChkCellPadding.IsChecked = true;
                NumCellPadding.Value = _element.CellPadding.Value;
            }

            if (_element.CellSpacing.HasValue)
            {
                ChkCellSpacing.IsChecked = true;
                NumCellSpacing.Value = _element.CellSpacing.Value;
            }

            if (!string.IsNullOrEmpty(_element.BorderColor))
            {
                ChkBorderColor.IsChecked = true;
                ColorConverter converter = new ColorConverter();
                Color drawingColor = (Color)converter.ConvertFromString(_element.BorderColor);
                System.Windows.Media.Color borderColor = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
                TxtBorderColor.Background = new SolidColorBrush(borderColor);
            }

            if (!string.IsNullOrEmpty(_element.BgColor))
            {
                ChkBgColor.IsChecked = true;
                LnkBtnBackgroundPicture.IsEnabled = true;
                ColorConverter converter = new ColorConverter();
                Color backgroundColor = (Color)converter.ConvertFromString(_element.BgColor);
                System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(
                    backgroundColor.A,
                    backgroundColor.R,
                    backgroundColor.G,
                    backgroundColor.B
                );
                TxtBgColor.Background = new SolidColorBrush(color);
            }

            if (_element.BorderCollapse.HasValue && _element.BorderCollapse.Value)
                ChkBorderCollapse.IsChecked = true;
            // preserve design-time defaults on empty fields.
            if (!string.IsNullOrEmpty(_element.SummaryDescription))
                TxtSummaryDescription.Text = _element.SummaryDescription;
            if (!string.IsNullOrEmpty(_element.Id))
                TxtId.Text = _element.Id;
            if (!string.IsNullOrEmpty(_element.Name))
                TxtName.Text = _element.Name;
            if (!string.IsNullOrEmpty(_element.CssClassName))
                TxtClassName.Text = _element.CssClassName;
            /////////////////
            if (!string.IsNullOrEmpty(_element.BackGround))
            {
                ChkBackgroundPicture.IsChecked = true;
                ImgBackgroundPicture.Source = new BitmapImage(new Uri(_element.BackGround));
            }

            string cssText = _element.GetCssStyleWithoutProperties(this._propertiesAffected);
            if (!string.IsNullOrEmpty(cssText))
                TxtCss.Text = cssText;
            ChkBorderToAll.IsChecked = true;
        }
    }

    #region ------------ UI Event Handlers --------------

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkWidth_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        TxtWidth.IsEnabled = ChkWidth.IsChecked == true;
        CmbWidthUnit.IsEnabled = ChkWidth.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkHeight control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkHeight_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        TxtHeight.IsEnabled = ChkHeight.IsChecked == true;
        CmbHeightUnit.IsEnabled = ChkHeight.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBorderWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBorderWidth_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        NumBorderWidth.IsEnabled = ChkBorderWidth.IsChecked == true;
        ChkBorderStyle.IsEnabled = ChkBorderWidth.IsChecked == true;

        //            chkBorderStyle.Checked = chkBorderWidth.Checked;
        CmbBorderStyle.IsEnabled = ChkBorderWidth.IsChecked == true && ChkBorderStyle.IsChecked == true;
        if (!ChkBorderWidth.IsChecked == true) ChkBorderToAll.IsChecked = true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBgColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBgColor_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        LnkButtonBgColor.IsEnabled = TxtBgColor.IsEnabled = ChkBgColor.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBorderColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBorderColor_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        LnkButtonBorderColor.IsEnabled = TxtBorderColor.IsEnabled = ChkBorderColor.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkCellPadding control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkCellPadding_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        NumCellPadding.IsEnabled = ChkCellPadding.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkCellSpacing control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkCellSpacing_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        NumCellSpacing.IsEnabled = ChkCellSpacing.IsChecked == true;
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnBackgroundPicture control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnBackgroundPicture_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        try
        {
            OpenFileDialog srcUrlDialog = new OpenFileDialog
            {
                Title = @"Please Select an image file.",
                RestoreDirectory = true,
                Filter = @"Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
                FilterIndex = 0,
                Multiselect = false
            };

            if (srcUrlDialog.ShowDialog() == true)
            {
                _backgroundPictureUrl = srcUrlDialog.FileName;
                ImgBackgroundPicture.Source = new BitmapImage(new Uri(_backgroundPictureUrl));
            }
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Opens a window and returns only when the newly opened window is closed
    /// </summary>
    /// <returns></returns>
    public new bool? ShowDialog()
    {
        return base.ShowDialog();
    }

    /// <summary>
    /// Handles the Click event of the btnCellProperties control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void btnCellProperties_Click(object sender, RoutedEventArgs e)
    {
        TableCellElement tblCellElement = this.GetOrInitCellElement();
        if (tblCellElement == null)
        {
            MessageBox.Show(
                "No cells were found.",
                "Error");
            return;
        }

        using ITableCellDialog tableCellDialog = _dialogService.TableCellDialog;
        tableCellDialog.Element = tblCellElement;
        using (tableCellDialog.LockOverrideSettingsToAllCells())
        {
            if (tableCellDialog.ShowDialog() == true)
                this.Element.CellElement = tableCellDialog.Element;
        }
    }

    private TableCellElement GetOrInitCellElement()
    {
        return this.Element.GetFirstCellElement();
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkCaption control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkCaption_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        TxtCaption.IsEnabled = ChkCaption.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBorderStyle control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBorderStyle_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        CmbBorderStyle.IsEnabled = ChkBorderStyle.IsChecked == true;
        ChkBorderCollapse.IsEnabled = ChkBorderStyle.IsChecked == true;
        ChkBorderColor.IsEnabled = ChkBorderStyle.IsChecked == true;
        if (!ChkBorderStyle.IsChecked == true) ChkBorderToAll.IsChecked = true;
    }

    /// <summary>
    /// Handles the ValueChanged event of the NumBorderWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedPropertyChangedEventArgs{T}" /> instance containing the event data.</param>
    private void numBorderWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (Math.Abs(NumBorderWidth.Value) < 0.01)
            ChkBorderToAll.IsChecked = true;
    }

    #endregion

    public void Dispose()
    {
    }

    /// <summary>
    /// Handles the Click event of the LnkButtonBorderColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkButtonBorderColor_OnClick(object sender, RoutedEventArgs e)
    {
        SolidColorBrush backgroundBrush = TxtBorderColor.Background as SolidColorBrush;
        System.Windows.Media.Color color = new System.Windows.Media.Color();
        if (backgroundBrush != null)
            color = backgroundBrush.Color;
        else
        {
            object convertFromString = System.Windows.Media.ColorConverter.ConvertFromString("Black");
            if (convertFromString != null)
                color = (System.Windows.Media.Color)convertFromString;
        }

        using IColorPickerDialog colorDialog = _dialogService.ColorPickerDialog;
        colorDialog.StartingColor = color;

        if (colorDialog.ShowDialog() == true)
            TxtBorderColor.Background = new SolidColorBrush(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the Click event of the LnkButtonBgColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkButtonBgColor_OnClick(object sender, RoutedEventArgs e)
    {
        SolidColorBrush backgroundBrush = TxtBgColor.Background as SolidColorBrush;
        System.Windows.Media.Color color = new System.Windows.Media.Color();
        if (backgroundBrush != null)
            color = backgroundBrush.Color;
        else
        {
            object convertFromString = System.Windows.Media.ColorConverter.ConvertFromString("Black");
            if (convertFromString != null)
                color = (System.Windows.Media.Color)convertFromString;
        }

        using IColorPickerDialog colorDialog = _dialogService.ColorPickerDialog;
        colorDialog.StartingColor = color;

        if (colorDialog.ShowDialog() == true)
            TxtBgColor.Background = new SolidColorBrush(colorDialog.SelectedColor);
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBackgroundPicture control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBackgroundPicture_OnCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        LnkBtnBackgroundPicture.IsEnabled = ChkBackgroundPicture.IsChecked == true;
    }

    /// <summary>
    /// Handles the MouseLeftButtonDown event of the DialogHeader control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    /// <summary>
    /// Handles the OnRightButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TwoButtonPanel_OnRightButtonClicked(object sender, RoutedEventArgs e)
    {       
        //cancel button

        Close();           
    }

    /// <summary>
    /// Handles the OnLeftButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void TwoButtonPanel_OnLeftButtonClicked(object sender, RoutedEventArgs e)
    {
        //ok button

        DialogResult = true;
    }
}