Imports System.Windows
Imports MvvmDataBinding.ViewModel

Namespace Global.MvvmDataBinding

    Partial Public Class TwoWayLostFocusBinding

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub TwoWayLostFocusBinding_OnLoaded(sender As Object, e As RoutedEventArgs)
            DataContext = New TwoWayLostFocusViewModel With {
                .BodyHtml = "<p>Type here, then click the button below without tabbing out first.</p>"
            }
        End Sub

        Private Sub ShowBodyHtmlButton_OnClick(sender As Object, e As RoutedEventArgs)
            MessageBox.Show(Me, CType(DataContext, TwoWayLostFocusViewModel).BodyHtml, "View model BodyHtml",
                MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

    End Class

End Namespace
