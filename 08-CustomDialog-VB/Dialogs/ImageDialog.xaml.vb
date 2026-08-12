Imports System
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports Microsoft.Win32
Imports SpiceLogic.HtmlEditor.Abstractions.Entities
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports Color = System.Drawing.Color
Imports Size = System.Drawing.Size

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class ImageDialog
    ''' </summary>
    Partial Public Class ImageDialog
        Implements IImageDialog

        ''' <summary>
        ''' The _the original element
        ''' </summary>
        Private _theOriginalElement As ImageElement
        ''' <summary>
        ''' The _width to height aspect ratio
        ''' </summary>
        Private _widthToHeightAspectRatio As Single?

        ''' <summary>
        ''' The dialog service
        ''' </summary>
        Private ReadOnly _dialogService As IDialogService

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ImageDialog" /> class.
        ''' </summary>
        ''' <param name="dialogService">The dialog service.</param>
        Public Sub New(dialogService As IDialogService)
            _dialogService = dialogService

            InitializeComponent()
        End Sub


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
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ImageInsertDialog control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub ImageInsertDialog_Loaded(sender As Object, e As RoutedEventArgs)
            If String.IsNullOrEmpty(_theOriginalElement.BaseUrl) Then
                RdoWorkingDirFile.IsEnabled = False
                RdoWorkingDirFile.ToolTip = "You need to set Base Url in order to use this option"
                RdoWorkingDirFile.SetValue(ToolTipService.ShowOnDisabledProperty, True)
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtnBrowseFile control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub btnBrowseFile_Click(sender As Object, e As RoutedEventArgs)
            Dim srcUrlDialog As New OpenFileDialog With {
                .RestoreDirectory = True,
                .Filter = "Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
                .FilterIndex = 0,
                .Multiselect = False
            }

            If srcUrlDialog.ShowDialog() = True Then
                TxtUrl.Text = srcUrlDialog.FileName
                Dim imageFileName As String = srcUrlDialog.FileName
                SetImageDimensionAndAspectRatio(imageFileName)
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

            UpdateBrowserButtonState()
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

            Dim isLocalDir As Boolean? = Me._theOriginalElement.IsBaseUrlALocalFolder()

            LnkBtnImportToBaseFolder.IsEnabled = RdoWorkingDirFile.IsEnabled AndAlso Equals(RdoWorkingDirFile.IsChecked, True) AndAlso isLocalDir.HasValue AndAlso isLocalDir.Value
            ChkOverwrite.IsEnabled = RdoWorkingDirFile.IsEnabled AndAlso Equals(RdoWorkingDirFile.IsChecked, True) AndAlso isLocalDir.HasValue AndAlso isLocalDir.Value
            LnkBtnBrowseWD.IsEnabled = RdoWorkingDirFile.IsEnabled AndAlso Equals(RdoWorkingDirFile.IsChecked, True) AndAlso isLocalDir.HasValue AndAlso isLocalDir.Value
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnBrowseWD control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        ''' <exception cref="Exception">File doesn't exist</exception>
        Private Sub lnkBtnBrowseWD_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Dim srcUrlDialog As New OpenFileDialog With {
                .RestoreDirectory = True,
                .Filter = "Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
                .FilterIndex = 0,
                .Multiselect = False,
                .InitialDirectory = Me._theOriginalElement.BaseUrl
            }

            If srcUrlDialog.ShowDialog() = True Then
                Dim baseUrl As String = If(Me._theOriginalElement.BaseUrl, String.Empty)
                If baseUrl.EndsWith("\") OrElse baseUrl.EndsWith("/") Then
                    baseUrl = baseUrl.Remove(baseUrl.Length - 1)
                End If

                Dim selectedFileDirectory As String = Path.GetDirectoryName(srcUrlDialog.FileName)
                If selectedFileDirectory Is Nothing Then
                    Return
                End If

                If selectedFileDirectory.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase) Then
                    Dim relativePath As String = srcUrlDialog.FileName.Replace(baseUrl, "")
                    If relativePath.StartsWith("\") Then
                        relativePath = relativePath.Remove(0, 1)
                    End If

                    TxtUrl.Text = relativePath
                Else
                    Dim dlgResult As MessageBoxResult = MessageBox.Show("The image you selected is not from the base directory for relative path. Do you want to import that file to your base directory ? If you choose YES, then it will be imported to the Base Directory, otherwise it will be treated as absolute path image file.", "Selected image is not from the base directory.", MessageBoxButton.YesNoCancel, MessageBoxImage.Question)
                    If dlgResult = MessageBoxResult.Yes Then
                        Dim newFilePath As String = Path.Combine(baseUrl, Path.GetFileName(srcUrlDialog.FileName))
                        Dim i As Integer = 0
                        While File.Exists(newFilePath) AndAlso Not Equals(ChkOverwrite.IsChecked, True)
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
                        TxtUrl.Text = Path.GetFileName(newFilePath)
                    ElseIf dlgResult = MessageBoxResult.No Then
                        TxtUrl.Text = srcUrlDialog.FileName
                    End If
                End If

                If Not String.IsNullOrEmpty(TxtUrl.Text) Then
                    Dim fullImagePath As String = If(File.Exists(TxtUrl.Text), TxtUrl.Text, Path.Combine(baseUrl, TxtUrl.Text))
                    If File.Exists(fullImagePath) Then
                        SetImageDimensionAndAspectRatio(fullImagePath)
                    Else
                        Throw New Exception("File doesn't exist")
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkAlignment control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkAlignment_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CmbAlign.IsEnabled = Equals(ChkAlignment.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBorderThickness control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBorderThickness_CheckedChanged(sender As Object, e As RoutedEventArgs)
            TxtBorder.IsEnabled = Equals(ChkBorderThickness.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkHeight control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkHeight_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TxtHeight.IsEnabled = Equals(ChkHeight.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkWidth_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TxtWidth.IsEnabled = Equals(ChkWidth.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBorderColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBorderColor_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            LnkBtnBgColor.IsEnabled = Equals(ChkBorderColor.IsChecked, True)
            TxtBgColor.IsEnabled = Equals(ChkBorderColor.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBorderStyle control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBorderStyle_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CmbBorderStyle.IsEnabled = Equals(ChkBorderStyle.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the TextChanged event of the TxtHeight control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub txtHeight_TextChanged(sender As Object, e As RoutedEventArgs)
            If TxtHeight.IsFocused AndAlso Equals(ChkLockAspectRatio.IsChecked, True) AndAlso (Me._widthToHeightAspectRatio.HasValue AndAlso Me._widthToHeightAspectRatio.Value > 0) Then
                Try
                    Dim value As String = TxtHeight.Text
                    Dim digitPart As String = Nothing
                    Dim unitPart As String = Nothing
                    GetValueAndUnit(value, digitPart, unitPart)

                    If digitPart.Length > 0 Then
                        Dim height As Single = Single.Parse(digitPart)
                        If height > 0 Then
                            Dim width As Single = Me._widthToHeightAspectRatio.Value * height
                            TxtWidth.Text = CInt(Math.Round(width)) & unitPart
                        End If
                    End If
                Catch
                    ' ignored
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Handles the TextChanged event of the TxtWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub txtWidth_TextChanged(sender As Object, e As EventArgs)
            If TxtWidth.IsFocused AndAlso Equals(ChkLockAspectRatio.IsChecked, True) AndAlso (Me._widthToHeightAspectRatio.HasValue AndAlso Me._widthToHeightAspectRatio.Value > 0) Then
                Try
                    Dim value As String = TxtWidth.Text
                    Dim digitPart As String = Nothing
                    Dim unitPart As String = Nothing
                    GetValueAndUnit(value, digitPart, unitPart)

                    If digitPart.Length > 0 Then
                        Dim width As Single = Single.Parse(digitPart)
                        If width > 0 Then
                            Dim height As Single = width / Me._widthToHeightAspectRatio.Value
                            TxtHeight.Text = CInt(Math.Round(height)) & unitPart
                        End If
                    End If
                Catch
                    ' ignored
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnImportToBaseFolder control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnImportToBaseFolder_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Dim myDialog As New OpenFileDialog With {
                .RestoreDirectory = True,
                .Filter = "Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
                .FilterIndex = 0,
                .Multiselect = False
            }

            If myDialog.ShowDialog() = True Then
                Dim newFilePath As String = Path.Combine(Me._theOriginalElement.BaseUrl, Path.GetFileName(myDialog.FileName))
                Dim i As Integer = 0
                While File.Exists(newFilePath) AndAlso Not Equals(ChkOverwrite.IsChecked, True)
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
                TxtUrl.Text = Path.GetFileName(newFilePath)
            End If
        End Sub

        ''' <summary>
        ''' Gets or sets the element.
        ''' </summary>
        ''' <value>The element.</value>
        Public Property Element As ImageElement Implements IImageDialog.Element
            Get
                Return ReadUi()
            End Get
            Set(value As ImageElement)
                Me._theOriginalElement = value
                Me.UpdateUi(value)
            End Set
        End Property

        ''' <summary>
        ''' Reads the UI.
        ''' </summary>
        ''' <returns>ImageElement.</returns>
        Private Function ReadUi() As ImageElement
            Dim src As String = Nothing

            If Equals(ChkInsertLocalBase64.IsChecked, True) Then
                Try
                    If File.Exists(TxtUrl.Text) Then
                        src = ImageElement.GetBase64DataUrlForLocalImage(TxtUrl.Text)
                    End If
                Catch ex As Exception
                    ' ignored
                End Try
            End If

            Dim theElement As New ImageElement With {
                .TheActiveHtmlElement = Me._theOriginalElement.TheActiveHtmlElement,
                .CssStyle = Me._theOriginalElement.CssStyle,
                .CssClassName = Me._theOriginalElement.CssClassName,
                .Name = Me._theOriginalElement.Name,
                .Id = Me._theOriginalElement.Id,
                .OnClickJavascript = Me._theOriginalElement.OnClickJavascript,
                .SrcUrl = If(src, TxtUrl.Text)
            }

            If Equals(ChkWidth.IsChecked, True) Then
                theElement.Width = TxtWidth.Text.Trim()
            End If
            If Equals(ChkHeight.IsChecked, True) Then
                theElement.Height = TxtHeight.Text.Trim()
            End If
            If Equals(ChkBorderColor.IsChecked, True) Then
                Dim bgBrush As SolidColorBrush = TryCast(TxtBgColor.Background, SolidColorBrush)
                If bgBrush IsNot Nothing Then
                    theElement.BorderColor = Color.FromArgb(
                        bgBrush.Color.A,
                        bgBrush.Color.R,
                        bgBrush.Color.G,
                        bgBrush.Color.B)
                End If
            End If

            If Equals(ChkBorderStyle.IsChecked, True) Then
                theElement.BorderStyle = CmbBorderStyle.Text
            End If
            If Equals(ChkBorderThickness.IsChecked, True) AndAlso Not String.IsNullOrEmpty(TxtBorder.Text) Then
                theElement.Border = Convert.ToInt32(TxtBorder.Text)
            End If
            theElement.Title = TxtToolTip.Text.Trim()
            theElement.AlternativeText = TxtAlt.Text.Trim()
            If Equals(ChkAlignment.IsChecked, True) AndAlso Not String.IsNullOrEmpty(CmbAlign.Text) Then
                theElement.Align = CmbAlign.Text
            End If
            Return theElement
        End Function

        ''' <summary>
        ''' Updates the UI.
        ''' </summary>
        ''' <param name="element">The element.</param>
        Private Sub UpdateUi(element As ImageElement)
            If Me.IsLocalResourceSelectionDisabled Then
                RdoLocalFile.IsEnabled = False
                BtnBrowseFile.IsEnabled = False
            End If

            ' preserve design-time defaults on empty fields.
            If Not String.IsNullOrEmpty(element.SrcUrl) Then
                TxtUrl.Text = element.SrcUrl
            End If
            If element.IsRelativePathOrUrl Then
                RdoWorkingDirFile.IsChecked = True
            ElseIf element.IsLocalFilePath AndAlso Not Me.IsLocalResourceSelectionDisabled Then
                RdoLocalFile.IsChecked = True
            End If
            If Not String.IsNullOrEmpty(element.Title) Then
                TxtToolTip.Text = element.Title
            End If
            If Not String.IsNullOrEmpty(element.AlternativeText) Then
                TxtAlt.Text = element.AlternativeText
            End If
            If Not String.IsNullOrEmpty(element.Align) Then
                CmbAlign.Text = element.Align
                ChkAlignment.IsChecked = True
            End If
            If element.Border.HasValue Then
                TxtBorder.Text = element.Border.Value.ToString(CultureInfo.InvariantCulture)
                ChkBorderThickness.IsChecked = True
            End If
            If Not String.IsNullOrEmpty(element.Width) Then
                TxtWidth.Text = element.Width
                ChkWidth.IsChecked = True
            End If
            If Not String.IsNullOrEmpty(element.Height) Then
                TxtHeight.Text = element.Height
                ChkHeight.IsChecked = True
            End If

            If element.BorderColor.HasValue Then
                ChkBorderColor.IsChecked = True
                Dim bgColor As System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(
                    element.BorderColor.Value.A,
                    element.BorderColor.Value.R,
                    element.BorderColor.Value.G,
                    element.BorderColor.Value.B)
                Dim bgBrush As New SolidColorBrush(bgColor)
                TxtBgColor.Background = bgBrush
            End If
            ' when no color, preserve design-time Background.

            If Not String.IsNullOrEmpty(element.BorderStyle) Then
                ChkBorderStyle.IsChecked = True
                CmbBorderStyle.Text = element.BorderStyle
            End If

            If Equals(ChkHeight.IsChecked, True) AndAlso Equals(ChkWidth.IsChecked, True) Then
                Try
                    Dim widthDigitPart As String = Nothing
                    Dim widthUnitDiscard As String = Nothing
                    GetValueAndUnit(TxtWidth.Text, widthDigitPart, widthUnitDiscard)

                    Dim heightDigitPart As String = Nothing
                    Dim heightUnitDiscard As String = Nothing
                    GetValueAndUnit(TxtHeight.Text, heightDigitPart, heightUnitDiscard)
                    If widthDigitPart.Length > 0 AndAlso heightDigitPart.Length > 0 Then
                        Dim width As Single = Single.Parse(widthDigitPart)
                        Dim height As Single = Single.Parse(heightDigitPart)
                        If width > 0 AndAlso height > 0 Then
                            Me._widthToHeightAspectRatio = width / height
                        Else
                            Me._widthToHeightAspectRatio = Nothing
                        End If
                    End If
                Catch
                    Me._widthToHeightAspectRatio = Nothing
                End Try
            Else
                Me._widthToHeightAspectRatio = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Sets the image dimension and aspect ratio.
        ''' </summary>
        ''' <param name="imageFileName">Name of the image file.</param>
        Private Sub SetImageDimensionAndAspectRatio(imageFileName As String)
            Dim theImageDimension As Size? = ImageUtils.GetImageDimension(imageFileName)
            If theImageDimension.HasValue Then
                ChkHeight.IsChecked = True
                TxtHeight.Text = theImageDimension.Value.Height.ToString(CultureInfo.InvariantCulture)
                ChkWidth.IsChecked = True
                TxtWidth.Text = theImageDimension.Value.Width.ToString(CultureInfo.InvariantCulture)
                If theImageDimension.Value.Width > 0 AndAlso theImageDimension.Value.Height > 0 Then
                    Me._widthToHeightAspectRatio = theImageDimension.Value.Width / CSng(theImageDimension.Value.Height)
                Else
                    Me._widthToHeightAspectRatio = Nothing
                End If
            Else
                Me._widthToHeightAspectRatio = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Gets the value and unit.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <param name="digitPart">The digit part.</param>
        ''' <param name="unitPart">The unit part.</param>
        Private Shared Sub GetValueAndUnit(value As String, ByRef digitPart As String, ByRef unitPart As String)
            Const digitRegEx As String = "\d+"
            digitPart = Regex.Match(value, digitRegEx, RegexOptions.IgnoreCase Or RegexOptions.Compiled).Groups(0).Value
            unitPart = value.Replace(digitPart, "").Trim()
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating whether this instance is local resource selection disabled.
        ''' </summary>
        ''' <value><c>true</c> if this instance is local resource selection disabled; otherwise, <c>false</c>.</value>
        Public Property IsLocalResourceSelectionDisabled As Boolean Implements IImageDialog.IsLocalResourceSelectionDisabled

        ''' <summary>
        ''' When true, the Image dialog writes width/height as inline
        ''' CSS in CssStyle ("style=width:240px;height:120px") instead of HTML
        ''' width=/height= attributes. Passthrough auto-property so this custom
        ''' dialog satisfies the interface contract.
        ''' </summary>
        Public Property UseInlineStyleForDimensions As Boolean Implements IImageDialog.UseInlineStyleForDimensions

        Private Sub UpdateBrowserButtonState()
            BtnBrowseFile.IsEnabled = Equals(RdoLocalFile.IsChecked, True)
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
            If String.IsNullOrEmpty(TxtUrl.Text) Then
                MessageBox.Show("Please provide Image Url")
                TxtUrl.Focus()
                Return
            End If

            DialogResult = True
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnBgColor control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="routedEventArgs">The event data</param>
        Private Sub lnkBtnBgColor_OnClick(sender As Object, routedEventArgs As RoutedEventArgs)
            Using colorDialog As IColorPickerDialog = _dialogService.ColorPickerDialog
                If colorDialog.ShowDialog() = True Then
                    TxtBgColor.Background = New SolidColorBrush(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles OnChecked event of the RdoInternetUrl RadioButton
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The event argument.</param>
        Private Sub RdoInternetUrl_OnChecked(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            ChkInsertLocalBase64.IsChecked = False
            ChkInsertLocalBase64.IsEnabled = False
        End Sub

        ''' <summary>
        ''' Handles OnUnchecked event of the RdoInternet control.
        ''' </summary>
        ''' <param name="sender">The event sender.</param>
        ''' <param name="e">The event sender.</param>
        Private Sub RdoInternet_OnUnchecked(sender As Object, e As RoutedEventArgs)
            ChkInsertLocalBase64.IsEnabled = True
        End Sub
    End Class
End Namespace
