Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder
Imports SpiceLogic.HtmlEditor.WPF.Extensions
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.ToolbarModule

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucEdges
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Edges", "margin-top,margin-bottom,margin-left,margin-right,padding-top,padding-bottom,padding-left,padding-right,border-top-style,border-bottom-style,border-left-style,border-right-style")>
    Partial Public Class ucEdges
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible value"

        ''' <summary>
        ''' The _ border top style
        ''' </summary>
        Private ReadOnly _borderTopStyle As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ border bottom style
        ''' </summary>
        Private ReadOnly _borderBottomStyle As List(Of KeyValuePair(Of String, String))
        ''' <summary>
        ''' The _ border left style
        ''' </summary>
        Private ReadOnly _borderLeftStyle As List(Of KeyValuePair(Of String, String))
        ''' <summary>
        ''' The _ border right style
        ''' </summary>
        Private ReadOnly _borderRightStyle As List(Of KeyValuePair(Of String, String))
#End Region

        ''' <summary>
        ''' A method creating the color dialog
        ''' </summary>
        Private ReadOnly _createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate


        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucEdges"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        ''' <param name="createColorDialogMethod">A method creating the color dialog</param>
        Public Sub New(dict As Dictionary(Of String, String), createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate) 'the argument order is important (Activator.CreateInstance uses it)
            _dict = dict
            _createColorDialogMethod = createColorDialogMethod

