using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using SpiceLogic.HtmlEditor.Abstractions.Entities;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class HyperLinkDialog
/// </summary>
public partial class HyperLinkDialog : IHyperlinkDialog
{
    /// <summary>
    /// The _the original element
    /// </summary>
    private HyperlinkElement _theOriginalElement;

    /// <summary>
    /// Initializes a new instance of the <see cref="HyperLinkDialog" /> class.
    /// </summary>
    public HyperLinkDialog()
    {
        InitializeComponent();
    }


    /// <summary>
    /// Handles the MouseLeftButtonDown event of the DialogHeader control
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    /// <summary>
    /// Handles the Loaded event of the HyperLinkDialog control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void HyperLinkDialog_Loaded(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(this._theOriginalElement.BaseUrl))
        {
            RdoWorkingDirFile.IsEnabled = false;
            RdoWorkingDirFile.ToolTip = "You need to set Base Url in order to use this option";
            RdoWorkingDirFile.SetValue(ToolTipService.ShowOnDisabledProperty, true);
        }
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    public HyperlinkElement Element
    {
        get => readUi();
        set
        {
            this._theOriginalElement = value;
            this.updateUI(value);
        }
    }


    public void Dispose()
    {
    }


    /// <summary>
    /// Reads the UI.
    /// </summary>
    /// <returns>HyperlinkElement.</returns>
    private HyperlinkElement readUi()
    {
        HyperlinkElement theElement = new HyperlinkElement
        {
            TheActiveHtmlElement = this._theOriginalElement.TheActiveHtmlElement,
            CssStyle = this._theOriginalElement.CssStyle,
            CssClassName = this._theOriginalElement.CssClassName,
            Name = this._theOriginalElement.Name,
            Id = this._theOriginalElement.Id,
            OnClickJavascript = this._theOriginalElement.OnClickJavascript,
            HrefUrl = TxtURL.Text,
            Title = TxtToolTip.Text.Trim(),
            InnerHtml = TxtInnerHtml.Text.Trim(),
            Target = ChkTargetIncluded.IsChecked == true ? CmbTarget.Text : null
        };

        return theElement;
    }

    /// <summary>
    /// Updates the UI.
    /// </summary>
    /// <param name="element">The element.</param>
    private void updateUI(HyperlinkElement element)
    {
        if (this.IsLocalResourceSelectionDisabled)
        {
            RdoLocalFile.IsEnabled = false;
            BtnBrowseFile.IsEnabled = false;
        }

        // preserve design-time defaults on empty fields.
        if (!string.IsNullOrEmpty(element.HrefUrl))
            TxtURL.Text = element.HrefUrl;
        if (element.IsRelativePathOrUrl)
            RdoWorkingDirFile.IsChecked = true;
        else if (element.IsLocalFilePath && !this.IsLocalResourceSelectionDisabled)
            RdoLocalFile.IsChecked = true;
        if (!string.IsNullOrEmpty(element.Title))
            TxtToolTip.Text = element.Title;
        if (!string.IsNullOrEmpty(element.Target))
            CmbTarget.Text = element.Target;
        ChkTargetIncluded.IsChecked = (!string.IsNullOrEmpty(element.Target));
        if (!string.IsNullOrEmpty(element.InnerHtml))
            TxtInnerHtml.Text = element.InnerHtml;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is local resource selection disabled.
    /// </summary>
    /// <value><c>true</c> if this instance is local resource selection disabled; otherwise, <c>false</c>.</value>
    public bool IsLocalResourceSelectionDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [remove link].
    /// </summary>
    /// <value><c>true</c> if [remove link]; otherwise, <c>false</c>.</value>
    public bool RemoveLink
    {
        get => ChkRemoveLink.IsChecked == true;
        set => ChkRemoveLink.IsChecked = value;
    }

    /// <summary>
    /// When true, the editor sets the tooltip Title to
    /// "Ctrl+Click to view" when the user leaves Title blank. Passthrough
    /// auto-property so this custom dialog satisfies the interface contract.
    /// </summary>
    public bool UseCtrlClickTooltipDefault { get; set; }

    /// <summary>
    /// Handles the Click event of the lnkBtnCheck control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnCheck_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        try
        {
            Process.Start(TxtURL.Text);
        }
        catch
        {
            //MessageBox.Show(EditorLangRes.ErrorURL, EditorLangRes.ErrorCallingURL);
        }
    }

