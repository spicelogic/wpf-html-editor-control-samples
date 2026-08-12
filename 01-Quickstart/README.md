# 01 - Quickstart

The smallest possible integration: the editor filling the whole window, starting content set in
code-behind, and a status bar character count driven by the `HtmlChanged` event.

**Key API used**

- `<se:WpfHtmlEditor>` in XAML, filling its container.
- `BodyHtml` set once at startup.
- `HtmlChanged` event, read back through `BodyHtml` inside the handler.

**Run it**

```
dotnet run --project 01-Quickstart
```

A VB.NET version of this same sample sits alongside in `01-Quickstart-VB`.
