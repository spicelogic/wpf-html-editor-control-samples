# 07 - Custom spell checker

Plugs a custom spell-checking engine into the editor by implementing `ISpellCheckerEngine`, useful
when an application needs a specialized dictionary (medical, legal, or multilingual) instead of the
built-in one. A radio-button toggle switches between the built-in engine and the demo custom engine,
which flags any word starting with "a" as misspelled and offers two canned suggestions.

**Key API used**

- `ISpellCheckerEngine` - the interface a custom engine implements (`Spell`, `Suggest`,
  `AddToUserDictionary`, `Initialize`, `Dispose`).
- `SpellCheckOptions.SpellChecker` - switches between `SpellCheckerEngineTypes.OpenOffice` (built-in)
  and `SpellCheckerEngineTypes.Custom`.
- `SpellCheckOptions.CustomSpellCheckerEngine` - the custom engine instance used when `SpellChecker`
  is set to `Custom`.
- `SpellCheckOptions.CurlyUnderlineImageFilePath` - the image tiled under misspelled words.

**Run it**

```
dotnet run --project 07-CustomSpellChecker
```

A VB.NET version of this same sample sits alongside it in `07-CustomSpellChecker-VB`.
