using System.Windows;

namespace MvvmDataBinding;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
        DataContext = _viewModel;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // The one gotcha worth knowing: the editor hosts a live document. The bound Html
        // property only reflects whatever WPF's binding last pushed into it, which by default
        // happens when the editor loses focus. If your code path reads the view model without
        // the editor first losing focus, or you simply want the latest keystrokes right now,
        // the view model can be behind the last few edits.
        //
        // UpdateBindings() pushes the editor's current DocumentHtml, BodyHtml and DocumentTitle
        // into their dependency properties immediately, which in turn flows into the bound Html
        // property through the binding. Always call it before reading a bound value on demand -
        // this is the single most common support question about this control.
        Editor.UpdateBindings();

        string savedHtml = _viewModel.Html;
        MessageBox.Show(this, $"Saved {savedHtml.Length} characters from the view model.",
            "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
