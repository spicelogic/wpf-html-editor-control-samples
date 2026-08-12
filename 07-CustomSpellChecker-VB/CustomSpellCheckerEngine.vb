Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows
Imports SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck

Namespace Global.CustomSpellChecker

    ''' <summary>
    ''' A minimal <see cref="ISpellCheckerEngine"/> implementation. Real-world engines would call
    ''' into a specialized dictionary (medical, legal, multilingual, and so on) instead of this
    ''' demo rule.
    ''' </summary>
    Public Class CustomSpellCheckerEngine
        Implements ISpellCheckerEngine

        Private ReadOnly _userDictionary As New HashSet(Of String)()
        Private _fixedUserDictionaryPath As String = String.Empty

        ''' <summary>
        ''' Initializes the specified dictionary path.
        ''' </summary>
        ''' <param name="dictionaryPath">The dictionary path.</param>
        ''' <param name="affixPath">The affix path.</param>
        ''' <param name="userDictionaryPath">The user dictionary path.</param>
        Public Sub Initialize(dictionaryPath As String, affixPath As String, userDictionaryPath As String) Implements ISpellCheckerEngine.Initialize
            InitializeUserDictionary(userDictionaryPath)
        End Sub

        ''' <summary>
        ''' Spells the specified word.
        ''' </summary>
        ''' <param name="word">The word.</param>
        Public Function Spell(word As String) As Boolean Implements ISpellCheckerEngine.Spell
            ' This demo logic checks if a word starts with a, then considers the word misspelled.
            ' Otherwise, the word is considered correctly spelled.

            If String.IsNullOrEmpty(word) Then
                Return True
            End If

            If Not word.StartsWith("a") Then
                Return True
            End If

            Return _userDictionary.Contains(word)
        End Function

        ''' <summary>
        ''' Suggests replacements for the specified word.
        ''' </summary>
        ''' <param name="word">The word.</param>
        ''' <param name="max">The maximum number of suggestions.</param>
        Public Function Suggest(word As String, Optional max As Integer? = Nothing) As IEnumerable(Of String) Implements ISpellCheckerEngine.Suggest
            ' This demo logic checks if a word starts with a, then suggests two words
            ' from an array. A real implementation would query the underlying dictionary instead.

            If Not String.IsNullOrEmpty(word) AndAlso word.StartsWith("a") Then
                Return New String() {"Aesthetics007", "Apple008"}
            End If

            Return New String() {}
        End Function

        ''' <summary>
        ''' Adds a word to the user dictionary.
        ''' </summary>
        ''' <param name="word">The word.</param>
        Public Sub AddToUserDictionary(word As String) Implements ISpellCheckerEngine.AddToUserDictionary
            If String.IsNullOrEmpty(word) Then
                Return
            End If

            Try
                ' create user dictionary if it does not exist
                If Not File.Exists(_fixedUserDictionaryPath) Then
                    Dim directoryName As String = If(Path.GetDirectoryName(_fixedUserDictionaryPath), "")
                    If Not Directory.Exists(directoryName) Then
                        MessageBox.Show("User dictionary directory does not exist")
                        Return
                    End If

                    Using File.Create(_fixedUserDictionaryPath)
                    End Using
                End If

                ' do not add the word twice
                Dim userDictionaryWords As String() = File.ReadAllLines(_fixedUserDictionaryPath)
                If Array.Exists(userDictionaryWords, Function(s) s = word) Then
                    Return
                End If

                Using writer As StreamWriter = File.AppendText(_fixedUserDictionaryPath)
                    writer.WriteLine(word)
                End Using

                _userDictionary.Add(word)
            Catch ex As UnauthorizedAccessException
                MessageBox.Show(
                    $"File cannot be created/updated at this path '{_fixedUserDictionaryPath}' due to permission restriction")
            Catch ex As Exception
                MessageBox.Show(
                    $"An unexpected error occurred while creating/updating the user dictionary at '{_fixedUserDictionaryPath}'")
            End Try
        End Sub

        ''' <summary>
        ''' Releases unmanaged and, optionally, managed resources.
        ''' </summary>
        Public Sub Dispose() Implements ISpellCheckerEngine.Dispose
            ' not required
        End Sub

        ''' <summary>
        ''' Resolves a dictionary path to a full, existing file path.
        ''' </summary>
        ''' <param name="filePath">The file path.</param>
        Private Shared Function FixDictionaryPath(filePath As String) As String
            If String.IsNullOrEmpty(filePath) Then
                Return String.Empty
            End If

            If Not Path.IsPathRooted(filePath) Then
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath)
            End If

            ' add a .dic extension if the file does not exist and the path does not already have it
            If Not File.Exists(filePath) AndAlso Not String.Equals(Path.GetExtension(filePath), ".dic", StringComparison.OrdinalIgnoreCase) Then
                filePath = filePath.Trim() & ".dic"
            End If

            Return filePath
        End Function

        ''' <summary>
        ''' Loads the user dictionary from disk into memory.
        ''' </summary>
        ''' <param name="userDictionaryPath">The user dictionary path.</param>
        Private Sub InitializeUserDictionary(userDictionaryPath As String)
            _fixedUserDictionaryPath = FixDictionaryPath(userDictionaryPath)

            If String.IsNullOrEmpty(_fixedUserDictionaryPath) OrElse Not File.Exists(_fixedUserDictionaryPath) Then
                Return
            End If

            ' append to the user dictionary all the words from the specified file
            For Each line As String In File.ReadAllLines(_fixedUserDictionaryPath)
                _userDictionary.Add(line)
            Next
        End Sub

    End Class

End Namespace
