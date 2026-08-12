# 10 - Full editor demo

The larger tour customers get as their quick start: the complete default toolbar, rich starting
content, a live character count in the status bar, and inline spell checking with a language
switcher. `01-Quickstart-CS` stays minimal; this sample shows more of what the editor does out of
the box.

**Key API used**

- `BodyHtml` set once at startup; read back through `HtmlChanged`.
- `SpellCheckOptions.SpellCheckLanguage`, `SpellCheckOptions.FireInlineSpellCheckingOnKeyStroke`,
  and `SpellCheckOptions.CurlyUnderlineImageFilePath`.

**Run it**

```
dotnet run --project 10-FullEditorDemo-CS
```

A VB.NET version of this same sample sits alongside it in `10-FullEditorDemo-VB`.
