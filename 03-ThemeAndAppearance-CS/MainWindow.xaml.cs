using System.Windows;
using System.Windows.Media;
using SpiceLogic.HtmlEditor.Abstractions;

namespace ThemeAndAppearance;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.BodyHtml = "<p>Use the buttons below to change the editor's border, default font " +
                           "and startup mode at runtime.</p>";
    }

    private void BrandBorderButton_Click(object sender, RoutedEventArgs e)
    {
        // Matches your application's chrome or brand color, rather than the editor's default.
        Editor.EditorBorderColor = Color.FromRgb(0x0A, 0x37, 0x64);
        Editor.EditorBorderWidth = new Thickness(2);
    }

    private void DocumentFontButton_Click(object sender, RoutedEventArgs e)
    {
        // Sets the default font for the document body, persisted in the body element's style.
        Editor.DefaultFontFamily = "Georgia";
        Editor.DefaultFontSizeInPt = "12pt";
    }

    private void DesignModeButton_Click(object sender, RoutedEventArgs e)
    {
        Editor.EditorMode = EditorModes.WysiwygDesign;
    }

    private void SourceModeButton_Click(object sender, RoutedEventArgs e)
    {
        Editor.EditorMode = EditorModes.HtmlEdit;
    }
}
