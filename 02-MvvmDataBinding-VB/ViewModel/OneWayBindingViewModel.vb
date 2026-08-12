Imports System.ComponentModel

Namespace Global.MvvmDataBinding.ViewModel

    ''' <summary>
    ''' Backs the OneWay binding scenario. An ordinary INotifyPropertyChanged view model; nothing
    ''' here is specific to the editor.
    ''' </summary>
    Public Class OneWayBindingViewModel
        Implements INotifyPropertyChanged

        Private _bodyHtml As String

        Public Property BodyHtml As String
            Get
                Return _bodyHtml
            End Get
            Set(value As String)
                If _bodyHtml = value Then
                    Return
                End If

                _bodyHtml = value
                OnPropertyChanged(NameOf(BodyHtml))
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

    End Class

End Namespace
