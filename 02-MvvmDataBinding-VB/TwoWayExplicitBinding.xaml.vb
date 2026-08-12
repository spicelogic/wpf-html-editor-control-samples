Imports System.Windows
Imports MvvmDataBinding.ViewModel

Namespace Global.MvvmDataBinding

    Partial Public Class TwoWayExplicitBinding

        Public Sub New()
            InitializeComponent()

            DataContext = New EmailViewModel("Quarterly update", "<p>Draft your message here.</p>")
        End Sub

        Private Sub InsertGreetingButton_OnClick(sender As Object, e As RoutedEventArgs)
            ' Setting Body on the view model flows into the editor automatically: a TwoWay binding
            ' always pushes a source PropertyChanged into its target, regardless of
            ' UpdateSourceTrigger - that setting only governs the opposite, target-to-source direction.
            Dim viewModel = CType(DataContext, EmailViewModel)
            viewModel.Body = "<p><strong>Hello!</strong></p>" & viewModel.Body
        End Sub

        Private Sub GetEmailButton_OnClick(sender As Object, e As RoutedEventArgs)
            ' The one gotcha worth knowing about this control: the editor hosts a live document, and
            ' the bound Body property only reflects whatever WPF's binding last pushed into it. With
            ' UpdateSourceTrigger=Explicit that never happens on its own - nothing flows from the
            ' editor to the view model until code asks for it.
            '
            ' UpdateBindings() pushes the editor's current DocumentHtml, BodyHtml and DocumentTitle
            ' into their dependency properties immediately, which in turn flows into any bound view
            ' model property through the binding. Always call it before reading a bound value on
            ' demand - this is the single most common support question about this control, and it
            ' applies no matter which UpdateSourceTrigger the binding uses (LostFocus and
            ' PropertyChanged included).
            Editor.UpdateBindings()

            Dim viewModel = CType(DataContext, EmailViewModel)
            MessageBox.Show(Me, viewModel.ToString(), "Email body", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

    End Class

End Namespace
