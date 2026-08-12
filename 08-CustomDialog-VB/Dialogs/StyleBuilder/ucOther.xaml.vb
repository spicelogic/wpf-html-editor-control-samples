Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucOther
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Other", "filter,behavior,cursor,border-collapse,table-layout")>
    Partial Public Class ucOther
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible values"

        ''' <summary>
        ''' The _ cursor
        ''' </summary>
        Private ReadOnly _cursor As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ border collapse
        ''' </summary>
        Private ReadOnly _borderCollapse As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ table layout
        ''' </summary>
        Private ReadOnly _tableLayout As New List(Of KeyValuePair(Of String, String))()
#End Region

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucOther"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        Public Sub New(dict As Dictionary(Of String, String))
            _dict = dict

#Region "Fill presets"
            _cursor.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _cursor.Add(New KeyValuePair(Of String, String)("Auto", "auto"))
            _cursor.Add(New KeyValuePair(Of String, String)("Default", "default"))
            _cursor.Add(New KeyValuePair(Of String, String)("Crosshair", "crosshair"))
            _cursor.Add(New KeyValuePair(Of String, String)("Hand", "hand"))
            _cursor.Add(New KeyValuePair(Of String, String)("Move", "move"))
            _cursor.Add(New KeyValuePair(Of String, String)("Top resize", "n-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Bottom resize", "s-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Left resize", "w-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Right resize", "e-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Top-left resize", "nw-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Bottom-left resize", "sw-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Top-right resize", "ne-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Bottom-right resize", "se-resize"))
            _cursor.Add(New KeyValuePair(Of String, String)("Text", "text"))
            _cursor.Add(New KeyValuePair(Of String, String)("Hourglass", "wait"))
            _cursor.Add(New KeyValuePair(Of String, String)("Help", "help"))

            _borderCollapse.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _borderCollapse.Add(New KeyValuePair(Of String, String)("Separate cell borders", "separate"))
            _borderCollapse.Add(New KeyValuePair(Of String, String)("Collapse cell borders", "collapse"))

            _tableLayout.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _tableLayout.Add(New KeyValuePair(Of String, String)("Auto", "auto"))
            _tableLayout.Add(New KeyValuePair(Of String, String)("Fixed layout", "fixed"))
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("filter")
            _dict.Remove("behavior")

            _dict("cursor") = CStr(CbCursor.SelectedValue)
            _dict("border-collapse") = CStr(CbBorders.SelectedValue)
            _dict("table-layout") = CStr(CbLayout.SelectedValue)

            If TbFilter.Text.Trim().Length > 0 Then
                _dict("filter") = TbFilter.Text
            End If
            If TbURL.Text.Trim().Length > 0 Then
                _dict("behavior") = $"url({TbURL.Text})"
            End If
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucOther control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub ucOther_Loaded(sender As Object, e As EventArgs)
#Region "set data sources"
            CbCursor.ItemsSource = _cursor
            CbCursor.DisplayMemberPath = "Key"
            CbCursor.SelectedValuePath = "Value"
            CbCursor.SelectedIndex = 0

            CbBorders.ItemsSource = _borderCollapse
            CbBorders.DisplayMemberPath = "Key"
            CbBorders.SelectedValuePath = "Value"
            CbBorders.SelectedIndex = 0

            CbLayout.ItemsSource = _tableLayout
            CbLayout.DisplayMemberPath = "Key"
            CbLayout.SelectedValuePath = "Value"
            CbLayout.SelectedIndex = 0
#End Region

#Region "parse"
            Dim value As String = Nothing
            If _dict.TryGetValue("cursor", value) Then
                Dim n As Integer = _cursor.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _cursor(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbCursor.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("border-collapse", value) Then
                Dim n As Integer = _borderCollapse.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _borderCollapse(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbBorders.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("table-layout", value) Then
                Dim n As Integer = _tableLayout.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _tableLayout(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbLayout.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("filter", value) Then
                TbFilter.Text = value
            End If

            If _dict.TryGetValue("behavior", value) Then
                If value.StartsWith("url(", StringComparison.InvariantCultureIgnoreCase) AndAlso value.EndsWith(")") Then
                    TbURL.Text = value.Substring(4, value.Length - 5)
                End If
            End If
#End Region
        End Sub
    End Class
End Namespace
