<div align="center">

# WPF HTML editor control

**Runnable C# and VB.NET sample projects for the SpiceLogic WPF HTML Editor**

[![NuGet](https://img.shields.io/nuget/v/SpiceLogic.HtmlEditor.WPF?label=NuGet&color=004880)](https://www.nuget.org/packages/SpiceLogic.HtmlEditor.WPF/)
[![Downloads](https://img.shields.io/nuget/dt/SpiceLogic.HtmlEditor.WPF?label=downloads&color=success)](https://www.nuget.org/packages/SpiceLogic.HtmlEditor.WPF/)
[![Samples license](https://img.shields.io/badge/samples-MIT-blue)](LICENSE)
[![Targets](https://img.shields.io/badge/.NET-Framework%204.5%20to%20.NET%2010-512BD4)](#targeting-a-different-net-version)
[![Languages](https://img.shields.io/badge/languages-C%23%20%2B%20VB.NET-brightgreen)](#samples)

[Product page](https://www.spicelogic.com/Products/WPF-HTML-Editor-Control-17)&nbsp; &middot;&nbsp;
[Documentation](https://www.spicelogic.com/docs/wpfHtmlEditor)&nbsp; &middot;&nbsp;
[NuGet](https://www.nuget.org/packages/SpiceLogic.HtmlEditor.WPF/)&nbsp; &middot;&nbsp;
[WinForms version](https://github.com/spicelogic/winforms-html-editor-control-samples)

</div>

> [!TIP]
> ### Coding with an AI assistant? Connect our MCP server first.
>
> ```text
> https://mcp.spicelogic.com/html-editor/wpf
> ```
>
> Claude Code, Cursor, GitHub Copilot, Windsurf, and any other MCP-capable client can read the
> real documentation for this control instead of guessing at member names. The server answers
> with verified API signatures, working samples, the current NuGet package id and version, and
> the exact licensing code, so the code it writes compiles the first time.
>
> <details>
> <summary><b>Add it to your assistant</b></summary>
>
> Claude Code, one line:
>
> ```bash
> claude mcp add --transport http spicelogic-wpf https://mcp.spicelogic.com/html-editor/wpf
> ```
>
> Any other client, in its MCP configuration file:
>
> ```json
> {
>   "mcpServers": {
>     "spicelogic-wpf": {
>       "type": "http",
>       "url": "https://mcp.spicelogic.com/html-editor/wpf"
>     }
>   }
> }
> ```
>
> Tools exposed: `get_quickstart`, `get_api`, `get_sample`, `search_docs`, `get_doc_page`,
> `get_licensing`. Full write-up:
> [MCP server for the WPF HTML editor](https://www.spicelogic.com/docs/wpfHtmlEditor/Programming/mcp-server-502).
>
> </details>

---

A **WPF HTML editor control** for building a rich text editing experience inside a .NET desktop
application. If you are looking for a **C# WPF rich text editor** that behaves like a real **WPF
WYSIWYG HTML editor**, with toolbar, formatting, tables, images, paste cleanup, and spell check,
the SpiceLogic WPF HTML Editor installs as a **WPF HTML editor NuGet** package and drops onto any
window like a normal control, including full **WPF HTML editor MVVM binding** support for
`BodyHtml` and friends. This repository is the runnable sample collection for it.

## What this is

Ten runnable WPF sample projects for the commercial SpiceLogic WPF HTML Editor control, which
installs from NuGet. Every sample ships as a matching pair: a C# project and a VB.NET twin in a
`-VB` folder next to it, with the same behavior and the same comments, and every twin is already
added to `WpfHtmlEditorSamples.sln`, so it builds and runs with no extra setup. The control itself
is closed source and commercial; the sample code in this repository is MIT licensed and free to
copy into your own project.

Every window declares the editor in XAML, so you can open any `MainWindow.xaml` in the Visual
Studio designer and lay your own controls out around it.

## Install

```bash
dotnet add package SpiceLogic.HtmlEditor.WPF
```

```powershell
PM> Install-Package SpiceLogic.HtmlEditor.WPF
```

The sample projects reference the package as `Version="*"`, so a restore always pulls the latest
published release and you are never evaluating an old build.

## Get the samples

```bash
git clone https://github.com/spicelogic/wpf-html-editor-control-samples.git
```

Open `WpfHtmlEditorSamples.sln` and start with **01-Quickstart-CS**.

Prerequisites: Windows (the control hosts a Windows web control, so it does not run on macOS or
Linux), and the .NET SDK for whichever target you build against. The control supports .NET
Framework 4.5, 4.7.2 and 4.8, and .NET 5 through .NET 10, all on Windows. The samples are checked
in targeting `net48`, because .NET Framework 4.8 is part of Windows 10 (1903 and later) and
Windows 11, so they run on a stock Windows machine with no runtime to install.

<details>
<summary><b>Targeting a different .NET version</b></summary>

Every sample project shares its `<TargetFramework>` from a single line in
[`Directory.Build.props`](Directory.Build.props) at the repository root. Edit that one line, then
rebuild the solution. There is nothing to change in the individual project files.

```xml
<!-- Directory.Build.props -->
<TargetFramework>net48</TargetFramework>
```

Valid values, all shipped in the `SpiceLogic.HtmlEditor.WPF` NuGet package:

| Family | Targets |
| --- | --- |
| .NET Framework | `net48` (the default here), `net472`, `net45` |
| .NET | `net5.0-windows`, `net6.0-windows`, `net7.0-windows`, `net8.0-windows`, `net9.0-windows`, `net10.0-windows` |

</details>

## Samples

Ten samples, twenty projects in the solution: every sample has a VB.NET twin folder alongside its
C# folder, using the identical scenario and the same key API members.

| # | Sample | What it shows | Question it answers |
| :-- | :-- | :-- | :-- |
| 01 | [Quickstart](01-Quickstart-CS) <br> [VB](01-Quickstart-VB) | The editor filling a window, a styled starting document, and a live character count from `HtmlChanged` | How do I drop the editor onto a window and react to edits? |
| 02 | [MvvmDataBinding](02-MvvmDataBinding-CS) <br> [VB](02-MvvmDataBinding-VB) | Five windows, one per WPF binding mode against `BodyHtml` (OneWay, TwoWay with LostFocus, TwoWay with Explicit, TwoWay with PropertyChanged, and an ElementName source), plus `UpdateBindings()` | How do I bind the editor's content in MVVM, and why is my bound value stale? |
| 03 | [ThemeAndAppearance](03-ThemeAndAppearance-CS) <br> [VB](03-ThemeAndAppearance-VB) | `EditorBorderColor`, `EditorBorderWidth`, `DefaultFontFamily`, `DefaultFontSizeInPt`, and switching `EditorMode` | How do I match my app's theme and toggle design and source view? |
| 04 | [ToolbarCustomization](04-ToolbarCustomization-CS) <br> [VB](04-ToolbarCustomization-VB) | Hiding, re-skinning, and rewiring built-in toolbar buttons at startup, plus hand-assembling a custom toolbar from scratch on demand | How do I add my own button to the toolbar, or replace it entirely? |
| 05 | [MailMergeTemplate](05-MailMergeTemplate-CS) <br> [VB](05-MailMergeTemplate-VB) | Registering `PlaceholderField`s, the built-in placeholder toolbar, and a live merge preview | How do I let end users build their own mail-merge templates? |
| 06 | [Localization](06-Localization-CS) <br> [VB](06-Localization-VB) | Switching `Language` at runtime, an independent spell-check dictionary language, and a JSON override file for individual strings | How do I localize the editor's UI without recompiling? |
| 07 | [CustomSpellChecker](07-CustomSpellChecker-CS) <br> [VB](07-CustomSpellChecker-VB) | Plugging in a custom `ISpellCheckerEngine`, toggled against the built-in engine | How do I use my own dictionary or spell-checking engine? |
| 08 | [CustomDialog](08-CustomDialog-CS) <br> [VB](08-CustomDialog-VB) | Replacing every built-in editor dialog (image, hyperlink, table, table cell, symbol, search, spell checker, style builder, YouTube insert, color picker) with your own WPF window | How do I make the editor's dialogs match my own look and feel? |
| 09 | [CustomContextMenu](09-CustomContextMenu-CS) <br> [VB](09-CustomContextMenu-VB) | Replacing the right-click menu with a custom `ContextMenu` kept in sync with live editor state (`StateQuery`) through `ContextMenuShowing` | How do I build a right-click menu that reflects what the user clicked on? |
| 10 | [FullEditorDemo](10-FullEditorDemo-CS) <br> [VB](10-FullEditorDemo-VB) | The complete default toolbar, a styled starting document, a live character count, and inline spell checking with a language switcher | What does the editor look like fully set up? |

## Quickstart code

```xml
xmlns:se="clr-namespace:SpiceLogic.HtmlEditor.WPF;assembly=SpiceLogic.HtmlEditor.WPF"

<se:WpfHtmlEditor x:Name="Editor" HtmlChanged="Editor_HtmlChanged" />
```

```csharp
private void Editor_HtmlChanged(object sender, EventArgs e)
{
    string currentHtml = Editor.BodyHtml;
    // Persist currentHtml, validate it, or sync it to your view model here.
}
```

> [!IMPORTANT]
> ### The one gotcha worth knowing
>
> The editor hosts a live document, not a plain string. When you bind `BodyHtml` or
> `DocumentHtml` in MVVM, the bound view model property only reflects whatever WPF's binding last
> pushed into it, which by default is when the editor loses focus. If you read the bound value
> from code that runs without the editor losing focus first (a keyboard shortcut, a background
> timer, a button that does not move focus), the view model can be behind the user's last few
> keystrokes.
>
> Call `editor.UpdateBindings()` before reading `BodyHtml` or `DocumentHtml` from a bound view
> model. It pushes the editor's current content into its dependency properties immediately, which
> in turn flows into your bound property. This is the single most common support question about
> this control, and [`02-MvvmDataBinding-CS`](02-MvvmDataBinding-CS) is built around exactly this
> pattern.

## What the control does

| | |
| :-- | :-- |
| **Binds like any WPF control** | `BodyHtml`, `DocumentHtml` and `DocumentTitle` are ordinary dependency properties |
| **Matches your app theme** | One `ResourceDictionary` |
| **Paste from Word and Outlook** | Without the mess |
| **Clean HTML** | Ready for your database and your email pipeline to trust |
| **Mail-merge templates** | Built by end users themselves, no code required |
| **Spell check** | No deployment drama, no external dictionary files to ship |
| **Tables** | Users edit them without calling your support line |
| **Images** | Arrive clean, not bloated with editor-only markup |
| **A full editor API** | Content, formatting, selection, and state queries |
| **Sharp toolbar icons** | At every DPI |
| **Toolbar customization** | Add your own buttons and commands |
| **A multilingual UI** | Toolbars, dialogs, and messages |
| **A CSS style builder** | For end users who need more than the toolbar exposes |
| **Source view** | WYSIWYG plus raw HTML, for power users and debugging |

## Trial and licensing

Every sample here runs unmodified in the control's free 14-day trial, so there is nothing to
obtain before you evaluate it.

Once you buy, applying the key is one line at application startup, before any window containing
the editor is created:

```csharp
// App.xaml.cs
using SpiceLogic.HtmlEditor.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        WpfHtmlEditor.LicenseKey = "PASTE-YOUR-LICENSE-KEY-HERE";
        base.OnStartup(e);
    }
}
```

```vbnet
' Application.xaml.vb
Imports SpiceLogic.HtmlEditor.WPF

Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        WpfHtmlEditor.LicenseKey = "PASTE-YOUR-LICENSE-KEY-HERE"
    End Sub
End Class
```

> [!IMPORTANT]
> A misplaced key does not throw an error, it silently leaves the application in trial mode.
>
> - `LicenseKey` is static, so one assignment covers every editor in the process. Never set it
>   per window or per instance.
> - Set it before the first editor is constructed. Assigning it later leaves editors that
>   already exist in trial mode.
> - Source code license customers who build the editor from the purchased source do not set
>   `LicenseKey` at all. That build is licensed automatically.

Your key is on your account at [members.spicelogic.com](https://members.spicelogic.com). Full
details, including what changes at the end of the trial, are in
[how to license the WPF HTML editor](https://www.spicelogic.com/docs/wpfHtmlEditor/StartUp/how-to-license-wpf-html-editor-152).

## Links

| | |
| :-- | :-- |
| Product page | https://www.spicelogic.com/Products/WPF-HTML-Editor-Control-17 |
| Documentation | https://www.spicelogic.com/docs/wpfHtmlEditor |
| MCP server | `https://mcp.spicelogic.com/html-editor/wpf` |
| NuGet package | https://www.nuget.org/packages/SpiceLogic.HtmlEditor.WPF/ |
| WinForms version of this control | https://www.spicelogic.com/Products/NET-WinForms-HTML-Editor-Control-8 |
| WinForms samples repo | https://github.com/spicelogic/winforms-html-editor-control-samples |
