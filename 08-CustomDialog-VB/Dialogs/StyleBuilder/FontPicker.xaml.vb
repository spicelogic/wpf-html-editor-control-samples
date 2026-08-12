Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Text
Imports System.Windows

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Interaction logic for FontPicker.xaml
    ''' </summary>
    Partial Public Class FontPicker

        ''' <summary>
        ''' List of font names
        ''' it is synchronized with the list box of selected fonts
        ''' </summary>
        Private ReadOnly _lSelectedFonts As New ObservableCollection(Of String)()

#Region "Exposed methods"
        ''' <summary>
        ''' Initializes a new instance of the <see cref="FontPicker"/> class.
        ''' </summary>
        ''' <param name="initFontList">The init font list.</param>
        Public Sub New(initFontList As String)
            ' handle selected fonts
            Dim arrFonts As String() = initFontList.Split(","c)
            For Each aFontName As String In arrFonts
                Dim candidate As String = aFontName.Trim()
                If candidate.Length >= 2 AndAlso candidate(0) = "'"c AndAlso candidate(candidate.Length - 1) = "'"c Then
                    candidate = candidate.Substring(1, candidate.Length - 2)
                End If
                If candidate.Length = 0 Then
                    Continue For
                End If
                _lSelectedFonts.Add(candidate)
            Next

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Comma-separeted list of fonts
        ''' </summary>
        ''' <value>The selected font list.</value>
        Public ReadOnly Property SelectedFontList As String
            Get
                ' Build from list
                Dim sb As New StringBuilder()
                Dim first As Boolean = True                              ' A flag indicating we're going to write the first entry

                ' Iterate through selected fonts
                For Each aFontname As String In _lSelectedFonts
                    ' Handle first entry
                    If Not first Then
                        sb.Append(", ")
                    Else
                        first = False
                    End If

                    ' Append font's name
                    If aFontname.Contains(" ") Then
                        sb.Append("'"c)
                        sb.Append(aFontname)
                        sb.Append("'"c)
                    Else
                        sb.Append(aFontname)
                    End If
                Next

                ' done
                Return sb.ToString()
            End Get
        End Property
#End Region

        ''' <summary>
        ''' Handles the Loaded event of the FontPicker control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub fontPicker_Loaded(sender As Object, e As RoutedEventArgs)
            ' populate list of selected fonts
            LbSelectedFonts.ItemsSource = _lSelectedFonts

            ' populate list of installed fonts
            LbInstalledFonts.ItemsSource = System.Drawing.FontFamily.Families.Select(Function(fam) fam.Name).OrderBy(Function(f) f.ToString())

            ' set some buttons' enable status
            lbSelectedFonts_SelectedChanged(Me, New RoutedEventArgs())
            tbCustomFont_TextChanged(Me, New RoutedEventArgs())
        End Sub

#Region "Adding fonts to the selected list"

        ''' <summary>
        ''' Handles the Click event of the BtAddInstalledFont control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btAddInstalledFont_Click(sender As Object, e As RoutedEventArgs)
            Dim sFont As String = CStr(LbInstalledFonts.SelectedItem)
            If Not String.IsNullOrEmpty(sFont) Then
                _lSelectedFonts.Add(sFont)
                LbInstalledFonts.SelectedItem = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtAddGenericFont control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btAddGenericFont_Click(sender As Object, e As RoutedEventArgs)
            Dim sFont As String = CbGenericFonts.Text
            If Not String.IsNullOrEmpty(sFont) Then
                _lSelectedFonts.Add(sFont)
                CbGenericFonts.SelectedItem = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BrAddCustomFont control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub brAddCustomFont_Click(sender As Object, e As RoutedEventArgs)
            _lSelectedFonts.Add(TbCustomFont.Text)
        End Sub
#End Region

#Region "Selected fonts handling"
        ''' <summary>
        ''' Handles the Click event of the BtMoveUp control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btMoveUp_Click(sender As Object, e As RoutedEventArgs)
            Dim sel As Integer = LbSelectedFonts.SelectedIndex
            If sel > 0 Then
                ' exchange items in the list box
                Dim oCurrent As String = CStr(LbSelectedFonts.SelectedItem)
                _lSelectedFonts.RemoveAt(sel)
                _lSelectedFonts.Insert(sel - 1, oCurrent)
                LbSelectedFonts.SelectedIndex = sel - 1
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtMoveDown control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btMoveDown_Click(sender As Object, e As RoutedEventArgs)
            Dim sel As Integer = LbSelectedFonts.SelectedIndex
            If sel <> -1 AndAlso sel < _lSelectedFonts.Count - 1 Then
                ' exchange items in the list box
                Dim oCurrent As String = CStr(LbSelectedFonts.SelectedItem)
                _lSelectedFonts.RemoveAt(sel)
                _lSelectedFonts.Insert(sel + 1, oCurrent)
                LbSelectedFonts.SelectedIndex = sel + 1
            End If
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtRemove control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btRemove_Click(sender As Object, e As RoutedEventArgs)
            Dim sel As Integer = LbSelectedFonts.SelectedIndex
            If sel <> -1 Then ' precaution
                ' remove from local list
                _lSelectedFonts.RemoveAt(sel)
            End If
        End Sub

        ''' <summary>
        ''' Handles the SelectedChanged event of the LbSelectedFonts control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub lbSelectedFonts_SelectedChanged(sender As Object, e As RoutedEventArgs)
            Dim sel As Integer = LbSelectedFonts.SelectedIndex
            If sel <> -1 Then
                BtRemove.IsEnabled = True
                BtMoveUp.IsEnabled = sel <> 0
                BtMoveDown.IsEnabled = sel <> _lSelectedFonts.Count - 1
            Else ' sel == -1 => disable some buttons
                BtMoveUp.IsEnabled = False
                BtMoveDown.IsEnabled = False
                BtRemove.IsEnabled = False
            End If
        End Sub
#End Region

        ''' <summary>
        ''' Handles the Click event of the BtOk control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btOk_Click(sender As Object, e As RoutedEventArgs)
            Me.DialogResult = True
            Me.Close()
        End Sub

        ''' <summary>
        ''' Handles the Click event of the BtCancel control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub btCancel_Click(sender As Object, e As RoutedEventArgs)
            Me.DialogResult = False
            Me.Close()
        End Sub

        ''' <summary>
        ''' Handles the TextChanged event of the TbCustomFont control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub tbCustomFont_TextChanged(sender As Object, e As RoutedEventArgs)
            BtAddCustomFont.IsEnabled = TbCustomFont.Text.Trim().Length > 0 AndAlso Not TbCustomFont.Text.Contains(",")
        End Sub
    End Class
End Namespace
