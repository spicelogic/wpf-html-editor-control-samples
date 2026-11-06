# Customize the editor interface

## Choose a built-in translation

Set the editor's `Language` to one of the fourteen built-in values. No JSON is
needed. Toolbar tooltips, dialogs and context menus follow that language.

## Change selected wording

Find the key in `_OVERRIDABLE-KEYS.reference.json`. Copy only the keys to change
into `EditorStrings.<culture>.json`, matching the selected built-in language,
for example `EditorStrings.pl-PL.json` for Polish. Other keys keep their built-in
translation. The reference catalog is documentation and is never loaded at runtime.

## Supply a new interface language

Select `EditorLanguage.Custom`. Its file is **`EditorStrings.custom.json`**,
not an English override and not an arbitrary culture filename. This sample's
custom file contains two Turkish labels to demonstrate selection; untranslated
keys fall back to neutral English. A complete translation supplies every key.

Your coding assistant can prepare that complete translation. Connect the
edition's documentation MCP server, then ask it to translate every **value** in
the reference catalog into your target language, preserve every key and format
placeholder such as `{0}`, write `EditorStrings.custom.json`, configure output
copying, load the folder before editor construction and select `Custom`.
Review the generated JSON, tweak wording and check the resulting tooltips and
dialogs. Machine translation still needs a fluent review before shipping.

## Put the file in the project and deploy it

1. Create a `SpiceLogic.HtmlEditor.Localization` folder in your application project.
2. Add your `EditorStrings.<culture>.json` or `EditorStrings.custom.json` inside it.
3. Select the JSON in **Solution Explorer**. In **Properties**, set **Build Action**
   to **Content**, and **Copy to Output Directory** to **Copy if newer**.
4. Load the folder once at startup, **before constructing the editor**:

   ```csharp
   LocalizationManager.SetJsonOverrideDirectory(
       Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
           "SpiceLogic.HtmlEditor.Localization"));
   // Construct the form/window containing the editor after this call.
   ```

   ```vb
   LocalizationManager.SetJsonOverrideDirectory(
       Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
           "SpiceLogic.HtmlEditor.Localization"))
   ' Construct the form/window containing the editor after this call.
   ```

   `LocalizationManager` and `EditorLanguage` are in
   `SpiceLogic.HtmlEditor.Resources.Localization`; `Path` is in `System.IO`.
   `AutoDiscoverJsonOverrides()` loads the same folder from the application's
   base directory when called at startup. It is an explicit alternative to the
   directory call, not automatic behavior triggered merely by selecting Custom.
5. Build and open the output folder. The deployed layout is:

   ```text
   Application folder/
     YourApplication.exe
     SpiceLogic.HtmlEditor.Localization/
       EditorStrings.custom.json
   ```

   On modern .NET, this folder is under `bin/Debug/<target-framework>/` when
   running a Debug build. Ship the JSON folder beside the executable as well.

For a custom translation, set the constructed editor's `Language` to
`EditorLanguage.Custom` in code, or `Language="Custom"` in WPF/Avalonia XAML.
For a built-in override, keep its corresponding language selected instead.
The samples' override checkbox demonstrates reloading at runtime; the startup
sequence above is the simpler pattern for a normal application.

## Behavior and limits

- A flat object of string key/value pairs is supported. Keep resource keys and
  format placeholders unchanged. Do not add comments, nested objects or arrays.
- Custom reads only the custom file. Built-in culture files may coexist in the
  same folder. Missing custom keys use neutral English resources, unaffected by
  any English JSON override; a missing or unreadable file uses the same fallback.
- `SetOverride("custom", key, value)` sets a custom string in code.
  `ClearOverrides()` reloads the configured files. Load before constructing the
  UI, or reapply its language when changing files in a running application.
- The configured directory and override cache are process-wide. To change the
  active custom translation, load a different directory and refresh the UI.
- Custom is an interface text option, with invariant formatting, not a new .NET
  culture or a bundled spelling dictionary. The default spelling fallback is US
  English; choose an explicit spelling language or configure your own dictionary.