#Region "Initialize presets"
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("None", "none"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Dotted", "dotted"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Dashed", "dashed"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Solid line", "solid"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Double line", "double"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Groove", "groove"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Ridge", "ridge"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Inset", "inset"))
            _borderTopStyle.Add(New KeyValuePair(Of String, String)("Outset", "outset"))

            _borderBottomStyle = New List(Of KeyValuePair(Of String, String))(_borderTopStyle)
            _borderLeftStyle = New List(Of KeyValuePair(Of String, String))(_borderTopStyle)
            _borderRightStyle = New List(Of KeyValuePair(Of String, String))(_borderTopStyle)
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("margin-top")
            _dict.Remove("margin-bottom")
            _dict.Remove("margin-left")
            _dict.Remove("margin-right")
            _dict.Remove("padding-top")
            _dict.Remove("padding-bottom")
            _dict.Remove("padding-left")
            _dict.Remove("padding-right")

            _dict.Remove("border-top-style")
            _dict.Remove("border-bottom-style")
            _dict.Remove("border-left-style")
            _dict.Remove("border-right-style")

            If TbMTop.Text.Trim().Length > 0 Then
                _dict("margin-top") = String.Concat(TbMTop.Text, CbMTopType.Text)
            End If
            If TbMBottom.Text.Trim().Length > 0 Then
                _dict("margin-bottom") = String.Concat(TbMBottom.Text, CbMBottomType.Text)
            End If
            If TbMLeft.Text.Trim().Length > 0 Then
                _dict("margin-left") = String.Concat(TbMLeft.Text, CbMLeftType.Text)
            End If
            If TbMRight.Text.Trim().Length > 0 Then
                _dict("margin-right") = String.Concat(TbMRight.Text, CbMRightType.Text)
            End If

            If TbPTop.Text.Trim().Length > 0 Then
                _dict("padding-top") = String.Concat(TbPTop.Text, CbPTopType.Text)
            End If
            If TbPBottom.Text.Trim().Length > 0 Then
                _dict("padding-bottom") = String.Concat(TbPBottom.Text, CbPBottomType.Text)
            End If
            If TbPLeft.Text.Trim().Length > 0 Then
                _dict("padding-left") = String.Concat(TbPLeft.Text, CbPLeftType.Text)
            End If
            If TbPRight.Text.Trim().Length > 0 Then
                _dict("padding-right") = String.Concat(TbPRight.Text, CbPRightType.Text)
            End If

            ' left border
            Dim sbLeft As New StringBuilder()
            If CbLeftStyle.SelectedIndex > 0 Then
                sbLeft.Append(CStr(CbLeftStyle.SelectedValue))
                sbLeft.Append(" "c)

                If CbLeftWidth.SelectedIndex > 0 Then
                    If CbLeftWidth.SelectedIndex = 4 Then
                        If TbLeftWidth.Text.Trim().Length > 0 Then
                            sbLeft.Append(String.Concat(TbLeftWidth.Text.Trim(), CbLeftWidthType.Text))
                            sbLeft.Append(" "c)
                        End If
                    Else
                        sbLeft.Append(CbLeftWidth.Text.ToLowerInvariant())
                        sbLeft.Append(" "c)
                    End If
                End If

                sbLeft.Append(CbLeftColor.Text.ToLowerInvariant())
            End If
            _dict("border-left-style") = sbLeft.ToString()

            ' right border
            Dim sbRight As New StringBuilder()
            If CbRightStyle.SelectedIndex > 0 Then
                sbRight.Append(CStr(CbRightStyle.SelectedValue))
                sbRight.Append(" "c)

                If CbRightWidth.SelectedIndex > 0 Then
                    If CbRightWidth.SelectedIndex = 4 Then
                        If TbRightWidth.Text.Trim().Length > 0 Then
                            sbRight.Append(String.Concat(TbRightWidth.Text.Trim(), CbRightWidthType.Text))
                            sbRight.Append(" "c)
                        End If
                    Else
                        sbRight.Append(CbRightWidth.Text.ToLowerInvariant())
                        sbRight.Append(" "c)
                    End If
                End If

                sbRight.Append(CbRightColor.Text.ToLowerInvariant())
            End If
            _dict("border-right-style") = sbRight.ToString()

            ' top border
            Dim sbTop As New StringBuilder()
            If CbTopStyle.SelectedIndex > 0 Then
                sbTop.Append(CStr(CbTopStyle.SelectedValue))
                sbTop.Append(" "c)

                If CbTopWidth.SelectedIndex > 0 Then
                    If CbTopWidth.SelectedIndex = 4 Then
                        If TbTopWidth.Text.Trim().Length > 0 Then
                            sbTop.Append(String.Concat(TbTopWidth.Text.Trim(), CbTopWidthType.Text))
                            sbTop.Append(" "c)
                        End If
                    Else
                        sbTop.Append(CbTopWidth.Text.ToLowerInvariant())
                        sbTop.Append(" "c)
                    End If
                End If

                sbTop.Append(CbTopColor.Text.ToLowerInvariant())
            End If
            _dict("border-top-style") = sbTop.ToString()

            ' bottom border
            Dim sbBottom As New StringBuilder()
            If CbBottomStyle.SelectedIndex > 0 Then
                sbBottom.Append(CStr(CbBottomStyle.SelectedValue))
                sbBottom.Append(" "c)

                If CbBottomWidth.SelectedIndex > 0 Then
                    If CbBottomWidth.SelectedIndex = 4 Then
                        If TbBottomWidth.Text.Trim().Length > 0 Then
                            sbBottom.Append(String.Concat(TbBottomWidth.Text.Trim(), CbBottomWidthType.Text))
                            sbBottom.Append(" "c)
                        End If
                    Else
                        sbBottom.Append(CbBottomWidth.Text.ToLowerInvariant())
                        sbBottom.Append(" "c)
                    End If
                End If

                sbBottom.Append(CbBottomColor.Text.ToLowerInvariant())
            End If
            _dict("border-bottom-style") = sbBottom.ToString()
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucEdges control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucEdges_Loaded(sender As Object, e As RoutedEventArgs)
#Region "set data sources"
            CbLeftStyle.ItemsSource = _borderLeftStyle
            CbLeftStyle.DisplayMemberPath = "Key"
            CbLeftStyle.SelectedValuePath = "Value"
            CbLeftStyle.SelectedIndex = 0

            CbRightStyle.ItemsSource = _borderRightStyle
            CbRightStyle.DisplayMemberPath = "Key"
            CbRightStyle.SelectedValuePath = "Value"
            CbRightStyle.SelectedIndex = 0

            CbTopStyle.ItemsSource = _borderTopStyle
            CbTopStyle.DisplayMemberPath = "Key"
            CbTopStyle.SelectedValuePath = "Value"
            CbTopStyle.SelectedIndex = 0

            CbBottomStyle.ItemsSource = _borderBottomStyle
            CbBottomStyle.DisplayMemberPath = "Key"
            CbBottomStyle.SelectedValuePath = "Value"
            CbBottomStyle.SelectedIndex = 0

            CbLeftWidth.SelectedIndex = 0
            CbRightWidth.SelectedIndex = 0
            CbTopWidth.SelectedIndex = 0
            CbBottomWidth.SelectedIndex = 0
#End Region

#Region "parse margins"
            Dim value As String = Nothing
            If _dict.TryGetValue("margin-top", value) Then
                Dim n As Integer = CbMTopType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim topType As String = TryCast(CType(CbMTopType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbMTopType.SelectedIndex = i
                        TbMTop.Text = value.Substring(0, value.Length - topType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("margin-bottom", value) Then
                Dim n As Integer = CbMBottomType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim bottomType As String = TryCast(CType(CbMBottomType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbMBottomType.SelectedIndex = i
                        TbMBottom.Text = value.Substring(0, value.Length - bottomType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("margin-left", value) Then
                Dim n As Integer = CbMLeftType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim leftType As String = TryCast(CType(CbMLeftType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbMLeftType.SelectedIndex = i
                        TbMLeft.Text = value.Substring(0, value.Length - leftType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("margin-right", value) Then
                Dim n As Integer = CbMRightType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim rightType As String = TryCast(CType(CbMRightType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbMRightType.SelectedIndex = i
                        TbMRight.Text = value.Substring(0, value.Length - rightType.Length)
                        Exit For
                    End If
                Next
            End If
#End Region

#Region "parse padding"
            If _dict.TryGetValue("padding-top", value) Then
                Dim n As Integer = CbPTopType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim topType As String = TryCast(CType(CbPTopType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(topType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPTopType.SelectedIndex = i
                        TbPTop.Text = value.Substring(0, value.Length - topType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("padding-bottom", value) Then
                Dim n As Integer = CbPBottomType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim bottomType As String = TryCast(CType(CbPBottomType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(bottomType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPBottomType.SelectedIndex = i
                        TbPBottom.Text = value.Substring(0, value.Length - bottomType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("padding-left", value) Then
                Dim n As Integer = CbPLeftType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim leftType As String = TryCast(CType(CbPLeftType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(leftType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPLeftType.SelectedIndex = i
                        TbPLeft.Text = value.Substring(0, value.Length - leftType.Length)
                        Exit For
                    End If
                Next
            End If
            If _dict.TryGetValue("padding-right", value) Then
                Dim n As Integer = CbPRightType.Items.Count
                For i As Integer = 0 To n - 1
                    Dim rightType As String = TryCast(CType(CbPRightType.Items(i), ComboBoxItem).Content, String)
                    If value.EndsWith(rightType, StringComparison.InvariantCultureIgnoreCase) Then
                        CbPRightType.SelectedIndex = i
                        TbPRight.Text = value.Substring(0, value.Length - rightType.Length)
                        Exit For
                    End If
                Next
            End If
#End Region

#Region "parse left border"
            If _dict.TryGetValue("border-left-style", value) Then
                Dim values As New List(Of String)(value.Split(" "c))
                ' Filter empty
                Dim fi As Integer = 0
                Do While fi < values.Count
                    If values(fi).Trim().Length = 0 Then
                        values.RemoveAt(fi)
                        fi -= 1
                    End If
                    fi += 1
                Loop

                Dim styleFound As Boolean = False

                Dim valI As Integer = 0
                Do While valI < values.Count AndAlso Not styleFound
                    value = values(valI)
                    Dim n As Integer = _borderLeftStyle.Count
                    Dim i As Integer = 0
                    Do While i < n AndAlso Not styleFound
                        If String.Equals(value, _borderLeftStyle(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                            CbLeftStyle.SelectedIndex = i
                            values.RemoveAt(valI)
                            styleFound = True
                        End If
                        i += 1
                    Loop
                    valI += 1
                Loop

                If styleFound AndAlso CbLeftStyle.SelectedIndex >= 2 Then
                    Dim widthFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not widthFound
                        value = values(valI)
                        For i As Integer = 1 To 3
                            If widthFound Then Exit For
                            Dim leftWidth As String = TryCast(CType(CbLeftWidth.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, leftWidth, StringComparison.InvariantCultureIgnoreCase) Then
                                CbLeftWidth.SelectedIndex = i
                                values.RemoveAt(valI)
                                widthFound = True
                            End If
                        Next
                        valI += 1
                    Loop

                    If Not widthFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not widthFound
                            value = values(valI)
                            Dim n As Integer = CbLeftWidthType.Items.Count
                            Dim i As Integer = 0
                            Do While i < n AndAlso Not widthFound
                                Dim cbLeftWidthTypeItem As String = TryCast(CType(CbLeftWidthType.Items(i), ComboBoxItem).Content, String)
                                If value.EndsWith(cbLeftWidthTypeItem, StringComparison.InvariantCultureIgnoreCase) Then
                                    CbLeftWidth.SelectedIndex = 4
                                    CbLeftWidthType.SelectedIndex = i
                                    TbLeftWidth.Text = value.Substring(0, value.Length - cbLeftWidthTypeItem.Length)
                                    values.RemoveAt(valI)
                                    widthFound = True
                                End If
                                i += 1
                            Loop
                            valI += 1
                        Loop
                    End If

                    Dim colorFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not colorFound
                        value = values(valI)
                        Dim n As Integer = CbLeftColor.Items.Count
                        Dim i As Integer = 0
                        Do While i < n AndAlso Not colorFound
                            Dim leftColor As String = TryCast(CType(CbLeftColor.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, leftColor, StringComparison.InvariantCultureIgnoreCase) Then
                                CbLeftColor.SelectedIndex = i
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            i += 1
                        Loop
                        valI += 1
                    Loop

                    If Not colorFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not colorFound
                            value = values(valI)
                            If value.StartsWith("#") Then
                                CbLeftColor.Text = value
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            valI += 1
                        Loop
                    End If
                End If
            End If
#End Region

#Region "parse Right border"
            If _dict.TryGetValue("border-right-style", value) Then
                Dim values As New List(Of String)(value.Split(" "c))
                ' Filter empty
                Dim fi As Integer = 0
                Do While fi < values.Count
                    If values(fi).Trim().Length = 0 Then
                        values.RemoveAt(fi)
                        fi -= 1
                    End If
                    fi += 1
                Loop

                Dim styleFound As Boolean = False

                Dim valI As Integer = 0
                Do While valI < values.Count AndAlso Not styleFound
                    value = values(valI)
                    Dim n As Integer = _borderRightStyle.Count
                    Dim i As Integer = 0
                    Do While i < n AndAlso Not styleFound
                        If String.Equals(value, _borderRightStyle(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                            CbRightStyle.SelectedIndex = i
                            values.RemoveAt(valI)
                            styleFound = True
                        End If
                        i += 1
                    Loop
                    valI += 1
                Loop

                If styleFound AndAlso CbRightStyle.SelectedIndex >= 2 Then
                    Dim widthFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not widthFound
                        value = values(valI)
                        For i As Integer = 1 To 3
                            If widthFound Then Exit For
                            Dim rightWidth As String = TryCast(CType(CbRightWidth.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, rightWidth, StringComparison.InvariantCultureIgnoreCase) Then
                                CbRightWidth.SelectedIndex = i
                                values.RemoveAt(valI)
                                widthFound = True
                            End If
                        Next
                        valI += 1
                    Loop

                    If Not widthFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not widthFound
                            value = values(valI)
                            Dim n As Integer = CbRightWidthType.Items.Count
                            Dim i As Integer = 0
                            Do While i < n AndAlso Not widthFound
                                Dim widthType As String = TryCast(CType(CbRightWidthType.Items(i), ComboBoxItem).Content, String)
                                If value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase) Then
                                    CbRightWidth.SelectedIndex = 4
                                    CbRightWidthType.SelectedIndex = i
                                    TbRightWidth.Text = value.Substring(0, value.Length - widthType.Length)
                                    values.RemoveAt(valI)
                                    widthFound = True
                                End If
                                i += 1
                            Loop
                            valI += 1
                        Loop
                    End If

                    Dim colorFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not colorFound
                        value = values(valI)
                        Dim n As Integer = CbRightColor.Items.Count
                        Dim i As Integer = 0
                        Do While i < n AndAlso Not colorFound
                            Dim rightColor As String = TryCast(CType(CbRightColor.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, rightColor, StringComparison.InvariantCultureIgnoreCase) Then
                                CbRightColor.SelectedIndex = i
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            i += 1
                        Loop
                        valI += 1
                    Loop

                    If Not colorFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not colorFound
                            value = values(valI)
                            If value.StartsWith("#") Then
                                CbRightColor.Text = value
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            valI += 1
                        Loop
                    End If
                End If
            End If
#End Region

#Region "parse Top border"
            If _dict.TryGetValue("border-top-style", value) Then
                Dim values As New List(Of String)(value.Split(" "c))
                ' Filter empty
                Dim fi As Integer = 0
                Do While fi < values.Count
                    If values(fi).Trim().Length = 0 Then
                        values.RemoveAt(fi)
                        fi -= 1
                    End If
                    fi += 1
                Loop

                Dim styleFound As Boolean = False

                Dim valI As Integer = 0
                Do While valI < values.Count AndAlso Not styleFound
                    value = values(valI)
                    Dim n As Integer = _borderTopStyle.Count
                    Dim i As Integer = 0
                    Do While i < n AndAlso Not styleFound
                        If String.Equals(value, _borderTopStyle(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                            CbTopStyle.SelectedIndex = i
                            values.RemoveAt(valI)
                            styleFound = True
                        End If
                        i += 1
                    Loop
                    valI += 1
                Loop

                If styleFound AndAlso CbTopStyle.SelectedIndex >= 2 Then
                    Dim widthFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not widthFound
                        value = values(valI)
                        For i As Integer = 1 To 3
                            If widthFound Then Exit For
                            Dim topWidth As String = TryCast(CType(CbTopWidth.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, topWidth, StringComparison.InvariantCultureIgnoreCase) Then
                                CbTopWidth.SelectedIndex = i
                                values.RemoveAt(valI)
                                widthFound = True
                            End If
                        Next
                        valI += 1
                    Loop

                    If Not widthFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not widthFound
                            value = values(valI)
                            Dim n As Integer = CbTopWidthType.Items.Count
                            Dim i As Integer = 0
                            Do While i < n AndAlso Not widthFound
                                Dim widthType As String = TryCast(CType(CbTopWidthType.Items(i), ComboBoxItem).Content, String)
                                If value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase) Then
                                    CbTopWidth.SelectedIndex = 4
                                    CbTopWidthType.SelectedIndex = i
                                    TbTopWidth.Text = value.Substring(0, value.Length - widthType.Length)
                                    values.RemoveAt(valI)
                                    widthFound = True
                                End If
                                i += 1
                            Loop
                            valI += 1
                        Loop
                    End If

                    Dim colorFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not colorFound
                        value = values(valI)
                        Dim n As Integer = CbTopColor.Items.Count
                        Dim i As Integer = 0
                        Do While i < n AndAlso Not colorFound
                            Dim topColor As String = TryCast(CType(CbTopColor.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, topColor, StringComparison.InvariantCultureIgnoreCase) Then
                                CbTopColor.SelectedIndex = i
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            i += 1
                        Loop
                        valI += 1
                    Loop

                    If Not colorFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not colorFound
                            value = values(valI)
                            If value.StartsWith("#") Then
                                CbTopColor.Text = value
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            valI += 1
                        Loop
                    End If
                End If
            End If
#End Region

#Region "parse Bottom border"
            If _dict.TryGetValue("border-bottom-style", value) Then
                Dim values As New List(Of String)(value.Split(" "c))
                ' Filter empty
                Dim fi As Integer = 0
                Do While fi < values.Count
                    If values(fi).Trim().Length = 0 Then
                        values.RemoveAt(fi)
                        fi -= 1
                    End If
                    fi += 1
                Loop

                Dim styleFound As Boolean = False

                Dim valI As Integer = 0
                Do While valI < values.Count AndAlso Not styleFound
                    value = values(valI)
                    Dim n As Integer = _borderBottomStyle.Count
                    Dim i As Integer = 0
                    Do While i < n AndAlso Not styleFound
                        If String.Equals(value, _borderBottomStyle(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                            CbBottomStyle.SelectedIndex = i
                            values.RemoveAt(valI)
                            styleFound = True
                        End If
                        i += 1
                    Loop
                    valI += 1
                Loop

                If styleFound AndAlso CbBottomStyle.SelectedIndex >= 2 Then
                    Dim widthFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not widthFound
                        value = values(valI)
                        For i As Integer = 1 To 3
                            If widthFound Then Exit For
                            Dim bottomWidth As String = TryCast(CType(CbBottomWidth.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, bottomWidth, StringComparison.InvariantCultureIgnoreCase) Then
                                CbBottomWidth.SelectedIndex = i
                                values.RemoveAt(valI)
                                widthFound = True
                            End If
                        Next
                        valI += 1
                    Loop

                    If Not widthFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not widthFound
                            value = values(valI)
                            Dim n As Integer = CbBottomWidthType.Items.Count
                            Dim i As Integer = 0
                            Do While i < n AndAlso Not widthFound
                                Dim widthType As String = TryCast(CType(CbBottomWidthType.Items(i), ComboBoxItem).Content, String)
                                If value.EndsWith(widthType, StringComparison.InvariantCultureIgnoreCase) Then
                                    CbBottomWidth.SelectedIndex = 4
                                    CbBottomWidthType.SelectedIndex = i
                                    TbBottomWidth.Text = value.Substring(0, value.Length - widthType.Length)
                                    values.RemoveAt(valI)
                                    widthFound = True
                                End If
                                i += 1
                            Loop
                            valI += 1
                        Loop
                    End If

                    Dim colorFound As Boolean = False

                    valI = 0
                    Do While valI < values.Count AndAlso Not colorFound
                        value = values(valI)
                        Dim n As Integer = CbBottomColor.Items.Count
                        Dim i As Integer = 0
                        Do While i < n AndAlso Not colorFound
                            Dim bottomColor As String = TryCast(CType(CbBottomColor.Items(i), ComboBoxItem).Content, String)
                            If String.Equals(value, bottomColor, StringComparison.InvariantCultureIgnoreCase) Then
                                CbBottomColor.SelectedIndex = i
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            i += 1
                        Loop
                        valI += 1
                    Loop

                    If Not colorFound Then
                        valI = 0
                        Do While valI < values.Count AndAlso Not colorFound
                            value = values(valI)
                            If value.StartsWith("#") Then
                                CbBottomColor.Text = value
                                values.RemoveAt(valI)
                                colorFound = True
                            End If
                            valI += 1
                        Loop
                    End If
                End If
            End If
#End Region
        End Sub

#Region "left border edge handlers"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbLeftWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbLeftWidth_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbLeftWidth.SelectedIndex = 4
            CbLeftWidthType.IsEnabled = enabled
            TbLeftWidth.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtLeftColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btLeftColor_Click(sender As Object, e As RoutedEventArgs)
            Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                If colorDialog.ShowDialog() = True Then
                    CbLeftColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbLeftStyle control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbLeftStyle_SelectionChanged(sender As Object, e As RoutedEventArgs)
            If CbLeftStyle.SelectedIndex >= 2 Then
                CbLeftWidth.IsEnabled = True
                TbLeftWidth.IsEnabled = True
                CbLeftWidthType.IsEnabled = True
                CbLeftColor.IsEnabled = True
                BtLeftColor.IsEnabled = True
                cbLeftWidth_SelectionChanged(Me, New RoutedEventArgs())
            Else
                CbLeftWidth.IsEnabled = False
                TbLeftWidth.IsEnabled = False
                CbLeftWidthType.IsEnabled = False
                CbLeftColor.IsEnabled = False
                BtLeftColor.IsEnabled = False
            End If
        End Sub
#End Region

#Region "Right border edge handlers"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbRightWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbRightWidth_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbRightWidth.SelectedIndex = 4
            CbRightWidthType.IsEnabled = enabled
            TbRightWidth.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtRightColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btRightColor_Click(sender As Object, e As RoutedEventArgs)
            Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                If colorDialog.ShowDialog() = True Then
                    CbRightColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbRightStyle control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbRightStyle_SelectionChanged(sender As Object, e As RoutedEventArgs)
            If CbRightStyle.SelectedIndex >= 2 Then
                CbRightWidth.IsEnabled = True
                TbRightWidth.IsEnabled = True
                CbRightWidthType.IsEnabled = True
                CbRightColor.IsEnabled = True
                BtRightColor.IsEnabled = True
                cbRightWidth_SelectionChanged(Me, New RoutedEventArgs())
            Else
                CbRightWidth.IsEnabled = False
                TbRightWidth.IsEnabled = False
                CbRightWidthType.IsEnabled = False
                CbRightColor.IsEnabled = False
                BtRightColor.IsEnabled = False
            End If
        End Sub
#End Region

#Region "Top border edge handlers"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbTopWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbTopWidth_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbTopWidth.SelectedIndex = 4
            CbTopWidthType.IsEnabled = enabled
            TbTopWidth.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtTopColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btTopColor_Click(sender As Object, e As RoutedEventArgs)
            Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                If colorDialog.ShowDialog() = True Then
                    CbTopColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbTopStyle control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbTopStyle_SelectionChanged(sender As Object, e As RoutedEventArgs)
            If CbTopStyle.SelectedIndex >= 2 Then
                CbTopWidth.IsEnabled = True
                TbTopWidth.IsEnabled = True
                CbTopWidthType.IsEnabled = True
                CbTopColor.IsEnabled = True
                BtTopColor.IsEnabled = True
                cbTopWidth_SelectionChanged(Me, New RoutedEventArgs())
            Else
                CbTopWidth.IsEnabled = False
                TbTopWidth.IsEnabled = False
                CbTopWidthType.IsEnabled = False
                CbTopColor.IsEnabled = False
                BtTopColor.IsEnabled = False
            End If
        End Sub
#End Region

#Region "Bottom border edge handlers"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbBottomWidth control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbBottomWidth_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbBottomWidth.SelectedIndex = 4
            CbBottomWidthType.IsEnabled = enabled
            TbBottomWidth.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtBottomColor control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btBottomColor_Click(sender As Object, e As RoutedEventArgs)
            Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                If colorDialog.ShowDialog() = True Then
                    CbBottomColor.Text = WpfColorTranslator.ToHtml(colorDialog.SelectedColor)
                End If
            End Using
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the CbBottomStyle control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbBottomStyle_SelectionChanged(sender As Object, e As RoutedEventArgs)
            If CbBottomStyle.SelectedIndex >= 2 Then
                CbBottomWidth.IsEnabled = True
                TbBottomWidth.IsEnabled = True
                CbBottomWidthType.IsEnabled = True
                CbBottomColor.IsEnabled = True
                BtBottomColor.IsEnabled = True
                cbBottomWidth_SelectionChanged(Me, New RoutedEventArgs())
            Else
                CbBottomWidth.IsEnabled = False
                TbBottomWidth.IsEnabled = False
                CbBottomWidthType.IsEnabled = False
                CbBottomColor.IsEnabled = False
                BtBottomColor.IsEnabled = False
            End If
        End Sub
#End Region
    End Class
End Namespace
