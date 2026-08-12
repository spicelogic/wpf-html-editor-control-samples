Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports mshtml
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck
Imports SpiceLogic.HtmlEditor.Abstractions.Options
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports SpiceLogic.HtmlEditor.WPF.Extensions

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Spell Checker Dialog implementation.
    ''' </summary>
    Partial Public Class SpellCheckerDialog
        Implements ISpellCheckerDialog

#Region "private members"

        ''' <summary>
        ''' Indicates that dialog is currently in searching mode
        ''' </summary>
        Private _isSearching As Boolean = True

        ''' <summary>
        ''' Holds current spell checking node
        ''' </summary>
        Private _node As SpellCheckerNode

        ''' <summary>
        ''' Maximum length for replacement word
        ''' </summary>
        Private Const ReplacementWordMaxLength As Integer = 150

        ''' <summary>
        ''' Color to mark current spelling word in text
        ''' </summary>
        Private Shared ReadOnly CurrentSpellingWordColor As Color = CType(ColorConverter.ConvertFromString("Red"), Color)

        ''' <summary>
        ''' Gets range of current misspelled word
        ''' </summary>
        Private _currentWordRange As IHTMLTxtRange

#End Region

#Region "public members"

        ''' <summary>
        ''' Gets or sets an overall words count
        ''' </summary>
        Public Property WordCount As Integer Implements ISpellCheckerDialogBase.WordCount

#End Region

        ''' <summary>
        ''' Creates a new instance of <see cref="SpellCheckerDialog" />
        ''' and initialize spell checker options reference
        ''' </summary>
        Public Sub New()
            InitializeComponent()
        End Sub

#Region "private properties"

        ''' <summary>
        ''' Indicates that dialog is currently in searching mode
        ''' </summary>
        ''' <value><c>true</c> if this instance is searching; otherwise, <c>false</c>.</value>
        Private Property isSearching As Boolean
            Get
                Return _isSearching
            End Get
            Set(value As Boolean)
                If _isSearching <> value Then
                    _isSearching = value
                    updateWindowState()
                End If
            End Set
        End Property

#End Region


#Region "public properties"

        ''' <summary>
        ''' Gets current spelling node
        ''' </summary>
        Public Property Node As SpellCheckerNode Implements ISpellCheckerDialogBase.Node
            Get
                Return _node
            End Get
            Private Set(value As SpellCheckerNode)
                _node = value
            End Set
        End Property

        ''' <summary>
        ''' The actual Spell Checker options reference
        ''' </summary>
        ''' <value>The options.</value>
        Public Property Options As ISpellCheckerOption Implements ISpellCheckerDialogBase.Options

        Public ReadOnly Property Visible As Boolean Implements ISpellCheckerDialogBase.Visible
            Get
                Return Me.IsVisible
            End Get
        End Property

        ''' <summary>
        ''' The text that is under spell checking at the moment
        ''' </summary>
        ''' <value>The document text.</value>
        Public Property DocumentText As String Implements ISpellCheckerDialogBase.DocumentText
            Get
                Return New TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd).Text
            End Get
            Set(value As String)
                Dim r As New TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd)
                r.Text = value
                updateWindowState()
            End Set
        End Property

#End Region

#Region "public events"

        ''' <summary>
        ''' An event that raises when a user choose a one of actions (Ignore, Delete, Replace etc.)
        ''' </summary>
        Public Event SpellingActionRequested As EventHandler(Of SpellingActionEventArgs) Implements ISpellCheckerDialogBase.SpellingActionRequested

        ''' <summary>
        ''' Raises when dialog windows is loaded
        ''' </summary>
        Public Event DialogLoaded As EventHandler Implements ISpellCheckerDialogBase.DialogLoaded

#End Region

#Region "public methods"

        ''' <summary>
        ''' Sets the current node.
        ''' </summary>
        ''' <param name="node">The node.</param>
        ''' <param name="currentWordRange">The current word range.</param>
        ''' <exception cref="System.ArgumentNullException">
        ''' node
        ''' or
        ''' currentWordRange
        ''' </exception>
        Public Sub SetCurrentNode(node As SpellCheckerNode, currentWordRange As IHTMLTxtRange) Implements ISpellCheckerDialogBase.SetCurrentNode
            If node Is Nothing Then Throw New ArgumentNullException(NameOf(node))
            _node = node
            If currentWordRange Is Nothing Then Throw New ArgumentNullException(NameOf(currentWordRange))
            Me._currentWordRange = currentWordRange

            Me.isSearching = True
            updateMisspelledWordStyle()
            updateWindowState()
        End Sub

        ''' <summary>
        ''' Closes the dialog with predefined result based on <paramref name="canceled"/>
        ''' </summary>
        ''' <param name="canceled">if set to True then DialogResult is Cancel, otherwise DialogResult is OK.</param>
        Public Overloads Sub Close(canceled As Boolean) Implements ISpellCheckerDialogBase.Close
            Me.DialogResult = Not canceled
            Me.Close()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose

        End Sub

        Public Overloads Function ShowDialog() As Boolean? Implements IDialogBase.ShowDialog
            Return MyBase.ShowDialog()
        End Function

        Public Property IDialog_Owner As Window Implements IDialog.Owner
            Get
                Return MyBase.Owner
            End Get
            Set(value As Window)
                MyBase.Owner = value
            End Set
        End Property

