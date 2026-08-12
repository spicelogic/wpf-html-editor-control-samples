using System;
using System.Windows;
using System.Windows.Input;
using SpiceLogic.HtmlEditor.WPF.Models.BOs.EditorEventArgs;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.EditorEventArgs;

namespace CustomDialog.Dialogs;

/// <summary>
/// Search Window Form
/// </summary>
public partial class SearchWindow : ISearchDialog
{
    /// <summary>
    /// Shows whether the Search textbox has been focused
    /// </summary>
    private bool _isSearchTextBoxFocused;
        
        
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchWindow" /> class.
    /// </summary>
    public SearchWindow()
    {
        InitializeComponent();
        // XAML already declares IsChecked="True" on
        // RdoDirectionDown; no runtime override needed. Preload the last
        // search text only when the host hasn't seeded a design-time value.
        if (string.IsNullOrEmpty(TxtSearchBox.Text) && !string.IsNullOrEmpty(_last))
            TxtSearchBox.Text = _last;
        updateButtonsAvailability();
    }

    /// <summary>
    /// Occurs when [find next clicked].
    /// </summary>
    public event EventHandler<SearchEventArgBase> FindNextClicked;

    /// <summary>
    /// Occurs when [dialog closed].
    /// </summary>
    public event EventHandler<EventArgs> DialogClosed;

    /// <summary>
    /// Occurs when [replace clicked]
    /// </summary>
    public event EventHandler<ReplaceEventArgBase> ReplaceClicked;

    /// <summary>
    /// Occures when [replace all clicked]
    /// </summary>
    public event EventHandler<ReplaceAllEventArgBase> ReplaceAllClicked;

    /// <summary>
    /// The last
    /// </summary>
    private static string _last;


    /// <summary>
    /// Initializes a new instance of the <see cref="SearchWindow" /> class.
    /// </summary>
    /// <param name="preloadedSearchText">The preloaded search text.</param>
    public SearchWindow(string preloadedSearchText)
    {
        TxtSearchBox.Text = preloadedSearchText;
        this.onFindNextClicked(this,
            new SearchEventArg(preloadedSearchText)
            {
                Direction = RdoDirectionDown.IsChecked == true
                    ? SearchEventArgBase.SearchDirection.Down
                    : SearchEventArgBase.SearchDirection.Up,
                MatchCase = ChkMatchCase.IsChecked == true,
                MatchWholeWordOnly = ChkMatchWholeWordOnly.IsChecked == true
            }
        );
    }

    /// <summary>
    /// Gets or sets the preloaded search text.
    /// </summary>
    /// <value>The preloaded search text.</value>
    public string PreloadedSearchText
    {
        get => TxtSearchBox.Text;
        set => TxtSearchBox.Text = value;
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
    /// Called when [search closed].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void onDialogClosed(object sender, EventArgs e)
    {
        _last = TxtSearchBox.Text;

        this.DialogClosed?.Invoke(sender, e);
    }

    /// <summary>
    /// Called when [find next clicked].
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void onFindNextClicked(object sender, SearchEventArgBase e)
    {
        this.FindNextClicked?.Invoke(sender, e);
    }

    /// <summary>
    /// Handles the Click event of the btnFindNext control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private void btnFindNext_Click(object sender, EventArgs e)
    {
        this.onFindNextClicked(sender, new SearchEventArg(TxtSearchBox.Text)
        {
            Direction = RdoDirectionDown.IsChecked == true
                ? SearchEventArgBase.SearchDirection.Down
                : SearchEventArgBase.SearchDirection.Up,
            MatchCase = ChkMatchCase.IsChecked == true,
            MatchWholeWordOnly = ChkMatchWholeWordOnly.IsChecked == true
        });
    }

    /// <summary>
    /// Handles the TextChanged event of the txtSearchBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private void txtSearchBox_TextChanged(object sender, EventArgs e)
    {
        updateButtonsAvailability();
    }

    /// <summary>
    /// Handles the FormClosed event of the SearchWindow control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void SearchWindow_FormClosed(object sender, EventArgs eventArgs)
    {
        this.onDialogClosed(sender, eventArgs);
    }

    /// <summary>
    /// Handles the Click event of the btnReplace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void btnReplace_Click(object sender, EventArgs e)
    {
        EventHandler<ReplaceEventArgBase> onReplaceClicked = this.ReplaceClicked;
        onReplaceClicked?.Invoke(sender, new ReplaceEventArg(TxtSearchBox.Text, TxtReplaceBox.Text)
        {
            Direction = RdoDirectionDown.IsChecked == true
                ? ReplaceEventArgBase.SearchDirection.Down
                : ReplaceEventArgBase.SearchDirection.Up,
            MatchCase = ChkMatchCase.IsChecked == true,
            MatchWholeWordOnly = ChkMatchWholeWordOnly.IsChecked == true
        });
    }

    /// <summary>
    /// Handles the Click event of the BtnReplaceAll control.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void btnReplaceAll_Click(object sender, EventArgs e)
    {
        EventHandler<ReplaceAllEventArgBase> onReplaceAllClicked = this.ReplaceAllClicked;
        onReplaceAllClicked?.Invoke(sender, new ReplaceAllEventArg(TxtSearchBox.Text, TxtReplaceBox.Text)
        {
            MatchCase = ChkMatchCase.IsChecked == true,
            MatchWholeWordOnly = ChkMatchWholeWordOnly.IsChecked == true
        });
    }

    /// <summary>
    /// Updates the buttons availability.
    /// </summary>
    private void updateButtonsAvailability()
    {
        bool enableButtons = TxtSearchBox.Text.Length > 0;
        BtnFindNext.IsEnabled = enableButtons;
        BtnReplace.IsEnabled = enableButtons;
        BtnReplaceAll.IsEnabled = enableButtons;
    }

    /// <summary>
    /// Handles the Activated event of the SearchWindow control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void SearchWindow_OnActivated(object sender, EventArgs e)
    {
        if (!_isSearchTextBoxFocused)
        {
            TxtSearchBox.Focus();
            _isSearchTextBoxFocused = true;
        }
    }

    /// <summary>
    /// Handles the Click event of the Close button.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The event data.</param>
    private void closeButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}