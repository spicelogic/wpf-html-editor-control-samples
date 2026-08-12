using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpiceLogic.HtmlEditor.WPF.Controls;

namespace ToolbarCustomization;

public partial class MainWindow : Window
{
    private bool _customToolbarBuilt;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.BodyHtml = "<p>Dear customer,</p><p>Thank you for your business.</p>";

        var toolbarItems = Editor.ToolbarItemOverrider.ToolbarItems;

        // Hide a built-in button you don't want end users to have. This collapses the button
        // wherever it lives - on the default toolbar now, and on any custom toolbar you move
        // it to later.
        toolbarItems.New.Visibility = Visibility.Collapsed;

        // Swap a built-in button's icon. The button's Image sits three levels down its
        // template (Border > ContentPresenter > Image); walk the visual tree to reach it.
        if (VisualTreeHelper.GetChild(toolbarItems.Save, childIndex: 0) is Border saveBorder &&
            VisualTreeHelper.GetChild(saveBorder, childIndex: 0) is ContentPresenter saveContent &&
            VisualTreeHelper.GetChild(saveContent, childIndex: 0) is Image saveImage)
        {
            saveImage.Source = new BitmapImage(new Uri(
                "pack://application:,,,/ToolbarCustomization;component/Resources/save_accept.png", UriKind.Absolute));
        }

        // Override a built-in button's click behavior. Handling this event replaces the
        // built-in save-file-dialog behavior with your own logic; the built-in behavior never runs.
        Editor.ToolbarItemOverrider.SaveButtonClicked += (_, _) =>
            MessageBox.Show(
                "Your own save logic runs here instead of the built-in file dialog, for example writing to a database or calling an API.",
                "Custom save logic");

        // Change a built-in button's tooltip.
        toolbarItems.Open.ToolTip = "Load a saved letter";

        AddSignatureButton();
    }

    private void AddSignatureButton()
    {
        // The built-in toolbars stay exactly as they are; your own items are appended
        // alongside them. Toolbar1Items and Toolbar2Items are plain WPF ItemCollections,
        // so any WPF control can be dropped in, not only Button.
        var signatureButton = new Button { Content = "Insert signature" };

        signatureButton.Click += (_, _) =>
        {
            // Content.InsertHtml inserts at the caret, manages undo, and raises the
            // editor's change notification, so it is preferred over reassigning BodyHtml.
            Editor.Content.InsertHtml(
                "<p>Best regards,<br/>The SpiceLogic team</p>", keepSelected: false);
        };

        Editor.Toolbar2Items.Add(signatureButton);
    }

    private void BuildCustomToolbarButton_Click(object sender, RoutedEventArgs e)
    {
        if (_customToolbarBuilt)
            return;
        _customToolbarBuilt = true;

        // Hide both built-in toolbars; their buttons move into CustomToolBar below.
        Editor.Toolbar1.Visibility = Visibility.Collapsed;
        Editor.Toolbar2.Visibility = Visibility.Collapsed;

        var toolbarItems = Editor.ToolbarItemOverrider.ToolbarItems;

        // Pick only the built-in buttons you want, in the order you want. Save and Open keep
        // the icon, click override, and tooltip set above - moving a control to a new
        // container does not reset its properties or event handlers.
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Save);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Open);
        CustomToolBar.Items.Add(new Separator());

        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Bold);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Italic);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Underline);
        CustomToolBar.Items.Add(new Separator());

        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Cut);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Copy);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Paste);
        CustomToolBar.Items.Add(new Separator());

        toolbarItems.FontName.Style = (Style)FindResource("DefaultComboBoxStyle");
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.FontName);
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.FontSize);
        CustomToolBar.Items.Add(new Separator());

        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.Hyperlink);
        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.Image);
        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.FontColor);
        CustomToolBar.Items.Add(new Separator());

        // A brand-new custom button, styled with the same image-button look the built-in
        // toolbar buttons use, wired to its own logic.
        var timestampButton = new ToolbarCustomButton
        {
            ToolTip = "Insert timestamp",
            Content = new Image
            {
                Source = new BitmapImage(new Uri(
                    "pack://application:,,,/ToolbarCustomization;component/Resources/save_accept.png", UriKind.Absolute))
            }
        };
        timestampButton.Click += (_, _) =>
            Editor.Content.InsertHtml(
                $"<p><em>Logged {DateTime.Now:MMMM d, yyyy 'at' h:mm tt}</em></p>", keepSelected: false);
        CustomToolBar.Items.Add(timestampButton);

        CustomToolbarTray.Visibility = Visibility.Visible;
        BuildCustomToolbarButton.IsEnabled = false;
        BuildCustomToolbarButton.Content = "Custom toolbar built";
    }

    private void MoveToCustomToolbar(ToolBar sourceToolbar, Control control)
    {
        sourceToolbar.Items.Remove(control);
        CustomToolBar.Items.Add(control);
    }
}
