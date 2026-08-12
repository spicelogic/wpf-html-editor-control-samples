Imports System.Windows

Namespace Global.MvvmDataBinding

    Partial Public Class MainWindow

        Public Sub New()
            InitializeComponent()

            ' No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        End Sub

        Private Sub OneWayButton_OnClick(sender As Object, e As RoutedEventArgs)
            Call New OneWayBinding().ShowDialog()
        End Sub

        Private Sub TwoWayLostFocusButton_OnClick(sender As Object, e As RoutedEventArgs)
            Call New TwoWayLostFocusBinding().ShowDialog()
        End Sub

        Private Sub TwoWayExplicitButton_OnClick(sender As Object, e As RoutedEventArgs)
            Call New TwoWayExplicitBinding().ShowDialog()
        End Sub

        Private Sub TwoWayPropertyButton_OnClick(sender As Object, e As RoutedEventArgs)
            Call New TwoWayPropertyChangedBinding().ShowDialog()
        End Sub

        Private Sub ElementBindingButton_OnClick(sender As Object, e As RoutedEventArgs)
            Call New ElementBinding().ShowDialog()
        End Sub

    End Class

End Namespace
