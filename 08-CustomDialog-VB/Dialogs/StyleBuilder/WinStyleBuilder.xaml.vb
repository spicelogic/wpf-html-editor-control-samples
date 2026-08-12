Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports SpiceLogic.HtmlEditor.Abstractions
Imports SpiceLogic.HtmlEditor.Abstractions.Dialogs.StyleBuilder
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services
Imports System.Linq
Imports SpiceLogic.HtmlEditor.WPF.ToolbarModule

Namespace Global.CustomDialog.Dialogs.StyleBuilder

    ''' <summary>
    ''' Interaction logic for WinStyleBuilder.xaml
    ''' </summary>
    Partial Public Class WinStyleBuilder
        Implements IStyleBuilderDialog

        Private _tabChanged As Boolean = False

        ''' <summary>
        ''' User control currently displayed on the right
        ''' </summary>
        Private _selectedPage As Control

        ''' <summary>
        ''' Parsed style string
        ''' </summary>
        Private ReadOnly _propertiesDict As New Dictionary(Of String, String)()

        ''' <summary>
        ''' A method creating the color dialog
        ''' </summary>
        Private ReadOnly _createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate

#Region "-------- Constructors ----------"

        ''' <summary>
        ''' Initializes a new instance of the <see cref="WinStyleBuilder" /> class.
        ''' </summary>
        ''' <param name="createColorDialogMethod">A method creating the color dialog</param>
        Public Sub New(createColorDialogMethod As DialogFactoryDelegates.CreateColorPickerDialogDelegate)
            _createColorDialogMethod = createColorDialogMethod

            InitializeComponent()
        End Sub

