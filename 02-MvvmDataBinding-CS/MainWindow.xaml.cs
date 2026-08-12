using System.Windows;

namespace MvvmDataBinding;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // No license key set, so the editor runs in trial mode. See the licensing docs linked in the README.
    }

    private void OneWayButton_OnClick(object sender, RoutedEventArgs e)
    {
        new OneWayBinding().ShowDialog();
    }

    private void TwoWayLostFocusButton_OnClick(object sender, RoutedEventArgs e)
    {
        new TwoWayLostFocusBinding().ShowDialog();
    }

    private void TwoWayExplicitButton_OnClick(object sender, RoutedEventArgs e)
    {
        new TwoWayExplicitBinding().ShowDialog();
    }

    private void TwoWayPropertyButton_OnClick(object sender, RoutedEventArgs e)
    {
        new TwoWayPropertyChangedBinding().ShowDialog();
    }

    private void ElementBindingButton_OnClick(object sender, RoutedEventArgs e)
    {
        new ElementBinding().ShowDialog();
    }
}
