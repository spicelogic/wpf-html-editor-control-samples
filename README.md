# WPF HTML editor control - C# and VB.NET sample projects

A **WPF HTML editor control** for building a rich text editing experience inside a .NET desktop
application. If you are looking for a **C# WPF rich text editor** that behaves like a real **WPF
WYSIWYG HTML editor** - toolbar, formatting, tables, images, paste cleanup, spell check - the
SpiceLogic WPF HTML Editor installs as a **WPF HTML editor NuGet** package and drops onto any
window like a normal control, including full **WPF HTML editor MVVM binding** support for
`BodyHtml` and friends. This repository is the runnable sample collection for it.

## What this is

Ten runnable WPF sample projects for the commercial SpiceLogic WPF HTML Editor control, which
installs from NuGet. Starting with `02-MvvmDataBinding-CS`, most samples also ship a VB.NET twin
in a matching `-VB` folder next to the C# original - same behavior, same comments, just VB.NET
syntax - and every twin is already added to `WpfHtmlEditorSamples.sln`, so it builds and runs
with no extra setup. Two samples, `03-ThemeAndAppearance-CS` and `05-MailMergeTemplate-CS`, are C#
only for now. The control itself is closed source and commercial; the sample code in this
repository is MIT licensed and free to copy into your own project.

## Install

```
dotnet add package SpiceLogic.HtmlEditor.WPF
```

or, from the Package Manager Console:

```
PM> Install-Package SpiceLogic.HtmlEditor.WPF
```

The sample projects reference the package as `Version="*"`, so a restore always pulls the
latest published release and you are never evaluating an old build.

## Run the samples

Prerequisites: Windows (the control hosts a Windows web control, so it does not run on macOS or
Linux), and the .NET SDK for whichever target framework you build against.

