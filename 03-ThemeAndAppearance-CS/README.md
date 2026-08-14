# 03 - Theme and appearance

Buttons that change the editor's border color and width to match an app theme, set a default
document font, and switch `EditorMode` between the design (WYSIWYG) and source (HTML) views.

**Key API used**

- `EditorBorderColor`, `EditorBorderWidth`
- `DefaultFontFamily`, `DefaultFontSizeInPt`
- `EditorMode` (`EditorModes.WysiwygDesign` / `EditorModes.HtmlEdit`)

**Run it**

```
dotnet run --project 03-ThemeAndAppearance-CS
```

A VB.NET version of this sample sits alongside in `03-ThemeAndAppearance-VB`.

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
