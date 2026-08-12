using System.Windows;
using MvvmDataBinding.ViewModel;

namespace MvvmDataBinding;

public partial class TwoWayLostFocusBinding
{
    public TwoWayLostFocusBinding()
    {
        InitializeComponent();
    }

    private void TwoWayLostFocusBinding_OnLoaded(object sender, RoutedEventArgs e)
    {
        DataContext = new TwoWayLostFocusViewModel
        {
            BodyHtml = "<p>Type here, then click the button below without tabbing out first.</p>"
        };
    }

    private void ShowBodyHtmlButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(this, ((TwoWayLostFocusViewModel)DataContext).BodyHtml, "View model BodyHtml",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
