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

        // HeaderStyleContent writes a <style> block into the document head, so the content
        // is styled by real CSS rather than inline formatting - exactly how you would brand
        // documents your end users author.
        Editor.HeaderStyleContentElementID = DocumentStyleElementId;
        Editor.HeaderStyleContent = DocumentStyle;

        Editor.BodyHtml = StarterDocumentHtml;

        PopulateLanguageComboBox();
    }

    private const string DocumentStyleElementId = "full_editor_demo_style";

    /// <summary>
    /// The document stylesheet. It sets no absolute font size: the text keeps the editor's
    /// own default size and everything else is expressed in em, so the document reads at the
    /// same scale as the rest of the application. The table is deliberately not width:100% -
    /// a percentage width is resolved against the viewport, which leaves the document a few
    /// pixels wider than the visible area once the vertical scrollbar appears.
    /// </summary>
    private const string DocumentStyle = """
        body { font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif; line-height: 1.6; color: #1f2933; margin: 24px 30px; }
        h1 { font-size: 2em; font-weight: 600; color: #0f2540; margin: 0 0 4px 0; }
        h2 { font-size: 1.3em; font-weight: 600; color: #0f2540; margin: 26px 0 10px 0; padding-bottom: 6px; border-bottom: 1px solid #e2e8f0; }
        p { margin: 0 0 12px 0; }
        a { color: #1a6fd4; text-decoration: none; border-bottom: 1px solid #bcd6f5; }
        ul, ol { margin: 0 0 14px 0; padding-left: 22px; }
        li { margin-bottom: 7px; }
        .eyebrow { font-size: 0.75em; font-weight: 600; color: #1a6fd4; letter-spacing: 1.2px; margin: 0 0 6px 0; }
        .lede { font-size: 1.1em; color: #52606d; margin: 0 0 22px 0; }
        .callout { background-color: #f2f7fd; border-left: 4px solid #1a6fd4; padding: 14px 18px; margin: 0 0 22px 0; }
        .callout p { margin: 0; }
        table { border-collapse: collapse; margin: 4px 0 18px 0; }
        th { background-color: #0f2540; color: #ffffff; text-align: left; font-weight: 600; padding: 10px 16px; }
        td { border-bottom: 1px solid #e2e8f0; padding: 10px 16px; }
        tr.alt td { background-color: #f7f9fc; }
        .badge { display: inline-block; padding: 2px 10px; font-size: 0.85em; font-weight: 600; border-radius: 10px; }
        .badge-live { background-color: #dff3e4; color: #1b6b3a; }
        .badge-review { background-color: #fdf0d5; color: #8a5a00; }
        .badge-planned { background-color: #eceff3; color: #52606d; }
        blockquote { margin: 0 0 14px 0; padding: 2px 0 2px 18px; border-left: 3px solid #cfd8e3; color: #52606d; font-style: italic; }
        .muted { color: #7b8794; font-size: 0.85em; }
        """;

    private const string StarterDocumentHtml = """
        <p class="eyebrow">Regional expansion</p>
        <h1>Northwind Traders rollout brief</h1>
        <p class="lede">The full editor experience: the complete default toolbar, rich text formatting,
        tables, images, and inline spell checking, all styled by the document stylesheet.</p>

        <div class="callout">
        <p><strong>Steering review:</strong> 4 September &nbsp;&middot;&nbsp; <strong>Go-live window:</strong>
        22 September to 6 October &nbsp;&middot;&nbsp; <strong>Owner:</strong> Alicia Kwan</p>
        </div>

        <h2>Rollout milestones</h2>
        <ol>
        <li>Vendor contracts signed</li>
        <li>Regional pricing approved</li>
        <li>Public launch</li>
        </ol>

        <h2>Coverage by market</h2>
        <table>
        <tr><th>Market</th><th>Status</th><th>Owner</th><th>Go-live</th></tr>
        <tr><td>Northeast</td><td><span class="badge badge-live">Live</span></td><td>Alicia Kwan</td><td>Complete</td></tr>
        <tr class="alt"><td>Midwest</td><td><span class="badge badge-review">In review</span></td><td>Diego Ferreira</td><td>22 September</td></tr>
        <tr><td>West coast</td><td><span class="badge badge-planned">Planned</span></td><td>Priya Natarajan</td><td>6 October</td></tr>
        <tr class="alt"><td>Canada</td><td><span class="badge badge-planned">Planned</span></td><td>Priya Natarajan</td><td>Q4</td></tr>
        </table>

        <h2>Commercial terms</h2>
        <blockquote>Fees are fixed for the initial three markets. Any market added after the September
        steering review is quoted separately against the same rate card.</blockquote>
        <p>Type a misspelled word (for example <i>teh</i>) and press space to see the curly underline
        from the built-in spell checker, or switch the dropdown above to check a different language.
        Full terms are documented on the <a href="https://www.spicelogic.com">partner portal</a>.</p>
        <p class="muted">Prepared by the delivery team &nbsp;&middot;&nbsp; Revision 4</p>
        """;

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
