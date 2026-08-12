using System.Windows;
using CustomDialog.Dialogs;
using CustomDialog.Dialogs.ColorPickerDialog;
using CustomDialog.Dialogs.StyleBuilder;

namespace CustomDialog;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        // It is not necessary to override every dialog. This sample wires up all of them so you can see
        // the pattern for each; comment out any assignment below for the dialogs you do not need to replace.

        WpfHtmlEditor.Dialog.CreateImageDialogMethod = () => new ImageDialog(WpfHtmlEditor.Dialog);
        WpfHtmlEditor.Dialog.CreateHyperlinkDialogMethod = () => new HyperLinkDialog();
        WpfHtmlEditor.Dialog.CreateSearchDialogMethod = () => new SearchWindow();
        WpfHtmlEditor.Dialog.CreateSpellcheckerDialogMethod = () => new SpellCheckerDialog { Options = WpfHtmlEditor.SpellCheckOptions };
        WpfHtmlEditor.Dialog.CreateTableCellDialogMethod = () => new TableCellPropertiesDialog(WpfHtmlEditor.Dialog);
        WpfHtmlEditor.Dialog.CreateTableDialogMethod = () => new TablePropertiesDialog(WpfHtmlEditor.Dialog);
        WpfHtmlEditor.Dialog.CreateSymbolDialogMethod = () => new SymbolDialog();
        WpfHtmlEditor.Dialog.CreateStyleBuilderDialogMethod = () => new WinStyleBuilder(() => new ColorPickerDialog());
        WpfHtmlEditor.Dialog.CreateYouTubeVideoInsertDialogMethod = () => new YouTubeVideoInsertDialog();
        WpfHtmlEditor.Dialog.CreateColorPickerDialogMethod = () => new ColorPickerDialog();
    }
}
