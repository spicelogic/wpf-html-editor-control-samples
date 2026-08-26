# Customizing / overriding HTML Editor UI text (JSON override)

Every visible string in the HTML Editor - toolbar tooltips, dialog
titles and labels, context-menu items, status messages - can be
overridden **without recompiling the control**, by dropping a small
JSON file next to your application.

## How it works

`LocalizationManager` resolves each UI string in this order:

1. **JSON override** for the active culture (this folder) - if present
2. The embedded **RESX** resource for the active culture
3. The embedded **RESX** resource for English (invariant fallback)
4. The resource key itself (last resort)

So a JSON file only needs the *handful of keys you want to change* - every
other string keeps its built-in translation.

## Step by step

1. **Find the key.** Open [`_OVERRIDABLE-KEYS.reference.json`](_OVERRIDABLE-KEYS.reference.json)
   in this folder. It lists **every** overridable key (496 of them) with its
   default English text - e.g. `"Toolbar_Bold": "Bold"`,
   `"Dialog_Hyperlink_Title": "..."`. Search it for the wording you see in
   the UI to discover the key.

2. **Create / edit the culture file.** In this folder create a file named:

   ```
   EditorStrings.<culture>.json
   ```

   where `<culture>` is the .NET culture code of the editor language you
   want to customize - `en-US`, `de-DE`, `fr-FR`, `pl-PL`, etc. (The culture
   code for each `EditorLanguage` is shown in the dropdown / enum.)
   Example: [`EditorStrings.pl-PL.json`](EditorStrings.pl-PL.json) in this
   folder customizes Polish.

3. **Add only the keys you want to change**, copied from the reference file:

   ```json
   {
     "Toolbar_Bold": "Make Bold",
     "Toolbar_Italic": "Make Italic",
     "Dialog_Hyperlink_Title": "Insert Web Link"
   }
   ```

4. **Point the editor at this folder** once at startup. The sample uses the
   "Enable JSON Override" checkbox, which calls:

   ```csharp
   LocalizationManager.SetJsonOverrideDirectory(
       Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "SpiceLogic.HtmlEditor.Localization"));
   ```

   Or call `LocalizationManager.AutoDiscoverJsonOverrides()` - it
   automatically loads a `SpiceLogic.HtmlEditor.Localization` subfolder
   from your app's base directory. (You can also set a single string
   programmatically with `LocalizationManager.SetOverride(culture, key, value)`.)

5. **Ship the file.** Make sure your `EditorStrings.<culture>.json` is set
   to **Copy to Output Directory** so it lands next to your `.exe`. The
   reference file does **not** need to be copied - it is documentation only.

## Trying it in this sample

1. Run the sample.
2. Pick **Polish** in the *Editor Language* dropdown - the UI switches to Polish.
3. Tick **Enable JSON Override**. Toolbar tooltips for Bold / Italic /
   Underline / Cut / Copy / Paste / Undo / Redo now show a `[CUSTOM]`
   prefix, proving `EditorStrings.pl-PL.json` overrode the embedded
   Polish RESX strings.
4. Untick it to fall back to the built-in Polish translation.

## Notes

- Files are matched by the pattern `EditorStrings.*.json`. The reference
  file is deliberately named `_OVERRIDABLE-KEYS.reference.json` so it is
  **not** picked up as a culture override - it is just a catalog you copy from.
- The JSON parser is intentionally minimal: a **flat** object of
  `"key": "value"` string pairs. No nested objects, no comments, no arrays.
  Escapes supported: `\" \\ \n \r \t`.
- Keys are case-insensitive.
