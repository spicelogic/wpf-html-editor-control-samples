using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SpiceLogic.HtmlEditor.Abstractions.Entities;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.Extensions;
using Color = System.Drawing.Color;
using ColorConverter = System.Drawing.ColorConverter;
using SpiceLogic.HtmlEditor.WPF.Models.Services;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class TableCellPropertiesDialog
/// </summary>
public partial class TableCellPropertiesDialog : ITableCellDialog
{
    /// <summary>
    /// The _element
    /// </summary>
    private TableCellElement _element;

    /// <summary>
    /// The dialog service
    /// </summary>
    private readonly IDialogService _dialogService;

    private readonly List<string> _propertiesAffected =
    [
        "BgColor",
        "CssClassName",
        "Width",
        "WidthUnit",
        "Height",
        "HeightUnit",
        "HorizontalAlign",
        "VerticalAlign",
        "NoWrap"
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="TableCellPropertiesDialog" /> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public TableCellPropertiesDialog(IDialogService dialogService)
    {
        _dialogService = dialogService;

        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    public TableCellElement Element
    {
        get
        {
            _element.ResetStyles(this._propertiesAffected);

            //                _element.BgColor = chkBgColor.Checked ? ColorTranslator.ToHtml(txtBgColor.BackColor) : null;

                
            if (ChkBgColor.IsChecked == true)
            {
                SolidColorBrush backgroundColorBrush = (SolidColorBrush)TxtBgColor.Background;
                _element.BgColor = WpfColorTranslator.ToHtml(backgroundColorBrush.Color);
            }
            else
                _element.BgColor = null;


            _element.CssClassName = TxtClassName.Text.Trim();
            _element.CssStyle = TxtCss.Text.Trim();

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

            if (CmbHorizontalAlign.SelectedItem != null)
                _element.HorizontalAlign = CmbHorizontalAlign.Text;

            if (CmbVerticalAlign.SelectedItem != null)
                _element.VerticalAlign = CmbVerticalAlign.Text;
            _element.NoWrap = ChkNoWrap.IsChecked == true;
            _element.OverrideSettingsToAllCells = ChkOverrideSettings4Cells.IsChecked == true;
            return _element;
        }
        set
        {
            _element = value;

            if (_element == null)
                return;

            ChkBgColor.IsChecked = !string.IsNullOrEmpty(_element.BgColor);
            if (ChkBgColor.IsChecked == true)
            {
                ColorConverter converter = new ColorConverter();
                object convertFromString = converter.ConvertFromString(_element.BgColor);
                if (convertFromString != null)
                {
                    Color backgroundDrawingColor = (Color) convertFromString;
                    System.Windows.Media.Color backgroundColor = System.Windows.Media.Color.FromArgb(
                        backgroundDrawingColor.A,
                        backgroundDrawingColor.R,
                        backgroundDrawingColor.G,
                        backgroundDrawingColor.B);
                    TxtBgColor.Background = new SolidColorBrush(backgroundColor);
                }
            }

            // preserve design-time defaults on empty fields.
            if (!string.IsNullOrEmpty(_element.CssClassName))
                TxtClassName.Text = _element.CssClassName;
            if (_element.Width.HasValue)
            {
                ChkWidth.IsChecked = true;
                TxtWidth.Text = _element.Width.Value.ToString(CultureInfo.InvariantCulture);
                if (_element.WidthUnit != null)
                    CmbWidthUnit.Text = _element.WidthUnit;
            }

            if (_element.Height.HasValue)
            {
                ChkHeight.IsChecked = true;
                TxtHeight.Text = _element.Height.Value.ToString(CultureInfo.InvariantCulture);
                if (_element.HeightUnit != null)
                    CmbHeightUnit.Text = _element.HeightUnit;
            }

            if (_element.HorizontalAlign != null)
                CmbHorizontalAlign.Text = _element.HorizontalAlign;

            if (_element.VerticalAlign != null)
                CmbVerticalAlign.Text = _element.VerticalAlign;
            if (_element.NoWrap)
                ChkNoWrap.IsChecked = true;
            if (_element.OverrideSettingsToAllCells)
                ChkOverrideSettings4Cells.IsChecked = true;

            string cssText = _element.GetCssStyleWithoutProperties(this._propertiesAffected);
            if (!string.IsNullOrEmpty(cssText))
                TxtCss.Text = cssText;
        }
    }
    /// <summary>
    /// Gets the cell attribute collection.
    /// </summary>
    /// <value>The cell attribute collection.</value>
    public NameValueCollection CellAttributeCollection
    {
        get
        {
            NameValueCollection myColl = new NameValueCollection();

            if (ChkWidth.IsChecked == true)
                myColl.Add("width", string.Concat(TxtWidth.Text.Trim(), CmbWidthUnit.Text.Trim()));

            if (ChkHeight.IsChecked == true)
                myColl.Add("height", string.Concat(TxtHeight.Text.Trim(), CmbHeightUnit.Text.Trim()));

            if (ChkBgColor.IsChecked == true)
            {
                SolidColorBrush solidColorBrush = (SolidColorBrush)TxtBgColor.Background;
                myColl.Add("bgcolor", WpfColorTranslator.ToHtml(solidColorBrush.Color));
            }

            if (CmbHorizontalAlign.SelectedIndex != 0)
                myColl.Add("align", CmbHorizontalAlign.Text);

            if (CmbVerticalAlign.SelectedIndex != 0)
                myColl.Add("valign", CmbVerticalAlign.Text);

            if (ChkNoWrap.IsChecked == true)
                myColl.Add("nowrap", "nowrap");

            return myColl;
        }
    }

    /// <summary>
    /// Gets the table cell attribute string.
    /// </summary>
    /// <value>The table cell attribute string.</value>
    public string TableCellAttributeString
    {
        get
        {
            string[] tableCellAttributes = [];
            string[] tableCellStyleAttributes = [];

            if (ChkWidth.IsChecked == true)
            {
                Array.Resize(ref tableCellStyleAttributes, tableCellStyleAttributes.Length + 1);
                tableCellStyleAttributes[tableCellStyleAttributes.Length - 1] = $"width: {TxtWidth.Text.Trim()}{CmbWidthUnit.Text.Trim()}";
            }

            if (ChkHeight.IsChecked == true)
            {
                Array.Resize(ref tableCellStyleAttributes, tableCellStyleAttributes.Length + 1);
                tableCellStyleAttributes[tableCellStyleAttributes.Length - 1] = $"height: {TxtHeight.Text.Trim()}{CmbHeightUnit.Text.Trim()}";
            }

            if (ChkBgColor.IsChecked == true)
            {
                Array.Resize(ref tableCellStyleAttributes, tableCellStyleAttributes.Length + 1);
                SolidColorBrush solidBackgroundBrush = (SolidColorBrush) TxtBgColor.Background;
                tableCellStyleAttributes[tableCellStyleAttributes.Length - 1] =
                    $"background-color: {WpfColorTranslator.ToHtml(solidBackgroundBrush.Color)}";
            }

            if (tableCellStyleAttributes.Length != 0)
            {
                string tableCellStyleText = $"style = \"{string.Join("; ", tableCellStyleAttributes)}\"";
                Array.Resize(ref tableCellAttributes, tableCellAttributes.Length + 1);
                tableCellAttributes[tableCellAttributes.Length - 1] = tableCellStyleText;
            }

            if (CmbHorizontalAlign.SelectedIndex != 0)
            {
                Array.Resize(ref tableCellAttributes, tableCellAttributes.Length + 1);
                tableCellAttributes[tableCellAttributes.Length - 1] = $"align=\"{CmbHorizontalAlign.Text}\"";
            }

            if (CmbVerticalAlign.SelectedIndex != 0)
            {
                Array.Resize(ref tableCellAttributes, tableCellAttributes.Length + 1);
                tableCellAttributes[tableCellAttributes.Length - 1] = $"valign=\"{CmbVerticalAlign.Text}\"";
            }

            if (ChkNoWrap.IsChecked == true)
            {
                Array.Resize(ref tableCellAttributes, tableCellAttributes.Length + 1);
                tableCellAttributes[tableCellAttributes.Length - 1] = "nowrap=\"nowrap\"";
            }

            if (!string.IsNullOrEmpty(TxtClassName.Text.Trim()))
            {
                Array.Resize(ref tableCellAttributes, tableCellAttributes.Length + 1);
                tableCellAttributes[tableCellAttributes.Length - 1] = "class=\"" + TxtClassName.Text.Trim() + "\"";
            }

            return string.Join(" ", tableCellAttributes);
        }
    }

    /// <summary>
    /// Get a lock for a property [override cell attributes].
    /// </summary>
    public IDisposable LockOverrideSettingsToAllCells()
    {
        return new LockOverrideSettingsToAllCellsClass(this);
    }

    public void Dispose()
    {
    }


    /// <summary>
    /// Gets a value indicating whether [override cell attributes].
    /// </summary>
    /// <value><c>true</c> if [override cell attributes]; otherwise, <c>false</c>.</value>
    public bool OverrideCellAttributes => ChkOverrideSettings4Cells.IsChecked == true;

    #region --------------- UI Event Handlers ------------------

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

    #endregion

    private class LockOverrideSettingsToAllCellsClass : IDisposable
    {
        private readonly TableCellPropertiesDialog _dialog;
        private readonly bool _initiallyEnabled;

        public LockOverrideSettingsToAllCellsClass(TableCellPropertiesDialog dialog)
        {
            _dialog = dialog;
            _initiallyEnabled = _dialog.ChkOverrideSettings4Cells.IsEnabled;
            _dialog.ChkOverrideSettings4Cells.IsEnabled = false;
        }

        public void Dispose()
        {
            _dialog.ChkOverrideSettings4Cells.IsEnabled = _initiallyEnabled;
        }
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnBgColor control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnBgColor_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        ShowDialogForChoosingColor();
    }

    private void ShowDialogForChoosingColor()
    {
        SolidColorBrush bgColorBrush = TxtBgColor.Background as SolidColorBrush;
        System.Windows.Media.Color color = new System.Windows.Media.Color();

        if (bgColorBrush != null)
            color = bgColorBrush.Color;
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
    /// Handles the OnLeftButtonClicked event of the TwoButtonPanel control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void TwoButtonPanel_OnLeftButtonClicked(object sender, RoutedEventArgs e)
    {
        //ok button
            
        DialogResult = true;			
    }

    /// <summary>
    /// Handles the OnRightButtonClicked event of the TwoButtonPanel control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void TwoButtonPanel_OnRightButtonClicked(object sender, RoutedEventArgs e)
    {
        //cancel
            
        Close();           
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
    /// Handles the Checked and Unchecked events of the ChkBgColor control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void chkBgColor_OnCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        LnkBtnBgColor.IsEnabled = ChkBgColor.IsChecked == true;
    }
}