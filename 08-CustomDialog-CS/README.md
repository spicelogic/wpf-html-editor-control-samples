# 08 - Custom dialog

Replaces every built-in editor dialog (image, hyperlink, table, table cell, symbol, search,
spell checker, style builder, and YouTube video insert) with your own WPF windows, so the
dialogs can match your application's look and feel instead of the editor's default styling.
The `Dialogs` folder contains full, working source for each replacement dialog, ready to
customize further or use as a starting point for your own designs.

**Key API used**

- `editor.Dialog.CreateImageDialogMethod`, `CreateHyperlinkDialogMethod`,
  `CreateTableDialogMethod`, `CreateTableCellDialogMethod`, `CreateSymbolDialogMethod`,
  `CreateSearchDialogMethod`, `CreateSpellcheckerDialogMethod`, `CreateStyleBuilderDialogMethod`,
  `CreateColorPickerDialogMethod`, and `CreateYouTubeVideoInsertDialogMethod` - factory delegates
  that hand the editor your dialog instance instead of its built-in one.
- The `IImageDialog`, `IHyperlinkDialog`, `ITableDialog`, `ITableCellDialog`, `ISymbolDialog`,
  `ISearchDialog`, `ISpellCheckerDialog`, `IStyleBuilderDialog`, `IColorPickerDialog`, and
  `IYouTubeVideoInsertDialog` interfaces each custom dialog implements to satisfy the contract
  the editor expects.


A VB.NET version of this same sample sits alongside in `08-CustomDialog-VB`.

## Building this with an AI assistant?

> [!TIP]
> Point your assistant at our MCP server and it can read the real API for this
> control instead of guessing at member names:
> `https://mcp.spicelogic.com/html-editor/wpf`
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
