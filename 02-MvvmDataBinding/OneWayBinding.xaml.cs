using System.Windows;
using MvvmDataBinding.ViewModel;

namespace MvvmDataBinding;

public partial class OneWayBinding
{
    public OneWayBinding()
    {
        InitializeComponent();
    }

    private void OneWayBinding_OnLoaded(object sender, RoutedEventArgs e)
    {
        DataContext = new OneWayBindingViewModel
        {
            BodyHtml = "<p>Type in the text box above and watch this editor follow along.</p>"
        };
    }
}
