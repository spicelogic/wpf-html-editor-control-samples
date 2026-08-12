Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports Microsoft.Win32
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder
Imports SpiceLogic.HtmlEditor.WPF.Extensions
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.ToolbarModule

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucBackground
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Background", "background-color,background-image,background-repeat,background-attachment,background-position-x,background-position-y")>
    Partial Public Class ucBackground
        Implements IEditorStylePage

#Region "Preset of possible values"

        ''' <summary>
        ''' The _ bg repeat
        ''' </summary>
        Private ReadOnly _bgRepeat As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ bg attachment
        ''' </summary>
        Private ReadOnly _bgAttachment As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ bg position X
        ''' </summary>
        Private ReadOnly _bgPositionX As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ bg position Y
        ''' </summary>
        Private ReadOnly _bgPositionY As New List(Of KeyValuePair(Of String, String))()
#End Region

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

        ''' <summary>
        ''' A method creating the color dialog
        ''' </summary>
        Private ReadOnly _createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucBackground"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        ''' <param name="createColorDialogMethod">A method creating the color dialog</param>
        Public Sub New(dict As Dictionary(Of String, String), createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate) 'the argument order is important (Activator.CreateInstance uses it)
#Region "Initialize presets"
            _bgRepeat.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _bgRepeat.Add(New KeyValuePair(Of String, String)("Tile in horizontal direction", "repeat-x"))
            _bgRepeat.Add(New KeyValuePair(Of String, String)("Tile in vertical direction", "repeat-y"))
            _bgRepeat.Add(New KeyValuePair(Of String, String)("Tile in both directions", "repeat"))
            _bgRepeat.Add(New KeyValuePair(Of String, String)("Do not tile", "no-repeat"))

            _bgAttachment.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _bgAttachment.Add(New KeyValuePair(Of String, String)("Scrolling background", "scroll"))
            _bgAttachment.Add(New KeyValuePair(Of String, String)("Fixed background", "fixed"))

            _bgPositionX.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _bgPositionX.Add(New KeyValuePair(Of String, String)("Left", "left"))
            _bgPositionX.Add(New KeyValuePair(Of String, String)("Center", "center"))
            _bgPositionX.Add(New KeyValuePair(Of String, String)("Right", "right"))
            _bgPositionX.Add(New KeyValuePair(Of String, String)("Custom", ""))

            _bgPositionY.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _bgPositionY.Add(New KeyValuePair(Of String, String)("Top", "top"))
            _bgPositionY.Add(New KeyValuePair(Of String, String)("Center", "center"))
            _bgPositionY.Add(New KeyValuePair(Of String, String)("Bottom", "bottom"))
            _bgPositionY.Add(New KeyValuePair(Of String, String)("Custom", ""))
