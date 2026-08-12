# 04 - Toolbar customization

Adds a custom "Insert signature" button to the editor's second toolbar strip from code-behind,
without touching the built-in factory buttons. Clicking it inserts a fixed HTML block at the
caret.

**Key API used**

- `editor.Toolbar2Items.Add(...)`
- `editor.Content.InsertHtml(html, keepSelected: false)`

**Run it**

```
dotnet run --project 04-ToolbarCustomization
```
