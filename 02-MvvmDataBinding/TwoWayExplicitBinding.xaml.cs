using System.Windows;
using MvvmDataBinding.ViewModel;

namespace MvvmDataBinding;

public partial class TwoWayExplicitBinding
{
    public TwoWayExplicitBinding()
    {
        InitializeComponent();

        DataContext = new EmailViewModel("Quarterly update", "<p>Draft your message here.</p>");
    }

    private void InsertGreetingButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Setting Body on the view model flows into the editor automatically: a TwoWay binding always
        // pushes a source PropertyChanged into its target, regardless of UpdateSourceTrigger - that
        // setting only governs the opposite, target-to-source direction.
        var viewModel = (EmailViewModel)DataContext;
        viewModel.Body = "<p><strong>Hello!</strong></p>" + viewModel.Body;
    }

    private void GetEmailButton_OnClick(object sender, RoutedEventArgs e)
    {
        // The one gotcha worth knowing about this control: the editor hosts a live document, and the
        // bound Body property only reflects whatever WPF's binding last pushed into it. With
        // UpdateSourceTrigger=Explicit that never happens on its own - nothing flows from the editor
        // to the view model until code asks for it.
        //
        // UpdateBindings() pushes the editor's current DocumentHtml, BodyHtml and DocumentTitle into
        // their dependency properties immediately, which in turn flows into any bound view model
        // property through the binding. Always call it before reading a bound value on demand - this
        // is the single most common support question about this control, and it applies no matter
        // which UpdateSourceTrigger the binding uses (LostFocus and PropertyChanged included).
        Editor.UpdateBindings();

        var viewModel = (EmailViewModel)DataContext;
        MessageBox.Show(this, viewModel.ToString(), "Email body", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
