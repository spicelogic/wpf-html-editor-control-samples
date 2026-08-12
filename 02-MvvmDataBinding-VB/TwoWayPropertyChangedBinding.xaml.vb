Imports System.Windows
Imports MvvmDataBinding.ViewModel

Namespace Global.MvvmDataBinding

    Partial Public Class TwoWayPropertyChangedBinding

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub TwoWayPropertyChangedBinding_OnLoaded(sender As Object, e As RoutedEventArgs)
            DataContext = New TwoWayPropertyChangedViewModel With {
                .BodyHtml = "<p>Type here, or in the text box below, and watch the other update instantly.</p>"
            }
        End Sub

        Private Sub ShowBodyHtmlButton_OnClick(sender As Object, e As RoutedEventArgs)
            MessageBox.Show(Me, CType(DataContext, TwoWayPropertyChangedViewModel).BodyHtml, "View model BodyHtml",
                MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

    End Class

End Namespace
