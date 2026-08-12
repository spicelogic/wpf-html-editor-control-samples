using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SpiceLogic.HtmlEditor.WPF.Models.Dialogs;

namespace CustomDialog.Dialogs.ColorPickerDialog;

/// <summary>
/// Interaction logic for ColorPickerDialog.xaml
/// </summary>
public partial class ColorPickerDialog : IColorPickerDialog
{
    private readonly Color _startingColor = new();

    public ColorPickerDialog()
    {
        InitializeComponent();
    }

        
    private void onSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
    {
        if (e.NewValue != SelectedColor)
            TwoButtonPanel.LeftButton.IsEnabled = true;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        TwoButtonPanel.LeftButton.IsEnabled = false;
        base.OnClosing(e);
    }

    /// <summary>
    /// The selected color
    /// </summary>
    public Color SelectedColor { get; private set; }

    /// <summary>
    /// The initial color that will be selected when the dialog opens
    /// </summary>
    public Color StartingColor
    {
        get => _startingColor;
        set
        {
            cPicker.SelectedColor = value;

            if (TwoButtonPanel.LeftButton != null)
                TwoButtonPanel.LeftButton.IsEnabled = false;
        }
    }

    private void TwoButtonPanel_OnOnRightButtonClicked(object sender, RoutedEventArgs e)
    {
        //Cancel button

        TwoButtonPanel.LeftButton.IsEnabled = false;
        DialogResult = false;
    }

    private void TwoButtonPanel_OnOnLeftButtonClicked(object sender, RoutedEventArgs e)
    {
        //OK button

        TwoButtonPanel.LeftButton.IsEnabled = false;
        SelectedColor = cPicker.SelectedColor;
        DialogResult = true;
        Hide();
    }

    private void DialogHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    public void Dispose()
    {
    }
}