#End Region

#Region "overrides"

        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Raises the Closed event.
        ''' </summary>
        ''' <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
        Protected Overrides Sub OnClosed(e As EventArgs)
            MyBase.OnClosed(e)
            Me.Node = Nothing
        End Sub

#End Region

#Region "Spell Check Dialog handlers"

        Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
            ' Raise dialog loaded event
            RaiseEvent DialogLoaded(Me, New EventArgs())

            updateWindowState()
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnIgnore control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnIgnore_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim eventArgs As New SpellingActionEventArgs(Me.Node, Me._currentWordRange, SpellingActionType.Ignore)
                RaiseEvent SpellingActionRequested(Me, eventArgs)
                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnIgnoreAll control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnIgnoreAll_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim eventArgs As New SpellingActionEventArgs(Me.Node, Me._currentWordRange, SpellingActionType.IgnoreAll)
                RaiseEvent SpellingActionRequested(Me, eventArgs)

                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnAddtoDictionary control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnAddtoDictionary_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim eventArgs As New SpellingActionEventArgs(Me.Node, Me._currentWordRange, SpellingActionType.AddToDictionary)
                RaiseEvent SpellingActionRequested(Me, eventArgs)

                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnDelete control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim eventArgs As New SpellingActionEventArgs(Me.Node, Me._currentWordRange, SpellingActionType.Delete)
                RaiseEvent SpellingActionRequested(Me, eventArgs)

                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnReplace control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnReplace_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim replacementWord As String = TxtReplacementWord.Text
                Dim eventArgs As New ReplaceActionEventArgs(Me.Node, Me._currentWordRange, replacementWord)
                RaiseEvent SpellingActionRequested(Me, eventArgs)

                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnReplaceAll control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btnReplaceAll_Click(sender As Object, e As RoutedEventArgs)
            Try
                Dim replacementWord As String = TxtReplacementWord.Text
                Dim eventArgs As New ReplaceActionEventArgs(Me.Node, Me._currentWordRange, replacementWord, SpellingActionType.ReplaceAll)
                RaiseEvent SpellingActionRequested(Me, eventArgs)

                Me.isSearching = True
            Catch
            End Try
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the SuggestionList control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        Private Sub suggestionList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            If SuggestionList.SelectedIndex > -1 Then
                TxtReplacementWord.Text = SuggestionList.SelectedValue.ToString()
                validateReplacementWord(TxtReplacementWord.Text)
            End If
        End Sub

        ''' <summary>
        ''' Validates the replacement word.
        ''' </summary>
        ''' <param name="replacementWord">The replacement word.</param>
        Private Sub validateReplacementWord(replacementWord As String)
            If isReplacementWordValid(replacementWord) Then
                MessageToolBarStatusTextBlock.Text = String.Empty
            ElseIf Me.Node IsNot Nothing AndAlso Me.Node.ErrorKind = SpellingErrorKind.MisspelledWord Then
                If String.IsNullOrEmpty(replacementWord) Then
                    MessageToolBarStatusTextBlock.Text = "No replacement word specified"
                ElseIf replacementWord.Length > ReplacementWordMaxLength Then
                    MessageToolBarStatusTextBlock.Text =
                        $"Replacement word length shouldn't exceed {ReplacementWordMaxLength} chars"
                End If
            End If

            updateDialogButtons()
        End Sub

        ''' <summary>
        ''' Handles replace word text box TextChanged event.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        Private Sub txtReplaceWord_TextChanged(sender As Object, e As TextChangedEventArgs)
            validateReplacementWord(TxtReplacementWord.Text)
        End Sub

        ''' <summary>
        ''' Handles the ButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub CloseButtonClicked(sender As Object, e As RoutedEventArgs)
            Close()
        End Sub

#End Region

