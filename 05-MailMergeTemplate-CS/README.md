# 05 - Mail merge template

Registers five `PlaceholderField`s (first name, last name, company, invoice number, due date) and
turns on the built-in placeholder toolbar so end users can drop merge tokens into a template
without writing any HTML themselves. A "Preview merged" button substitutes the tokens from an
in-memory sample record and renders the result in a second, read-only editor.

**Key API used**

- `editor.Content.MailMerge.PlaceholderFields` and `new PlaceholderField(displayName, token)`
- `editor.ShowPlaceholderToolbar = true`
- `editor.UpdateBindings()` before reading `BodyHtml` for the preview
- `EditorMode="ReadOnlyPreview"` on the second editor


A VB.NET version of this sample sits alongside in `05-MailMergeTemplate-VB`.

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
