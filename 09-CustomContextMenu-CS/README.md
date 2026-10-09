# 09 - Custom context menu

Replaces the editor's built-in right-click menu with a fully custom WPF `ContextMenu`, and keeps
its items in sync with the editor's live state: Cut/Copy/Delete enable only when there is
something to act on, table-specific commands appear only inside a table, and the image/link
properties items check themselves when the cursor sits on that content.

**Key API used**

- `EditorContextMenuStrip` set to a custom `ContextMenu` resource.
- `ContextMenuShowing` event, used to toggle `IsEnabled`, `Visibility`, and `IsChecked` on menu
  items right before the menu opens.
- `StateQuery` (`CanCut`, `CanCopy`, `CanDelete`, `CanPaste`, `IsTable`, `IsTableCell`, `IsImage`,
  `IsHyperLink`, `IsYouTubeVideo`, `CanMergeTableCells`) to read editor state.
- `ToolbarItemOverrider` and `TableAuthoringService` to reuse the editor's own dialogs and table
  commands from custom menu item handlers.


A VB.NET version of this same sample sits alongside in `09-CustomContextMenu-VB`.

## Keep the default menu

Use `ContextMenuItems` to change only selected built-in commands. Configure these properties in the editor's Loaded or TemplateApplied event. A custom template that omits a menu or command returns null for the corresponding reference.

```csharp
editor.ContextMenuItems.ViewSource.Visible = false;
editor.ContextMenuItems.Paste.Enabled = false;
```

```vb
editor.ContextMenuItems.ViewSource.Visible = False
editor.ContextMenuItems.Paste.Enabled = False
```

Use `Text` and `Icon` for persistent caption and icon overrides. Use each reference's `Item` and `DefaultContextMenu.Items` to reorder existing items or add native commands. Built-in command handlers remain attached. Setting `Visible` or `Enabled` back to true restores selection-based behavior; it does not force unavailable commands enabled. `ContextMenuShowing` runs before display.

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
