using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using SpiceLogic.HtmlEditor.Abstractions.Entities.MailMerge;

namespace MailMergeTemplate;

public partial class MainWindow : Window
{
    // A sample record used to substitute merge tokens for the preview. In a real
    // application this would come from your database, CRM, or invoicing system.
    private static readonly Dictionary<string, string> SampleRecord = new()
    {
        ["{{FirstName}}"] = "Jordan",
        ["{{LastName}}"] = "Avery",
        ["{{Company}}"] = "Example Corp",
        ["{{InvoiceNumber}}"] = "INV-1042",
        ["{{DueDate}}"] = "September 1, 2026"
    };

    // An inserted field is a protected span, <span class="sl-placeholder" ...>{{FirstName}}</span>.
    // Replace the whole span rather than only its token, so the merged text does not keep the
    // field's blue editing style. Tokens typed as plain text are replaced afterwards.
    private static readonly Regex PlaceholderSpan = new(
        @"<span\b[^>]*\bclass\s*=\s*[""']?sl-placeholder[""']?[^>]*>(.*?)</span>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline);

    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        var fields = new List<PlaceholderField>
        {
            new("First name", "{{FirstName}}"),
            new("Last name", "{{LastName}}"),
            new("Company", "{{Company}}"),
            new("Invoice number", "{{InvoiceNumber}}"),
            new("Due date", "{{DueDate}}")
        };

        Editor.Content.MailMerge.PlaceholderFields = fields;
        Editor.ShowPlaceholderToolbar = true;

        Editor.BodyHtml =
            "<p>Dear {{FirstName}} {{LastName}},</p>" +
            "<p>This is a reminder that invoice {{InvoiceNumber}} for {{Company}} is due on {{DueDate}}.</p>";
    }

    private void PreviewButton_Click(object sender, RoutedEventArgs e)
    {
        // The editor hosts a live document, so pull the latest edits into BodyHtml before
        // reading it - otherwise the most recent typing is not there yet.
        Editor.UpdateBindings();

        string merged = PlaceholderSpan.Replace(Editor.BodyHtml, match =>
            SampleRecord.TryGetValue(WebUtility.HtmlDecode(match.Groups[1].Value), out string? value)
                ? WebUtility.HtmlEncode(value)
                : match.Groups[1].Value);
        foreach (KeyValuePair<string, string> pair in SampleRecord)
        {
            merged = merged.Replace(pair.Key, WebUtility.HtmlEncode(pair.Value));
        }

        PreviewEditor.BodyHtml = merged;
    }
}
