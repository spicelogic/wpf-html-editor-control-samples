using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Quickstart;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.BodyHtml = "<p>Start typing here. This is the SpiceLogic WPF HTML editor control, " +
                           "dropped onto a window with nothing more than the NuGet package reference.</p>";
    }

    private void Editor_HtmlChanged(object sender, EventArgs e)
    {
        // BodyHtml reflects the editor's current content whenever it is read from inside
        // a HtmlChanged handler, so no extra step is needed here. For reading BodyHtml from
        // outside an editor event, for example from a data-bound view model, see the
        // 02-MvvmDataBinding sample and its UpdateBindings() call.
        string currentHtml = Editor.BodyHtml;
        string plainText = Regex.Replace(currentHtml, "<.*?>", string.Empty);
        CharacterCountText.Text = $"{plainText.Length} characters";
    }
}
