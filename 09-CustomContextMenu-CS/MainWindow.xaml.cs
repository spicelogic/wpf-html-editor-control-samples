using System.Windows;
using System.Windows.Controls;
using SpiceLogic.HtmlEditor.Abstractions;
using SpiceLogic.HtmlEditor.WPF.EditorEventArgs;
using mshtml;

namespace CustomContextMenu;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void contextMenuItem_imageProperties_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.ToolbarItemOverrider.OnImageButtonClicked(sender, e);
    }

    private void contextMenuItem_linkProperties_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.ToolbarItemOverrider.OnHyperLinkButtonClicked(sender, e);
    }

    private void contextMenuItem_cellProperties_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.ToolbarItemOverrider.OnTableCellEditingClicked(sender, e);
    }

    private void tablePropertiesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.ToolbarItemOverrider.OnTableModifyButtonClicked(sender, e);
    }

    private void insertRowBeforeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.Before);
    }

    private void insertRowAfterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.After);
    }

    private void deleteRowToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.DeleteRow();
    }

    private void insertColumnBeforeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.Before);
    }

    private void insertColumnAfterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.After);
    }

    private void deleteColumnToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.DeleteColumn();
    }

    private void mergeCellsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Content.TableAuthoringService.MergeSelectedCells();
    }

    private void youTubeVideoPropertiesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.ToolbarItemOverrider.OnYouTubeVideoInsertButtonClicked(sender, e);
    }

    private void contextMenuItem_align_left_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Formatting.AlignLeft();
    }

    private void contextMenuItem_align_center_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Formatting.AlignCenter();
    }

    private void contextMenuItem_align_right_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Formatting.AlignRight();
    }

    private void contextMenuItem_align_remove_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Formatting.RemoveAlignment();
    }

    private void contextMenuItem_cut_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Editor.Cut();
    }

    private void contextMenuItem_copy_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Editor.Copy();
    }

    private void contextMenuItem_paste_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Editor.Paste();
    }

    private void contextMenuItem_delete_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Editor.Delete();
    }

    private void contextMenuItem_selectAll_Click(object sender, RoutedEventArgs e)
    {
        WpfHtmlEditor.Selection.SelectAll();
    }

    private void WpfHtmlEditor_OnContextMenuShowing(object sender, ContextMenuShowingEventArgs e)
    {
        if (FindResource("ContextMenuCustom") is not ContextMenu customContextMenu)
            return;

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_delete") is MenuItem contextMenuItemDelete)
            contextMenuItemDelete.IsEnabled = WpfHtmlEditor.StateQuery.CanDelete();

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_copy") is MenuItem contextMenuItemCopy)
            contextMenuItemCopy.IsEnabled = WpfHtmlEditor.StateQuery.CanCopy();

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cut") is MenuItem contextMenuItemCut)
            contextMenuItemCut.IsEnabled = WpfHtmlEditor.StateQuery.CanCut();

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_paste") is MenuItem contextMenuItemPaste)
            contextMenuItemPaste.IsEnabled = WpfHtmlEditor.StateQuery.CanPaste();

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "youTubeVideoPropertiesToolStripMenuItem") is MenuItem youTubeVideoPropertiesToolStripMenuItem)
            youTubeVideoPropertiesToolStripMenuItem.Visibility = WpfHtmlEditor.StateQuery.IsYouTubeVideo() ? Visibility.Visible : Visibility.Collapsed;

        MenuItem? contextMenuItemImageProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_imageProperties") as MenuItem;
        MenuItem? contextMenuItemLinkProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_linkProperties") as MenuItem;

        IHTMLElement activeHtmlElement = WpfHtmlEditor.StateQuery.GetActiveHtmlElement();
        if (WpfHtmlEditor.StateQuery.IsImage()
            && activeHtmlElement.parentElement.tagName.ToLower() == "a"
            && activeHtmlElement.parentElement.innerHTML == activeHtmlElement.outerHTML)
        {
            if (contextMenuItemLinkProperties is { IsVisible: true })
                contextMenuItemLinkProperties.IsChecked = true;

            if (contextMenuItemImageProperties is { IsVisible: true })
                contextMenuItemImageProperties.IsChecked = true;
        }
        else
        {
            if (contextMenuItemLinkProperties is { IsVisible: true })
                contextMenuItemLinkProperties.IsChecked = WpfHtmlEditor.StateQuery.IsActiveOrAncestorElementHyperLink();

            if (contextMenuItemImageProperties is { IsVisible: true })
                contextMenuItemImageProperties.IsChecked = WpfHtmlEditor.StateQuery.IsImage();
        }

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "toolStripSeparator1Context") is Separator toolStripSeparator1Context)
            toolStripSeparator1Context.Visibility = (WpfHtmlEditor.StateQuery.IsTable() || WpfHtmlEditor.StateQuery.IsTableCell() || WpfHtmlEditor.StateQuery.IsHyperLink()) ? Visibility.Visible : Visibility.Collapsed;

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_tableProperties") is MenuItem contextMenuItemTableProperties)
            contextMenuItemTableProperties.Visibility = WpfHtmlEditor.StateQuery.IsTable() || WpfHtmlEditor.StateQuery.IsTableCell() ? Visibility.Visible : Visibility.Collapsed;

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cellProperties") is MenuItem contextMenuItemCellProperties)
            contextMenuItemCellProperties.Visibility = WpfHtmlEditor.StateQuery.IsTableCell() ? Visibility.Visible : Visibility.Collapsed;

        if (LogicalTreeHelper.FindLogicalNode(customContextMenu, "mergeCellsToolStripMenuItem") is MenuItem mergeCellsToolStripMenuItem)
            mergeCellsToolStripMenuItem.IsEnabled = WpfHtmlEditor.StateQuery.CanMergeTableCells();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (FindResource("ContextMenuCustom") is not ContextMenu customContextMenu)
            return;

        WpfHtmlEditor.EditorContextMenuStrip = customContextMenu;

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        WpfHtmlEditor.BodyHtml = @"
            <h3>Right-click to try the custom context menu</h3>
            <p>This editor uses a <strong>custom context menu</strong> instead of the built-in one.
               Right-click anywhere to see it. Try right-clicking inside the table below to see
               table-specific options like Insert Row, Delete Column, and Merge Cells.</p>
            <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse; margin-top:10px;'>
                <tr style='background-color:#4472C4; color:white;'>
                    <th>Product</th><th>Category</th><th>Status</th>
                </tr>
                <tr>
                    <td>HTML Editor</td><td>WPF Controls</td><td>Active</td>
                </tr>
                <tr>
                    <td>Data Grid</td><td>WPF Controls</td><td>Planning</td>
                </tr>
            </table>
            <p style='margin-top:10px;'>Select some text and right-click to see <em>Cut</em>, <em>Copy</em>,
               and <em>Delete</em> options become enabled in the context menu.</p>";
    }
}
