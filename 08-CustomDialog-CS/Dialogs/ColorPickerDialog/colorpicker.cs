using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomDialog.Dialogs.ColorPickerDialog;

[DesignTimeVisible(false)]
public class ColorPicker : Control
{
    private const string ColorMarkerName = "PART_ColorMarker";
    private const string ColorSliderName = "PART_ColorSlider";
    private const string ColorDetailName = "PART_ColorDetail";

    private readonly TranslateTransform _markerTransform = new();

    private SpectrumSlider _mColorSlider;
    private FrameworkElement _mColorDetail;
    private Path _mColorMarker;
    private Point? _mColorPosition;
    private Color _mColor;
    private bool _shouldFindPoint;
    private bool _templateApplied;
    private bool _isAlphaChange;

    /// <summary>
    /// Initializes the <see cref="ColorPicker"/> class.
    /// </summary>
    static ColorPicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
    public ColorPicker()
    {
        _templateApplied = false;
        _mColor = Colors.White;
        _shouldFindPoint = true;
        SetValue(AProperty, _mColor.A);
        SetValue(RProperty, _mColor.R);
        SetValue(GProperty, _mColor.G);
        SetValue(BProperty, _mColor.B);
        SetValue(SelectedColorProperty, _mColor);
    }

    /// <summary>
    /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _mColorDetail = GetTemplateChild(ColorDetailName) as FrameworkElement;
        _mColorMarker = GetTemplateChild(ColorMarkerName) as Path;
        _mColorSlider = GetTemplateChild(ColorSliderName) as SpectrumSlider;

        if (_mColorSlider != null)
            _mColorSlider.ValueChanged += baseColorChanged;

        if (_mColorMarker != null)
        {
            _mColorMarker.RenderTransform = _markerTransform;
            _mColorMarker.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        if (_mColorDetail != null)
        {
            _mColorDetail.MouseLeftButtonDown += onMouseLeftButtonDown;
            _mColorDetail.PreviewMouseMove += onMouseMove;
            _mColorDetail.SizeChanged += colorDetailSizeChanged;
        }

        _templateApplied = true;
        _shouldFindPoint = true;
        _isAlphaChange = false;

        SelectedColor = _mColor;
    }