    /// <summary>
    /// Handles the Checked and Unchecked events of the ChkTargetIncluded control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void chkTargetIncluded_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;
            
        CmbTarget.IsEnabled = ChkTargetIncluded.IsChecked == true;
    }

    /// <summary>
    /// Handles the Click event of the BtnBrowseFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
    private void btnBrowseFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog myDialog = new OpenFileDialog();
        if (myDialog.ShowDialog() == true)
            TxtURL.Text = myDialog.FileName;
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
            
        updateBrowserButtonState();
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
            
        updateBrowserButtonState();
            
        bool? isLocalDir = this._theOriginalElement.IsBaseUrlALocalFolder();

        LnkBtnImportToBaseFolder.IsEnabled = RdoWorkingDirFile.IsEnabled
                                             && RdoWorkingDirFile.IsChecked == true 
                                             && isLocalDir.HasValue 
                                             && isLocalDir.Value;
        ChkOverwrite.IsEnabled = RdoWorkingDirFile.IsEnabled 
                                 && RdoWorkingDirFile.IsChecked == true 
                                 && isLocalDir.HasValue 
                                 && isLocalDir.Value;
        LnkBtnBrowseWD.IsEnabled = RdoWorkingDirFile.IsEnabled
                                   && RdoWorkingDirFile.IsChecked == true
                                   && isLocalDir.HasValue 
                                   && isLocalDir.Value;
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnBrowseWD control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnBrowseWD_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        OpenFileDialog srcUrlDialog = new OpenFileDialog
        {
            InitialDirectory = this._theOriginalElement.BaseUrl
        };

        if (srcUrlDialog.ShowDialog() != true)
            return;
            
        string directory = Path.GetDirectoryName(srcUrlDialog.FileName);
        string baseUrl = this._theOriginalElement.BaseUrl;
        if (this._theOriginalElement.BaseUrl.EndsWith("\\") || this._theOriginalElement.BaseUrl.EndsWith("/"))
            baseUrl = this._theOriginalElement.BaseUrl.Remove(this._theOriginalElement.BaseUrl.Length - 1);
        if (baseUrl.Equals(directory, StringComparison.OrdinalIgnoreCase))
            TxtURL.Text = Path.GetFileName(srcUrlDialog.FileName);
        else
        {
            MessageBoxResult dlgResult = MessageBox.Show(@"The file you selected is not from the base directory for relative path. Do you want to import that file to your base directory ? If you choose YES, then it will be imported to the Base Directory, otherwise the link target will be treated as absolute path file.", "Selected file is not from the base directory.", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (dlgResult)
            {
                case MessageBoxResult.Yes:
                    string newFilePath = Path.Combine(baseUrl, Path.GetFileName(srcUrlDialog.FileName) ?? string.Empty);
                    int i = 0;
                        
                    while (File.Exists(newFilePath) && (!ChkOverwrite.IsChecked == true))
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
                    TxtURL.Text = Path.GetFileName(newFilePath);
                    break;
                case MessageBoxResult.No:
                    TxtURL.Text = srcUrlDialog.FileName;
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the LnkBtnImportToBaseFolder control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
    private void lnkBtnImportToBaseFolder_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        OpenFileDialog myDialog = new OpenFileDialog();

        if (myDialog.ShowDialog() != true)
            return;
          
        string newFilePath = Path.Combine(this._theOriginalElement.BaseUrl, Path.GetFileName(myDialog.FileName) ?? string.Empty);
        int i = 0;
              
        while (File.Exists(newFilePath) && (!ChkOverwrite.IsChecked == true))
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
               
        TxtURL.Text = Path.GetFileName(newFilePath);
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
        if (string.IsNullOrEmpty(TxtURL.Text))
        {
            MessageBox.Show("Please provide Url");
            TxtURL.Focus();
            return;
        }
            
        DialogResult = true;
    }

    /// <summary>
    /// Updates the state of the browser button.
    /// </summary>
    private void updateBrowserButtonState()
    {
        BtnBrowseFile.IsEnabled = RdoLocalFile.IsChecked == true;
    }
}