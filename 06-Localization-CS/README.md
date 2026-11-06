# 06 - Localization

Switching the editor's UI language at runtime, picking an independent spell-check dictionary
language, and overriding individual localized strings with a JSON file, without rebuilding the
app.

**Key API used**

- `Editor.Language` (the `EditorLanguage` enum) - switches every toolbar tooltip, context menu
  item, and dialog string to the selected language.
- `Editor.SpellCheckOptions.SpellCheckLanguage` (the `SpellCheckLanguage` enum) - defaults to
  `SameAsEditorLanguage`, which keeps the spell-check dictionary in sync with `Language`; pick a
  specific language to run spell check independently of the UI language.
- `Editor.SpellCheckOptions.FireInlineSpellCheckingOnKeyStroke` - turns on the inline spell check
  shown in this sample; misspelled words get the editor's built-in wavy underline, with no image
  to deploy.
- `LocalizationManager.SetJsonOverrideDirectory(path)` - points the editor at a folder of
  `EditorStrings.<culture>.json` files whose entries override the built-in localized strings for
  that culture. This sample ships `SpiceLogic.HtmlEditor.Localization\EditorStrings.pl-PL.json`,
  overriding a handful of Polish toolbar tooltips with a `[Custom]` prefix so the effect is
  obvious.


Pick a language from the dropdown to see the toolbar, context menu, and dialogs relocalize. Pick
*Polish*, then check *Enable JSON override*, to see the JSON file's overrides take effect over the
built-in strings.

A VB.NET version of this same sample sits alongside it in
[`06-Localization-VB`](../06-Localization-VB).

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```

## A custom interface language

Select `Custom` and load `SpiceLogic.HtmlEditor.Localization` to use
`EditorStrings.custom.json`. WinForms and WPF expose the JSON override checkbox;
Avalonia loads the folder at startup and provides a Custom interface button.
The supplied file demonstrates two Turkish labels. Remaining labels fall back
to neutral English. For a complete language, ask your coding assistant to translate
every value in `_OVERRIDABLE-KEYS.reference.json`, preserving the keys and format
placeholders, and save the result as `EditorStrings.custom.json`. Review the
translation and test the actual dialogs before shipping. Custom does not supply
a spelling dictionary or infer a .NET culture; configure spelling separately.
