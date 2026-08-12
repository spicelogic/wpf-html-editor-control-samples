using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using SpiceLogic.HtmlEditor.Abstractions.Entities;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.Models.Services;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class ImageDialog
/// </summary>
public partial class ImageDialog : IImageDialog
{
    /// <summary>
    /// The _the original element
    /// </summary>
    private ImageElement _theOriginalElement;
    /// <summary>
    /// The _width to height aspect ratio
    /// </summary>
    private float? _widthToHeightAspectRatio;

    /// <summary>
    /// The dialog service
    /// </summary>
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageDialog" /> class.
    /// </summary>
    /// <param name="dialogService">The dialog service.</param>
    public ImageDialog(IDialogService dialogService)
    {
        _dialogService = dialogService;

        InitializeComponent();
    }


    public void Dispose()
    {
    }


    /// <summary>
    /// Handles the MouseLeftButtonDown event of the DialogHeader control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    /// <summary>
    /// Handles the Loaded event of the ImageInsertDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void ImageInsertDialog_Loaded(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_theOriginalElement.BaseUrl))
        {
            RdoWorkingDirFile.IsEnabled = false;
            RdoWorkingDirFile.ToolTip = "You need to set Base Url in order to use this option";
            RdoWorkingDirFile.SetValue(ToolTipService.ShowOnDisabledProperty, true);
        }
    }

    /// <summary>
    /// Handles the Click event of the BtnBrowseFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog srcUrlDialog = new OpenFileDialog
        {
            RestoreDirectory = true,
            Filter = @"Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
            FilterIndex = 0,
            Multiselect = false
        };

        if (srcUrlDialog.ShowDialog() == true)
        {
            TxtUrl.Text = srcUrlDialog.FileName;
            string imageFileName = srcUrlDialog.FileName;
            SetImageDimensionAndAspectRatio(imageFileName);
        }
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the RdoLocalFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void rdoLocalFile_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        UpdateBrowserButtonState();
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the RdoWorkingDirFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void rdoWorkingDirFile_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        bool? isLocalDir = this._theOriginalElement.IsBaseUrlALocalFolder();

        LnkBtnImportToBaseFolder.IsEnabled = RdoWorkingDirFile.IsEnabled && RdoWorkingDirFile.IsChecked == true && isLocalDir.HasValue && isLocalDir.Value;
        ChkOverwrite.IsEnabled = RdoWorkingDirFile.IsEnabled && RdoWorkingDirFile.IsChecked == true && isLocalDir.HasValue && isLocalDir.Value;
        LnkBtnBrowseWD.IsEnabled = RdoWorkingDirFile.IsEnabled && RdoWorkingDirFile.IsChecked == true && isLocalDir.HasValue && isLocalDir.Value;
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnBrowseWD control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    /// <exception cref="Exception">File doesn't exist</exception>
    private void lnkBtnBrowseWD_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        OpenFileDialog srcUrlDialog = new OpenFileDialog
        {
            RestoreDirectory = true,
            Filter = @"Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
            FilterIndex = 0,
            Multiselect = false,
            InitialDirectory = this._theOriginalElement.BaseUrl
        };

        if (srcUrlDialog.ShowDialog() == true)
        {
            string baseUrl = this._theOriginalElement.BaseUrl ?? string.Empty;
            if (baseUrl.EndsWith("\\") || baseUrl.EndsWith("/"))
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);

            string selectedFileDirectory = Path.GetDirectoryName(srcUrlDialog.FileName);
            if (selectedFileDirectory == null)
                return;

            if (selectedFileDirectory.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
            {
                string relativePath = srcUrlDialog.FileName.Replace(baseUrl, "");
                if (relativePath.StartsWith("\\"))
                    relativePath = relativePath.Remove(0, 1);

                TxtUrl.Text = relativePath;
            }
            else
            {
                MessageBoxResult dlgResult = MessageBox.Show(@"The image you selected is not from the base directory for relative path. Do you want to import that file to your base directory ? If you choose YES, then it will be imported to the Base Directory, otherwise it will be treated as absolute path image file.", "Selected image is not from the base directory.", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (dlgResult == MessageBoxResult.Yes)
                {
                    string newFilePath = Path.Combine(baseUrl, Path.GetFileName(srcUrlDialog.FileName));
                    int i = 0;
                    while (File.Exists(newFilePath) && !ChkOverwrite.IsChecked == true)
                    {
                        i++;
                        string newFileName = Path.GetFileNameWithoutExtension(srcUrlDialog.FileName) + i + Path.GetExtension(srcUrlDialog.FileName);
                        newFilePath = Path.Combine(baseUrl, newFileName);
                    }
                    if (File.Exists(newFilePath))
                    {
                        try
                        {
                            File.SetAttributes(newFilePath, FileAttributes.Normal);
                            File.Delete(newFilePath);
                            File.Copy(srcUrlDialog.FileName, newFilePath);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message, @"Error copying file to the destination");
                        }
                    }
                    else
                        File.Copy(srcUrlDialog.FileName, newFilePath);
                    TxtUrl.Text = Path.GetFileName(newFilePath);
                }
                else if (dlgResult == MessageBoxResult.No)
                    TxtUrl.Text = srcUrlDialog.FileName;
            }

            if (!string.IsNullOrEmpty(TxtUrl.Text))
            {
                string fullImagePath = File.Exists(TxtUrl.Text) ? TxtUrl.Text : Path.Combine(baseUrl, TxtUrl.Text);
                if (File.Exists(fullImagePath))
                    SetImageDimensionAndAspectRatio(fullImagePath);
                else
                    throw new Exception("File doesn't exist");
            }
        }
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkAlignment control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkAlignment_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        CmbAlign.IsEnabled = ChkAlignment.IsChecked == true;
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkBorderThickness control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkBorderThickness_CheckedChanged(object sender, RoutedEventArgs e)
    {
        TxtBorder.IsEnabled = ChkBorderThickness.IsChecked == true;
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
    }

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

        LnkBtnBgColor.IsEnabled = ChkBorderColor.IsChecked == true;
        TxtBgColor.IsEnabled = ChkBorderColor.IsChecked == true;
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
    }

    /// <summary>
    /// Handles the TextChanged event of the TxtHeight control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void txtHeight_TextChanged(object sender, RoutedEventArgs e)
    {
        if (TxtHeight.IsFocused && ChkLockAspectRatio.IsChecked == true && this._widthToHeightAspectRatio is > 0)
        {
            try
            {
                string value = TxtHeight.Text;
                GetValueAndUnit(value, out var digitPart, out var unitPart);

                if (digitPart.Length > 0)
                {
                    float height = float.Parse(digitPart);
                    if (height > 0)
                    {
                        float width = this._widthToHeightAspectRatio.Value * height;
                        TxtWidth.Text = (int)Math.Round(width) + unitPart;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// Handles the TextChanged event of the TxtWidth control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private void txtWidth_TextChanged(object sender, EventArgs e)
    {
        if (TxtWidth.IsFocused && ChkLockAspectRatio.IsChecked == true && this._widthToHeightAspectRatio is > 0)
        {
            try
            {
                string value = TxtWidth.Text;
                GetValueAndUnit(value, out var digitPart, out var unitPart);

                if (digitPart.Length > 0)
                {
                    float width = float.Parse(digitPart);
                    if (width > 0)
                    {
                        float height = width / this._widthToHeightAspectRatio.Value;
                        TxtHeight.Text = (int)Math.Round(height) + unitPart;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnImportToBaseFolder control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnImportToBaseFolder_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        OpenFileDialog myDialog = new OpenFileDialog
        {
            RestoreDirectory = true,
            Filter = @"Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
            FilterIndex = 0,
            Multiselect = false
        };

        if (myDialog.ShowDialog() == true)
        {
            string newFilePath = Path.Combine(this._theOriginalElement.BaseUrl, Path.GetFileName(myDialog.FileName));
            int i = 0;
            while (File.Exists(newFilePath) && !ChkOverwrite.IsChecked == true)
            {
                i++;
                string newFileName = Path.GetFileNameWithoutExtension(myDialog.FileName) + i + Path.GetExtension(myDialog.FileName);
                newFilePath = Path.Combine(this._theOriginalElement.BaseUrl, newFileName);
            }
            if (File.Exists(newFilePath))
            {
                try
                {
                    File.SetAttributes(newFilePath, FileAttributes.Normal);
                    File.Delete(newFilePath);
                    File.Copy(myDialog.FileName, newFilePath);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, @"Error copying file to the destination");
                }
            }
            else
                File.Copy(myDialog.FileName, newFilePath);
            TxtUrl.Text = Path.GetFileName(newFilePath);
        }
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    public ImageElement Element
    {
        get => ReadUi();
        set
        {
            this._theOriginalElement = value;
            this.UpdateUi(value);
        }
    }

    /// <summary>
    /// Reads the UI.
    /// </summary>
    /// <returns>ImageElement.</returns>
    private ImageElement ReadUi()
    {
        string src = null;

        if (ChkInsertLocalBase64.IsChecked == true)
        {
            try
            {
                if (File.Exists(TxtUrl.Text))
                {
                    src = ImageElement.GetBase64DataUrlForLocalImage(TxtUrl.Text);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
            
        ImageElement theElement = new ImageElement
        {
            TheActiveHtmlElement = this._theOriginalElement.TheActiveHtmlElement,
            CssStyle = this._theOriginalElement.CssStyle,
            CssClassName = this._theOriginalElement.CssClassName,
            Name = this._theOriginalElement.Name,
            Id = this._theOriginalElement.Id,
            OnClickJavascript = this._theOriginalElement.OnClickJavascript,
            SrcUrl = src ?? TxtUrl.Text
        };

        if (ChkWidth.IsChecked == true)
            theElement.Width = TxtWidth.Text.Trim();
        if (ChkHeight.IsChecked == true)
            theElement.Height = TxtHeight.Text.Trim();
        if (ChkBorderColor.IsChecked == true)
        {
            if (TxtBgColor.Background is SolidColorBrush bgBrush)
            {
                theElement.BorderColor = Color.FromArgb(
                    bgBrush.Color.A,
                    bgBrush.Color.R,
                    bgBrush.Color.G,
                    bgBrush.Color.B);
            }
        }

        if (ChkBorderStyle.IsChecked == true)
            theElement.BorderStyle = CmbBorderStyle.Text;
        if (ChkBorderThickness.IsChecked == true && !string.IsNullOrEmpty(TxtBorder.Text))
            theElement.Border = Convert.ToInt32(TxtBorder.Text);
        theElement.Title = TxtToolTip.Text.Trim();
        theElement.AlternativeText = TxtAlt.Text.Trim();
        if (ChkAlignment.IsChecked == true && !string.IsNullOrEmpty(CmbAlign.Text))
            theElement.Align = CmbAlign.Text;
        return theElement;
    }

    /// <summary>
    /// Updates the UI.
    /// </summary>
    /// <param name="element">The element.</param>
    private void UpdateUi(ImageElement element)
    {
        if (this.IsLocalResourceSelectionDisabled)
        {
            RdoLocalFile.IsEnabled = false;
            BtnBrowseFile.IsEnabled = false;
        }

        // preserve design-time defaults on empty fields.
        if (!string.IsNullOrEmpty(element.SrcUrl))
            TxtUrl.Text = element.SrcUrl;
        if (element.IsRelativePathOrUrl)
            RdoWorkingDirFile.IsChecked = true;
        else if (element.IsLocalFilePath && !this.IsLocalResourceSelectionDisabled)
            RdoLocalFile.IsChecked = true;
        if (!string.IsNullOrEmpty(element.Title))
            TxtToolTip.Text = element.Title;
        if (!string.IsNullOrEmpty(element.AlternativeText))
            TxtAlt.Text = element.AlternativeText;
        if (!string.IsNullOrEmpty(element.Align))
        {
            CmbAlign.Text = element.Align;
            ChkAlignment.IsChecked = true;
        }
        if (element.Border.HasValue)
        {
            TxtBorder.Text = element.Border.Value.ToString(CultureInfo.InvariantCulture);
            ChkBorderThickness.IsChecked = true;
        }
        if (!string.IsNullOrEmpty(element.Width))
        {
            TxtWidth.Text = element.Width;
            ChkWidth.IsChecked = true;
        }
        if (!string.IsNullOrEmpty(element.Height))
        {
            TxtHeight.Text = element.Height;
            ChkHeight.IsChecked = true;
        }

        if (element.BorderColor.HasValue)
        {
            ChkBorderColor.IsChecked = true;
            System.Windows.Media.Color bgColor = System.Windows.Media.Color.FromArgb(
                element.BorderColor.Value.A,
                element.BorderColor.Value.R,
                element.BorderColor.Value.G,
                element.BorderColor.Value.B);
            SolidColorBrush bgBrush = new SolidColorBrush(bgColor);
            TxtBgColor.Background = bgBrush;
        }
        // when no color, preserve design-time Background.

        if (!string.IsNullOrEmpty(element.BorderStyle))
        {
            ChkBorderStyle.IsChecked = true;
            CmbBorderStyle.Text = element.BorderStyle;
        }

        if (ChkHeight.IsChecked == true && ChkWidth.IsChecked == true)
        {
            try
            {
                GetValueAndUnit(TxtWidth.Text, out var widthDigitPart, out _);

                GetValueAndUnit(TxtHeight.Text, out var heightDigitPart, out _);
                if (widthDigitPart.Length > 0 && heightDigitPart.Length > 0)
                {
                    float width = float.Parse(widthDigitPart);
                    float height = float.Parse(heightDigitPart);
                    if (width > 0 && height > 0)
                        this._widthToHeightAspectRatio = width / height;
                    else
                        this._widthToHeightAspectRatio = null;
                }
            }
            catch
            {
                this._widthToHeightAspectRatio = null;
            }
        }
        else
            this._widthToHeightAspectRatio = null;
    }

    /// <summary>
    /// Sets the image dimension and aspect ratio.
    /// </summary>
    /// <param name="imageFileName">Name of the image file.</param>
    private void SetImageDimensionAndAspectRatio(string imageFileName)
    {
        Size? theImageDimension = ImageUtils.GetImageDimension(imageFileName);
        if (theImageDimension.HasValue)
        {
            ChkHeight.IsChecked = true;
            TxtHeight.Text = theImageDimension.Value.Height.ToString(CultureInfo.InvariantCulture);
            ChkWidth.IsChecked = true;
            TxtWidth.Text = theImageDimension.Value.Width.ToString(CultureInfo.InvariantCulture);
            if (theImageDimension.Value is { Width: > 0, Height: > 0 })
                this._widthToHeightAspectRatio = theImageDimension.Value.Width / (float)theImageDimension.Value.Height;
            else
                this._widthToHeightAspectRatio = null;
        }
        else
            this._widthToHeightAspectRatio = null;
    }

    /// <summary>
    /// Gets the value and unit.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="digitPart">The digit part.</param>
    /// <param name="unitPart">The unit part.</param>
    private static void GetValueAndUnit(string value, out string digitPart, out string unitPart)
    {
        const string digitRegEx = @"\d+";
        digitPart = Regex.Match(value, digitRegEx, RegexOptions.IgnoreCase | RegexOptions.Compiled).Groups[0].Value;
        unitPart = value.Replace(digitPart, "").Trim();
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is local resource selection disabled.
    /// </summary>
    /// <value><c>true</c> if this instance is local resource selection disabled; otherwise, <c>false</c>.</value>
    public bool IsLocalResourceSelectionDisabled { get; set; }

    /// <summary>
    /// When true, the Image dialog writes width/height as inline
    /// CSS in CssStyle ("style=width:240px;height:120px") instead of HTML
    /// width=/height= attributes. Passthrough auto-property so this custom
    /// dialog satisfies the interface contract.
    /// </summary>
    public bool UseInlineStyleForDimensions { get; set; }

    private void UpdateBrowserButtonState()
    {
        BtnBrowseFile.IsEnabled = RdoLocalFile.IsChecked == true;
    }

    /// <summary>
    /// Handles the OnRightButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void TwoButtonPanel_OnOnRightButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Handles the OnLeftButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void TwoButtonPanel_OnOnLeftButtonClicked(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TxtUrl.Text))
        {
            MessageBox.Show("Please provide Image Url");
            TxtUrl.Focus();
            return;
        }
            
        DialogResult = true;
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnBgColor control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="routedEventArgs">The event data</param>
    private void lnkBtnBgColor_OnClick(object sender, RoutedEventArgs routedEventArgs)
    {
        using IColorPickerDialog colorDialog = _dialogService.ColorPickerDialog;
        if (colorDialog.ShowDialog() == true)
        {
            TxtBgColor.Background = new SolidColorBrush(colorDialog.SelectedColor);
        }
    }

    /// <summary>
    /// Handles OnChecked event of the RdoInternetUrl RadioButton
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event argument.</param>
    private void RdoInternetUrl_OnChecked(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        ChkInsertLocalBase64.IsChecked = false;
        ChkInsertLocalBase64.IsEnabled = false;
    }

    /// <summary>
    /// Handles OnUnchecked event of the RdoInternet control.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event sender.</param>
    private void RdoInternet_OnUnchecked(object sender, RoutedEventArgs e)
    {
        ChkInsertLocalBase64.IsEnabled = true;
    }
}