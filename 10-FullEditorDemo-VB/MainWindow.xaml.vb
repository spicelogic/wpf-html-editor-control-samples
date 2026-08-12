Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck
Imports SpiceLogic.HtmlEditor.WPF

Namespace FullEditorDemoVB

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
                AppDomain.CurrentDomain.BaseDirectory & "curly_underline_for_spell_checker.gif"

            Editor.BodyHtml =
                "<h2>Welcome to the WPF HTML editor control</h2>" &
                "<p>This is the full editor experience: the complete default toolbar, rich text " &
                "formatting, tables, images, and inline spell checking, all in one window.</p>" &
                "<ul>" &
                "<li>Use the toolbar to try headings, lists, tables, and hyperlinks.</li>" &
                "<li>Type a misspelled word (for example <i>teh</i>) and press space to see the " &
                "curly underline from the built-in spell checker.</li>" &
                "<li>Switch the dropdown above to spell check in a different language.</li>" &
                "</ul>" &
                "<p>&nbsp;</p>"

            PopulateLanguageComboBox()
        End Sub

        Private Sub PopulateLanguageComboBox()
            Dim languages = [Enum].GetValues(GetType(SpellCheckLanguage)) _
                .Cast(Of SpellCheckLanguage)() _
                .Select(Function(lang) New LanguageItem(lang)) _
                .ToList()

            LanguageComboBox.ItemsSource = languages
            LanguageComboBox.DisplayMemberPath = NameOf(LanguageItem.DisplayName)
            LanguageComboBox.SelectedValuePath = NameOf(LanguageItem.Language)
            LanguageComboBox.SelectedValue = Editor.SpellCheckOptions.SpellCheckLanguage
        End Sub

        Private Sub LanguageComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            If TypeOf LanguageComboBox.SelectedValue Is SpellCheckLanguage Then
                Dim selected As SpellCheckLanguage = CType(LanguageComboBox.SelectedValue, SpellCheckLanguage)
                Editor.SpellCheckOptions.SpellCheckLanguage = selected
            End If
        End Sub

        Private Sub Editor_HtmlChanged(sender As Object, e As EventArgs)
            ' BodyHtml reflects the editor's current content whenever it is read from inside
            ' a HtmlChanged handler, so no extra step is needed here.
            Dim currentHtml As String = Editor.BodyHtml
            Dim plainText As String = Regex.Replace(currentHtml, "<.*?>", String.Empty)
            CharacterCountText.Text = $"{plainText.Length} characters"
        End Sub

        Private NotInheritable Class LanguageItem

            Public Sub New(language As SpellCheckLanguage)
                Me.Language = language
                Me.DisplayName = If(language.GetType() _
                    .GetField(language.ToString()) _
                    ?.GetCustomAttribute(Of DescriptionAttribute)() _
                    ?.Description, language.ToString())
            End Sub

            Public ReadOnly Property Language As SpellCheckLanguage

            Public ReadOnly Property DisplayName As String

        End Class

    End Class

End Namespace