The **control** itself supports .NET Framework 4.5, 4.7.2 and 4.8, and .NET 5 through .NET 10, all
on Windows - including .NET Framework 4.8, which is still the standard on most corporate desktops.
The samples in this repository are checked in targeting `net48`, because .NET Framework 4.8 is
part of Windows 10 (1903 and later) and Windows 11, so they run on a stock Windows machine with no
runtime to install. Point them at whatever your own application uses; see
[Targeting a different .NET version](#targeting-a-different-net-version) below to point them at
.NET Framework 4.8 or any other supported target with a one-line edit.

```
git clone https://github.com/spicelogic/wpf-html-editor-control-samples.git
cd wpf-html-editor-control-samples
dotnet build WpfHtmlEditorSamples.sln
dotnet run --project 01-Quickstart-CS
```

Swap `01-Quickstart-CS` for any of the folder names below to run a different sample.

## Targeting a different .NET version

Every sample project shares its `<TargetFramework>` from a single line in
[`Directory.Build.props`](Directory.Build.props) at the repository root. Edit that one line, then
rebuild the solution - there is nothing to change in the individual `.csproj` files.

Valid values, all shipped in the `SpiceLogic.HtmlEditor.WPF` NuGet package:

| .NET Framework | Modern .NET |
| --- | --- |
| `net45` | `net5.0-windows` |
| `net472` | `net6.0-windows` |
| `net48` (the default here) | `net7.0-windows` |
| | `net8.0-windows` |
| | `net9.0-windows` |
| | `net10.0-windows` |

```xml
<!-- Directory.Build.props -->
<TargetFramework>net48</TargetFramework>
```

## Samples

The "VB.NET twin" column links the matching `-VB` folder where one exists; "-" means that sample
is C# only.

| Folder | What it shows | Question it answers | VB.NET twin |
| --- | --- | --- | --- |
| [`01-Quickstart-CS`](01-Quickstart-CS) | The editor filling a window, starting content, and a live character count from `HtmlChanged` | How do I drop the editor onto a window and react to edits? | - |
| [`02-MvvmDataBinding-CS`](02-MvvmDataBinding-CS) | Five windows, one per WPF binding mode against `BodyHtml` (OneWay, TwoWay/LostFocus, TwoWay/Explicit, TwoWay/PropertyChanged, and an ElementName source), plus `UpdateBindings()` | How do I bind the editor's content in MVVM, and why is my bound value stale? | [`02-MvvmDataBinding-VB`](02-MvvmDataBinding-VB) |
| [`03-ThemeAndAppearance-CS`](03-ThemeAndAppearance-CS) | `EditorBorderColor`, `EditorBorderWidth`, `DefaultFontFamily`, `DefaultFontSizeInPt`, and switching `EditorMode` | How do I match my app's theme and toggle design/source view? | - |
| [`04-ToolbarCustomization-CS`](04-ToolbarCustomization-CS) | Hiding, re-skinning, and re-wiring built-in toolbar buttons at startup, plus hand-assembling a fully custom toolbar from scratch on demand | How do I add my own button to the toolbar, or replace it entirely? | [`04-ToolbarCustomization-VB`](04-ToolbarCustomization-VB) |
| [`05-MailMergeTemplate-CS`](05-MailMergeTemplate-CS) | Registering `PlaceholderField`s, the built-in placeholder toolbar, and a merge preview | How do I let end users build their own mail-merge templates? | - |
| [`06-Localization-CS`](06-Localization-CS) | Switching `Language` at runtime, an independent spell-check dictionary language, and a JSON override file for individual strings | How do I localize the editor's UI, and override a handful of its built-in strings? | [`06-Localization-VB`](06-Localization-VB) |
| [`07-CustomSpellChecker-CS`](07-CustomSpellChecker-CS) | Plugging in a custom `ISpellCheckerEngine` (a demo engine flags any word starting with "a"), toggled against the built-in engine | How do I use my own dictionary or spell-checking engine instead of the built-in one? | [`07-CustomSpellChecker-VB`](07-CustomSpellChecker-VB) |
| [`08-CustomDialog-CS`](08-CustomDialog-CS) | Replacing every built-in editor dialog (image, hyperlink, table, table cell, symbol, search, spell checker, style builder, YouTube insert, color picker) with your own WPF window | How do I make the editor's dialogs match my application's own look and feel? | [`08-CustomDialog-VB`](08-CustomDialog-VB) |
| [`09-CustomContextMenu-CS`](09-CustomContextMenu-CS) | Replacing the built-in right-click menu with a custom `ContextMenu` kept in sync with live editor state (`StateQuery`) via the `ContextMenuShowing` event | How do I build my own right-click menu that still reflects what the user clicked on? | [`09-CustomContextMenu-VB`](09-CustomContextMenu-VB) |
| [`10-FullEditorDemo-CS`](10-FullEditorDemo-CS) | The complete default toolbar, rich starting content, a live character count, and inline spell checking with a language switcher | What does the editor look like fully set up, out of the box? | [`10-FullEditorDemo-VB`](10-FullEditorDemo-VB) |

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

## The one gotcha worth knowing

The editor hosts a live document, not a plain string. When you bind `BodyHtml` or `DocumentHtml`
in MVVM, the bound view model property only reflects whatever WPF's binding last pushed into it -
by default, when the editor loses focus. If you read the bound value from code that runs without
the editor losing focus first (a keyboard shortcut, a background timer, a button that does not
move focus), the view model can be behind the user's last few keystrokes.

Call `editor.UpdateBindings()` before reading `BodyHtml` or `DocumentHtml` from a bound view
model. It pushes the editor's current content into its dependency properties right now, which in
turn flows into your bound property. This is the single most common support question we get about
this control, and the [`02-MvvmDataBinding-CS`](02-MvvmDataBinding-CS) sample builds around exactly this
pattern.

## What the control does

- Binds like any other WPF control, with `BodyHtml`, `DocumentHtml` and `DocumentTitle` as
  ordinary dependency properties.
- Matches your app theme with one `ResourceDictionary`.
- Pastes from Word and Outlook without the mess.
- Produces clean HTML your database and email pipeline can trust.
- Ships mail-merge templates end users build themselves, no code required.
- Spell checks with no deployment drama - no external dictionary files to ship.
- Gives users tables they can edit without calling your support line.
- Handles images that arrive clean, not bloated with editor-only markup.
- Exposes a full editor API for content, formatting, selection, and state queries.
- Renders toolbar icons that stay sharp at every DPI.
- Supports toolbar customization, so you can add your own buttons and commands.
- Speaks a multilingual UI for toolbars, dialogs, and messages.
- Includes a CSS style builder for end users who need more than the toolbar exposes.
- Switches between WYSIWYG and source view for power users and debugging.

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

Three things are worth knowing, because a misplaced key does not throw an error - it silently
leaves the application in trial mode:

- `LicenseKey` is static, so one assignment covers every editor in the process. Never set it per
  window or per instance.
- Set it before the first editor is constructed. Assigning it later leaves editors that already
  exist in trial mode.
- Source code license customers who build the editor from the purchased source do not set
  `LicenseKey` at all. That build is licensed automatically.

Your key is on your account at [members.spicelogic.com](https://members.spicelogic.com). Full
details, including what changes at the end of the trial, are in
[how to license the WPF HTML editor](https://www.spicelogic.com/docs/wpfHtmlEditor/StartUp/how-to-license-wpf-html-editor-152).

## Links

- Product page: https://www.spicelogic.com/Products/WPF-HTML-Editor-Control-17
- Documentation: https://www.spicelogic.com/docs/wpfHtmlEditor
- NuGet package: https://www.nuget.org/packages/SpiceLogic.HtmlEditor.WPF/
- WinForms version of this control: https://www.spicelogic.com/Products/NET-WinForms-HTML-Editor-Control-8
- WinForms samples repo: https://github.com/spicelogic/winforms-html-editor-control-samples

An official MCP server is also available for AI coding assistants (Claude Code, Cursor, VS Code
Copilot, and others), documented at
https://www.spicelogic.com/docs/wpfHtmlEditor/Programming/mcp-server-502.
