Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports Microsoft.Win32
Imports SpiceLogic.HtmlEditor.Abstractions.Entities
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class HyperLinkDialog
    ''' </summary>
    Partial Public Class HyperLinkDialog
        Implements IHyperlinkDialog

        ''' <summary>
        ''' The _the original element
        ''' </summary>
        Private _theOriginalElement As HyperlinkElement

        ''' <summary>
        ''' Initializes a new instance of the <see cref="HyperLinkDialog" /> class.
        ''' </summary>
        Public Sub New()
            InitializeComponent()
        End Sub


        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the HyperLinkDialog control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub HyperLinkDialog_Loaded(sender As Object, e As RoutedEventArgs)
            If String.IsNullOrEmpty(Me._theOriginalElement.BaseUrl) Then
                RdoWorkingDirFile.IsEnabled = False
                RdoWorkingDirFile.ToolTip = "You need to set Base Url in order to use this option"
                RdoWorkingDirFile.SetValue(ToolTipService.ShowOnDisabledProperty, True)
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets the element.
        ''' </summary>
        ''' <value>The element.</value>
        Public Property Element As HyperlinkElement Implements IHyperlinkDialog.Element
            Get
                Return readUi()
            End Get
            Set(value As HyperlinkElement)
                Me._theOriginalElement = value
                Me.updateUI(value)
            End Set
        End Property


        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub

        Public Overloads Function ShowDialog() As Boolean? Implements IDialogBase.ShowDialog
            Return MyBase.ShowDialog()
        End Function

        Public Property IDialog_Owner As Window Implements IDialog.Owner
            Get
                Return MyBase.Owner
            End Get
            Set(value As Window)
                MyBase.Owner = value
            End Set
        End Property


        ''' <summary>
        ''' Reads the UI.
        ''' </summary>
        ''' <returns>HyperlinkElement.</returns>
        Private Function readUi() As HyperlinkElement
            Dim theElement As New HyperlinkElement With {
                .TheActiveHtmlElement = Me._theOriginalElement.TheActiveHtmlElement,
                .CssStyle = Me._theOriginalElement.CssStyle,
                .CssClassName = Me._theOriginalElement.CssClassName,
                .Name = Me._theOriginalElement.Name,
                .Id = Me._theOriginalElement.Id,
                .OnClickJavascript = Me._theOriginalElement.OnClickJavascript,
                .HrefUrl = TxtURL.Text,
                .Title = TxtToolTip.Text.Trim(),
                .InnerHtml = TxtInnerHtml.Text.Trim(),
                .Target = If(Equals(ChkTargetIncluded.IsChecked, True), CmbTarget.Text, Nothing)
            }

            Return theElement
        End Function

        ''' <summary>
        ''' Updates the UI.
        ''' </summary>
        ''' <param name="element">The element.</param>
        Private Sub updateUI(element As HyperlinkElement)
            If Me.IsLocalResourceSelectionDisabled Then
                RdoLocalFile.IsEnabled = False
                BtnBrowseFile.IsEnabled = False
            End If

            ' preserve design-time defaults on empty fields.
            If Not String.IsNullOrEmpty(element.HrefUrl) Then
                TxtURL.Text = element.HrefUrl
            End If
            If element.IsRelativePathOrUrl Then
                RdoWorkingDirFile.IsChecked = True
            ElseIf element.IsLocalFilePath AndAlso Not Me.IsLocalResourceSelectionDisabled Then
                RdoLocalFile.IsChecked = True
            End If
            If Not String.IsNullOrEmpty(element.Title) Then
                TxtToolTip.Text = element.Title
            End If
            If Not String.IsNullOrEmpty(element.Target) Then
                CmbTarget.Text = element.Target
            End If
            ChkTargetIncluded.IsChecked = (Not String.IsNullOrEmpty(element.Target))
            If Not String.IsNullOrEmpty(element.InnerHtml) Then
                TxtInnerHtml.Text = element.InnerHtml
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating whether this instance is local resource selection disabled.
        ''' </summary>
        ''' <value><c>true</c> if this instance is local resource selection disabled; otherwise, <c>false</c>.</value>
        Public Property IsLocalResourceSelectionDisabled As Boolean Implements IHyperlinkDialog.IsLocalResourceSelectionDisabled

        ''' <summary>
        ''' Gets or sets a value indicating whether [remove link].
        ''' </summary>
        ''' <value><c>true</c> if [remove link]; otherwise, <c>false</c>.</value>
        Public Property RemoveLink As Boolean Implements IHyperlinkDialog.RemoveLink
            Get
                Return Equals(ChkRemoveLink.IsChecked, True)
            End Get
            Set(value As Boolean)
                ChkRemoveLink.IsChecked = value
            End Set
        End Property

        ''' <summary>
        ''' When true, the editor sets the tooltip Title to
        ''' "Ctrl+Click to view" when the user leaves Title blank. Passthrough
        ''' auto-property so this custom dialog satisfies the interface contract.
        ''' </summary>
        Public Property UseCtrlClickTooltipDefault As Boolean Implements IHyperlinkDialog.UseCtrlClickTooltipDefault

        ''' <summary>
        ''' Handles the Click event of the lnkBtnCheck control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnCheck_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Try
                Process.Start(TxtURL.Text)
            Catch
                'MessageBox.Show(EditorLangRes.ErrorURL, EditorLangRes.ErrorCallingURL);
            End Try
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkTargetIncluded control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkTargetIncluded_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CmbTarget.IsEnabled = Equals(ChkTargetIncluded.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnBrowseFile control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub btnBrowseFile_Click(sender As Object, e As RoutedEventArgs)
            Dim myDialog As New OpenFileDialog()
            If myDialog.ShowDialog() = True Then
                TxtURL.Text = myDialog.FileName
            End If
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the RdoLocalFile control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub rdoLocalFile_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            updateBrowserButtonState()
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the RdoWorkingDirFile control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub rdoWorkingDirFile_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            updateBrowserButtonState()

            Dim isLocalDir As Boolean? = Me._theOriginalElement.IsBaseUrlALocalFolder()

            LnkBtnImportToBaseFolder.IsEnabled = RdoWorkingDirFile.IsEnabled _
                                                 AndAlso Equals(RdoWorkingDirFile.IsChecked, True) _
                                                 AndAlso isLocalDir.HasValue _
                                                 AndAlso isLocalDir.Value
            ChkOverwrite.IsEnabled = RdoWorkingDirFile.IsEnabled _
                                     AndAlso Equals(RdoWorkingDirFile.IsChecked, True) _
                                     AndAlso isLocalDir.HasValue _
                                     AndAlso isLocalDir.Value
            LnkBtnBrowseWD.IsEnabled = RdoWorkingDirFile.IsEnabled _
                                       AndAlso Equals(RdoWorkingDirFile.IsChecked, True) _
                                       AndAlso isLocalDir.HasValue _
                                       AndAlso isLocalDir.Value
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnBrowseWD control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnBrowseWD_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Dim srcUrlDialog As New OpenFileDialog With {
                .InitialDirectory = Me._theOriginalElement.BaseUrl
            }

            If srcUrlDialog.ShowDialog() <> True Then
                Return
            End If

            Dim directory As String = Path.GetDirectoryName(srcUrlDialog.FileName)
            Dim baseUrl As String = Me._theOriginalElement.BaseUrl
            If Me._theOriginalElement.BaseUrl.EndsWith("\") OrElse Me._theOriginalElement.BaseUrl.EndsWith("/") Then
                baseUrl = Me._theOriginalElement.BaseUrl.Remove(Me._theOriginalElement.BaseUrl.Length - 1)
            End If
            If baseUrl.Equals(directory, StringComparison.OrdinalIgnoreCase) Then
                TxtURL.Text = Path.GetFileName(srcUrlDialog.FileName)
            Else
                Dim dlgResult As MessageBoxResult = MessageBox.Show("The file you selected is not from the base directory for relative path. Do you want to import that file to your base directory ? If you choose YES, then it will be imported to the Base Directory, otherwise the link target will be treated as absolute path file.", "Selected file is not from the base directory.", MessageBoxButton.YesNoCancel, MessageBoxImage.Question)

                Select Case dlgResult
                    Case MessageBoxResult.Yes
                        Dim newFilePath As String = Path.Combine(baseUrl, If(Path.GetFileName(srcUrlDialog.FileName), String.Empty))
                        Dim i As Integer = 0

                        While File.Exists(newFilePath) AndAlso (Not Equals(ChkOverwrite.IsChecked, True))
                            i += 1
                            Dim newFileName As String = Path.GetFileNameWithoutExtension(srcUrlDialog.FileName) & i & Path.GetExtension(srcUrlDialog.FileName)
                            newFilePath = Path.Combine(baseUrl, newFileName)
                        End While

                        If File.Exists(newFilePath) Then
                            Try
                                File.SetAttributes(newFilePath, FileAttributes.Normal)
                                File.Delete(newFilePath)
                                File.Copy(srcUrlDialog.FileName, newFilePath)
                            Catch err As Exception
                                MessageBox.Show(err.Message, "Error copying file to the destination")
                            End Try
                        Else
                            File.Copy(srcUrlDialog.FileName, newFilePath)
                        End If
                        TxtURL.Text = Path.GetFileName(newFilePath)
                    Case MessageBoxResult.No
                        TxtURL.Text = srcUrlDialog.FileName
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnImportToBaseFolder control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnImportToBaseFolder_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Dim myDialog As New OpenFileDialog()

            If myDialog.ShowDialog() <> True Then
                Return
            End If

            Dim newFilePath As String = Path.Combine(Me._theOriginalElement.BaseUrl, If(Path.GetFileName(myDialog.FileName), String.Empty))
            Dim i As Integer = 0

            While File.Exists(newFilePath) AndAlso (Not Equals(ChkOverwrite.IsChecked, True))
                i += 1
                Dim newFileName As String = Path.GetFileNameWithoutExtension(myDialog.FileName) & i & Path.GetExtension(myDialog.FileName)
                newFilePath = Path.Combine(Me._theOriginalElement.BaseUrl, newFileName)
            End While

            If File.Exists(newFilePath) Then
                Try
                    File.SetAttributes(newFilePath, FileAttributes.Normal)
                    File.Delete(newFilePath)
                    File.Copy(myDialog.FileName, newFilePath)
                Catch err As Exception
                    MessageBox.Show(err.Message, "Error copying file to the destination")
                End Try
            Else
                File.Copy(myDialog.FileName, newFilePath)
            End If

            TxtURL.Text = Path.GetFileName(newFilePath)
        End Sub

        ''' <summary>
        ''' Handles the OnRightButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TwoButtonPanel_OnOnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            Close()
        End Sub

        ''' <summary>
        ''' Handles the OnLeftButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TwoButtonPanel_OnOnLeftButtonClicked(sender As Object, e As RoutedEventArgs)
            If String.IsNullOrEmpty(TxtURL.Text) Then
                MessageBox.Show("Please provide Url")
                TxtURL.Focus()
                Return
            End If

            DialogResult = True
        End Sub

        ''' <summary>
        ''' Updates the state of the browser button.
        ''' </summary>
        Private Sub updateBrowserButtonState()
            BtnBrowseFile.IsEnabled = Equals(RdoLocalFile.IsChecked, True)
        End Sub
    End Class
End Namespace
