using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;
using SpiceLogic.HtmlEditor.WPF;

namespace FullEditorDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
            AppDomain.CurrentDomain.BaseDirectory + "curly_underline_for_spell_checker.gif";

        Editor.BodyHtml =
            "<h2>Welcome to the WPF HTML editor control</h2>" +
            "<p>This is the full editor experience: the complete default toolbar, rich text " +
            "formatting, tables, images, and inline spell checking, all in one window.</p>" +
            "<ul>" +
            "<li>Use the toolbar to try headings, lists, tables, and hyperlinks.</li>" +
            "<li>Type a misspelled word (for example <i>teh</i>) and press space to see the " +
            "curly underline from the built-in spell checker.</li>" +
            "<li>Switch the dropdown above to spell check in a different language.</li>" +
            "</ul>" +
            "<p>&nbsp;</p>";

        PopulateLanguageComboBox();
    }

    private void PopulateLanguageComboBox()
    {
        var languages = Enum.GetValues(typeof(SpellCheckLanguage))
            .Cast<SpellCheckLanguage>()
            .Select(lang => new LanguageItem(lang))
            .ToList();

        LanguageComboBox.ItemsSource = languages;
        LanguageComboBox.DisplayMemberPath = nameof(LanguageItem.DisplayName);
        LanguageComboBox.SelectedValuePath = nameof(LanguageItem.Language);
        LanguageComboBox.SelectedValue = Editor.SpellCheckOptions.SpellCheckLanguage;
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedValue is SpellCheckLanguage selected)
        {
            Editor.SpellCheckOptions.SpellCheckLanguage = selected;
        }
    }

    private void Editor_HtmlChanged(object sender, EventArgs e)
    {
        // BodyHtml reflects the editor's current content whenever it is read from inside
        // a HtmlChanged handler, so no extra step is needed here.
        string currentHtml = Editor.BodyHtml;
        string plainText = Regex.Replace(currentHtml, "<.*?>", string.Empty);
        CharacterCountText.Text = $"{plainText.Length} characters";
    }

    private sealed class LanguageItem
    {
        public LanguageItem(SpellCheckLanguage language)
        {
            Language = language;
            DisplayName = language.GetType()
                .GetField(language.ToString())
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? language.ToString();
        }

        public SpellCheckLanguage Language { get; }
        public string DisplayName { get; }
    }
}
