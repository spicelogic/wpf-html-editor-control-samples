# 01 - Quickstart

The smallest possible integration: the editor filling the whole window, starting content set in
code-behind, and a status bar character count driven by the `HtmlChanged` event.

**Key API used**

- `<se:WpfHtmlEditor>` in XAML, filling its container.
- `BodyHtml` set once at startup.
- `HtmlChanged` event, read back through `BodyHtml` inside the handler.

**Run it**

```
dotnet run --project 01-Quickstart-CS
```

A VB.NET version of this same sample sits alongside in `01-Quickstart-VB`.

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
