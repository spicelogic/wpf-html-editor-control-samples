using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using mshtml;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
using SpiceLogic.HtmlEditor.Abstractions.Options;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;
using SpiceLogic.HtmlEditor.WPF.Extensions;

namespace CustomDialog.Dialogs;

/// <summary>
/// Spell Checker Dialog implementation.
/// </summary>
public partial class SpellCheckerDialog : ISpellCheckerDialog
{
    #region private members

    /// <summary>
    /// Indicates that dialog is currently in searching mode
    /// </summary>
    private bool _isSearching = true;

    /// <summary>
    /// Holds current spell checking node
    /// </summary>
    private SpellCheckerNode _node;

    /// <summary>
    /// Maximum length for replacement word
    /// </summary>
    private const int ReplacementWordMaxLength = 150;

    /// <summary>
    /// Color to mark current spelling word in text
    /// </summary>
    private static readonly Color CurrentSpellingWordColor = (Color)ColorConverter.ConvertFromString("Red");

    /// <summary>
    /// Gets range of current misspelled word
    /// </summary>
    private IHTMLTxtRange _currentWordRange;

    #endregion

    #region public members

    /// <summary>
    /// Gets or sets an overall words count
    /// </summary>
    public int WordCount { get; set; }

    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="SpellCheckerDialog" />
    /// and initialize spell checker options reference
    /// </summary>
    public SpellCheckerDialog()
    {
        InitializeComponent();
    }

    #region private properties

    /// <summary>
    /// Indicates that dialog is currently in searching mode
    /// </summary>
    /// <value><c>true</c> if this instance is searching; otherwise, <c>false</c>.</value>
    private bool isSearching
    {
        get => _isSearching;
        set
        {
            if (_isSearching != value)
            {
                _isSearching = value;
                updateWindowState();
            }
        }
    }

    #endregion


    #region public properties

    /// <summary>
    /// Gets current spelling node
    /// </summary>
    public SpellCheckerNode Node
    {
        get => _node;
        private set => _node = value;
    }

    /// <summary>
    /// The actual Spell Checker options reference
    /// </summary>
    /// <value>The options.</value>
    public ISpellCheckerOption Options { get; set; }

    public bool Visible => this.IsVisible;

