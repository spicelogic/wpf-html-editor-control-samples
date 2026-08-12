Imports System.Windows
Imports System.Windows.Input
Imports SpiceLogic.HtmlEditor.Abstractions.Entities
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class YouTubeVideoInsertDialog
    ''' </summary>
    Partial Public Class YouTubeVideoInsertDialog
        Implements IYouTubeVideoInsertDialog

        ''' <summary>
        ''' The _the original element
        ''' </summary>
        Private _theOriginalElement As YouTubeVideoElement

        ''' <summary>
        ''' Initializes a new instance of the <see cref="YouTubeVideoInsertDialog" /> class.
        ''' </summary>
        Public Sub New()
            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Gets or sets the element.
        ''' </summary>
        ''' <value>The element.</value>
        Public Property Element As YouTubeVideoElement Implements IYouTubeVideoInsertDialogBase.Element
            Get
                If _theOriginalElement Is Nothing Then
                    Dim theElement As New YouTubeVideoElement With {
                        .Url = TxtUrl.Text.Trim(),
                        .Width = TxtWidth.Text,
                        .Height = TxtHeight.Text
                    }

                    Return theElement
                End If

                _theOriginalElement.Url = TxtUrl.Text.Trim()
                _theOriginalElement.Width = TxtWidth.Text
                _theOriginalElement.Height = TxtHeight.Text
                _theOriginalElement.CssStyle = TxtCssStyle.Text

                Return _theOriginalElement
            End Get
            Set(value As YouTubeVideoElement)
                _theOriginalElement = value
                ' preserve design-time defaults on empty fields.
                If Not String.IsNullOrEmpty(value.Url) Then
                    TxtUrl.Text = value.Url
                End If
                If Not String.IsNullOrEmpty(value.Height) Then
                    TxtHeight.Text = value.Height
                End If
                If Not String.IsNullOrEmpty(value.Width) Then
                    TxtWidth.Text = value.Width
                End If
                If Not String.IsNullOrEmpty(value.CssStyle) Then
                    TxtCssStyle.Text = value.CssStyle
                End If
            End Set
        End Property


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


        ''' <summary>
        ''' Handles the OnRightButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TwoButtonPanel_OnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            'cancel button

            Close()
        End Sub

        ''' <summary>
        ''' Handles the OnLeftButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TwoButtonPanel_OnLeftButtonClicked(sender As Object, e As RoutedEventArgs)
            'OK button

            If String.IsNullOrEmpty(TxtUrl.Text) Then
                MessageBox.Show("The YouTube URL cannot be empty.")
                TxtUrl.Focus()
                Return
            End If

            Dim theUrl As String = TxtUrl.Text.Trim()
            If theUrl = String.Empty Then
                MessageBox.Show("The YouTube URL cannot be empty.")
                TxtUrl.Focus()
                Return
            End If

            If Not theUrl.ToLower().Contains("youtube.com") Then
                MessageBox.Show("The URL you provided does not contain the YouTube Domain name", "Invalid URL")
                Return
            End If

            Me.DialogResult = True
        End Sub

        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub
    End Class
End Namespace
