Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucText
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Text", "text-align,vertical-align,text-justify,letter-spacing,line-height,direction,text-indent")>
    Partial Public Class ucText
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible values"

        ''' <summary>
        ''' The _ text align
        ''' </summary>
        Private ReadOnly _textAlign As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ vertical align
        ''' </summary>
        Private ReadOnly _verticalAlign As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ text justify
        ''' </summary>
        Private ReadOnly _textJustify As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ letter spacing
        ''' </summary>
        Private ReadOnly _letterSpacing As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ line height
        ''' </summary>
        Private ReadOnly _lineHeight As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ direction
        ''' </summary>
        Private ReadOnly _direction As New List(Of KeyValuePair(Of String, String))()

#End Region

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucText"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        Public Sub New(dict As Dictionary(Of String, String))
            _dict = dict

#Region "Initialize presets"
            _textAlign.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _textAlign.Add(New KeyValuePair(Of String, String)("Left", "left"))
            _textAlign.Add(New KeyValuePair(Of String, String)("Center", "center"))
            _textAlign.Add(New KeyValuePair(Of String, String)("Right", "right"))
            _textAlign.Add(New KeyValuePair(Of String, String)("Justified", "justify"))

            _verticalAlign.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("baseline", "baseline"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("sub", "sub"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("super", "super"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("top", "top"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("text-top", "text-top"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("middle", "middle"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("bottom", "bottom"))
            _verticalAlign.Add(New KeyValuePair(Of String, String)("text-bottom", "text-bottom"))

            _textJustify.Add(New KeyValuePair(Of String, String)("", ""))
            _textJustify.Add(New KeyValuePair(Of String, String)("Auto", "auto"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Space words", "inter-word"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Newspaper style", "newspaper"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Distribute spacing", "distribute"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Distribute all lines", "dibtribute-all-lines"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Inter-cluster", "inter-cluster"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Inter-ideograph", "inter-ideograph"))
            _textJustify.Add(New KeyValuePair(Of String, String)("Kashida", "kashida"))

            _letterSpacing.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _letterSpacing.Add(New KeyValuePair(Of String, String)("Normal", "normal"))
            _letterSpacing.Add(New KeyValuePair(Of String, String)("Custom", ""))

            _lineHeight.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _lineHeight.Add(New KeyValuePair(Of String, String)("Normal", "normal"))
            _lineHeight.Add(New KeyValuePair(Of String, String)("Custom", ""))

            _direction.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _direction.Add(New KeyValuePair(Of String, String)("Left to right", "ltr"))
            _direction.Add(New KeyValuePair(Of String, String)("Right to left", "rtl"))
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            ' remove previous entries
            _dict.Remove("text-align")
            _dict.Remove("vertical-align")
            _dict.Remove("text-justify")

            _dict.Remove("letter-spacing")
            _dict.Remove("line-height")

            _dict.Remove("direction")
            _dict.Remove("text-indent")

            ' save form's data
            _dict("text-align") = CStr(CbAlHorizontal.SelectedValue)
            If CbAlHorizontal.SelectedIndex = 4 Then
                _dict("text-justify") = CStr(CbAlJustification.SelectedValue)
            End If
            _dict("vertical-align") = CStr(CbAlVertical.SelectedValue)

            _dict("letter-spacing") = If(CbSpacingLetters.SelectedIndex <> 2,
                CStr(CbSpacingLetters.SelectedValue),
                TbSpacingLetters.Text & CbSpacingLettersCustom.Text)

            _dict("line-height") = If(CbSpacingLines.SelectedIndex <> 2,
                CStr(CbSpacingLines.SelectedValue),
                TbSpacingLines.Text & CbSpacingLinesCustom.Text)

            If TbTextFlowIndentation.Text.Trim().Length > 0 Then
                _dict("text-indent") = TbTextFlowIndentation.Text & CbTextFlowCustom.Text
            End If

            _dict("direction") = CStr(CbTextFlowDirection.SelectedValue)
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucText control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucText_Loaded(sender As Object, e As RoutedEventArgs)
#Region "set data sources"
            CbAlHorizontal.ItemsSource = _textAlign
            CbAlHorizontal.DisplayMemberPath = "Key"
            CbAlHorizontal.SelectedValuePath = "Value"
            CbAlHorizontal.SelectedIndex = 0

            CbAlVertical.ItemsSource = _verticalAlign
            CbAlVertical.DisplayMemberPath = "Key"
            CbAlVertical.SelectedValuePath = "Value"
            CbAlVertical.SelectedIndex = 0

            CbAlJustification.ItemsSource = _textJustify
            CbAlJustification.DisplayMemberPath = "Key"
            CbAlJustification.SelectedValuePath = "Value"
            CbAlJustification.SelectedIndex = 0

            CbSpacingLetters.ItemsSource = _letterSpacing
            CbSpacingLetters.DisplayMemberPath = "Key"
            CbSpacingLetters.SelectedValuePath = "Value"
            CbSpacingLetters.SelectedIndex = 0

            CbSpacingLines.ItemsSource = _lineHeight
            CbSpacingLines.DisplayMemberPath = "Key"
            CbSpacingLines.SelectedValuePath = "Value"
            CbSpacingLines.SelectedIndex = 0

            CbTextFlowDirection.ItemsSource = _direction
            CbTextFlowDirection.DisplayMemberPath = "Key"
            CbTextFlowDirection.SelectedValuePath = "Value"
            CbTextFlowDirection.SelectedIndex = 0
#End Region

#Region "parse alignment"
            Dim value As String = Nothing
            If _dict.TryGetValue("vertical-align", value) Then
                Dim n As Integer = _verticalAlign.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _verticalAlign(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbAlVertical.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("text-justify", value) Then
                Dim n As Integer = _textJustify.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _textJustify(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbAlJustification.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("text-align", value) Then
                Dim n As Integer = _textAlign.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _textAlign(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbAlHorizontal.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
#End Region

#Region "parse spacing"
            If _dict.TryGetValue("letter-spacing", value) Then
                Dim handled As Boolean = False

                Dim na As Integer = _letterSpacing.Count
                Dim ia As Integer = 0
                Do While ia < na AndAlso Not handled
                    If value.Equals(_letterSpacing(ia).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbSpacingLetters.SelectedIndex = ia
                        handled = True
                    End If
                    ia += 1
                Loop

                If Not handled Then
                    Dim n As Integer = CbSpacingLettersCustom.Items.Count
                    For i As Integer = 0 To n - 1
                        Dim lettersCustom As String = TryCast(TryCast(CbSpacingLettersCustom.Items(i), ComboBoxItem)?.Content, String)
                        If lettersCustom IsNot Nothing AndAlso value.EndsWith(lettersCustom, StringComparison.InvariantCultureIgnoreCase) Then
                            TbSpacingLetters.Text = value.Substring(0, value.Length - lettersCustom.Length)
                            CbSpacingLettersCustom.SelectedIndex = i
                            CbSpacingLetters.SelectedIndex = 2
                            Exit For
                        End If
                    Next
                End If
            End If

            If _dict.TryGetValue("line-height", value) Then
                Dim handled As Boolean = False

                Dim na As Integer = _lineHeight.Count
                Dim ia As Integer = 0
                Do While ia < na AndAlso Not handled
                    If value.Equals(_lineHeight(ia).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbSpacingLines.SelectedIndex = ia
                        handled = True
                    End If
                    ia += 1
                Loop

                If Not handled Then
                    Dim n As Integer = CbSpacingLinesCustom.Items.Count
                    For i As Integer = 0 To n - 1
                        Dim linesCustom As String = TryCast(TryCast(CbSpacingLinesCustom.Items(i), ComboBoxItem)?.Content, String)
                        If linesCustom IsNot Nothing AndAlso value.EndsWith(linesCustom, StringComparison.InvariantCultureIgnoreCase) Then
                            TbSpacingLines.Text = value.Substring(0, value.Length - linesCustom.Length)
                            CbSpacingLinesCustom.SelectedIndex = i
                            CbSpacingLines.SelectedIndex = 2
                            Exit For
                        End If
                    Next
                End If
            End If
#End Region

#Region "parse text flow"
            If _dict.TryGetValue("text-indent", value) Then
                Dim n As Integer = CbTextFlowCustom.Items.Count
                For i As Integer = 0 To n - 1
                    Dim textFlowCustom As String = TryCast(TryCast(CbTextFlowCustom.Items(i), ComboBoxItem)?.Content, String)
                    If textFlowCustom IsNot Nothing AndAlso value.EndsWith(textFlowCustom, StringComparison.InvariantCultureIgnoreCase) Then
                        TbTextFlowIndentation.Text = value.Substring(0, value.Length - textFlowCustom.Length)
                        CbTextFlowCustom.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("direction", value) Then
                Dim n As Integer = _direction.Count
                For i As Integer = 0 To n - 1
                    If value.Equals(_direction(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbTextFlowDirection.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
#End Region
        End Sub

#Region "UI handling"
        ''' <summary>
        ''' Handles the SelectionChanged event of the CbAlHorizontal control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbAlHorizontal_SelectionChanged(sender As Object, e As RoutedEventArgs)
            CbAlJustification.IsEnabled = CbAlHorizontal.SelectedIndex = 4
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the cbSpacingLetters control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbSpacingLetters_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbSpacingLetters.SelectedIndex = 2
            TbSpacingLetters.IsEnabled = enabled
            CbSpacingLettersCustom.IsEnabled = enabled
        End Sub

        ''' <summary>
        ''' Handles the SelectionChanged event of the cbSpacingLines control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbSpacingLines_SelectionChanged(sender As Object, e As RoutedEventArgs)
            Dim enabled As Boolean = CbSpacingLines.SelectedIndex = 2
            TbSpacingLines.IsEnabled = enabled
            CbSpacingLinesCustom.IsEnabled = enabled
        End Sub
#End Region
    End Class
End Namespace
