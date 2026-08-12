Imports System
Imports System.Windows
Imports System.Windows.Input
Imports SpiceLogic.HtmlEditor.WPF.Models.BOs.EditorEventArgs
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports SpiceLogic.HtmlEditor.WPF.EditorEventArgs

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Search Window Form
    ''' </summary>
    Partial Public Class SearchWindow
        Implements ISearchDialog

        ''' <summary>
        ''' Shows whether the Search textbox has been focused
        ''' </summary>
        Private _isSearchTextBoxFocused As Boolean


        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchWindow" /> class.
        ''' </summary>
        Public Sub New()
            InitializeComponent()
            ' XAML already declares IsChecked="True" on
            ' RdoDirectionDown; no runtime override needed. Preload the last
            ' search text only when the host hasn't seeded a design-time value.
            If String.IsNullOrEmpty(TxtSearchBox.Text) AndAlso Not String.IsNullOrEmpty(_last) Then
                TxtSearchBox.Text = _last
            End If
            updateButtonsAvailability()
        End Sub

        ''' <summary>
        ''' Occurs when [find next clicked].
        ''' </summary>
        Public Event FindNextClicked As EventHandler(Of SearchEventArgBase) Implements ISearchDialogBase.FindNextClicked

        ''' <summary>
        ''' Occurs when [dialog closed].
        ''' </summary>
        Public Event DialogClosed As EventHandler(Of EventArgs) Implements ISearchDialogBase.DialogClosed

        ''' <summary>
        ''' Occurs when [replace clicked]
        ''' </summary>
        Public Event ReplaceClicked As EventHandler(Of ReplaceEventArgBase) Implements ISearchDialogBase.ReplaceClicked

        ''' <summary>
        ''' Occures when [replace all clicked]
        ''' </summary>
        Public Event ReplaceAllClicked As EventHandler(Of ReplaceAllEventArgBase) Implements ISearchDialogBase.ReplaceAllClicked

        ''' <summary>
        ''' The last
        ''' </summary>
        Private Shared _last As String


        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchWindow" /> class.
        ''' </summary>
        ''' <param name="preloadedSearchText">The preloaded search text.</param>
        Public Sub New(preloadedSearchText As String)
            TxtSearchBox.Text = preloadedSearchText
            Me.onFindNextClicked(Me,
                New SearchEventArg(preloadedSearchText) With {
                    .Direction = If(Equals(RdoDirectionDown.IsChecked, True),
                        SearchEventArgBase.SearchDirection.Down,
                        SearchEventArgBase.SearchDirection.Up),
                    .MatchCase = Equals(ChkMatchCase.IsChecked, True),
                    .MatchWholeWordOnly = Equals(ChkMatchWholeWordOnly.IsChecked, True)
                }
            )
        End Sub

        ''' <summary>
        ''' Gets or sets the preloaded search text.
        ''' </summary>
        ''' <value>The preloaded search text.</value>
        Public Property PreloadedSearchText As String Implements ISearchDialogBase.PreloadedSearchText
            Get
                Return TxtSearchBox.Text
            End Get
            Set(value As String)
                TxtSearchBox.Text = value
            End Set
        End Property


        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub

        Public Overloads Sub Show() Implements ISearchDialog.Show
            MyBase.Show()
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


        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Called when [search closed].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub onDialogClosed(sender As Object, e As EventArgs)
            _last = TxtSearchBox.Text

            RaiseEvent DialogClosed(sender, e)
        End Sub

        ''' <summary>
        ''' Called when [find next clicked].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The e.</param>
        Private Sub onFindNextClicked(sender As Object, e As SearchEventArgBase)
            RaiseEvent FindNextClicked(sender, e)
        End Sub

        ''' <summary>
        ''' Handles the Click event of the btnFindNext control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub btnFindNext_Click(sender As Object, e As EventArgs)
            Me.onFindNextClicked(sender, New SearchEventArg(TxtSearchBox.Text) With {
                .Direction = If(Equals(RdoDirectionDown.IsChecked, True),
                    SearchEventArgBase.SearchDirection.Down,
                    SearchEventArgBase.SearchDirection.Up),
                .MatchCase = Equals(ChkMatchCase.IsChecked, True),
                .MatchWholeWordOnly = Equals(ChkMatchWholeWordOnly.IsChecked, True)
            })
        End Sub

        ''' <summary>
        ''' Handles the TextChanged event of the txtSearchBox control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub txtSearchBox_TextChanged(sender As Object, e As EventArgs)
            updateButtonsAvailability()
        End Sub

        ''' <summary>
        ''' Handles the FormClosed event of the SearchWindow control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub SearchWindow_FormClosed(sender As Object, eventArgs As EventArgs)
            Me.onDialogClosed(sender, eventArgs)
        End Sub

        ''' <summary>
        ''' Handles the Click event of the btnReplace control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub btnReplace_Click(sender As Object, e As EventArgs)
            RaiseEvent ReplaceClicked(sender, New ReplaceEventArg(TxtSearchBox.Text, TxtReplaceBox.Text) With {
                .Direction = If(Equals(RdoDirectionDown.IsChecked, True),
                    ReplaceEventArgBase.SearchDirection.Down,
                    ReplaceEventArgBase.SearchDirection.Up),
                .MatchCase = Equals(ChkMatchCase.IsChecked, True),
                .MatchWholeWordOnly = Equals(ChkMatchWholeWordOnly.IsChecked, True)
            })
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnReplaceAll control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub btnReplaceAll_Click(sender As Object, e As EventArgs)
            RaiseEvent ReplaceAllClicked(sender, New ReplaceAllEventArg(TxtSearchBox.Text, TxtReplaceBox.Text) With {
                .MatchCase = Equals(ChkMatchCase.IsChecked, True),
                .MatchWholeWordOnly = Equals(ChkMatchWholeWordOnly.IsChecked, True)
            })
        End Sub

        ''' <summary>
        ''' Updates the buttons availability.
        ''' </summary>
        Private Sub updateButtonsAvailability()
            Dim enableButtons As Boolean = TxtSearchBox.Text.Length > 0
            BtnFindNext.IsEnabled = enableButtons
            BtnReplace.IsEnabled = enableButtons
            BtnReplaceAll.IsEnabled = enableButtons
        End Sub

        ''' <summary>
        ''' Handles the Activated event of the SearchWindow control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub SearchWindow_OnActivated(sender As Object, e As EventArgs)
            If Not _isSearchTextBoxFocused Then
                TxtSearchBox.Focus()
                _isSearchTextBoxFocused = True
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the Close button.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub closeButtonClicked(sender As Object, e As RoutedEventArgs)
            Close()
        End Sub
    End Class
End Namespace
