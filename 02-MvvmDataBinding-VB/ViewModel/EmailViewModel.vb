Imports System.ComponentModel

Namespace Global.MvvmDataBinding.ViewModel

    Public Class EmailViewModel
        Implements INotifyPropertyChanged

        Private _subject As String
        Private _body As String

        Public Sub New()
        End Sub

        Public Sub New(subject As String, body As String)
            _subject = subject
            _body = body
        End Sub

        Public Property Subject As String
            Get
                Return _subject
            End Get
            Set(value As String)
                If _subject = value Then
                    Return
                End If

                _subject = value
                OnPropertyChanged(NameOf(Subject))
            End Set
        End Property

        Public Property Body As String
            Get
                Return _body
            End Get
            Set(value As String)
                If _body = value Then
                    Return
                End If

                _body = value
                OnPropertyChanged(NameOf(Body))
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Overrides Function ToString() As String
            Return $"Subject: {Subject}{vbLf}Body: {Body}"
        End Function

    End Class

End Namespace
