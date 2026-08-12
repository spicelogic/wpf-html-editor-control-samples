Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder
Imports SpiceLogic.HtmlEditor.WPF.Extensions
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.ToolbarModule

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucFont
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("Font", "font;font-family;font-size;text-decoration;font-weight;text-transform;color;font-style;font-variant")>
    Partial Public Class UcFont
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

        ''' <summary>
        ''' A method creating the color dialog
        ''' </summary>
        Private ReadOnly _createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate

#Region "Preset of possible value"

        ''' <summary>
        ''' The _ system fonts
        ''' </summary>
        Private ReadOnly _systemFonts As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ font style
        ''' </summary>
        Private ReadOnly _fontStyle As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ font variant
        ''' </summary>
        Private ReadOnly _fontVariant As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ bold absolute
        ''' </summary>
        Private ReadOnly _boldAbsolute As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ bold relative
        ''' </summary>
        Private ReadOnly _boldRelative As New List(Of KeyValuePair(Of String, String))()
        ''' <summary>
        ''' The _ text transform
        ''' </summary>
        Private ReadOnly _textTransform As New List(Of KeyValuePair(Of String, String))()
#End Region

        ''' <summary>
        ''' Creates the lists.
        ''' </summary>
        Private Sub CreateLists()
#Region "Initialize presets"
            _systemFonts.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _systemFonts.Add(New KeyValuePair(Of String, String)("Window caption", "caption"))
            _systemFonts.Add(New KeyValuePair(Of String, String)("ToolWindow caption", "small-caption"))
            _systemFonts.Add(New KeyValuePair(Of String, String)("Dialog text", "message-box"))
            _systemFonts.Add(New KeyValuePair(Of String, String)("Icon labels", "icon"))
            _systemFonts.Add(New KeyValuePair(Of String, String)("Menu text", "menu"))
            _systemFonts.Add(New KeyValuePair(Of String, String)("Tooltip text", "status-bar"))

            _fontStyle.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _fontStyle.Add(New KeyValuePair(Of String, String)("Normal", "normal"))
            _fontStyle.Add(New KeyValuePair(Of String, String)("Italic", "italic"))

            _fontVariant.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _fontVariant.Add(New KeyValuePair(Of String, String)("Normal", "normal"))
            _fontVariant.Add(New KeyValuePair(Of String, String)("Small Caps", "small-caps"))

            _boldAbsolute.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _boldAbsolute.Add(New KeyValuePair(Of String, String)("Normal", "normal"))
            _boldAbsolute.Add(New KeyValuePair(Of String, String)("Bold", "bold"))

            _boldRelative.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _boldRelative.Add(New KeyValuePair(Of String, String)("Lighter", "lighter"))
            _boldRelative.Add(New KeyValuePair(Of String, String)("Bolder", "bolder"))

            _textTransform.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _textTransform.Add(New KeyValuePair(Of String, String)("None", "none"))
            _textTransform.Add(New KeyValuePair(Of String, String)("Initial Cap", "capitalize"))
            _textTransform.Add(New KeyValuePair(Of String, String)("lowercase", "lowercase"))
            _textTransform.Add(New KeyValuePair(Of String, String)("UPPERCASE", "uppercase"))
