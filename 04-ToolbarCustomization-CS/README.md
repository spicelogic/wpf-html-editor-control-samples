# 04 - Toolbar customization

Shows two ways to customize the toolbars. At startup: hide a built-in button, swap its icon,
override its click handler, and change another button's tooltip, all on the default toolbar,
plus a plain WPF `Button` appended to the second strip without touching a single factory button.
On demand ("Build a fully custom toolbar"): hide both built-in toolbars entirely and hand
assemble your own from just the buttons you want, in your own order, plus a new icon-based
`ToolbarCustomButton`.

**Key API used**

- `editor.ToolbarItemOverrider.ToolbarItems.<Name>` to reach a specific built-in button
  (`.Visibility`, `.ToolTip`, or its icon via the visual tree).
- `editor.ToolbarItemOverrider.SaveButtonClicked` (and the matching event for every other
  built-in button) to override a button's default behavior.
- `editor.Toolbar1` / `editor.Toolbar2` (`.Visibility`, `.ToolBar.Items`) to hide the built-in
  toolbars and move their buttons into your own `ToolBar`.
- `editor.Toolbar2Items.Add(...)` to append a custom control without touching the built-ins.
- `editor.Content.InsertHtml(html, keepSelected: false)` to insert HTML from a custom button.

**Run it**

```
dotnet run --project 04-ToolbarCustomization-CS
```

A VB.NET version of this same sample sits alongside in `04-ToolbarCustomization-VB`.
