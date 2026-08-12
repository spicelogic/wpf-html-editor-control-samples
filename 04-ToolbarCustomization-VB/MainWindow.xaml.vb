Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports SpiceLogic.HtmlEditor.WPF.Controls

Namespace Global.ToolbarCustomizationVB

Partial Public Class MainWindow

    Private _customToolbarBuilt As Boolean

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainWindow_OnLoaded(sender As Object, e As RoutedEventArgs)
        ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        Editor.BodyHtml = "<p>Dear customer,</p><p>Thank you for your business.</p>"

        Dim toolbarItems = Editor.ToolbarItemOverrider.ToolbarItems

        ' Hide a built-in button you don't want end users to have. This collapses the button
        ' wherever it lives - on the default toolbar now, and on any custom toolbar you move
        ' it to later.
        toolbarItems.[New].Visibility = Visibility.Collapsed

        ' Swap a built-in button's icon. The button's Image sits three levels down its
        ' template (Border > ContentPresenter > Image); walk the visual tree to reach it.
        Dim saveBorder = TryCast(VisualTreeHelper.GetChild(toolbarItems.Save, childIndex:=0), Border)
        If saveBorder IsNot Nothing Then
            Dim saveContent = TryCast(VisualTreeHelper.GetChild(saveBorder, childIndex:=0), ContentPresenter)
            If saveContent IsNot Nothing Then
                Dim saveImage = TryCast(VisualTreeHelper.GetChild(saveContent, childIndex:=0), Image)
                If saveImage IsNot Nothing Then
                    saveImage.Source = New BitmapImage(New Uri(
                        "pack://application:,,,/ToolbarCustomizationVB;component/Resources/save_accept.png", UriKind.Absolute))
                End If
            End If
        End If

        ' Override a built-in button's click behavior. Handling this event replaces the
        ' built-in save-file-dialog behavior with your own logic; the built-in behavior never runs.
        AddHandler Editor.ToolbarItemOverrider.SaveButtonClicked,
            Sub()
                MessageBox.Show(
                    "Your own save logic runs here instead of the built-in file dialog, for example writing to a database or calling an API.",
                    "Custom save logic")
            End Sub

        ' Change a built-in button's tooltip.
        toolbarItems.Open.ToolTip = "Load a saved letter"

        AddSignatureButton()
    End Sub

    Private Sub AddSignatureButton()
        ' The built-in toolbars stay exactly as they are; your own items are appended
        ' alongside them. Toolbar1Items and Toolbar2Items are plain WPF ItemCollections,
        ' so any WPF control can be dropped in, not only Button.
        Dim signatureButton As New Button With {.Content = "Insert signature"}

        AddHandler signatureButton.Click,
            Sub()
                ' Content.InsertHtml inserts at the caret, manages undo, and raises the
                ' editor's change notification, so it is preferred over reassigning BodyHtml.
                Editor.Content.InsertHtml(
                    "<p>Best regards,<br/>The SpiceLogic team</p>", keepSelected:=False)
            End Sub

        Editor.Toolbar2Items.Add(signatureButton)
    End Sub

    Private Sub BuildCustomToolbarButton_Click(sender As Object, e As RoutedEventArgs)
        If _customToolbarBuilt Then
            Return
        End If
        _customToolbarBuilt = True

        ' Hide both built-in toolbars; their buttons move into CustomToolBar below.
        Editor.Toolbar1.Visibility = Visibility.Collapsed
        Editor.Toolbar2.Visibility = Visibility.Collapsed

        Dim toolbarItems = Editor.ToolbarItemOverrider.ToolbarItems

        ' Pick only the built-in buttons you want, in the order you want. Save and Open keep
        ' the icon, click override, and tooltip set above - moving a control to a new
        ' container does not reset its properties or event handlers.
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Save)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Open)
        CustomToolBar.Items.Add(New Separator())

        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Bold)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Italic)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Underline)
        CustomToolBar.Items.Add(New Separator())

        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Cut)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Copy)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.Paste)
        CustomToolBar.Items.Add(New Separator())

        toolbarItems.FontName.Style = CType(FindResource("DefaultComboBoxStyle"), Style)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.FontName)
        MoveToCustomToolbar(Editor.Toolbar1.ToolBar, toolbarItems.FontSize)
        CustomToolBar.Items.Add(New Separator())

        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.Hyperlink)
        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.Image)
        MoveToCustomToolbar(Editor.Toolbar2.ToolBar, toolbarItems.FontColor)
        CustomToolBar.Items.Add(New Separator())

        ' A brand-new custom button, styled with the same image-button look the built-in
        ' toolbar buttons use, wired to its own logic.
        Dim timestampButton As New ToolbarCustomButton With {
            .ToolTip = "Insert timestamp",
            .Content = New Image With {
                .Source = New BitmapImage(New Uri(
                    "pack://application:,,,/ToolbarCustomizationVB;component/Resources/save_accept.png", UriKind.Absolute))
            }
        }
        AddHandler timestampButton.Click,
            Sub()
                Editor.Content.InsertHtml(
                    $"<p><em>Logged {DateTime.Now:MMMM d, yyyy 'at' h:mm tt}</em></p>", keepSelected:=False)
            End Sub
        CustomToolBar.Items.Add(timestampButton)

        CustomToolbarTray.Visibility = Visibility.Visible
        BuildCustomToolbarButton.IsEnabled = False
        BuildCustomToolbarButton.Content = "Custom toolbar built"
    End Sub

    Private Sub MoveToCustomToolbar(sourceToolbar As ToolBar, control As Control)
        sourceToolbar.Items.Remove(control)
        CustomToolBar.Items.Add(control)
    End Sub

End Class

End Namespace
