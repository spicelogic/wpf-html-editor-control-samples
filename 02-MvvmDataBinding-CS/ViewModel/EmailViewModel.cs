using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmDataBinding.ViewModel;

public class EmailViewModel : INotifyPropertyChanged
{
    private string? _subject;
    private string? _body;

    public EmailViewModel()
    {
    }

    public EmailViewModel(string subject, string body)
    {
        _subject = subject;
        _body = body;
    }

    public string? Subject
    {
        get => _subject;
        set
        {
            if (_subject == value)
            {
                return;
            }

            _subject = value;
            OnPropertyChanged();
        }
    }

    public string? Body
    {
        get => _body;
        set
        {
            if (_body == value)
            {
                return;
            }

            _body = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public override string ToString() => $"Subject: {Subject}\nBody: {Body}";
}