    /// <summary>
    /// The text that is under spell checking at the moment
    /// </summary>
    /// <value>The document text.</value>
    public string DocumentText
    {
        get => new TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd).Text;
        set
        {
            new TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd).Text = value;
            updateWindowState();
        }
    }

    #endregion

    #region public events

    /// <summary>
    /// An event that raises when a user choose a one of actions (Ignore, Delete, Replace etc.)
    /// </summary>
    public event EventHandler<SpellingActionEventArgs> SpellingActionRequested;

    /// <summary>
    /// Raises when dialog windows is loaded
    /// </summary>
    public event EventHandler DialogLoaded;

    #endregion

    #region public methods

    /// <summary>
    /// Sets the current node.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="currentWordRange">The current word range.</param>
    /// <exception cref="System.ArgumentNullException">
    /// node
    /// or
    /// currentWordRange
    /// </exception>
    public void SetCurrentNode(SpellCheckerNode node, IHTMLTxtRange currentWordRange)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
        this._currentWordRange = currentWordRange ?? throw new ArgumentNullException(nameof(currentWordRange));

        this.isSearching = true;
        updateMisspelledWordStyle();
        updateWindowState();
    }

    /// <summary>
    /// Closes the dialog with predefined result based on <paramref name="canceled"/>
    /// </summary>
    /// <param name="canceled">if set to True then DialogResult is Cancel, otherwise DialogResult is OK.</param>
    public void Close(bool canceled)
    {
        this.DialogResult = !canceled;
        this.Close();
    }

    public void Dispose()
    {

    }

    #endregion

    #region overrides

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
    /// Raises the Closed event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        this.Node = null;
    }

    #endregion

    #region Spell Check Dialog handlers

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Raise dialog loaded event
        this.DialogLoaded?.Invoke(this, new EventArgs());

        updateWindowState();
    }
        
    /// <summary>
    /// Handles the Click event of the BtnIgnore control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnIgnore_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                SpellingActionEventArgs eventArgs = new SpellingActionEventArgs(this.Node, this._currentWordRange, SpellingActionType.Ignore);
                handler(this, eventArgs);
            }
            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the Click event of the BtnIgnoreAll control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnIgnoreAll_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                SpellingActionEventArgs eventArgs = new SpellingActionEventArgs(this.Node, this._currentWordRange, SpellingActionType.IgnoreAll);
                handler(this, eventArgs);
            }

            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the Click event of the BtnAddtoDictionary control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnAddtoDictionary_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                SpellingActionEventArgs eventArgs = new SpellingActionEventArgs(this.Node, this._currentWordRange, SpellingActionType.AddToDictionary);
                handler(this, eventArgs);
            }

            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the Click event of the BtnDelete control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                SpellingActionEventArgs eventArgs = new SpellingActionEventArgs(this.Node, this._currentWordRange, SpellingActionType.Delete);
                handler(this, eventArgs);
            }

            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the Click event of the BtnReplace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnReplace_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                string replacementWord = TxtReplacementWord.Text;
                ReplaceActionEventArgs eventArgs = new ReplaceActionEventArgs(this.Node, this._currentWordRange, replacementWord);
                handler(this, eventArgs);
            }

            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the Click event of the BtnReplaceAll control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void btnReplaceAll_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            EventHandler<SpellingActionEventArgs> handler = this.SpellingActionRequested;
            if (handler != null)
            {
                string replacementWord = TxtReplacementWord.Text;
                ReplaceActionEventArgs eventArgs = new ReplaceActionEventArgs(this.Node, this._currentWordRange, replacementWord, SpellingActionType.ReplaceAll);
                handler(this, eventArgs);
            }

            this.isSearching = true;
        }
        catch
        { }
    }

    /// <summary>
    /// Handles the SelectionChanged event of the SuggestionList control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
    private void suggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SuggestionList.SelectedIndex > -1)
        {
            TxtReplacementWord.Text = SuggestionList.SelectedValue.ToString();
            validateReplacementWord(TxtReplacementWord.Text);
        }
    }

    /// <summary>
    /// Validates the replacement word.
    /// </summary>
    /// <param name="replacementWord">The replacement word.</param>
    private void validateReplacementWord(string replacementWord)
    {
        if (isReplacementWordValid(replacementWord))
            MessageToolBarStatusTextBlock.Text = string.Empty;
        else if (this.Node is { ErrorKind: SpellingErrorKind.MisspelledWord })
        {
            if (string.IsNullOrEmpty(replacementWord))
                MessageToolBarStatusTextBlock.Text = @"No replacement word specified";
            else if (replacementWord.Length > ReplacementWordMaxLength)
                MessageToolBarStatusTextBlock.Text =
                    $"Replacement word length shouldn\'t exceed {ReplacementWordMaxLength} chars";
        }

        updateDialogButtons();
    }

    /// <summary>
    /// Handles replace word text box TextChanged event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
    private void txtReplaceWord_TextChanged(object sender, TextChangedEventArgs e)
    {
        validateReplacementWord(TxtReplacementWord.Text);
    }

    /// <summary>
    /// Handles the ButtonClicked event of the TwoButtonPanel control
    /// </summary>
    /// <param name="sender">The event source</param>
    /// <param name="e">The event data</param>
    private void CloseButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion

    #region private methods

    /// <summary>
    /// Updates all window controls state using current context
    /// </summary>
    private void updateWindowState()
    {
        updateSuggestionList();
        updateDialogButtons();
        updateStatusBar();
    }

    /// <summary>
    /// Updates the status bar.
    /// </summary>
    private void updateStatusBar()
    {
        if (this.isSearching)
            WordStatusTextBlock.Text = this.Options.WaitAlertMessage;
        else if (this.Node != null)
        {
            WordStatusTextBlock.Text = getFormattedSpellingWord(this.Node.Word);
            WordCountToolBarStatusTextBlock.Text = $"Word {this.Node.WordIndex + 1} from {this.WordCount}";
        }
    }

    /// <summary>
    /// Gets currently spelling word surrounded by some extra information
    /// about concrete spelling error kind such as: Misspelled word: [Word], Duplicate word: [Word]
    /// </summary>
    /// <param name="word">Current spelling word</param>
    /// <returns>Formatted string contains current spelling word</returns>
    private string getFormattedSpellingWord(string word)
    {
        string wordStatusFormat;
        switch (this.Node.ErrorKind)
        {
            case SpellingErrorKind.MisspelledWord:
                wordStatusFormat = "Misspelled word: {0}";
                break;
            case SpellingErrorKind.DuplicateWord:
                wordStatusFormat = "Duplicate word: {0}";
                break;
            default:
                wordStatusFormat = "Current word: {0}";
                break;
        }

        return string.Format(wordStatusFormat, word);
    }

    /// <summary>
    /// Updates all dialog buttons
    /// </summary>
    private void updateDialogButtons()
    {
        SuggestionList.IsEnabled = !this.isSearching;
        BtnIgnore.IsEnabled = this.Node != null && !string.IsNullOrEmpty(this.DocumentText) && !this.isSearching;
        BtnIgnoreAll.IsEnabled = this.Node != null && !string.IsNullOrEmpty(this.DocumentText) && !this.isSearching;
        BtnDelete.IsEnabled = this.Node != null && !string.IsNullOrEmpty(this.DocumentText) && !this.isSearching;
        BtnAddToDictionary.IsEnabled = this.Node != null && !string.IsNullOrEmpty(this.DocumentText) && !this.isSearching &&
                                       this.Node.ErrorKind == SpellingErrorKind.MisspelledWord;
        BtnReplace.IsEnabled = this.Node != null && isReplacementWordValid(TxtReplacementWord.Text) && !this.isSearching;
        BtnReplaceAll.IsEnabled = this.Node != null && isReplacementWordValid(TxtReplacementWord.Text) && !this.isSearching;

        BtnAddToDictionary.Content = this.Options.AddToDictionaryText;
        BtnAddToDictionary.Visibility = this.Options.DictionaryFile.EnableUserDictionary &&
                                        (this.Node != null && this.Node.ErrorKind != SpellingErrorKind.DuplicateWord)
            ? Visibility.Visible : Visibility.Collapsed;

        string deleteButtonContent = this.Node is { ErrorKind: SpellingErrorKind.DuplicateWord } ?
            this.Options.DeleteDuplicateText : this.Options.DeleteText;
        BtnDelete.Content = deleteButtonContent;
        BtnIgnore.Content = this.Options.IgnoreText;
        BtnIgnoreAll.Content = this.Options.IgnoreAllText;
    }

    private static bool isReplacementWordValid(string replacementWord)
    {
        return !string.IsNullOrEmpty(replacementWord) && replacementWord.Length <= ReplacementWordMaxLength;
    }

    /// <summary>
    /// Update suggestion list using current context
    /// </summary>
    private void updateSuggestionList()
    {
        TxtReplacementWord.Text = string.Empty;

        try
        {
            SuggestionList.ItemsSource = null;
            if (this.Node is { HasSuggestions: true })
            {
                List<string> constrainedSuggestionsList = [];
                for (int i = 0; i < this.Options.MaxSuggestionsForDialogs && i < this.Node.Suggestions.Length; ++i)
                {
                    constrainedSuggestionsList.Add(this.Node.Suggestions[i]);
                }

                SuggestionList.ItemsSource = constrainedSuggestionsList;
                SuggestionList.SelectedIndex = 0;
            }
        }
        finally
        {
            // Move caret cursot to the end of current replacement word
            TxtReplacementWord.Select(TxtReplacementWord.Text.Length, 0);
            TxtReplacementWord.Focus();
        }
    }

    /// <summary>
    /// Updates document selecting misspelled words
    /// </summary>
    private void updateMisspelledWordStyle()
    {
        if (this.Node == null || string.IsNullOrEmpty(this.DocumentText))
            return;

        TextRange textRange = new TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd);

        if (textRange.Text.Length >= this.Node.TextPosition + this.Node.Word.Length)
        {
            TextPointer startPointer =
                textRange.Start.GetInsertionPositionAtOffset(this.Node.TextPosition, LogicalDirection.Forward);
            if (startPointer == null)
                return;

            TextPointer endPointer = textRange.Start.GetInsertionPositionAtOffset(this.Node.TextPosition + this.Node.Word.Length, LogicalDirection.Forward);
            if (endPointer == null)
                return;

            TextRange range = new TextRange(startPointer, endPointer);
            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(CurrentSpellingWordColor));
            range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }

        this.isSearching = false;
    }

    #endregion

        
}