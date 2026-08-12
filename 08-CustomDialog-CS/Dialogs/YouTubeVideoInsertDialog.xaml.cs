using System.Windows;
using System.Windows.Input;
using SpiceLogic.HtmlEditor.Abstractions.Entities;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;

namespace CustomDialog.Dialogs;

/// <summary>
/// Class YouTubeVideoInsertDialog
/// </summary>
public partial class YouTubeVideoInsertDialog : IYouTubeVideoInsertDialog
{
    /// <summary>
    /// The _the original element
    /// </summary>
    private YouTubeVideoElement _theOriginalElement;

    /// <summary>
    /// Initializes a new instance of the <see cref="YouTubeVideoInsertDialog" /> class.
    /// </summary>
    public YouTubeVideoInsertDialog()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    /// <value>The element.</value>
    public YouTubeVideoElement Element
    {
        get
        {
            if (_theOriginalElement == null)
            {
                YouTubeVideoElement theElement = new YouTubeVideoElement
                {
                    Url = TxtUrl.Text.Trim(),
                    Width = TxtWidth.Text,
                    Height = TxtHeight.Text
                };

                return theElement;
            }

            _theOriginalElement.Url = TxtUrl.Text.Trim();
            _theOriginalElement.Width = TxtWidth.Text;
            _theOriginalElement.Height = TxtHeight.Text;
            _theOriginalElement.CssStyle = TxtCssStyle.Text;

            return _theOriginalElement;
        }
        set
        {
            _theOriginalElement = value;
            // preserve design-time defaults on empty fields.
            if (!string.IsNullOrEmpty(value.Url))
                TxtUrl.Text = value.Url;
            if (!string.IsNullOrEmpty(value.Height))
                TxtHeight.Text = value.Height;
            if (!string.IsNullOrEmpty(value.Width))
                TxtWidth.Text = value.Width;
            if (!string.IsNullOrEmpty(value.CssStyle))
                TxtCssStyle.Text = value.CssStyle;
        }
    }


    public void Dispose()
    {
    }


    /// <summary>
    /// Handles the OnRightButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
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
        //OK button
            
        if (string.IsNullOrEmpty(TxtUrl.Text))
        {
            MessageBox.Show("The YouTube URL cannot be empty.");
            TxtUrl.Focus();
            return;
        }

        string theUrl = TxtUrl.Text.Trim();
        if (theUrl == string.Empty)
        {
            MessageBox.Show("The YouTube URL cannot be empty.");
            TxtUrl.Focus();
            return;
        }

        if (!theUrl.ToLower().Contains("youtube.com"))
        {
            MessageBox.Show("The URL you provided does not contain the YouTube Domain name", "Invalid URL");
            return;
        }

        this.DialogResult = true;
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
}