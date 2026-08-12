Imports System.Windows
Imports System.Windows.Media
Imports SpiceLogic.HtmlEditor.Abstractions

Namespace Global.ThemeAndAppearance

    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            Editor.BodyHtml = "<p>Use the buttons below to change the editor's border, default font " &
                               "and startup mode at runtime.</p>"
        End Sub

        Private Sub BrandBorderButton_Click(sender As Object, e As RoutedEventArgs)
            ' Matches your application's chrome or brand color, rather than the editor's default.
            Editor.EditorBorderColor = Color.FromRgb(&H0A, &H37, &H64)
            Editor.EditorBorderWidth = New Thickness(2)
        End Sub

        Private Sub DocumentFontButton_Click(sender As Object, e As RoutedEventArgs)
            ' Sets the default font for the document body, persisted in the body element's style.
            Editor.DefaultFontFamily = "Georgia"
            Editor.DefaultFontSizeInPt = "12pt"
        End Sub

        Private Sub DesignModeButton_Click(sender As Object, e As RoutedEventArgs)
            Editor.EditorMode = EditorModes.WysiwygDesign
        End Sub

        Private Sub SourceModeButton_Click(sender As Object, e As RoutedEventArgs)
            Editor.EditorMode = EditorModes.HtmlEdit
        End Sub

    End Class

End Namespace
