using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;

namespace CustomSpellChecker;

/// <summary>
/// A minimal <see cref="ISpellCheckerEngine"/> implementation. Real-world engines would call into
/// a specialized dictionary (medical, legal, multilingual, and so on) instead of this demo rule.
/// </summary>
public class CustomSpellCheckerEngine : ISpellCheckerEngine
{
    private readonly HashSet<string> _userDictionary = [];
    private string _fixedUserDictionaryPath = string.Empty;

    /// <summary>
    /// Initializes the specified dictionary path.
    /// </summary>
    /// <param name="dictionaryPath">The dictionary path.</param>
    /// <param name="affixPath">The affix path.</param>
    /// <param name="userDictionaryPath">The user dictionary path.</param>
    public void Initialize(string dictionaryPath, string affixPath, string userDictionaryPath)
    {
        InitializeUserDictionary(userDictionaryPath);
    }

    /// <summary>
    /// Spells the specified word.
    /// </summary>
    /// <param name="word">The word.</param>
    public bool Spell(string word)
    {
        // This demo logic checks if a word starts with a, then considers the word misspelled.
        // Otherwise, the word is considered correctly spelled.

        if (string.IsNullOrEmpty(word))
            return true;

        if (!word.StartsWith("a"))
            return true;

        return _userDictionary.Contains(word);
    }

    /// <summary>
    /// Suggests replacements for the specified word.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <param name="max">The maximum number of suggestions.</param>
    public IEnumerable<string> Suggest(string word, int? max = null)
    {
        // This demo logic checks if a word starts with a, then suggests two words
        // from an array. A real implementation would query the underlying dictionary instead.

        if (!string.IsNullOrEmpty(word) && word.StartsWith("a"))
            return ["Aesthetics007", "Apple008"];

        return [];
    }

    /// <summary>
    /// Adds a word to the user dictionary.
    /// </summary>
    /// <param name="word">The word.</param>
    public void AddToUserDictionary(string word)
    {
        if (string.IsNullOrEmpty(word))
            return;

        try
        {
            // create user dictionary if it does not exist
            if (!File.Exists(_fixedUserDictionaryPath))
            {
                string directoryName = Path.GetDirectoryName(_fixedUserDictionaryPath) ?? "";
                if (!Directory.Exists(directoryName))
                {
                    MessageBox.Show("User dictionary directory does not exist");
                    return;
                }

                using (File.Create(_fixedUserDictionaryPath))
                {
                }
            }

            // do not add the word twice
            string[] userDictionaryWords = File.ReadAllLines(_fixedUserDictionaryPath);
            if (Array.Exists(userDictionaryWords, s => s == word))
                return;

            using (StreamWriter writer = File.AppendText(_fixedUserDictionaryPath))
                writer.WriteLine(word);

            _userDictionary.Add(word);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                $"File cannot be created/updated at this path '{_fixedUserDictionaryPath}' due to permission restriction");
        }
        catch (Exception)
        {
            MessageBox.Show(
                $"An unexpected error occurred while creating/updating the user dictionary at '{_fixedUserDictionaryPath}'");
        }
    }

    /// <summary>
    /// Releases unmanaged and, optionally, managed resources.
    /// </summary>
    public void Dispose()
    {
        // not required
    }

    /// <summary>
    /// Resolves a dictionary path to a full, existing file path.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    private static string FixDictionaryPath(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return string.Empty;

        // The null-forgiving operator here is safe: IsNullOrEmpty above already ruled out null.
        // (On .NET Framework targets the older reference assemblies are nullable-oblivious, so
        // the compiler cannot infer that narrowing on its own.)
        string nonNullPath = filePath!;

        string resolvedPath = Path.IsPathRooted(nonNullPath)
            ? nonNullPath
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nonNullPath);

        // add a .dic extension if the file does not exist and the path does not already have it
        if (!File.Exists(resolvedPath) && !string.Equals(Path.GetExtension(resolvedPath), ".dic", StringComparison.OrdinalIgnoreCase))
            resolvedPath = resolvedPath.Trim() + ".dic";

        return resolvedPath;
    }

    /// <summary>
    /// Loads the user dictionary from disk into memory.
    /// </summary>
    /// <param name="userDictionaryPath">The user dictionary path.</param>
    private void InitializeUserDictionary(string? userDictionaryPath)
    {
        _fixedUserDictionaryPath = FixDictionaryPath(userDictionaryPath);

        if (string.IsNullOrEmpty(_fixedUserDictionaryPath) || !File.Exists(_fixedUserDictionaryPath))
            return;

        // append to the user dictionary all the words from the specified file
        foreach (string line in File.ReadAllLines(_fixedUserDictionaryPath))
        {
            _userDictionary.Add(line);
        }
    }
}
