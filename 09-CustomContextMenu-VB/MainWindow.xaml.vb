Imports System.Windows
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions
Imports SpiceLogic.HtmlEditor.WPF.EditorEventArgs
Imports mshtml

Namespace Global.CustomContextMenu

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub contextMenuItem_imageProperties_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.ToolbarItemOverrider.OnImageButtonClicked(sender, e)
        End Sub

        Private Sub contextMenuItem_linkProperties_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.ToolbarItemOverrider.OnHyperLinkButtonClicked(sender, e)
        End Sub

        Private Sub contextMenuItem_cellProperties_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.ToolbarItemOverrider.OnTableCellEditingClicked(sender, e)
        End Sub

        Private Sub tablePropertiesToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.ToolbarItemOverrider.OnTableModifyButtonClicked(sender, e)
        End Sub

        Private Sub insertRowBeforeToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.Before)
        End Sub

        Private Sub insertRowAfterToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.After)
        End Sub

        Private Sub deleteRowToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.DeleteRow()
        End Sub

        Private Sub insertColumnBeforeToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.Before)
        End Sub

        Private Sub insertColumnAfterToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.After)
        End Sub

        Private Sub deleteColumnToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.DeleteColumn()
        End Sub

        Private Sub mergeCellsToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Content.TableAuthoringService.MergeSelectedCells()
        End Sub

        Private Sub youTubeVideoPropertiesToolStripMenuItem_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.ToolbarItemOverrider.OnYouTubeVideoInsertButtonClicked(sender, e)
        End Sub

        Private Sub contextMenuItem_align_left_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Formatting.AlignLeft()
        End Sub

        Private Sub contextMenuItem_align_center_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Formatting.AlignCenter()
        End Sub

        Private Sub contextMenuItem_align_right_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Formatting.AlignRight()
        End Sub

        Private Sub contextMenuItem_align_remove_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Formatting.RemoveAlignment()
        End Sub

        Private Sub contextMenuItem_cut_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Editor.Cut()
        End Sub

        Private Sub contextMenuItem_copy_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Editor.Copy()
        End Sub

        Private Sub contextMenuItem_paste_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Editor.Paste()
        End Sub

        Private Sub contextMenuItem_delete_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Editor.Delete()
        End Sub

        Private Sub contextMenuItem_selectAll_Click(sender As Object, e As RoutedEventArgs)
            WpfHtmlEditor.Selection.SelectAll()
        End Sub

        Private Sub WpfHtmlEditor_OnContextMenuShowing(sender As Object, e As ContextMenuShowingEventArgs)
            Dim customContextMenu As ContextMenu = TryCast(FindResource("ContextMenuCustom"), ContextMenu)
            If customContextMenu Is Nothing Then
                Return
            End If

            Dim contextMenuItemDelete As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_delete"), MenuItem)
            If contextMenuItemDelete IsNot Nothing Then
                contextMenuItemDelete.IsEnabled = WpfHtmlEditor.StateQuery.CanDelete()
            End If

            Dim contextMenuItemCopy As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_copy"), MenuItem)
            If contextMenuItemCopy IsNot Nothing Then
                contextMenuItemCopy.IsEnabled = WpfHtmlEditor.StateQuery.CanCopy()
            End If

            Dim contextMenuItemCut As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cut"), MenuItem)
            If contextMenuItemCut IsNot Nothing Then
                contextMenuItemCut.IsEnabled = WpfHtmlEditor.StateQuery.CanCut()
            End If

            Dim contextMenuItemPaste As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_paste"), MenuItem)
            If contextMenuItemPaste IsNot Nothing Then
                contextMenuItemPaste.IsEnabled = WpfHtmlEditor.StateQuery.CanPaste()
            End If

            Dim youTubeVideoPropertiesToolStripMenuItem As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "youTubeVideoPropertiesToolStripMenuItem"), MenuItem)
            If youTubeVideoPropertiesToolStripMenuItem IsNot Nothing Then
                youTubeVideoPropertiesToolStripMenuItem.Visibility = If(WpfHtmlEditor.StateQuery.IsYouTubeVideo(), Visibility.Visible, Visibility.Collapsed)
            End If

            Dim contextMenuItemImageProperties As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_imageProperties"), MenuItem)
            Dim contextMenuItemLinkProperties As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_linkProperties"), MenuItem)

            Dim activeHtmlElement As IHTMLElement = WpfHtmlEditor.StateQuery.GetActiveHtmlElement()
            If WpfHtmlEditor.StateQuery.IsImage() _
                AndAlso activeHtmlElement.parentElement.tagName.ToLower() = "a" _
                AndAlso activeHtmlElement.parentElement.innerHTML = activeHtmlElement.outerHTML Then
                If contextMenuItemLinkProperties IsNot Nothing AndAlso contextMenuItemLinkProperties.IsVisible Then
                    contextMenuItemLinkProperties.IsChecked = True
                End If

                If contextMenuItemImageProperties IsNot Nothing AndAlso contextMenuItemImageProperties.IsVisible Then
                    contextMenuItemImageProperties.IsChecked = True
                End If
            Else
                If contextMenuItemLinkProperties IsNot Nothing AndAlso contextMenuItemLinkProperties.IsVisible Then
                    contextMenuItemLinkProperties.IsChecked = WpfHtmlEditor.StateQuery.IsActiveOrAncestorElementHyperLink()
                End If

                If contextMenuItemImageProperties IsNot Nothing AndAlso contextMenuItemImageProperties.IsVisible Then
                    contextMenuItemImageProperties.IsChecked = WpfHtmlEditor.StateQuery.IsImage()
                End If
            End If

            Dim toolStripSeparator1Context As Separator = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "toolStripSeparator1Context"), Separator)
            If toolStripSeparator1Context IsNot Nothing Then
                toolStripSeparator1Context.Visibility = If((WpfHtmlEditor.StateQuery.IsTable() OrElse WpfHtmlEditor.StateQuery.IsTableCell() OrElse WpfHtmlEditor.StateQuery.IsHyperLink()), Visibility.Visible, Visibility.Collapsed)
            End If

            Dim contextMenuItemTableProperties As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_tableProperties"), MenuItem)
            If contextMenuItemTableProperties IsNot Nothing Then
                contextMenuItemTableProperties.Visibility = If(WpfHtmlEditor.StateQuery.IsTable() OrElse WpfHtmlEditor.StateQuery.IsTableCell(), Visibility.Visible, Visibility.Collapsed)
            End If

            Dim contextMenuItemCellProperties As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cellProperties"), MenuItem)
            If contextMenuItemCellProperties IsNot Nothing Then
                contextMenuItemCellProperties.Visibility = If(WpfHtmlEditor.StateQuery.IsTableCell(), Visibility.Visible, Visibility.Collapsed)
            End If

            Dim mergeCellsToolStripMenuItem As MenuItem = TryCast(LogicalTreeHelper.FindLogicalNode(customContextMenu, "mergeCellsToolStripMenuItem"), MenuItem)
            If mergeCellsToolStripMenuItem IsNot Nothing Then
                mergeCellsToolStripMenuItem.IsEnabled = WpfHtmlEditor.StateQuery.CanMergeTableCells()
            End If
        End Sub

        Private Sub MainWindow_OnLoaded(sender As Object, e As RoutedEventArgs)
            Dim customContextMenu As ContextMenu = TryCast(FindResource("ContextMenuCustom"), ContextMenu)
            If customContextMenu Is Nothing Then
                Return
            End If

            WpfHtmlEditor.EditorContextMenuStrip = customContextMenu

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
            WpfHtmlEditor.BodyHtml = vbCrLf &
                "                <h3>Right-click to try the custom context menu</h3>" & vbCrLf &
                "                <p>This editor uses a <strong>custom context menu</strong> instead of the built-in one." & vbCrLf &
                "                   Right-click anywhere to see it. Try right-clicking inside the table below to see" & vbCrLf &
                "                   table-specific options like Insert Row, Delete Column, and Merge Cells.</p>" & vbCrLf &
                "                <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse; margin-top:10px;'>" & vbCrLf &
                "                    <tr style='background-color:#4472C4; color:white;'>" & vbCrLf &
                "                        <th>Product</th><th>Category</th><th>Status</th>" & vbCrLf &
                "                    </tr>" & vbCrLf &
                "                    <tr>" & vbCrLf &
                "                        <td>HTML Editor</td><td>WPF Controls</td><td>Active</td>" & vbCrLf &
                "                    </tr>" & vbCrLf &
                "                    <tr>" & vbCrLf &
                "                        <td>Data Grid</td><td>WPF Controls</td><td>Planning</td>" & vbCrLf &
                "                    </tr>" & vbCrLf &
                "                </table>" & vbCrLf &
                "                <p style='margin-top:10px;'>Select some text and right-click to see <em>Cut</em>, <em>Copy</em>," & vbCrLf &
                "                   and <em>Delete</em> options become enabled in the context menu.</p>"
        End Sub

    End Class

End Namespace