    // Gets or sets the selected color.
    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set
        {
            SetValue(SelectedColorProperty, _mColor);
            setColor(value);
        }
    }

    // Gets or sets the ARGB alpha value of the selected color.
    public byte A
    {
        get => (byte)GetValue(AProperty);
        set => SetValue(AProperty, value);
    }

    // Gets or sets the ARGB red value of the selected color.
    public byte R
    {
        get => (byte)GetValue(RProperty);
        set => SetValue(RProperty, value);
    }

    // Gets or sets the ARGB green value of the selected color.
    public byte G
    {
        get => (byte)GetValue(GProperty);
        set => SetValue(GProperty, value);
    }

    // Gets or sets the ARGB blue value of the selected color.
    public byte B
    {
        get => (byte)GetValue(BProperty);
        set => SetValue(BProperty, value);
    }

    // Gets or sets the ScRGB alpha value of the selected color.
    public double ScA
    {
        get => (double)GetValue(ScAProperty);
        set => SetValue(ScAProperty, value);
    }

    // Gets or sets the ScRGB red value of the selected color.
    public double ScR
    {
        get => (double)GetValue(ScRProperty);
        set => SetValue(RProperty, value);
    }

    // Gets or sets the ScRGB green value of the selected color.
    public double ScG
    {
        get => (double)GetValue(ScGProperty);
        set => SetValue(GProperty, value);
    }

    // Gets or sets the ScRGB blue value of the selected color.
    public double ScB
    {
        get => (double)GetValue(BProperty);
        set => SetValue(BProperty, value);
    }

    // Gets or sets the the selected color in hexadecimal notation.
    public string HexadecimalString
    {
        get => (string)GetValue(HexadecimalStringProperty);
        set => SetValue(HexadecimalStringProperty, value);
    }

    /// <summary>
    /// Occurs when [selected color changed].
    /// </summary>
    public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
    {
        add => AddHandler(SelectedColorChangedEvent, value);
        remove => RemoveHandler(SelectedColorChangedEvent, value);
    }

    /// <summary>
    /// The selected color property
    /// </summary>
    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register
        (nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
            new PropertyMetadata(Colors.Transparent, selectedColor_changed));

    /// <summary>
    /// The sc a property
    /// </summary>
    public static readonly DependencyProperty ScAProperty =
        DependencyProperty.Register
        ("ScA", typeof(float), typeof(ColorPicker),
            new PropertyMetadata((float)1,
                scAChanged
            ));

    /// <summary>
    /// The sc r property
    /// </summary>
    public static readonly DependencyProperty ScRProperty =
        DependencyProperty.Register
        ("ScR", typeof(float), typeof(ColorPicker),
            new PropertyMetadata((float)1,
                scRChanged
            ));

    /// <summary>
    /// The sc g property
    /// </summary>
    public static readonly DependencyProperty ScGProperty =
        DependencyProperty.Register
        ("ScG", typeof(float), typeof(ColorPicker),
            new PropertyMetadata((float)1,
                scGChanged
            ));

    /// <summary>
    /// The sc b property
    /// </summary>
    public static readonly DependencyProperty ScBProperty =
        DependencyProperty.Register
        ("ScB", typeof(float), typeof(ColorPicker),
            new PropertyMetadata((float)1,
                scBChanged
            ));

    /// <summary>
    /// a property
    /// </summary>
    public static readonly DependencyProperty AProperty =
        DependencyProperty.Register
        (nameof(A), typeof(byte), typeof(ColorPicker),
            new PropertyMetadata((byte)255,
                aChanged
            ));

    /// <summary>
    /// The r property
    /// </summary>
    public static readonly DependencyProperty RProperty =
        DependencyProperty.Register
        (nameof(R), typeof(byte), typeof(ColorPicker),
            new PropertyMetadata((byte)255,
                rChanged
            ));

    /// <summary>
    /// The g property
    /// </summary>
    public static readonly DependencyProperty GProperty =
        DependencyProperty.Register
        (nameof(G), typeof(byte), typeof(ColorPicker),
            new PropertyMetadata((byte)255,
                gChanged
            ));

    /// <summary>
    /// The b property
    /// </summary>
    public static readonly DependencyProperty BProperty =
        DependencyProperty.Register
        (nameof(B), typeof(byte), typeof(ColorPicker),
            new PropertyMetadata((byte)255,
                bChanged
            ));

    /// <summary>
    /// The hexadecimal string property
    /// </summary>
    public static readonly DependencyProperty HexadecimalStringProperty =
        DependencyProperty.Register
        (nameof(HexadecimalString), typeof(string), typeof(ColorPicker),
            new PropertyMetadata("#FFFFFFFF",
                hexadecimalStringChanged
            ));

    /// <summary>
    /// The selected color changed event
    /// </summary>
    public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
        "SelectedColorChanged",
        RoutingStrategy.Bubble,
        typeof(RoutedPropertyChangedEventHandler<Color>),
        typeof(ColorPicker));

    /// <summary>
    /// as the changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void aChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnAChanged((byte)e.NewValue);
    }

    /// <summary>
    /// Called when [a changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnAChanged(byte newValue)
    {
        _mColor.A = newValue;
        SetValue(ScAProperty, _mColor.ScA);
        SetValue(SelectedColorProperty, _mColor);
    }

    /// <summary>
    /// rs the changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void rChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnRChanged((byte)e.NewValue);
    }

    /// <summary>
    /// Called when [r changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnRChanged(byte newValue)
    {
        _mColor.R = newValue;
        SetValue(ScRProperty, _mColor.ScR);
        SetValue(SelectedColorProperty, _mColor);
    }

    /// <summary>
    /// gs the changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void gChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnGChanged((byte)e.NewValue);
    }

    /// <summary>
    /// Called when [g changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnGChanged(byte newValue)
    {
        _mColor.G = newValue;
        SetValue(ScGProperty, _mColor.ScG);
        SetValue(SelectedColorProperty, _mColor);
    }

    /// <summary>
    /// bs the changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void bChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnBChanged((byte)e.NewValue);
    }

    /// <summary>
    /// Called when [b changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnBChanged(byte newValue)
    {
        _mColor.B = newValue;
        SetValue(ScBProperty, _mColor.ScB);
        SetValue(SelectedColorProperty, _mColor);
    }

    /// <summary>
    /// Scs a changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void scAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnScAChanged((float)e.NewValue);
    }

    /// <summary>
    /// Called when [sc a changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnScAChanged(float newValue)
    {
        _isAlphaChange = true;

        if (_shouldFindPoint)
        {
            _mColor.ScA = newValue;
            SetValue(AProperty, _mColor.A);
            SetValue(SelectedColorProperty, _mColor);
            SetValue(HexadecimalStringProperty, _mColor.ToString());
        }

        _isAlphaChange = false;
    }

    /// <summary>
    /// Scs the r changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void scRChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnScRChanged((float)e.NewValue);
    }

    /// <summary>
    /// Called when [sc r changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnScRChanged(float newValue)
    {
        if (!_shouldFindPoint)
            return;

        _mColor.ScR = newValue;
        SetValue(RProperty, _mColor.R);
        SetValue(SelectedColorProperty, _mColor);
        SetValue(HexadecimalStringProperty, _mColor.ToString());
    }

    /// <summary>
    /// Scs the g changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void scGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnScGChanged((float)e.NewValue);
    }

    /// <summary>
    /// Called when [sc g changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnScGChanged(float newValue)
    {
        if (!_shouldFindPoint)
            return;

        _mColor.ScG = newValue;
        SetValue(GProperty, _mColor.G);
        SetValue(SelectedColorProperty, _mColor);
        SetValue(HexadecimalStringProperty, _mColor.ToString());
    }

    /// <summary>
    /// Scs the b changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void scBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnScBChanged((float)e.NewValue);
    }

    /// <summary>
    /// Called when [sc b changed].
    /// </summary>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnScBChanged(float newValue)
    {
        if (!_shouldFindPoint)
            return;

        _mColor.ScB = newValue;
        SetValue(BProperty, _mColor.B);
        SetValue(SelectedColorProperty, _mColor);
        SetValue(HexadecimalStringProperty, _mColor.ToString());
    }

    /// <summary>
    /// Hexadecimals the string changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void hexadecimalStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker c = (ColorPicker)d;
        c.OnHexadecimalStringChanged((string)e.OldValue, (string)e.NewValue);
    }

    /// <summary>
    /// Called when [hexadecimal string changed].
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    protected virtual void OnHexadecimalStringChanged(string oldValue, string newValue)
    {
        try
        {
            if (_shouldFindPoint)
            {
                object convertFromString = ColorConverter.ConvertFromString(newValue);
                if (convertFromString != null)
                    _mColor = (Color)convertFromString;
            }

            SetValue(AProperty, _mColor.A);
            SetValue(RProperty, _mColor.R);
            SetValue(GProperty, _mColor.G);
            SetValue(BProperty, _mColor.B);

            if (_shouldFindPoint && !_isAlphaChange && _templateApplied)
            {
                updateMarkerPosition(_mColor);
            }
        }
        catch (FormatException)
        {
            SetValue(HexadecimalStringProperty, oldValue);
        }
    }

    /// <summary>
    /// Selecteds the color_changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void selectedColor_changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ColorPicker cPicker = (ColorPicker)d;
        cPicker.OnSelectedColorChanged((Color)e.OldValue, (Color)e.NewValue);
    }

    /// <summary>
    /// Called when [selected color changed].
    /// </summary>
    /// <param name="oldColor">The old color.</param>
    /// <param name="newColor">The new color.</param>
    protected virtual void OnSelectedColorChanged(Color oldColor, Color newColor)
    {
        RoutedPropertyChangedEventArgs<Color> newEventArgs = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor) { RoutedEvent = SelectedColorChangedEvent };
        RaiseEvent(newEventArgs);
    }

    /// <summary>
    /// Called whenever the control's template changes.
    /// </summary>
    /// <param name="oldTemplate">The old template.</param>
    /// <param name="newTemplate">The new template.</param>
    protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
    {
        _templateApplied = false;

        if (oldTemplate != null)
        {
            _mColorSlider.ValueChanged -= baseColorChanged;
            _mColorDetail.MouseLeftButtonDown -= onMouseLeftButtonDown;
            _mColorDetail.PreviewMouseMove -= onMouseMove;
            _mColorDetail.SizeChanged -= colorDetailSizeChanged;
            _mColorDetail = null;
            _mColorMarker = null;
            _mColorSlider = null;
        }

        base.OnTemplateChanged(oldTemplate, newTemplate);
    }

    /// <summary>
    /// Bases the color changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="double"/> instance containing the event data.</param>
    private void baseColorChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_mColorPosition != null)
            determineColor((Point)_mColorPosition);
    }

    /// <summary>
    /// Ons the mouse left button down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
    private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Point p = e.GetPosition(_mColorDetail);
        updateMarkerPosition(p);
    }

    /// <summary>
    /// Ons the mouse move.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    private void onMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
            return;

        Point p = e.GetPosition(_mColorDetail);
        updateMarkerPosition(p);
        Mouse.Synchronize();
    }

    /// <summary>
    /// Colors the detail size changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
    private void colorDetailSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (args.PreviousSize != Size.Empty &&
            args.PreviousSize.Width != 0 && args.PreviousSize.Height != 0)
        {
            double widthDifference = args.NewSize.Width / args.PreviousSize.Width;
            double heightDifference = args.NewSize.Height / args.PreviousSize.Height;
            _markerTransform.X *= widthDifference;
            _markerTransform.Y *= heightDifference;
        }
        else if (_mColorPosition != null)
        {
            _markerTransform.X = ((Point)_mColorPosition).X * args.NewSize.Width;
            _markerTransform.Y = ((Point)_mColorPosition).Y * args.NewSize.Height;
        }
    }

    /// <summary>
    /// Sets the color.
    /// </summary>
    /// <param name="theColor">The color.</param>
    private void setColor(Color theColor)
    {
        _mColor = theColor;

        if (!_templateApplied)
            return;

        SetValue(AProperty, _mColor.A);
        SetValue(RProperty, _mColor.R);
        SetValue(GProperty, _mColor.G);
        SetValue(BProperty, _mColor.B);
        updateMarkerPosition(theColor);
    }

    /// <summary>
    /// Updates the marker position.
    /// </summary>
    /// <param name="p">The p.</param>
    private void updateMarkerPosition(Point p)
    {
        _markerTransform.X = p.X;
        _markerTransform.Y = p.Y;
        p.X /= _mColorDetail.ActualWidth;
        p.Y /= _mColorDetail.ActualHeight;
        _mColorPosition = p;
        determineColor(p);
    }

    /// <summary>
    /// Updates the marker position.
    /// </summary>
    /// <param name="theColor">The color.</param>
    private void updateMarkerPosition(Color theColor)
    {
        _mColorPosition = null;

        HsvColor hsv = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B);

        _mColorSlider.Value = hsv.H;

        Point p = new Point(hsv.S, 1 - hsv.V);

        _mColorPosition = p;
        p.X *= _mColorDetail.ActualWidth;
        p.Y *= _mColorDetail.ActualHeight;
        _markerTransform.X = p.X;
        _markerTransform.Y = p.Y;
    }

    /// <summary>
    /// Determines the color.
    /// </summary>
    /// <param name="p">The p.</param>
    private void determineColor(Point p)
    {
        HsvColor hsv = new HsvColor(360 - _mColorSlider.Value, 1, 1) { S = p.X, V = 1 - p.Y };

        _mColor = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
        _shouldFindPoint = false;
        _mColor.ScA = (float)GetValue(ScAProperty);
        SetValue(HexadecimalStringProperty, _mColor.ToString());
        _shouldFindPoint = true;
    }
}