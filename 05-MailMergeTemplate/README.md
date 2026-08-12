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

**Run it**

```
dotnet run --project 05-MailMergeTemplate
```
