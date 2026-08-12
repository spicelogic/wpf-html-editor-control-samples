using System;
using System.Windows;
using SpiceLogic.HtmlEditor.Abstractions.Entities.SpellCheck;

namespace CustomSpellChecker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.SpellCheckOptions.CurlyUnderlineImageFilePath =
            AppDomain.CurrentDomain.BaseDirectory + "underline.gif";

        Editor.BodyHtml = @"
            <h3>Try the spell checker</h3>
            <p>This paragraph has a <strong>misspellled</strong> word that the built-in engine will catch.
               Switch to the <em>Custom Engine</em> and it will flag words like
               <strong>apple</strong>, <strong>amazing</strong>, and <strong>adventure</strong> instead,
               because the demo custom engine treats every word starting with ""a"" as misspelled.</p>
            <p>Type some text below to see real-time inline spell checking in action:</p>
            <p>&nbsp;</p>";
    }

    private void BuiltInRadio_OnCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (!IsInitialized)
            return;

        if (BuiltInRadio.IsChecked == true)
        {
            Editor.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.OpenOffice;
        }
        else
        {
            Editor.SpellCheckOptions.SpellChecker = SpellCheckerEngineTypes.Custom;
            Editor.SpellCheckOptions.CustomSpellCheckerEngine = new CustomSpellCheckerEngine();
        }
    }
}