#End Region
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="UcFont"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        ''' <param name="createColorDialogMethod">A method creating the color dialog</param>
        Public Sub New(dict As Dictionary(Of String, String), createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate) 'the argument order is important (Activator.CreateInstance uses it)
            _dict = dict
            _createColorDialogMethod = createColorDialogMethod
            CreateLists()
            InitializeComponent()

        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("font-family")
            _dict.Remove("font")
            _dict.Remove("color")
            _dict.Remove("font-style")
            _dict.Remove("font-variant")
            _dict.Remove("font-weight")
            _dict.Remove("font-size")
            _dict.Remove("text-decoration")

            ' handle text decoration
            Dim sb As New StringBuilder()
            If Equals(CbEffectNone.IsChecked, True) Then
                sb.Append(" none")
            End If
            If Equals(CbEffectUnderline.IsChecked, True) Then
                sb.Append(" underline")
            End If
            If Equals(CbEffectStrikethrough.IsChecked, True) Then
                sb.Append(" line-through")
            End If
            If Equals(CbEffectOverline.IsChecked, True) Then
                sb.Append(" overline")
            End If
            _dict("text-decoration") = sb.ToString()

            _dict("text-transform") = CStr(CbCapitalization.SelectedValue)

            If Equals(RbFamily.IsChecked, True) Then
                _dict("font-family") = TbFontFamily.Text
                _dict("color") = WpfColorTranslator.ToHtml(CType(TxtForeColor.Background, SolidColorBrush).Color) ' cbColor.Text.ToLowerInvariant();
                _dict("font-style") = CStr(CbFontStyle.SelectedValue)
                _dict("font-variant") = CStr(CbFontVariant.SelectedValue)

                ' font-size
                If Equals(RbSizeSpecific.IsChecked, True) Then
                    If TbSpecificSize.Text.Trim().Length > 0 Then
                        _dict("font-size") = TbSpecificSize.Text & CbSpecificSizeType.Text
                    End If
                ElseIf Equals(RbSizeAbsolute.IsChecked, True) Then
                    _dict("font-size") = CbAbsoluteSize.Text.ToLowerInvariant()
                Else
                    _dict("font-size") = CbRelativeSize.Text.ToLowerInvariant()
                End If

                ' font-weight
                If Equals(RbBoldAbsolute.IsChecked, True) Then
                    _dict("font-weight") = CStr(CbBoldAbsolute.SelectedValue)
                Else
                    _dict("font-weight") = CStr(CbBoldRelative.SelectedValue)
                End If
            End If

            If Equals(RbSystemFont.IsChecked, True) Then
                _dict("font") = CStr(CbSystemFont.SelectedValue)
            End If
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucFont control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucFont_Loaded(sender As Object, e As RoutedEventArgs)
#Region "set data sources"
            CbSystemFont.ItemsSource = _systemFonts
            CbSystemFont.DisplayMemberPath = "Key"
            CbSystemFont.SelectedValuePath = "Value"
            CbSystemFont.SelectedIndex = 0

            CbFontStyle.ItemsSource = _fontStyle
            CbFontStyle.DisplayMemberPath = "Key"
            CbFontStyle.SelectedValuePath = "Value"
            CbFontStyle.SelectedIndex = 0

            CbFontVariant.ItemsSource = _fontVariant
            CbFontVariant.DisplayMemberPath = "Key"
            CbFontVariant.SelectedValuePath = "Value"
            CbFontVariant.SelectedIndex = 0

            CbBoldAbsolute.ItemsSource = _boldAbsolute
            CbBoldAbsolute.DisplayMemberPath = "Key"
            CbBoldAbsolute.SelectedValuePath = "Value"
            CbBoldAbsolute.SelectedIndex = 0

            CbBoldRelative.ItemsSource = _boldRelative
            CbBoldRelative.DisplayMemberPath = "Key"
            CbBoldRelative.SelectedValuePath = "Value"
            CbBoldRelative.SelectedIndex = 0

            CbCapitalization.ItemsSource = _textTransform
            CbCapitalization.DisplayMemberPath = "Key"
            CbCapitalization.SelectedValuePath = "Value"
            CbCapitalization.SelectedIndex = 0
#End Region

            ' all radio-group defaults (RbFamily,
            ' RbSizeSpecific, RbBoldAbsolute) are declared in the XAML
            ' (IsChecked="True"). No runtime seeding -- a host customizer who
            ' picks a different default in the XAML is honored.

#Region "parse"

            Dim value As String = Nothing
            If _dict.TryGetValue("color", value) Then
                TxtForeColor.Background = New SolidColorBrush(WpfColorTranslator.FromHtml(value))
            End If

            If _dict.TryGetValue("font-style", value) Then
                Dim n As Integer = _fontStyle.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _fontStyle(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbFontStyle.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            If _dict.TryGetValue("font-size", value) Then
                Dim handled As Boolean = False

                Dim na As Integer = CbAbsoluteSize.Items.Count
                Dim ia As Integer = 0
                Do While ia < na AndAlso Not handled
                    If String.Equals(value, TryCast(TryCast(CbAbsoluteSize.Items(ia), ComboBoxItem)?.Content, String), StringComparison.InvariantCultureIgnoreCase) Then
                        CbAbsoluteSize.SelectedIndex = ia
                        RbSizeAbsolute.IsChecked = True
                        handled = True
                    End If
                    ia += 1
                Loop

                Dim nr As Integer = CbRelativeSize.Items.Count
                Dim ir As Integer = 0
                Do While ir < nr AndAlso Not handled
                    If String.Equals(value, TryCast(TryCast(CbRelativeSize.Items(ir), ComboBoxItem)?.Content, String), StringComparison.InvariantCultureIgnoreCase) Then
                        CbRelativeSize.SelectedIndex = ir
                        RbSizeRelative.IsChecked = True
                        handled = True
                    End If
                    ir += 1
                Loop

                If Not handled Then
                    RbSizeSpecific.IsChecked = True
                    Dim ns As Integer = CbSpecificSizeType.Items.Count
                    Dim isx As Integer = 0
                    Do While isx < ns AndAlso Not handled
                        Dim cbSpecificSizeTypeItem As String = TryCast(TryCast(CbSpecificSizeType.Items(isx), ComboBoxItem)?.Content, String)
                        If Not String.IsNullOrEmpty(cbSpecificSizeTypeItem) AndAlso value.EndsWith(cbSpecificSizeTypeItem, StringComparison.InvariantCultureIgnoreCase) Then
                            CbSpecificSizeType.SelectedIndex = isx
                            TbSpecificSize.Text = value.Substring(0, value.Length - cbSpecificSizeTypeItem.Length)
                            handled = True
                        End If
                        isx += 1
                    Loop
                End If
            End If

            If _dict.TryGetValue("font-weight", value) Then
                Dim handled As Boolean = False

                Dim na As Integer = _boldAbsolute.Count
                Dim ia As Integer = 0
                Do While ia < na AndAlso Not handled
                    If String.Equals(value, _boldAbsolute(ia).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbBoldAbsolute.SelectedIndex = ia
                        RbBoldAbsolute.IsChecked = True
                        handled = True
                    End If
                    ia += 1
                Loop

                Dim nr As Integer = _boldRelative.Count
                Dim ir As Integer = 0
                Do While ir < nr AndAlso Not handled
                    If String.Equals(value, _boldRelative(ir).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbBoldRelative.SelectedIndex = ir
                        RbBoldRelative.IsChecked = True
                        handled = True
                    End If
                    ir += 1
                Loop
            End If

            If _dict.TryGetValue("text-decoration", value) Then
                Dim loValue As String = value.ToLowerInvariant()
                CbEffectUnderline.IsChecked = loValue.Contains("underline")
                CbEffectStrikethrough.IsChecked = loValue.Contains("line-through") OrElse loValue.Contains("linethrough")
                CbEffectOverline.IsChecked = loValue.Contains("overline")
                CbEffectNone.IsChecked = loValue.Contains("none")
            End If

            If _dict.TryGetValue("text-transform", value) Then
                Dim n As Integer = _textTransform.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _textTransform(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbCapitalization.SelectedIndex = i
                    End If
                Next
            End If

            If _dict.TryGetValue("font-family", value) Then
                RbFamily.IsChecked = True
                TbFontFamily.Text = value
            End If

            If _dict.TryGetValue("font", value) Then
                RbSystemFont.IsChecked = True

                Dim n As Integer = _systemFonts.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(If(value, "").Replace("-", "").Trim(), _systemFonts(i).Value.Replace("-", ""), StringComparison.InvariantCultureIgnoreCase) Then
                        CbSystemFont.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
#End Region
        End Sub

#Region "UI handlers"
        ''' <summary>
        ''' Handles the Click event of the BtFontFamilySelect control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btFontFamilySelect_Click(sender As Object, e As RoutedEventArgs)
            Dim subForm As New FontPicker(TbFontFamily.Text)
            If subForm.ShowDialog() = True Then
                TbFontFamily.Text = subForm.SelectedFontList
            End If
        End Sub

        ''' <summary>
        ''' Fonts the type changed.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub FontTypeChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TbFontFamily.IsEnabled = Equals(RbFamily.IsChecked, True)
            btFontFamilySelect.IsEnabled = Equals(RbFamily.IsChecked, True)
            CbSystemFont.IsEnabled = Not Equals(RbFamily.IsChecked, True)

            Dim familyChecked As Boolean = Equals(RbFamily.IsChecked, True)
            GbSize.IsEnabled = familyChecked
            GbBold.IsEnabled = familyChecked
            CbFontStyle.IsEnabled = familyChecked
            CbFontVariant.IsEnabled = familyChecked
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the RbSizeSpecific control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub rbSizeSpecific_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            TbSpecificSize.IsEnabled = True
            CbSpecificSizeType.IsEnabled = True
            CbRelativeSize.IsEnabled = False
            CbAbsoluteSize.IsEnabled = False
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the RbSizeAbsolute control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub rbSizeAbsolute_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CbAbsoluteSize.IsEnabled = True
            TbSpecificSize.IsEnabled = False
            CbSpecificSizeType.IsEnabled = False
            CbRelativeSize.IsEnabled = False
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the RbSizeRelative control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub rbSizeRelative_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CbRelativeSize.IsEnabled = True
            TbSpecificSize.IsEnabled = False
            CbSpecificSizeType.IsEnabled = False
            CbAbsoluteSize.IsEnabled = False
        End Sub

        ''' <summary>
        ''' Handles the Checked and Unchecked events of the cbEffectNone control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub cbEffectNone_CheckedChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            If Equals(CbEffectNone.IsChecked, True) Then
                CbEffectOverline.IsChecked = False
                CbEffectStrikethrough.IsChecked = False
                CbEffectUnderline.IsChecked = False
                CbEffectOverline.IsEnabled = False
                CbEffectStrikethrough.IsEnabled = False
                CbEffectUnderline.IsEnabled = False
            Else
                CbEffectOverline.IsEnabled = True
                CbEffectStrikethrough.IsEnabled = True
                CbEffectUnderline.IsEnabled = True
            End If
        End Sub

        ''' <summary>
        ''' Rbs the bold radio button changed.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub RbBoldRadioButtonChanged(sender As Object, e As RoutedEventArgs)
            If Not IsInitialized Then
                Return
            End If

            CbBoldAbsolute.IsEnabled = Equals(RbBoldAbsolute.IsChecked, True)
            CbBoldRelative.IsEnabled = Equals(RbBoldRelative.IsChecked, True)
        End Sub
#End Region


        ''' <summary>
        ''' Handles the PreviewMouseLeftButtonUp events of the TxtForeColor control.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub TxtForeColor_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            Dim solidColorBrush As SolidColorBrush = TryCast(TxtForeColor.Background, SolidColorBrush)
            If solidColorBrush IsNot Nothing Then
                Using colorDialog As IColorPickerDialog = _createColorDialogMethod()
                    colorDialog.StartingColor = solidColorBrush.Color
                    If colorDialog.ShowDialog() = True Then
                        TxtForeColor.Background = New SolidColorBrush(colorDialog.SelectedColor)
                    End If
                End Using
            End If
        End Sub
    End Class
End Namespace
