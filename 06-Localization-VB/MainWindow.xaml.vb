Imports System
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck
Imports SpiceLogic.HtmlEditor.Resources.Localization

Namespace Global.LocalizationVB

Partial Public Class MainWindow

    Public Sub New()
        InitializeComponent()

        ' Populate the editor language dropdown from the full EditorLanguage enum.
        For Each lang As EditorLanguage In [Enum].GetValues(GetType(EditorLanguage))
            LanguageCombo.Items.Add(lang)
        Next
        LanguageCombo.SelectedItem = EditorLanguage.EnglishUs

        ' Populate the spell-check language dropdown. SpellCheckLanguage has a
        ' SameAsEditorLanguage member (the default) - when selected, the dictionary
        ' tracks the editor language; pick a specific language to override it
        ' independently and watch inline spell-check switch dictionaries.
        For Each lang As SpellCheckLanguage In [Enum].GetValues(GetType(SpellCheckLanguage))
            SpellCheckCombo.Items.Add(lang)
        Next
        SpellCheckCombo.SelectedItem = SpellCheckLanguage.SameAsEditorLanguage

        ' No license key set, so the editor runs in trial mode. See the licensing docs linked
        ' in the README.
        Editor.BodyHtml = "<h2>Localization</h2>" _
                        & "<p>Change the language dropdown above to see every toolbar tooltip, " _
                        & "context menu item, and dialog string update.</p>" _
                        & "<p>Right-click to test context menu localization.</p>" _
                        & "<p>Click toolbar buttons (hyperlink, image, table, and so on) to test " _
                        & "dialog localization.</p>" _
                        & "<p>Inline spell check is enabled - type a misspelled word followed by a " _
                        & "space (for example <i>helllo</i>) to see the curly underline. The " _
                        & "<b>spell check language</b> dropdown above defaults to " _
                        & "<i>SameAsEditorLanguage</i> (the dictionary tracks the editor language); " _
                        & "pick a specific language there to override it independently.</p>" _
                        & "<hr/>" _
                        & "<p><b>JSON override demo:</b> select <i>Polish</i> from the language " _
                        & "dropdown, then check <i>Enable JSON override</i>. Toolbar tooltips like " _
                        & "bold, italic, and underline change to show a <code>[CUSTOM]</code> " _
                        & "prefix, proving that the JSON file overrides the embedded resource " _
                        & "strings.</p>"

        ' Inline spell-check: SpellCheckLanguage defaults to SameAsEditorLanguage, so picking
        ' Polish in the language dropdown also switches the dictionary to pl-PL.
        Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "underline.gif")
        Editor.SpellCheckOptions.FireInlineSpellCheckingOnKeyStroke = True
    End Sub

    Private Sub LanguageCombo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If TypeOf LanguageCombo.SelectedItem Is EditorLanguage Then
            Dim selectedLang = CType(LanguageCombo.SelectedItem, EditorLanguage)
            Editor.Language = selectedLang
            CurrentLanguageLabel.Text = $"Current: {selectedLang}"
        End If
    End Sub

    Private Sub SpellCheckCombo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        ' Setting SpellCheckLanguage flags the engine to reload the dictionary on the next
        ' inline spell-check pass.
        If TypeOf SpellCheckCombo.SelectedItem Is SpellCheckLanguage Then
            Dim selectedLang = CType(SpellCheckCombo.SelectedItem, SpellCheckLanguage)
            Editor.SpellCheckOptions.SpellCheckLanguage = selectedLang
        End If
    End Sub

    Private Sub JsonOverrideCheckBox_Changed(sender As Object, e As RoutedEventArgs)
        If JsonOverrideCheckBox.IsChecked = True Then
            Dim overrideDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpiceLogic.HtmlEditor.Localization")
            If Not Directory.Exists(overrideDir) Then
                MessageBox.Show(
                    $"Override directory not found:{vbLf}{overrideDir}{vbLf}{vbLf}Make sure the JSON file is set to copy to the output directory.",
                    "JSON override", MessageBoxButton.OK, MessageBoxImage.Warning)
                JsonOverrideCheckBox.IsChecked = False
                Return
            End If

            LocalizationManager.SetJsonOverrideDirectory(overrideDir)
        Else
            ' Clear overrides by setting the directory back to null.
            LocalizationManager.SetJsonOverrideDirectory(Nothing)
        End If

        ' Force the editor to re-apply localization. A WPF dependency property does not fire its
        ' changed callback when the value is unchanged, so briefly switch to a different
        ' language and back to guarantee a refresh.
        If TypeOf LanguageCombo.SelectedItem Is EditorLanguage Then
            Dim selectedLang = CType(LanguageCombo.SelectedItem, EditorLanguage)
            Dim tempLang = If(selectedLang = EditorLanguage.EnglishUs,
                EditorLanguage.EnglishGb,
                EditorLanguage.EnglishUs)
            Editor.Language = tempLang
            Editor.Language = selectedLang
        End If
    End Sub

End Class

End Namespace
