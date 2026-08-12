Imports System
Imports System.Text.RegularExpressions
Imports System.Windows

Namespace Global.Quickstart

    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            Editor.BodyHtml = "<p>Start typing here. This is the SpiceLogic WPF HTML editor control, " &
                               "dropped onto a window with nothing more than the NuGet package reference.</p>"
        End Sub

        Private Sub Editor_HtmlChanged(sender As Object, e As EventArgs)
            ' BodyHtml reflects the editor's current content whenever it is read from inside
            ' a HtmlChanged handler, so no extra step is needed here. For reading BodyHtml from
            ' outside an editor event, for example from a data-bound view model, see the
            ' 02-MvvmDataBinding sample and its UpdateBindings() call.
            Dim currentHtml As String = Editor.BodyHtml
            Dim plainText As String = Regex.Replace(currentHtml, "<.*?>", String.Empty)
            CharacterCountText.Text = $"{plainText.Length} characters"
        End Sub

    End Class

End Namespace