#Region "private methods"

        ''' <summary>
        ''' Updates all window controls state using current context
        ''' </summary>
        Private Sub updateWindowState()
            updateSuggestionList()
            updateDialogButtons()
            updateStatusBar()
        End Sub

        ''' <summary>
        ''' Updates the status bar.
        ''' </summary>
        Private Sub updateStatusBar()
            If Me.isSearching Then
                WordStatusTextBlock.Text = Me.Options.WaitAlertMessage
            ElseIf Me.Node IsNot Nothing Then
                WordStatusTextBlock.Text = getFormattedSpellingWord(Me.Node.Word)
                WordCountToolBarStatusTextBlock.Text = $"Word {Me.Node.WordIndex + 1} from {Me.WordCount}"
            End If
        End Sub

        ''' <summary>
        ''' Gets currently spelling word surrounded by some extra information
        ''' about concrete spelling error kind such as: Misspelled word: [Word], Duplicate word: [Word]
        ''' </summary>
        ''' <param name="word">Current spelling word</param>
        ''' <returns>Formatted string contains current spelling word</returns>
        Private Function getFormattedSpellingWord(word As String) As String
            Dim wordStatusFormat As String
            Select Case Me.Node.ErrorKind
                Case SpellingErrorKind.MisspelledWord
                    wordStatusFormat = "Misspelled word: {0}"
                Case SpellingErrorKind.DuplicateWord
                    wordStatusFormat = "Duplicate word: {0}"
                Case Else
                    wordStatusFormat = "Current word: {0}"
            End Select

            Return String.Format(wordStatusFormat, word)
        End Function

        ''' <summary>
        ''' Updates all dialog buttons
        ''' </summary>
        Private Sub updateDialogButtons()
            SuggestionList.IsEnabled = Not Me.isSearching
            BtnIgnore.IsEnabled = Me.Node IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.DocumentText) AndAlso Not Me.isSearching
            BtnIgnoreAll.IsEnabled = Me.Node IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.DocumentText) AndAlso Not Me.isSearching
            BtnDelete.IsEnabled = Me.Node IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.DocumentText) AndAlso Not Me.isSearching
            BtnAddToDictionary.IsEnabled = Me.Node IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.DocumentText) AndAlso Not Me.isSearching AndAlso
                                           Me.Node.ErrorKind = SpellingErrorKind.MisspelledWord
            BtnReplace.IsEnabled = Me.Node IsNot Nothing AndAlso isReplacementWordValid(TxtReplacementWord.Text) AndAlso Not Me.isSearching
            BtnReplaceAll.IsEnabled = Me.Node IsNot Nothing AndAlso isReplacementWordValid(TxtReplacementWord.Text) AndAlso Not Me.isSearching

            BtnAddToDictionary.Content = Me.Options.AddToDictionaryText
            BtnAddToDictionary.Visibility = If(Me.Options.DictionaryFile.EnableUserDictionary AndAlso
                                            (Me.Node IsNot Nothing AndAlso Me.Node.ErrorKind <> SpellingErrorKind.DuplicateWord),
                Visibility.Visible, Visibility.Collapsed)

            Dim deleteButtonContent As String = If(Me.Node IsNot Nothing AndAlso Me.Node.ErrorKind = SpellingErrorKind.DuplicateWord,
                Me.Options.DeleteDuplicateText, Me.Options.DeleteText)
            BtnDelete.Content = deleteButtonContent
            BtnIgnore.Content = Me.Options.IgnoreText
            BtnIgnoreAll.Content = Me.Options.IgnoreAllText
        End Sub

        Private Shared Function isReplacementWordValid(replacementWord As String) As Boolean
            Return Not String.IsNullOrEmpty(replacementWord) AndAlso replacementWord.Length <= ReplacementWordMaxLength
        End Function

        ''' <summary>
        ''' Update suggestion list using current context
        ''' </summary>
        Private Sub updateSuggestionList()
            TxtReplacementWord.Text = String.Empty

            Try
                SuggestionList.ItemsSource = Nothing
                If Me.Node IsNot Nothing AndAlso Me.Node.HasSuggestions Then
                    Dim constrainedSuggestionsList As New List(Of String)()
                    For i As Integer = 0 To Math.Min(Me.Options.MaxSuggestionsForDialogs, Me.Node.Suggestions.Length) - 1
                        constrainedSuggestionsList.Add(Me.Node.Suggestions(i))
                    Next

                    SuggestionList.ItemsSource = constrainedSuggestionsList
                    SuggestionList.SelectedIndex = 0
                End If
            Finally
                ' Move caret cursot to the end of current replacement word
                TxtReplacementWord.Select(TxtReplacementWord.Text.Length, 0)
                TxtReplacementWord.Focus()
            End Try
        End Sub

        ''' <summary>
        ''' Updates document selecting misspelled words
        ''' </summary>
        Private Sub updateMisspelledWordStyle()
            If Me.Node Is Nothing OrElse String.IsNullOrEmpty(Me.DocumentText) Then
                Return
            End If

            Dim textRange As New TextRange(RichTxtDocument.Document.ContentStart, RichTxtDocument.Document.ContentEnd)

            If textRange.Text.Length >= Me.Node.TextPosition + Me.Node.Word.Length Then
                Dim startPointer As TextPointer =
                    textRange.Start.GetInsertionPositionAtOffset(Me.Node.TextPosition, LogicalDirection.Forward)
                If startPointer Is Nothing Then
                    Return
                End If

                Dim endPointer As TextPointer = textRange.Start.GetInsertionPositionAtOffset(Me.Node.TextPosition + Me.Node.Word.Length, LogicalDirection.Forward)
                If endPointer Is Nothing Then
                    Return
                End If

                Dim range As New TextRange(startPointer, endPointer)
                range.ApplyPropertyValue(TextElement.ForegroundProperty, New SolidColorBrush(CurrentSpellingWordColor))
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline)
            End If

            Me.isSearching = False
        End Sub

#End Region


    End Class
End Namespace
