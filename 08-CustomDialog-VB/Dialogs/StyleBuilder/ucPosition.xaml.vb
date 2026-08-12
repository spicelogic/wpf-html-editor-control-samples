Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucPosition
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Position", "position,top,left,width,height,z-index")>
    Partial Public Class ucPosition
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible values"

        ''' <summary>
        ''' The _ position
        ''' </summary>
        Private ReadOnly _position As New List(Of KeyValuePair(Of String, String))()
#End Region

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucPosition"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        Public Sub New(dict As Dictionary(Of String, String))
            _dict = dict

#Region "Initialize presets"
            _position.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _position.Add(New KeyValuePair(Of String, String)("Position in normal flow", "static"))
            _position.Add(New KeyValuePair(Of String, String)("Offset from normal flow", "relative"))
            _position.Add(New KeyValuePair(Of String, String)("Absolutely position", "absolute"))
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("position")
            _dict.Remove("top")
            _dict.Remove("left")
            _dict.Remove("width")
            _dict.Remove("height")
            _dict.Remove("z-index")

            If CbPositionMode.SelectedIndex >= 2 Then
                If TbLeft.Text.Trim().Length > 0 Then
                    _dict("left") = TbLeft.Text & CbLeftType.Text
                End If
                If TbTop.Text.Trim().Length > 0 Then
                    _dict("top") = TbTop.Text & CbTopType.Text
                End If

                If CbPositionMode.SelectedIndex = 3 Then
                    _dict("z-index") = TbZIndex.Text
                End If
            End If

            If TbHeight.Text.Trim().Length > 0 Then
                _dict("height") = TbHeight.Text & CbHeightType.Text
            End If
            If TbWidth.Text.Trim().Length > 0 Then
                _dict("width") = TbWidth.Text & CbWidthType.Text
            End If

            _dict("position") = CStr(CbPositionMode.SelectedValue)
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucPosition control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucPosition_Loaded(sender As Object, e As RoutedEventArgs)
#Region "set data sources"
            CbPositionMode.ItemsSource = _position
            CbPositionMode.DisplayMemberPath = "Key"
            CbPositionMode.SelectedValuePath = "Value"
            CbPositionMode.SelectedIndex = 0
#End Region

#Region "parse"
            Dim value As String = Nothing
            If _dict.TryGetValue("left", value) Then
                Dim n As Integer = CbLeftType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim leftType As String = TryCast(TryCast(CbLeftType.Items(i), ComboBoxItem)?.Content, String)
                    If leftType IsNot Nothing AndAlso value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase) Then
                        TbLeft.Text = value.Substring(0, value.Length - leftType.Length)
                        CbLeftType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("top", value) Then
                Dim n As Integer = CbTopType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim topType As String = TryCast(TryCast(CbTopType.Items(i), ComboBoxItem)?.Content, String)
                    If topType IsNot Nothing AndAlso value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase) Then
                        TbTop.Text = value.Substring(0, value.Length - topType.Length)
                        CbTopType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("width", value) Then
                Dim n As Integer = CbWidthType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim widthType As String = TryCast(TryCast(CbWidthType.Items(i), ComboBoxItem)?.Content, String)
                    If widthType IsNot Nothing AndAlso value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase) Then
                        TbWidth.Text = value.Substring(0, value.Length - widthType.Length)
                        CbWidthType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("height", value) Then
                Dim n As Integer = CbHeightType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim heightType As String = TryCast(TryCast(CbHeightType.Items(i), ComboBoxItem)?.Content, String)
                    If heightType IsNot Nothing AndAlso value.EndsWith(heightType, StringComparison.InvariantCultureIgnoreCase) Then
                        TbHeight.Text = value.Substring(0, value.Length - heightType.Length)
                        CbHeightType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("z-index", value) Then
                TbZIndex.Text = value
            End If

            If _dict.TryGetValue("position", value) Then
                Dim n As Integer = _position.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _position(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPositionMode.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
#End Region
        End Sub

#Region "UI handlers"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbPositionMode control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbPositionMode_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbPositionMode.SelectedIndex >= 2
            TbLeft.IsEnabled = enabled
            CbLeftType.IsEnabled = enabled
            TbTop.IsEnabled = enabled
            CbTopType.IsEnabled = enabled

            TbZIndex.IsEnabled = CbPositionMode.SelectedIndex = 3
        End Sub
#End Region
    End Class
End Namespace