#End Region


        ''' <summary>
        ''' Parses the style argument.
        ''' </summary>
        ''' <param name="styleArg">The style arg.</param>
        ''' <param name="parsingTolerance">The parsing tolerance.</param>
        ''' <exception cref="System.ArgumentException">
        ''' </exception>
        Public Sub ParseStyleArgument(styleArg As String, parsingTolerance As ParsingTolerance) Implements IStyleBuilderDialogBase.ParseStyleArgument
            Try
                Dim position As Integer = 0

                If String.IsNullOrEmpty(styleArg) Then
                    styleArg = String.Empty
                End If
                styleArg = styleArg.Trim()

                ' Handle enclosed quotes
                If styleArg.Length >= 2 AndAlso styleArg(0) = """"c AndAlso styleArg(styleArg.Length - 1) = """"c Then
                    styleArg = styleArg.Substring(1, styleArg.Length - 2).Trim()
                End If

                Do While position < styleArg.Length
                    Dim separatorI As Integer = styleArg.IndexOf(":"c, position)
                    If separatorI <> -1 Then ' ':' found
                        ' localize tag
                        Dim tag As String = styleArg.Substring(position, separatorI - position).Trim().ToLowerInvariant()

                        ' localize parameters
                        Dim nextSemicolonI As Integer = styleArg.IndexOf(";"c, separatorI + 1)
                        If nextSemicolonI = -1 Then
                            nextSemicolonI = styleArg.Length
                        End If
                        Dim parameters As String = styleArg.Substring(separatorI + 1, nextSemicolonI - separatorI - 1).Trim()

                        ' store in dictionary
                        _propertiesDict(tag) = parameters

                        ' move starting position
                        position = nextSemicolonI + 1
                    Else                 ' ':' not found
                        Dim reminder As String = styleArg.Substring(position).Trim()
                        If reminder.Length <> 0 AndAlso parsingTolerance <> ParsingTolerance.TolerateUnknownTags Then
                            Throw New ArgumentException($"Tag {reminder} has no specification")
                        End If

                        Exit Do           ' no exception was thrown - end parsing
                    End If
                Loop

                If parsingTolerance = ParsingTolerance.NoTolerance Then
                    ' create list of handling classes
                    Dim handlingClasses As New List(Of FormSelectorPageAttribute)()
                    Dim aThisAssembly As Assembly = Assembly.GetAssembly(Me.GetType())
                    If aThisAssembly IsNot Nothing Then
                        For Each t As Type In aThisAssembly.GetTypes()
                            Dim attr As Attribute = Attribute.GetCustomAttribute(t, GetType(FormSelectorPageAttribute))
                            If attr IsNot Nothing Then
                                handlingClasses.Add(CType(attr, FormSelectorPageAttribute))
                            End If
                        Next
                    End If

                    ' iterate through tags
                    For Each kvp As KeyValuePair(Of String, String) In _propertiesDict
                        Dim tagHandled As Boolean = handlingClasses.Any(Function(attr) attr.HandlesTag(kvp.Key))
                        ' search for a class to handle this attribute

                        If Not tagHandled Then
                            Throw New ArgumentException($"Tag {kvp.Key} can't be handled.")
                        End If
                    Next
                End If
            Catch err As Exception
                MessageBox.Show(err.Message, "Error parsing")
            End Try
        End Sub

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


#Region "------- Exposed Getters ----------------"

        ''' <summary>
        ''' Gets a value indicating whether [remove style].
        ''' </summary>
        ''' <value><c>true</c> if [remove style]; otherwise, <c>false</c>.</value>
        Public Property RemoveStyle As Boolean Implements IStyleBuilderDialogBase.RemoveStyle
            Get
                Return Equals(ChkRemoveStyle.IsChecked, True)
            End Get
            Set(value As Boolean)
                ChkRemoveStyle.IsChecked = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the CSS text value.
        ''' </summary>
        ''' <value>The CSS text value.</value>
        Public ReadOnly Property CSSTextValue As String Implements IStyleBuilderDialogBase.CSSTextValue
            Get
                ' iterate through properties and construct string representation
                Dim sb As New StringBuilder()
                For Each kvp As KeyValuePair(Of String, String) In _propertiesDict
                    If kvp.Value IsNot Nothing AndAlso kvp.Value.Trim().Length > 0 Then
                        sb.Append(kvp.Key)
                        sb.Append(": ")
                        sb.Append(kvp.Value)
                        sb.Append("; ")
                    End If
                Next
                Return sb.ToString().Trim()
            End Get
        End Property

#End Region


        ''' <summary>
        ''' Selects first page on Loaded event
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        Private Sub winStyleBuilder_Loaded(sender As Object, e As RoutedEventArgs)
            TabControl.SelectedIndex = 0
        End Sub

        ''' <summary>
        ''' Updates the currently visible page
        ''' Internally this method looks up current assembly using reflection for the appropriate page control to be shown
        ''' The page control should be marked with FormSelectorPageAttribute and have a constructor which accepts Dictionary&lt;string, string&gt;
        ''' </summary>
        ''' <param name="sender">The event source</param>
        ''' <param name="e">The event data</param>
        Private Sub TabControl_OnSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            'ignore SelectionChanged events of other selector controls
            If Not Equals(e.OriginalSource, TabControl) Then
                Return
            End If

            ' Dispose currently selector control if any
            If _selectedPage IsNot Nothing Then
                CType(_selectedPage, IEditorStylePage).FlushContent()

                Dim selectedTabItem As TabItem = TryCast(TabControl.Items(TabControl.SelectedIndex), TabItem)
                If selectedTabItem IsNot Nothing Then
                    selectedTabItem.Content = Nothing
                End If

                _selectedPage = Nothing
            End If

            ' Check selection and look for the control
            If TabControl.SelectedItem IsNot Nothing Then
                ' Name of the page
                Dim sName As String = CStr(CType(TabControl.SelectedItem, TabItem).Header)

                ' Search this assembly for the appropriate control
                Dim aThisAssembly As Assembly = Assembly.GetAssembly(Me.GetType())
                If aThisAssembly IsNot Nothing Then
                    For Each t As Type In aThisAssembly.GetTypes()
                        Dim attr As Attribute = Attribute.GetCustomAttribute(t, GetType(FormSelectorPageAttribute))
                        If attr IsNot Nothing Then
                            Dim pageAttr As FormSelectorPageAttribute = CType(attr, FormSelectorPageAttribute)
                            If String.Compare(pageAttr.PageName, sName, StringComparison.OrdinalIgnoreCase) = 0 Then
                                Dim constructorInfo As ConstructorInfo = t.GetConstructors().FirstOrDefault()
                                If constructorInfo IsNot Nothing Then
                                    If constructorInfo.GetParameters().Any(Function(q) q.ParameterType =
                                            GetType(DialogFactoryDelegates.CreateColorPickerDialogDelegate)) Then
                                        'the constructors need DialogFactoryDelegates.CreateColorPickerDialogDelegate
                                        _selectedPage = CType(Activator.CreateInstance(t, _propertiesDict,
                                            _createColorDialogMethod), Control)
                                    Else
                                        _selectedPage = CType(Activator.CreateInstance(t, _propertiesDict), Control)
                                    End If
                                End If

                                Exit For
                            End If
                        End If
                    Next
                End If

                ' Dummy check
                If _selectedPage Is Nothing Then
                    Throw New NotImplementedException($"Page {sName} is not implemented in Style Page Selector.")
                End If

                ' add it to the form
                Dim selectedTabItem As TabItem = TryCast(TabControl.Items(TabControl.SelectedIndex), TabItem)
                If selectedTabItem IsNot Nothing Then
                    selectedTabItem.Content = _selectedPage
                End If

            End If
        End Sub

        ''' <summary>
        ''' Handles the Closed event of the frmFontSelector control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        Private Sub winStyleBuilder_Closed(sender As Object, eventArgs As EventArgs)
            If _selectedPage IsNot Nothing Then
                CType(_selectedPage, IEditorStylePage).FlushContent()

                Dim selectedTabItem As TabItem = TryCast(TabControl.Items(TabControl.SelectedIndex), TabItem)
                If selectedTabItem IsNot Nothing Then
                    selectedTabItem.Content = Nothing
                End If

                _selectedPage = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Handles the MouseLeftButtonDown event of the DialogHeader control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
        End Sub

        ''' <summary>
        ''' Handles the OnRightButtonClicked event of the TwoButtonPanel control.
        ''' </summary>
        ''' <param name="sender">The event source.</param>
        ''' <param name="e">The event data.</param>
        Private Sub TwoButtonPanel_OnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            'cancel button

            DialogResult = False
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
    End Class
End Namespace
