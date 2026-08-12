using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmDataBinding;

/// <summary>
/// A minimal view model with a plain string property bound to the editor's BodyHtml.
/// Nothing here is specific to the SpiceLogic control; this is an ordinary INotifyPropertyChanged
/// view model, which is the point: the editor binds like any other WPF control.
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private string _html =
        "<p>Edit this document, then click <b>Save to view model</b> to see the current " +
        "view model value below.</p>";

    public string Html
    {
        get => _html;
        set
        {
            if (_html == value)
            {
                return;
            }

            _html = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
