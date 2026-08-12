Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports Microsoft.Win32
Imports SpiceLogic.HtmlEditor.Abstractions.Entities
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Extensions
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports Color = System.Drawing.Color
Imports ColorConverter = System.Drawing.ColorConverter

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class TablePropertiesDialog
    ''' </summary>
    Partial Public Class TablePropertiesDialog
        Implements ITableDialog

        ''' <summary>
        ''' The _element
        ''' </summary>
        Private _element As TableElement

        ''' <summary>
        ''' The _table cell dialog
        ''' </summary>
        ''' <summary>
        ''' The _background picture URL
        ''' </summary>
        Private _backgroundPictureUrl As String = String.Empty

        Private ReadOnly _propertiesAffected As New List(Of String) From {
            "Rows",
            "Columns",
            "Width",
            "Height",
            "Caption",
            "BorderWidth",
            "CellPadding",
            "CellSpacing",
            "BorderColor",
            "BorderAttr",
            "BorderStyle",
            "BgColor",
            "BorderCollapse",
            "SummaryDescription",
            "ID",
            "Name",
            "CssClassName",
            "BackGround"
        }

        ''' <summary>
        ''' The dialog service
        ''' </summary>
        Private ReadOnly _dialogService As IDialogService


        ''' <summary>
        ''' Initializes a new instance of the <see cref="TablePropertiesDialog" /> class.
        ''' </summary>
        ''' <param name="dialogService">The dialog service.</param>
        Public Sub New(dialogService As IDialogService)
            _dialogService = dialogService

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the TablePropertiesDialog control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub TablePropertiesDialog_Loaded(sender As Object, e As RoutedEventArgs)
            BtnCellProperties.Visibility = If(Me.GetOrInitCellElement() IsNot Nothing, Visibility.Visible, Visibility.Hidden)
            TxtId.Visibility = Visibility.Collapsed
            TxtName.Visibility = Visibility.Collapsed
        End Sub

        ''' <summary>
        ''' Gets or sets the element.
        ''' </summary>
        ''' <value>The element.</value>
        Public Property Element As TableElement Implements ITableDialogBase.Element
            Get
                If _element Is Nothing Then _element = New TableElement(Nothing)

                _element.ResetValues(Me._propertiesAffected)

                _element.Rows = CInt(NumRows.Value)
                _element.Columns = CInt(NumCols.Value)

                If Equals(ChkCellPadding.IsChecked, True) Then
                    _element.CellPadding = CInt(NumCellPadding.Value)
                End If
                If Equals(ChkCellSpacing.IsChecked, True) Then
                    _element.CellSpacing = CInt(NumCellSpacing.Value)
                End If

                If Equals(ChkWidth.IsChecked, True) AndAlso CmbWidthUnit.SelectedItem IsNot Nothing Then
                    _element.Width = Convert.ToInt32(TxtWidth.Text.Trim())
                    _element.WidthUnit = CmbWidthUnit.Text
                End If

                If Equals(ChkHeight.IsChecked, True) AndAlso CmbHeightUnit.SelectedItem IsNot Nothing Then
                    _element.Height = Convert.ToInt32(TxtHeight.Text.Trim())
                    _element.HeightUnit = CmbHeightUnit.Text
                End If

                If Equals(ChkCaption.IsChecked, True) Then
                    _element.Caption = TxtCaption.Text.Trim()
                End If

                If Equals(ChkBorderWidth.IsChecked, True) Then
                    If Equals(ChkBorderStyle.IsChecked, True) AndAlso CmbBorderStyle.SelectedItem IsNot Nothing Then
                        _element.BorderStyle = CmbBorderStyle.Text
                        _element.BorderWidth = CInt(NumBorderWidth.Value)
                    Else
                        _element.BorderAttr = CInt(NumBorderWidth.Value)
                    End If
                End If

                If LnkButtonBorderColor.IsEnabled AndAlso Equals(ChkBorderColor.IsChecked, True) Then
                    Dim brush As SolidColorBrush = TryCast(TxtBorderColor.Background, SolidColorBrush)
                    If brush IsNot Nothing Then
                        _element.BorderColor = WpfColorTranslator.ToHtml(brush.Color)
                    End If
                End If

                If Equals(ChkBgColor.IsChecked, True) Then
                    Dim brush As SolidColorBrush = TryCast(TxtBgColor.Background, SolidColorBrush)
                    If brush IsNot Nothing Then
                        _element.BgColor = WpfColorTranslator.ToHtml(brush.Color)
                    End If
                End If

                _element.SummaryDescription = TxtSummaryDescription.Text
                _element.BorderCollapse = ChkBorderCollapse.IsEnabled AndAlso Equals(ChkBorderCollapse.IsChecked, True)
                _element.Id = TxtId.Text.Trim()
                _element.Name = TxtName.Text.Trim()
                _element.CssClassName = TxtClassName.Text.Trim()
                _element.CssStyle = TxtCss.Text.Trim()

                If Equals(ChkBackgroundPicture.IsChecked, True) Then
                    _element.BackGround = ImgBackgroundPicture.Source.ToString()
                End If

                _element.BorderToAll = Equals(ChkBorderToAll.IsChecked, True)

                Return _element
            End Get
            Set(value As TableElement)
                _element = value
                If _element Is Nothing Then
                    Return
                End If

                ' every element→UI assignment is gated on the
                ' element carrying data, so design-time defaults survive otherwise.
                If _element.Rows > 0 Then
                    NumRows.Value = _element.Rows
                End If
                If _element.Columns > 0 Then
                    NumCols.Value = _element.Columns
                End If

                If _element.Width.HasValue Then
                    ChkWidth.IsChecked = True
                    TxtWidth.Text = _element.Width.Value.ToString(CultureInfo.InvariantCulture)
                    If _element.WidthUnit IsNot Nothing Then
                        CmbWidthUnit.SelectedItem = _element.WidthUnit
                    End If
                End If

                If _element.Height.HasValue Then
                    ChkHeight.IsChecked = True
                    TxtHeight.Text = _element.Height.Value.ToString(CultureInfo.InvariantCulture)
                    If _element.HeightUnit IsNot Nothing Then
                        CmbHeightUnit.SelectedItem = _element.HeightUnit
                    End If
                End If

                If Not String.IsNullOrEmpty(_element.Caption) Then
                    ChkCaption.IsChecked = True
                    TxtCaption.Text = _element.Caption
                End If

                If _element.BorderWidth.HasValue OrElse _element.BorderAttr.HasValue Then
                    ChkBorderWidth.IsChecked = True
                End If

                If Not String.IsNullOrEmpty(_element.BorderStyle) Then
                    ChkBorderStyle.IsChecked = True
                    CmbBorderStyle.IsEnabled = True
                    ChkBorderCollapse.IsEnabled = True
                    ChkBorderColor.IsEnabled = True
                    CmbBorderStyle.Text = _element.BorderStyle
                    If Equals(ChkBorderWidth.IsChecked, True) Then
                        NumBorderWidth.Value = If(_element.BorderWidth, 0)
                    End If
                ElseIf Equals(ChkBorderWidth.IsChecked, True) Then
                    NumBorderWidth.Value = If(_element.BorderAttr, If(_element.BorderWidth, 0))
                End If

                If _element.CellPadding.HasValue Then
                    ChkCellPadding.IsChecked = True
                    NumCellPadding.Value = _element.CellPadding.Value
                End If

                If _element.CellSpacing.HasValue Then
                    ChkCellSpacing.IsChecked = True
                    NumCellSpacing.Value = _element.CellSpacing.Value
                End If

                If Not String.IsNullOrEmpty(_element.BorderColor) Then
                    ChkBorderColor.IsChecked = True
                    Dim converter As New ColorConverter()
                    Dim drawingColor As Color = CType(converter.ConvertFromString(_element.BorderColor), Color)
                    Dim borderColor As System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B)
                    TxtBorderColor.Background = New SolidColorBrush(borderColor)
                End If

                If Not String.IsNullOrEmpty(_element.BgColor) Then
                    ChkBgColor.IsChecked = True
                    LnkBtnBackgroundPicture.IsEnabled = True
                    Dim converter As New ColorConverter()
                    Dim backgroundColor As Color = CType(converter.ConvertFromString(_element.BgColor), Color)
                    Dim color As System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(
                        backgroundColor.A,
                        backgroundColor.R,
                        backgroundColor.G,
                        backgroundColor.B
                    )
                    TxtBgColor.Background = New SolidColorBrush(color)
                End If

                If _element.BorderCollapse IsNot Nothing AndAlso _element.BorderCollapse.Value Then
                    ChkBorderCollapse.IsChecked = True
                End If
                ' preserve design-time defaults on empty fields.
                If Not String.IsNullOrEmpty(_element.SummaryDescription) Then
                    TxtSummaryDescription.Text = _element.SummaryDescription
                End If
                If Not String.IsNullOrEmpty(_element.Id) Then
                    TxtId.Text = _element.Id
                End If
                If Not String.IsNullOrEmpty(_element.Name) Then
                    TxtName.Text = _element.Name
                End If
                If Not String.IsNullOrEmpty(_element.CssClassName) Then
                    TxtClassName.Text = _element.CssClassName
                End If
                '///////////////
                ChkBackgroundPicture.IsChecked = Not String.IsNullOrEmpty(_element.BackGround)

                If Equals(ChkBackgroundPicture.IsChecked, True) Then
                    ImgBackgroundPicture.Source = New BitmapImage(New Uri(_element.BackGround))
                End If

                Dim cssText As String = _element.GetCssStyleWithoutProperties(Me._propertiesAffected)
                If Not String.IsNullOrEmpty(cssText) Then
                    TxtCss.Text = cssText
                End If
                ChkBorderToAll.IsChecked = True
            End Set
        End Property

#Region "------------ UI Event Handlers --------------"

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
            CmbWidthUnit.IsEnabled = Equals(ChkWidth.IsChecked, True)
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
            CmbHeightUnit.IsEnabled = Equals(ChkHeight.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBorderWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBorderWidth_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            NumBorderWidth.IsEnabled = Equals(ChkBorderWidth.IsChecked, True)
            ChkBorderStyle.IsEnabled = Equals(ChkBorderWidth.IsChecked, True)

            '            chkBorderStyle.Checked = chkBorderWidth.Checked;
            CmbBorderStyle.IsEnabled = Equals(ChkBorderWidth.IsChecked, True) AndAlso Equals(ChkBorderStyle.IsChecked, True)
            If Not Equals(ChkBorderWidth.IsChecked, True) Then ChkBorderToAll.IsChecked = True
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBgColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBgColor_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            LnkButtonBgColor.IsEnabled = Equals(ChkBgColor.IsChecked, True)
            TxtBgColor.IsEnabled = Equals(ChkBgColor.IsChecked, True)
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

            LnkButtonBorderColor.IsEnabled = Equals(ChkBorderColor.IsChecked, True)
            TxtBorderColor.IsEnabled = Equals(ChkBorderColor.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkCellPadding control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkCellPadding_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            NumCellPadding.IsEnabled = Equals(ChkCellPadding.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkCellSpacing control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkCellSpacing_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            NumCellSpacing.IsEnabled = Equals(ChkCellSpacing.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkBtnBackgroundPicture control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnBackgroundPicture_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            Try
                Dim srcUrlDialog As New OpenFileDialog With {
                    .Title = "Please Select an image file.",
                    .RestoreDirectory = True,
                    .Filter = "Image Files|*.png;*.bmp;*.gif;*.jpg|All files(*.*)|*.*",
                    .FilterIndex = 0,
                    .Multiselect = False
                }

                If srcUrlDialog.ShowDialog() = True Then
                    _backgroundPictureUrl = srcUrlDialog.FileName
                    ImgBackgroundPicture.Source = New BitmapImage(New Uri(_backgroundPictureUrl))
                End If
            Catch
                ' ignored
            End Try
        End Sub

        ''' <summary>
        ''' Opens a window and returns only when the newly opened window is closed
        ''' </summary>
        ''' <returns></returns>
        Public Overloads Function ShowDialog() As Boolean? Implements IDialogBase.ShowDialog
            Return MyBase.ShowDialog()
        End Function

        ''' <summary>
        ''' Handles the Click event of the btnCellProperties control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub btnCellProperties_Click(sender As Object, e As RoutedEventArgs)
            Dim tblCellElement As TableCellElement = Me.GetOrInitCellElement()
            If tblCellElement Is Nothing Then
                MessageBox.Show(
                    "No cells were found.",
                    "Error")
                Return
            End If

            Using tableCellDialog As ITableCellDialog = _dialogService.TableCellDialog
                tableCellDialog.Element = tblCellElement
                Using tableCellDialog.LockOverrideSettingsToAllCells()
                    If tableCellDialog.ShowDialog() = True Then
                        Me.Element.CellElement = tableCellDialog.Element
                    End If
                End Using
            End Using
        End Sub

        Private Function GetOrInitCellElement() As TableCellElement
            Return Me.Element.GetFirstCellElement()
        End Function

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkCaption control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkCaption_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TxtCaption.IsEnabled = Equals(ChkCaption.IsChecked, True)
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
            ChkBorderCollapse.IsEnabled = Equals(ChkBorderStyle.IsChecked, True)
            ChkBorderColor.IsEnabled = Equals(ChkBorderStyle.IsChecked, True)
            If Not Equals(ChkBorderStyle.IsChecked, True) Then ChkBorderToAll.IsChecked = True
        End Sub

        ''' <summary>
        ''' Handles the ValueChanged event of the NumBorderWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedPropertyChangedEventArgs{T}" /> instance containing the event data.</param>
        Private Sub numBorderWidth_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
            If Math.Abs(NumBorderWidth.Value) < 0.01 Then
                ChkBorderToAll.IsChecked = True
            End If
        End Sub

#End Region

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub

        Public Property IDialog_Owner As Window Implements IDialog.Owner
            Get
                Return MyBase.Owner
            End Get
            Set(value As Window)
                MyBase.Owner = value
            End Set
        End Property

        ''' <summary>
        ''' Handles the Click event of the LnkButtonBorderColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkButtonBorderColor_OnClick(sender As Object, e As RoutedEventArgs)
            Dim backgroundBrush As SolidColorBrush = TryCast(TxtBorderColor.Background, SolidColorBrush)
            Dim color As New System.Windows.Media.Color()
            If backgroundBrush IsNot Nothing Then
                color = backgroundBrush.Color
            Else
                Dim convertFromString As Object = System.Windows.Media.ColorConverter.ConvertFromString("Black")
                If convertFromString IsNot Nothing Then
                    color = CType(convertFromString, System.Windows.Media.Color)
                End If
            End If

            Using colorDialog As IColorPickerDialog = _dialogService.ColorPickerDialog
                colorDialog.StartingColor = color

                If colorDialog.ShowDialog() = True Then
                    TxtBorderColor.Background = New SolidColorBrush(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the Click event of the LnkButtonBgColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkButtonBgColor_OnClick(sender As Object, e As RoutedEventArgs)
            Dim backgroundBrush As SolidColorBrush = TryCast(TxtBgColor.Background, SolidColorBrush)
            Dim color As New System.Windows.Media.Color()
            If backgroundBrush IsNot Nothing Then
                color = backgroundBrush.Color
            Else
                Dim convertFromString As Object = System.Windows.Media.ColorConverter.ConvertFromString("Black")
                If convertFromString IsNot Nothing Then
                    color = CType(convertFromString, System.Windows.Media.Color)
                End If
            End If

            Using colorDialog As IColorPickerDialog = _dialogService.ColorPickerDialog
                colorDialog.StartingColor = color

                If colorDialog.ShowDialog() = True Then
                    TxtBgColor.Background = New SolidColorBrush(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the ChkBackgroundPicture control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub chkBackgroundPicture_OnCheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            LnkBtnBackgroundPicture.IsEnabled = Equals(ChkBackgroundPicture.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Handles the OnRightButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub TwoButtonPanel_OnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            'cancel button

            Close()
        End Sub

        ''' <summary>
        ''' Handles the OnLeftButtonClicked event of the TwoButtonPanel control
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TwoButtonPanel_OnLeftButtonClicked(sender As Object, e As RoutedEventArgs)
            'ok button

            DialogResult = True
        End Sub
    End Class
End Namespace
