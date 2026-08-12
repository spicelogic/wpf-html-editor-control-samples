using System.Windows;
using MvvmDataBinding.ViewModel;

namespace MvvmDataBinding;

public partial class TwoWayPropertyChangedBinding
{
    public TwoWayPropertyChangedBinding()
    {
        InitializeComponent();
    }

    private void TwoWayPropertyChangedBinding_OnLoaded(object sender, RoutedEventArgs e)
    {
        DataContext = new TwoWayPropertyChangedViewModel
        {
            BodyHtml = "<p>Type here, or in the text box below, and watch the other update instantly.</p>"
        };
    }

    private void ShowBodyHtmlButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(this, ((TwoWayPropertyChangedViewModel)DataContext).BodyHtml, "View model BodyHtml",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
