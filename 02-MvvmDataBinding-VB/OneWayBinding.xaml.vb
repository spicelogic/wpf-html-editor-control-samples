Imports System.Windows
Imports MvvmDataBinding.ViewModel

Namespace Global.MvvmDataBinding

    Partial Public Class OneWayBinding

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub OneWayBinding_OnLoaded(sender As Object, e As RoutedEventArgs)
            DataContext = New OneWayBindingViewModel With {
                .BodyHtml = "<p>Type in the text box above and watch this editor follow along.</p>"
            }
        End Sub

    End Class

End Namespace