#End Region

            _dict = dict
            _createColorDialogMethod = createColorDialogMethod

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucBackground control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucBackground_Loaded(sender As Object, e As RoutedEventArgs)
            CbTiling.ItemsSource = _bgRepeat
            CbTiling.DisplayMemberPath = "Key"
            CbTiling.SelectedValuePath = "Value"
            CbTiling.SelectedIndex = 0

            CbScrolling.ItemsSource = _bgAttachment
            CbScrolling.DisplayMemberPath = "Key"
            CbScrolling.SelectedValuePath = "Value"
            CbScrolling.SelectedIndex = 0

            CbHorizontal.ItemsSource = _bgPositionX
            CbHorizontal.DisplayMemberPath = "Key"
            CbHorizontal.SelectedValuePath = "Value"
            CbHorizontal.SelectedIndex = 0

            CbVertical.ItemsSource = _bgPositionY
            CbVertical.DisplayMemberPath = "Key"
            CbVertical.SelectedValuePath = "Value"
            CbVertical.SelectedIndex = 0

            Dim value As String = Nothing

            If _dict.TryGetValue("background-color", value) Then
                If If(value, "").Trim().Equals("transparent", StringComparison.InvariantCultureIgnoreCase) Then
                    CbBgColorTransparent.IsChecked = True
                Else
                    TxtBgColor.Background = New SolidColorBrush(WpfColorTranslator.FromHtml(value))
                    CbBgColorTransparent.IsChecked = False
                End If
            End If

            If _dict.TryGetValue("background-repeat", value) Then
                Dim val As String = value.Trim().ToLowerInvariant()
                For i As Integer = 0 To _bgRepeat.Count - 1
                    If String.Equals(val, _bgRepeat(i).Value) Then
                        CbTiling.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("background-attachment", value) Then
                Dim val As String = value.Trim().ToLowerInvariant()
                For i As Integer = 0 To _bgAttachment.Count - 1
                    If String.Equals(val, _bgAttachment(i).Value) Then
                        CbScrolling.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("background-position-x", value) Then
                Dim handled As Boolean = False

                Dim i As Integer = 0
                Do While i < _bgPositionX.Count AndAlso Not handled
                    If String.Equals(value, _bgPositionX(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbHorizontal.SelectedIndex = i
                        handled = True
                    End If
                    i += 1
                Loop

                If Not handled Then
                    For j As Integer = 0 To CbHorCustType.Items.Count - 1
                        Dim customMeasureItems As String = TryCast(TryCast(CbHorCustType.Items(j), ComboBoxItem)?.Content, String)
                        If customMeasureItems IsNot Nothing AndAlso value.EndsWith(customMeasureItems, StringComparison.InvariantCultureIgnoreCase) Then
                            CbHorizontal.SelectedIndex = 4 ' Custom
                            CbHorCustType.SelectedIndex = j
                            TbHorCust.Text = value.Substring(0, value.Length - customMeasureItems.Length)
                            Exit For
                        End If
                    Next
                End If
            End If

            If _dict.TryGetValue("background-position-y", value) Then
                Dim handled As Boolean = False

                Dim i As Integer = 0
                Do While i < _bgPositionY.Count AndAlso Not handled
                    If String.Equals(value, _bgPositionY(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbVertical.SelectedIndex = i
                        handled = True
                    End If
                    i += 1
                Loop

                If Not handled Then
                    For j As Integer = 0 To CbVerCustType.Items.Count - 1
                        Dim customMeasureItems As String = TryCast(TryCast(CbVerCustType.Items(j), ComboBoxItem)?.Content, String)
                        If customMeasureItems IsNot Nothing AndAlso value.EndsWith(customMeasureItems, StringComparison.InvariantCultureIgnoreCase) Then
                            CbVertical.SelectedIndex = 4 ' Custom
                            CbVerCustType.SelectedIndex = j
                            TbVerCust.Text = value.Substring(0, value.Length - customMeasureItems.Length)
                            Exit For
                        End If
                    Next
                End If
            End If

            If _dict.TryGetValue("background-image", value) Then
                If If(value, "").Trim().Equals("none", StringComparison.InvariantCultureIgnoreCase) Then
                    CbDoNotUseBackground.IsChecked = True
                Else
                    CbDoNotUseBackground.IsChecked = False
                    Try
                        If value IsNot Nothing Then
                            TbBgImage.Text = Regex.Match(value, "url\((.+?)\)", RegexOptions.IgnoreCase).Groups(1).Value
                        End If
                    Catch
                        TbBgImage.Text = value
                    End Try
                End If
            Else
                tbBgImage_TextChanged(Me, New RoutedEventArgs())
            End If
        End Sub

#Region "UI handling"

        ''' <summary>
        ''' Handles the Checked and Unchecked event of the CbBgColorTransparent control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbBgColorTransparent_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TxtBgColor.IsEnabled = Not Equals(CbBgColorTransparent.IsChecked, True)
        End Sub

        ''' <summary>
        ''' Handles the TextChanged event of the TbBgImage control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub tbBgImage_TextChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = TbBgImage.Text.Trim().Length > 0
            CbScrolling.IsEnabled = enabled
            CbTiling.IsEnabled = enabled
            GbPosition.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbHorizontal control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbHorizontal_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbHorizontal.SelectedIndex = 4
            CbHorCustType.IsEnabled = enabled
            TbHorCust.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbVertical control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbVertical_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbVertical.SelectedIndex = 4
            CbVerCustType.IsEnabled = enabled
            TbVerCust.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the CbDoNotUseBackground control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbDoNotUseBackground_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            If Equals(CbDoNotUseBackground.IsChecked, True) Then
                CbScrolling.IsEnabled = False
                CbTiling.IsEnabled = False
                BtChooseBgImage.IsEnabled = False
                TbBgImage.IsEnabled = False
                GbPosition.IsEnabled = False
            Else
                CbScrolling.IsEnabled = True
                CbTiling.IsEnabled = True
                BtChooseBgImage.IsEnabled = True
                TbBgImage.IsEnabled = True
                GbPosition.IsEnabled = True
                tbBgImage_TextChanged(Me, New RoutedEventArgs())
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtChooseBgImage control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btChooseBgImage_Click(sender As Object, e As RoutedEventArgs)
            Dim dlgBgImage As New OpenFileDialog()

            If dlgBgImage.ShowDialog() = True Then
                TbBgImage.Text = dlgBgImage.FileName
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("background-color")
            _dict.Remove("background-image")
            _dict.Remove("background-repeat")
            _dict.Remove("background-attachment")
            _dict.Remove("background-position-x")
            _dict.Remove("background-position-y")

            If Equals(CbBgColorTransparent.IsChecked, True) Then
                _dict("background-color") = "transparent"
            Else
                _dict("background-color") = If(Equals(CbBgColorTransparent.IsChecked, True), "transparent", WpfColorTranslator.ToHtml(CType(TxtBgColor.Background, SolidColorBrush).Color))
            End If

            If Equals(CbDoNotUseBackground.IsChecked, True) Then
                _dict("background-image") = "none"
            Else
                _dict("background-image") = If(String.IsNullOrEmpty(TbBgImage.Text.Trim()), String.Empty, $"url({TbBgImage.Text.Trim()})")
                _dict("background-repeat") = CStr(CbTiling.SelectedValue)
                _dict("background-attachment") = CStr(CbScrolling.SelectedValue)

                If CbHorizontal.SelectedIndex <> 4 Then
                    _dict("background-position-x") = CStr(CbHorizontal.SelectedValue)
                Else
                    _dict("background-position-x") = TbHorCust.Text & CbHorCustType.Text
                End If

                If CbVertical.SelectedIndex <> 4 Then
                    _dict("background-position-y") = CStr(CbVertical.SelectedValue)
                Else
                    _dict("background-position-y") = TbVerCust.Text & CbVerCustType.Text
                End If
            End If
        End Sub

        ''' <summary>
        ''' Handles the PreviewMouseDown event of the TxtBgColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub TxtBgColor_OnPreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
            Dim solidColorBrush As SolidColorBrush = TryCast(TxtBgColor.Background, SolidColorBrush)
            If solidColorBrush IsNot Nothing Then
                Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                    colorDialog.StartingColor = solidColorBrush.Color
                    If colorDialog.ShowDialog() = True Then
                        TxtBgColor.Background = New SolidColorBrush(colorDialog.SelectedColor)
                    End If
                End Using
            End If
        End Sub
    End Class
End Namespace
