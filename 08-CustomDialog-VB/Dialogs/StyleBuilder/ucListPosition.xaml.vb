Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Class ucLists
    ''' </summary>
    <ToolboxItem(False)>
    <FormSelectorPage("List Position", "list-style-position")>
    Partial Public Class ucListPosition
        Implements IEditorStylePage

        ''' <summary>
        ''' The _dict
        ''' </summary>
        Private ReadOnly _dict As Dictionary(Of String, String)

#Region "Preset of possible values"

        ''' <summary>
        ''' The _ list style position
        ''' </summary>
        Private ReadOnly _listStylePosition As New List(Of KeyValuePair(Of String, String))()
#End Region

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ucListPosition"/> class.
        ''' </summary>
        ''' <param name="dict">The dict.</param>
        Public Sub New(dict As Dictionary(Of String, String))
            _dict = dict

#Region "Initialize presets"
            _listStylePosition.Add(New KeyValuePair(Of String, String)("<Not Set>", ""))
            _listStylePosition.Add(New KeyValuePair(Of String, String)("Outside (text is indented in)", "outside"))
            _listStylePosition.Add(New KeyValuePair(Of String, String)("Inside (text is not indented)", "inside"))
#End Region

            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Flushes the content of the user control back to the dictionary
        ''' </summary>
        Public Sub FlushContent() Implements IEditorStylePage.FlushContent
            _dict.Remove("list-style-position")
            _dict("list-style-position") = CStr(CbBulletPosition.SelectedValue)
        End Sub

        ''' <summary>
        ''' Handles the Loaded event of the ucLists control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub ucLists_Loaded(sender As Object, e As RoutedEventArgs)
#Region "set data sources"
            CbBulletPosition.ItemsSource = _listStylePosition
            CbBulletPosition.DisplayMemberPath = "Key"
            CbBulletPosition.SelectedValuePath = "Value"
            CbBulletPosition.SelectedIndex = 0
#End Region

#Region "parse"
            Dim value As String = Nothing
            If _dict.TryGetValue("list-style-position", value) Then
                Dim n As Integer = _listStylePosition.Count
                For i As Integer = 0 To n - 1
                    If String.Equals(value, _listStylePosition(i).Value, StringComparison.InvariantCultureIgnoreCase) Then
                        CbBulletPosition.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

#End Region
        End Sub
    End Class
End Namespace
