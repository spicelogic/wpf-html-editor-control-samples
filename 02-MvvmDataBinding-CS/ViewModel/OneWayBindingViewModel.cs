using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmDataBinding.ViewModel;

/// <summary>
/// Backs the OneWay binding scenario. An ordinary INotifyPropertyChanged view model; nothing here
/// is specific to the editor.
/// </summary>
public class OneWayBindingViewModel : INotifyPropertyChanged
{
    private string? _bodyHtml;

    public string? BodyHtml
    {
        get => _bodyHtml;
        set
        {
            if (_bodyHtml == value)
            {
                return;
            }

            _bodyHtml = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
