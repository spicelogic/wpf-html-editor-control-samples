Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media
Imports SpiceLogic.HtmlEditor.Abstractions.Entities
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Extensions
Imports Color = System.Drawing.Color
Imports ColorConverter = System.Drawing.ColorConverter
Imports SpiceLogic.HtmlEditor.WPF.Models.Services

Namespace Global.CustomDialog.Dialogs

    ''' <summary>
    ''' Class TableCellPropertiesDialog
    ''' </summary>
    Partial Public Class TableCellPropertiesDialog
        Implements ITableCellDialog

        ''' <summary>
        ''' The _element
        ''' </summary>
        Private _element As TableCellElement

        ''' <summary>
        ''' The dialog service
        ''' </summary>
        Private ReadOnly _dialogService As IDialogService

        Private ReadOnly _propertiesAffected As New List(Of String) From {
            "BgColor",
            "CssClassName",
            "Width",
            "WidthUnit",
            "Height",
            "HeightUnit",
            "HorizontalAlign",
            "VerticalAlign",
            "NoWrap"
        }

        ''' <summary>
        ''' Initializes a new instance of the <see cref="TableCellPropertiesDialog" /> class.
        ''' </summary>
        ''' <param name="dialogService">The dialog service.</param>
        Public Sub New(dialogService As IDialogService)
            _dialogService = dialogService

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Gets or sets the element.
        ''' </summary>
        ''' <value>The element.</value>
        Public Property Element As TableCellElement Implements ITableCellDialogBase.Element
            Get
                _element.ResetStyles(Me._propertiesAffected)

                '                _element.BgColor = chkBgColor.Checked ? ColorTranslator.ToHtml(txtBgColor.BackColor) : null;


                If Equals(ChkBgColor.IsChecked, True) Then
                    Dim backgroundColorBrush As SolidColorBrush = CType(TxtBgColor.Background, SolidColorBrush)
                    _element.BgColor = WpfColorTranslator.ToHtml(backgroundColorBrush.Color)
                Else
                    _element.BgColor = Nothing
                End If


                _element.CssClassName = TxtClassName.Text.Trim()
                _element.CssStyle = TxtCss.Text.Trim()

                If Equals(ChkWidth.IsChecked, True) AndAlso CmbWidthUnit.SelectedItem IsNot Nothing Then
                    _element.Width = Convert.ToInt32(TxtWidth.Text.Trim())
                    _element.WidthUnit = CmbWidthUnit.Text
                End If

                If Equals(ChkHeight.IsChecked, True) AndAlso CmbHeightUnit.SelectedItem IsNot Nothing Then
                    _element.Height = Convert.ToInt32(TxtHeight.Text.Trim())
                    _element.HeightUnit = CmbHeightUnit.Text
                End If

                If CmbHorizontalAlign.SelectedItem IsNot Nothing Then
                    _element.HorizontalAlign = CmbHorizontalAlign.Text
                End If

                If CmbVerticalAlign.SelectedItem IsNot Nothing Then
                    _element.VerticalAlign = CmbVerticalAlign.Text
                End If
                _element.NoWrap = Equals(ChkNoWrap.IsChecked, True)
                _element.OverrideSettingsToAllCells = Equals(ChkOverrideSettings4Cells.IsChecked, True)
                Return _element
            End Get
            Set(value As TableCellElement)
                _element = value

                If _element Is Nothing Then
                    Return
                End If

                ChkBgColor.IsChecked = Not String.IsNullOrEmpty(_element.BgColor)
                If Equals(ChkBgColor.IsChecked, True) Then
                    Dim converter As New ColorConverter()
                    Dim convertFromString As Object = converter.ConvertFromString(_element.BgColor)
                    If convertFromString IsNot Nothing Then
                        Dim backgroundDrawingColor As Color = CType(convertFromString, Color)
                        Dim backgroundColor As System.Windows.Media.Color = System.Windows.Media.Color.FromArgb(
                            backgroundDrawingColor.A,
                            backgroundDrawingColor.R,
                            backgroundDrawingColor.G,
                            backgroundDrawingColor.B)
                        TxtBgColor.Background = New SolidColorBrush(backgroundColor)
                    End If
                End If

                ' preserve design-time defaults on empty fields.
                If Not String.IsNullOrEmpty(_element.CssClassName) Then
                    TxtClassName.Text = _element.CssClassName
                End If
                If _element.Width.HasValue Then
                    ChkWidth.IsChecked = True
                    TxtWidth.Text = _element.Width.Value.ToString(CultureInfo.InvariantCulture)
                    If _element.WidthUnit IsNot Nothing Then
                        CmbWidthUnit.Text = _element.WidthUnit
                    End If
                End If

                If _element.Height.HasValue Then
                    ChkHeight.IsChecked = True
                    TxtHeight.Text = _element.Height.Value.ToString(CultureInfo.InvariantCulture)
                    If _element.HeightUnit IsNot Nothing Then
                        CmbHeightUnit.Text = _element.HeightUnit
                    End If
                End If

                If _element.HorizontalAlign IsNot Nothing Then
                    CmbHorizontalAlign.Text = _element.HorizontalAlign
                End If

                If _element.VerticalAlign IsNot Nothing Then
                    CmbVerticalAlign.Text = _element.VerticalAlign
                End If
                If _element.NoWrap Then
                    ChkNoWrap.IsChecked = True
                End If
                If _element.OverrideSettingsToAllCells Then
                    ChkOverrideSettings4Cells.IsChecked = True
                End If

                Dim cssText As String = _element.GetCssStyleWithoutProperties(Me._propertiesAffected)
                If Not String.IsNullOrEmpty(cssText) Then
                    TxtCss.Text = cssText
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the cell attribute collection.
        ''' </summary>
        ''' <value>The cell attribute collection.</value>
        Public ReadOnly Property CellAttributeCollection As NameValueCollection
            Get
                Dim myColl As New NameValueCollection()

                If Equals(ChkWidth.IsChecked, True) Then
                    myColl.Add("width", String.Concat(TxtWidth.Text.Trim(), CmbWidthUnit.Text.Trim()))
                End If

                If Equals(ChkHeight.IsChecked, True) Then
                    myColl.Add("height", String.Concat(TxtHeight.Text.Trim(), CmbHeightUnit.Text.Trim()))
                End If

                If Equals(ChkBgColor.IsChecked, True) Then
                    Dim solidColorBrush As SolidColorBrush = CType(TxtBgColor.Background, SolidColorBrush)
                    myColl.Add("bgcolor", WpfColorTranslator.ToHtml(solidColorBrush.Color))
                End If

                If CmbHorizontalAlign.SelectedIndex <> 0 Then
                    myColl.Add("align", CmbHorizontalAlign.Text)
                End If

                If CmbVerticalAlign.SelectedIndex <> 0 Then
                    myColl.Add("valign", CmbVerticalAlign.Text)
                End If

                If Equals(ChkNoWrap.IsChecked, True) Then
                    myColl.Add("nowrap", "nowrap")
                End If

                Return myColl
            End Get
        End Property

        ''' <summary>
        ''' Gets the table cell attribute string.
        ''' </summary>
        ''' <value>The table cell attribute string.</value>
        Public ReadOnly Property TableCellAttributeString As String
            Get
                Dim tableCellAttributes As String() = {}
                Dim tableCellStyleAttributes As String() = {}

                If Equals(ChkWidth.IsChecked, True) Then
                    Array.Resize(tableCellStyleAttributes, tableCellStyleAttributes.Length + 1)
                    tableCellStyleAttributes(tableCellStyleAttributes.Length - 1) = $"width: {TxtWidth.Text.Trim()}{CmbWidthUnit.Text.Trim()}"
                End If

                If Equals(ChkHeight.IsChecked, True) Then
                    Array.Resize(tableCellStyleAttributes, tableCellStyleAttributes.Length + 1)
                    tableCellStyleAttributes(tableCellStyleAttributes.Length - 1) = $"height: {TxtHeight.Text.Trim()}{CmbHeightUnit.Text.Trim()}"
                End If

                If Equals(ChkBgColor.IsChecked, True) Then
                    Array.Resize(tableCellStyleAttributes, tableCellStyleAttributes.Length + 1)
                    Dim solidBackgroundBrush As SolidColorBrush = CType(TxtBgColor.Background, SolidColorBrush)
                    tableCellStyleAttributes(tableCellStyleAttributes.Length - 1) =
                        $"background-color: {WpfColorTranslator.ToHtml(solidBackgroundBrush.Color)}"
                End If

                If tableCellStyleAttributes.Length <> 0 Then
                    Dim tableCellStyleText As String = $"style = ""{String.Join("; ", tableCellStyleAttributes)}"""
                    Array.Resize(tableCellAttributes, tableCellAttributes.Length + 1)
                    tableCellAttributes(tableCellAttributes.Length - 1) = tableCellStyleText
                End If

                If CmbHorizontalAlign.SelectedIndex <> 0 Then
                    Array.Resize(tableCellAttributes, tableCellAttributes.Length + 1)
                    tableCellAttributes(tableCellAttributes.Length - 1) = $"align=""{CmbHorizontalAlign.Text}"""
                End If

                If CmbVerticalAlign.SelectedIndex <> 0 Then
                    Array.Resize(tableCellAttributes, tableCellAttributes.Length + 1)
                    tableCellAttributes(tableCellAttributes.Length - 1) = $"valign=""{CmbVerticalAlign.Text}"""
                End If

                If Equals(ChkNoWrap.IsChecked, True) Then
                    Array.Resize(tableCellAttributes, tableCellAttributes.Length + 1)
                    tableCellAttributes(tableCellAttributes.Length - 1) = "nowrap=""nowrap"""
                End If

                If Not String.IsNullOrEmpty(TxtClassName.Text.Trim()) Then
                    Array.Resize(tableCellAttributes, tableCellAttributes.Length + 1)
                    tableCellAttributes(tableCellAttributes.Length - 1) = "class=""" & TxtClassName.Text.Trim() & """"
                End If

                Return String.Join(" ", tableCellAttributes)
            End Get
        End Property

        ''' <summary>
        ''' Get a lock for a property [override cell attributes].
        ''' </summary>
        Public Function LockOverrideSettingsToAllCells() As IDisposable Implements ITableCellDialogBase.LockOverrideSettingsToAllCells
            Return New LockOverrideSettingsToAllCellsClass(Me)
        End Function

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
        ''' Gets a value indicating whether [override cell attributes].
        ''' </summary>
        ''' <value><c>true</c> if [override cell attributes]; otherwise, <c>false</c>.</value>
        Public ReadOnly Property OverrideCellAttributes As Boolean
            Get
                Return Equals(ChkOverrideSettings4Cells.IsChecked, True)
            End Get
        End Property

#Region "--------------- UI Event Handlers ------------------"

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

#End Region

        Private Class LockOverrideSettingsToAllCellsClass
            Implements IDisposable

            Private ReadOnly _dialog As TableCellPropertiesDialog
            Private ReadOnly _initiallyEnabled As Boolean

            Public Sub New(dialog As TableCellPropertiesDialog)
                _dialog = dialog
                _initiallyEnabled = _dialog.ChkOverrideSettings4Cells.IsEnabled
                _dialog.ChkOverrideSettings4Cells.IsEnabled = False
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                _dialog.ChkOverrideSettings4Cells.IsEnabled = _initiallyEnabled
            End Sub
        End Class

        ''' <summary>
        ''' Handles the Click event of the LnkBtnBgColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="routedEventArgs">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        Private Sub lnkBtnBgColor_Click(sender As Object, routedEventArgs As RoutedEventArgs)
            ShowDialogForChoosingColor()
        End Sub

        Private Sub ShowDialogForChoosingColor()
            Dim bgColorBrush As SolidColorBrush = TryCast(TxtBgColor.Background, SolidColorBrush)
            Dim color As New System.Windows.Media.Color()

            If bgColorBrush IsNot Nothing Then
                color = bgColorBrush.Color
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
        ''' Handles the OnLeftButtonClicked event of the TwoButtonPanel control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub TwoButtonPanel_OnLeftButtonClicked(sender As Object, e As RoutedEventArgs)
            'ok button

            DialogResult = True
        End Sub

        ''' <summary>
        ''' Handles the OnRightButtonClicked event of the TwoButtonPanel control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub TwoButtonPanel_OnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            'cancel

            Close()
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
        ''' Handles the Checked and Unchecked events of the ChkBgColor control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub chkBgColor_OnCheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            LnkBtnBgColor.IsEnabled = Equals(ChkBgColor.IsChecked, True)
        End Sub
    End Class
End Namespace
