using System.Windows;
using System.Windows.Controls;

namespace ToolbarCustomization;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.BodyHtml = "<p>Dear customer,</p><p>Thank you for your business.</p>";

        AddSignatureButton();
    }

    private void AddSignatureButton()
    {
        // The built-in toolbars stay exactly as they are; your own items are appended
        // alongside them. Toolbar1Items and Toolbar2Items are plain WPF ItemCollections,
        // so any WPF control can be dropped in, not only Button.
        var signatureButton = new Button
        {
            Content = "Insert signature"
        };

        signatureButton.Click += (_, _) =>
        {
            // Content.InsertHtml inserts at the caret, manages undo, and raises the
            // editor's change notification, so it is preferred over reassigning BodyHtml.
            Editor.Content.InsertHtml(
                "<p>Best regards,<br/>The SpiceLogic team</p>", keepSelected: false);
        };

        Editor.Toolbar2Items.Add(signatureButton);
    }
}
