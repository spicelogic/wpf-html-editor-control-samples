Imports System.Windows
Imports CustomDialog.Dialogs
Imports CustomDialog.Dialogs.ColorPickerDialog
Imports CustomDialog.Dialogs.StyleBuilder

Namespace Global.CustomDialog

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        End Sub


        Private Sub MainWindow_OnLoaded(sender As Object, e As RoutedEventArgs)
            ' It is not necessary to override every dialog. This sample wires up all of them so you can see
            ' the pattern for each; comment out any assignment below for the dialogs you do not need to replace.

            WpfHtmlEditor.Dialog.CreateImageDialogMethod = Function() New ImageDialog(WpfHtmlEditor.Dialog)
            WpfHtmlEditor.Dialog.CreateHyperlinkDialogMethod = Function() New HyperLinkDialog()
            WpfHtmlEditor.Dialog.CreateSearchDialogMethod = Function() New SearchWindow()
            WpfHtmlEditor.Dialog.CreateSpellcheckerDialogMethod = Function() New SpellCheckerDialog With {.Options = WpfHtmlEditor.SpellCheckOptions}
            WpfHtmlEditor.Dialog.CreateTableCellDialogMethod = Function() New TableCellPropertiesDialog(WpfHtmlEditor.Dialog)
            WpfHtmlEditor.Dialog.CreateTableDialogMethod = Function() New TablePropertiesDialog(WpfHtmlEditor.Dialog)
            WpfHtmlEditor.Dialog.CreateSymbolDialogMethod = Function() New SymbolDialog()
            WpfHtmlEditor.Dialog.CreateStyleBuilderDialogMethod = Function() New WinStyleBuilder(Function() New ColorPickerDialog())
            WpfHtmlEditor.Dialog.CreateYouTubeVideoInsertDialogMethod = Function() New YouTubeVideoInsertDialog()
            WpfHtmlEditor.Dialog.CreateColorPickerDialogMethod = Function() New ColorPickerDialog()
        End Sub
    End Class
End Namespace
