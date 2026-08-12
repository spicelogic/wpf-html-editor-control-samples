Imports System
Imports System.Windows
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck

Namespace Global.CustomSpellChecker

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MainWindow_OnLoaded(sender As Object, e As RoutedEventArgs)
            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
                AppDomain.CurrentDomain.BaseDirectory + "underline.gif"

            Editor.BodyHtml = "
                <h3>Try the spell checker</h3>
                <p>This paragraph has a <strong>misspellled</strong> word that the built-in engine will catch.
                   Switch to the <em>Custom Engine</em> and it will flag words like
                   <strong>apple</strong>, <strong>amazing</strong>, and <strong>adventure</strong> instead,
                   because the demo custom engine treats every word starting with ""a"" as misspelled.</p>
                <p>Type some text below to see real-time inline spell checking in action:</p>
                <p>&nbsp;</p>"
        End Sub

        Private Sub BuiltInRadio_OnCheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            If BuiltInRadio.IsChecked = True Then
                Editor.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.OpenOffice
            Else
                Editor.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.Custom
                Editor.SpellCheckOptions.CustomSpellCheckerEngine = New CustomSpellCheckerEngine()
            End If
        End Sub

    End Class

End Namespace
