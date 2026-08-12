Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucLayout
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Layout", "clip,visibility,display,float,clear,overflow,page-break-before,page-break-after")>
    Partial Public Class ucLayout
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible value"

        ''' <summary>
        ''' The _ visibility
        ''' </summary>
        Private ReadOnly _visibility As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ display
        ''' </summary>
        Private ReadOnly _display As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ float
        ''' </summary>
        Private ReadOnly _float As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ clear
        ''' </summary>
        Private ReadOnly _clear As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ overflow
        ''' </summary>
        Private ReadOnly _overflow As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ page break before
        ''' </summary>
        Private ReadOnly _pageBreakBefore As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ page break after
        ''' </summary>
        Private ReadOnly _pageBreakAfter As List(Of KeyValuePair(Of String, String))
#End Region

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucLayout"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        Public Sub New(dict As Dictionary(Of String, String))
            _dict = dict

#Region "Fill lists"
            _visibility.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _visibility.Add(New KeyValuePair(Of String, String)("Hidden", "hidden"))
            _visibility.Add(New KeyValuePair(Of String, String)("Visible", "visible"))

            _display.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _display.Add(New KeyValuePair(Of String, String)("Do not display", "none"))
            _display.Add(New KeyValuePair(Of String, String)("As a block element", "block"))
            _display.Add(New KeyValuePair(Of String, String)("As an inflow element", "inline"))

            _float.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _float.Add(New KeyValuePair(Of String, String)("Don't allow text on sides", "none"))
            _float.Add(New KeyValuePair(Of String, String)("To the right", "right"))
            _float.Add(New KeyValuePair(Of String, String)("To the left", "left"))

            _clear.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _clear.Add(New KeyValuePair(Of String, String)("On either side", "none"))
            _clear.Add(New KeyValuePair(Of String, String)("Only on right", "right"))
            _clear.Add(New KeyValuePair(Of String, String)("Only on left", "left"))
            _clear.Add(New KeyValuePair(Of String, String)("Do not allow", "both"))

            _overflow.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _overflow.Add(New KeyValuePair(Of String, String)("Use scrollbars if needed", "auto"))
            _overflow.Add(New KeyValuePair(Of String, String)("Always use scrollbars", "scroll"))
            _overflow.Add(New KeyValuePair(Of String, String)("Content is not clipped", "visible"))
            _overflow.Add(New KeyValuePair(Of String, String)("Content is clipped", "hidden"))

            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("Auto", "auto"))
            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("Force a page break", "always"))
            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("No page break", "avoid"))
            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("Until a blank left page", "left"))
            _pageBreakBefore.Add(New KeyValuePair(Of String, String)("Until a blank right page", "right"))

            _pageBreakAfter = New List(Of KeyValuePair(Of String, String))(_pageBreakBefore)
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("clip")

            _dict("visibility") = CStr(CbVisibility.SelectedValue)
            _dict("display") = CStr(CbDisplay.SelectedValue)
            _dict("float") = CStr(CbAllowFloatingObject.SelectedValue)
            _dict("clear") = CStr(CbAllowTextToFlow.SelectedValue)
            _dict("overflow") = CStr(CbOverflow.SelectedValue)
            _dict("page-break-before") = CStr(CbPbBefore.SelectedValue)
            _dict("page-break-after") = CStr(CbPbAfter.SelectedValue)

            ' clip
            Dim top As String = TbTop.Text.Trim()
            Dim right As String = TbRight.Text.Trim()
            Dim bottom As String = TbBottom.Text.Trim()
            Dim left As String = TbLeft.Text.Trim()

            If top.Length + right.Length + bottom.Length + left.Length > 0 Then
                ' fix values if any
                top = If(top.Length = 0, "auto", top & CbTopType.Text)

                right = If(right.Length = 0, "auto", right & CbRightType.Text)

                bottom = If(bottom.Length = 0, "auto", bottom & CbBottomType.Text)

                left = If(left.Length = 0, "auto", left & CbLeftType.Text)

                ' store to the dictionary
                _dict("clip") = $"rect({top} {right} {bottom} {left})"
            End If
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucLayout control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub ucLayout_Loaded(sender As Object, e As EventArgs)
#Region "set data sources"
            CbVisibility.ItemsSource = _visibility
            CbVisibility.DisplayMemberPath = "Key"
            CbVisibility.SelectedValuePath = "Value"
            CbVisibility.SelectedIndex = 0

            CbDisplay.ItemsSource = _display
            CbDisplay.DisplayMemberPath = "Key"
            CbDisplay.SelectedValuePath = "Value"
            CbDisplay.SelectedIndex = 0

            CbAllowTextToFlow.ItemsSource = _float
            CbAllowTextToFlow.DisplayMemberPath = "Key"
            CbAllowTextToFlow.SelectedValuePath = "Value"
            CbAllowTextToFlow.SelectedIndex = 0

            CbAllowFloatingObject.ItemsSource = _clear
            CbAllowFloatingObject.DisplayMemberPath = "Key"
            CbAllowFloatingObject.SelectedValuePath = "Value"
            CbAllowFloatingObject.SelectedIndex = 0

            CbOverflow.ItemsSource = _overflow
            CbOverflow.DisplayMemberPath = "Key"
            CbOverflow.SelectedValuePath = "Value"
            CbOverflow.SelectedIndex = 0

            CbPbBefore.ItemsSource = _pageBreakBefore
            CbPbBefore.DisplayMemberPath = "Key"
            CbPbBefore.SelectedValuePath = "Value"
            CbPbBefore.SelectedIndex = 0

            CbPbAfter.ItemsSource = _pageBreakAfter
            CbPbAfter.DisplayMemberPath = "Key"
            CbPbAfter.SelectedValuePath = "Value"
            CbPbAfter.SelectedIndex = 0
#End Region

#Region "parse dictionary's values"
            Dim value As String = Nothing
            If _dict.TryGetValue("overflow", value) Then
                Dim n As Integer = _overflow.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _overflow(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbOverflow.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("visibility", value) Then
                Dim n As Integer = _visibility.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _visibility(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbVisibility.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("display", value) Then
                Dim n As Integer = _display.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _display(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbDisplay.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("float", value) Then
                Dim n As Integer = _float.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _float(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbAllowTextToFlow.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("clear", value) Then
                Dim n As Integer = _clear.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _clear(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbAllowFloatingObject.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("page-break-before", value) Then
                Dim n As Integer = _pageBreakBefore.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _pageBreakBefore(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPbBefore.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("page-break-after", value) Then
                Dim n As Integer = _pageBreakAfter.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _pageBreakAfter(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPbAfter.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("clip", value) AndAlso value.StartsWith("rect(", StringComparison.InvariantCultureIgnoreCase) AndAlso value.EndsWith(")") Then
                Dim inner As String = value.Substring(5, value.Length - 6)
                Dim parts As String() = inner.Split(" "c)
                If parts.Length >= 4 Then
                    Dim top As String = parts(0)
                    Dim right As String = parts(1)
                    Dim bottom As String = parts(2)
                    Dim left As String = parts(3)

                    Dim nt As Integer = CbTopType.Items.Count
                    For i As Integer = 0 To nt - 1
                        Dim topType As String = TryCast(TryCast(CbTopType.Items(i), ComboBoxItem)?.Content, String)
                        If topType IsNot Nothing AndAlso top.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase) Then
                            TbTop.Text = top.Substring(0, top.Length - topType.Length)
                            CbTopType.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    Dim nr As Integer = CbRightType.Items.Count
                    For i As Integer = 0 To nr - 1
                        Dim rightType As String = TryCast(TryCast(CbRightType.Items(i), ComboBoxItem)?.Content, String)
                        If rightType IsNot Nothing AndAlso right.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase) Then
                            TbRight.Text = right.Substring(0, right.Length - rightType.Length)
                            CbRightType.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    Dim nb As Integer = CbBottomType.Items.Count
                    For i As Integer = 0 To nb - 1
                        Dim bottomType As String = TryCast(TryCast(CbBottomType.Items(i), ComboBoxItem)?.Content, String)
                        If bottomType IsNot Nothing AndAlso bottom.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase) Then
                            TbBottom.Text = bottom.Substring(0, bottom.Length - bottomType.Length)
                            CbBottomType.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    Dim nl As Integer = CbLeftType.Items.Count
                    For i As Integer = 0 To nl - 1
                        Dim leftType As String = TryCast(CType(CbLeftType.Items(i), ComboBoxItem).Content, String)
                        If left.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase) Then
                            TbLeft.Text = left.Substring(0, left.Length - leftType.Length)
                            CbLeftType.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If
            End If
#End Region
        End Sub
    End Class
End Namespace
