using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
using SpiceLogic.HtmlEditor.Resources.Localization;

namespace Localization;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // Populate the editor language dropdown from the full EditorLanguage enum.
        foreach (EditorLanguage lang in Enum.GetValues(typeof(EditorLanguage)))
        {
            LanguageCombo.Items.Add(lang);
        }
        LanguageCombo.SelectedItem = EditorLanguage.EnglishUs;

        // Populate the spell-check language dropdown. SpellCheckLanguage has a
        // SameAsEditorLanguage member (the default) - when selected, the dictionary
        // tracks the editor language; pick a specific language to override it
        // independently and watch inline spell-check switch dictionaries.
        foreach (SpellCheckLanguage lang in Enum.GetValues(typeof(SpellCheckLanguage)))
        {
            SpellCheckCombo.Items.Add(lang);
        }
        SpellCheckCombo.SelectedItem = SpellCheckLanguage.SameAsEditorLanguage;

        // No license key set, so the editor runs in trial mode. See the licensing docs linked
        // in the README.
        Editor.BodyHtml = "<h2>Localization</h2>"
                        + "<p>Change the language dropdown above to see every toolbar tooltip, "
                        + "context menu item, and dialog string update.</p>"
                        + "<p>Right-click to test context menu localization.</p>"
                        + "<p>Click toolbar buttons (hyperlink, image, table, and so on) to test "
                        + "dialog localization.</p>"
                        + "<p>Inline spell check is enabled - type a misspelled word followed by a "
                        + "space (for example <i>helllo</i>) to see the curly underline. The "
                        + "<b>spell check language</b> dropdown above defaults to "
                        + "<i>SameAsEditorLanguage</i> (the dictionary tracks the editor language); "
                        + "pick a specific language there to override it independently.</p>"
                        + "<hr/>"
                        + "<p><b>JSON override demo:</b> select <i>Polish</i> from the language "
                        + "dropdown, then check <i>Enable JSON override</i>. Toolbar tooltips like "
                        + "bold, italic, and underline change to show a <code>[CUSTOM]</code> "
                        + "prefix, proving that the JSON file overrides the embedded resource "
                        + "strings.</p>";

        // Inline spell-check: SpellCheckLanguage defaults to SameAsEditorLanguage, so picking
        // Polish in the language dropdown also switches the dictionary to pl-PL.
        Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "underline.gif");
        Editor.SpellCheckOptions.FireInlineSpellCheckingOnKeyStroke = true;
    }

    private void LanguageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageCombo.SelectedItem is EditorLanguage selectedLang)
        {
            Editor.Language = selectedLang;
            CurrentLanguageLabel.Text = $"Current: {selectedLang}";
        }
    }

    private void SpellCheckCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Setting SpellCheckLanguage flags the engine to reload the dictionary on the next
        // inline spell-check pass.
        if (SpellCheckCombo.SelectedItem is SpellCheckLanguage selectedLang)
        {
            Editor.SpellCheckOptions.SpellCheckLanguage = selectedLang;
        }
    }

    private void JsonOverrideCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (JsonOverrideCheckBox.IsChecked == true)
        {
            var overrideDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpiceLogic.HtmlEditor.Localization");
            if (!Directory.Exists(overrideDir))
            {
                MessageBox.Show(
                    $"Override directory not found:\n{overrideDir}\n\nMake sure the JSON file is set to copy to the output directory.",
                    "JSON override", MessageBoxButton.OK, MessageBoxImage.Warning);
                JsonOverrideCheckBox.IsChecked = false;
                return;
            }

            LocalizationManager.SetJsonOverrideDirectory(overrideDir);
        }
        else
        {
            // Clear overrides by setting the directory back to null.
            LocalizationManager.SetJsonOverrideDirectory(null);
        }

        // Force the editor to re-apply localization. A WPF dependency property does not fire its
        // changed callback when the value is unchanged, so briefly switch to a different
        // language and back to guarantee a refresh.
        if (LanguageCombo.SelectedItem is EditorLanguage selectedLang)
        {
            var tempLang = selectedLang == EditorLanguage.EnglishUs
                ? EditorLanguage.EnglishGb
                : EditorLanguage.EnglishUs;
            Editor.Language = tempLang;
            Editor.Language = selectedLang;
        }
    }
}
