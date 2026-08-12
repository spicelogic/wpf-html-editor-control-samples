Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Input
Imports System.Windows.Media
Imports SpiceLogic.HtmlEditor.WPF.Models.Dialogs
Imports SpiceLogic.HtmlEditor.WPF.Models.Services

Namespace Global.CustomDialog.Dialogs.ColorPickerDialog

    ''' <summary>
    ''' Interaction logic for ColorPickerDialog.xaml
    ''' </summary>
    Partial Public Class ColorPickerDialog
        Implements IColorPickerDialog

        Private ReadOnly _startingColor As New Color()

        Public Sub New()
            InitializeComponent()
        End Sub


        Private Sub onSelectedColorChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Color))
            If e.NewValue <> SelectedColor Then
                TwoButtonPanel.LeftButton.IsEnabled = True
            End If
        End Sub

        Protected Overrides Sub OnClosing(e As CancelEventArgs)
            TwoButtonPanel.LeftButton.IsEnabled = False
            MyBase.OnClosing(e)
        End Sub

        ''' <summary>
        ''' The selected color
        ''' </summary>
        Public Property SelectedColor As Color Implements IColorPickerDialog.SelectedColor

        ''' <summary>
        ''' The initial color that will be selected when the dialog opens
        ''' </summary>
        Public Property StartingColor As Color Implements IColorPickerDialog.StartingColor
            Get
                Return _startingColor
            End Get
            Set(value As Color)
                cPicker.SelectedColor = value

                If TwoButtonPanel.LeftButton IsNot Nothing Then
                    TwoButtonPanel.LeftButton.IsEnabled = False
                End If
            End Set
        End Property

        Private Sub TwoButtonPanel_OnOnRightButtonClicked(sender As Object, e As RoutedEventArgs)
            'Cancel button

            TwoButtonPanel.LeftButton.IsEnabled = False
            DialogResult = False
        End Sub

        Private Sub TwoButtonPanel_OnOnLeftButtonClicked(sender As Object, e As RoutedEventArgs)
            'OK button

            TwoButtonPanel.LeftButton.IsEnabled = False
            SelectedColor = cPicker.SelectedColor
            DialogResult = True
            Hide()
        End Sub

        Private Sub DialogHeader_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            DragMove()
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
    End Class
End Namespace
