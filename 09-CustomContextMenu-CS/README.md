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

**Run it**

```
dotnet run --project 09-CustomContextMenu-CS
```

A VB.NET version of this same sample sits alongside in `09-CustomContextMenu-VB`.
