Imports System
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Shapes

Namespace Global.CustomDialog.Dialogs.ColorPickerDialog

    <DesignTimeVisible(False)>
    Public Class ColorPicker
        Inherits Control

        Private Const ColorMarkerName As String = "PART_ColorMarker"
        Private Const ColorSliderName As String = "PART_ColorSlider"
        Private Const ColorDetailName As String = "PART_ColorDetail"

        Private ReadOnly _markerTransform As New TranslateTransform()

        Private _mColorSlider As SpectrumSlider
        Private _mColorDetail As FrameworkElement
        Private _mColorMarker As Path
        Private _mColorPosition As Point?
        Private _mColor As Color
        Private _shouldFindPoint As Boolean
        Private _templateApplied As Boolean
        Private _isAlphaChange As Boolean

        ''' <summary>
        ''' Initializes the <see cref="ColorPicker"/> class.
        ''' </summary>
        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ColorPicker), New FrameworkPropertyMetadata(GetType(ColorPicker)))
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ColorPicker"/> class.
        ''' </summary>
        Public Sub New()
            _templateApplied = False
            _mColor = Colors.White
            _shouldFindPoint = True
            SetValue(AProperty, _mColor.A)
            SetValue(RProperty, _mColor.R)
            SetValue(GProperty, _mColor.G)
            SetValue(BProperty, _mColor.B)
            SetValue(SelectedColorProperty, _mColor)
        End Sub

        ''' <summary>
        ''' When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        ''' </summary>
        Public Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()

            _mColorDetail = TryCast(GetTemplateChild(ColorDetailName), FrameworkElement)
            _mColorMarker = TryCast(GetTemplateChild(ColorMarkerName), Path)
            _mColorSlider = TryCast(GetTemplateChild(ColorSliderName), SpectrumSlider)

            If _mColorSlider IsNot Nothing Then
                AddHandler _mColorSlider.ValueChanged, AddressOf baseColorChanged
            End If

            If _mColorMarker IsNot Nothing Then
                _mColorMarker.RenderTransform = _markerTransform
                _mColorMarker.RenderTransformOrigin = New Point(0.5, 0.5)
            End If

            If _mColorDetail IsNot Nothing Then
                AddHandler _mColorDetail.MouseLeftButtonDown, AddressOf onMouseLeftButtonDown
                AddHandler _mColorDetail.PreviewMouseMove, AddressOf onMouseMove
                AddHandler _mColorDetail.SizeChanged, AddressOf colorDetailSizeChanged
            End If

            _templateApplied = True
            _shouldFindPoint = True
            _isAlphaChange = False

            SelectedColor = _mColor
        End Sub

        ' Gets or sets the selected color.
        Public Property SelectedColor As Color
            Get
                Return CType(GetValue(SelectedColorProperty), Color)
            End Get
            Set(value As Color)
                SetValue(SelectedColorProperty, _mColor)
                setColor(value)
            End Set
        End Property

        ' Gets or sets the ARGB alpha value of the selected color.
        Public Property A As Byte
            Get
                Return CByte(GetValue(AProperty))
            End Get
            Set(value As Byte)
                SetValue(AProperty, value)
            End Set
        End Property

        ' Gets or sets the ARGB red value of the selected color.
        Public Property R As Byte
            Get
                Return CByte(GetValue(RProperty))
            End Get
            Set(value As Byte)
                SetValue(RProperty, value)
            End Set
        End Property

        ' Gets or sets the ARGB green value of the selected color.
        Public Property G As Byte
            Get
                Return CByte(GetValue(GProperty))
            End Get
            Set(value As Byte)
                SetValue(GProperty, value)
            End Set
        End Property

        ' Gets or sets the ARGB blue value of the selected color.
        Public Property B As Byte
            Get
                Return CByte(GetValue(BProperty))
            End Get
            Set(value As Byte)
                SetValue(BProperty, value)
            End Set
        End Property

        ' Gets or sets the ScRGB alpha value of the selected color.
        Public Property ScA As Double
            Get
                Return CDbl(GetValue(ScAProperty))
            End Get
            Set(value As Double)
                SetValue(ScAProperty, value)
            End Set
        End Property

        ' Gets or sets the ScRGB red value of the selected color.
        Public Property ScR As Double
            Get
                Return CDbl(GetValue(ScRProperty))
            End Get
            Set(value As Double)
                SetValue(RProperty, value)
            End Set
        End Property

        ' Gets or sets the ScRGB green value of the selected color.
        Public Property ScG As Double
            Get
                Return CDbl(GetValue(ScGProperty))
            End Get
            Set(value As Double)
                SetValue(GProperty, value)
            End Set
        End Property

        ' Gets or sets the ScRGB blue value of the selected color.
        Public Property ScB As Double
            Get
                Return CDbl(GetValue(BProperty))
            End Get
            Set(value As Double)
                SetValue(BProperty, value)
            End Set
        End Property

        ' Gets or sets the the selected color in hexadecimal notation.
        Public Property HexadecimalString As String
            Get
                Return CStr(GetValue(HexadecimalStringProperty))
            End Get
            Set(value As String)
                SetValue(HexadecimalStringProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Occurs when [selected color changed].
        ''' </summary>
        Public Custom Event SelectedColorChanged As RoutedPropertyChangedEventHandler(Of Color)
            AddHandler(value As RoutedPropertyChangedEventHandler(Of Color))
                MyBase.AddHandler(SelectedColorChangedEvent, value)
            End AddHandler
            RemoveHandler(value As RoutedPropertyChangedEventHandler(Of Color))
                MyBase.RemoveHandler(SelectedColorChangedEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As RoutedPropertyChangedEventArgs(Of Color))
                MyBase.RaiseEvent(e)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' The selected color property
        ''' </summary>
        Public Shared ReadOnly SelectedColorProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(SelectedColor), GetType(Color), GetType(ColorPicker),
                New PropertyMetadata(Colors.Transparent, AddressOf selectedColor_changed))

        ''' <summary>
        ''' The sc a property
        ''' </summary>
        Public Shared ReadOnly ScAProperty As DependencyProperty =
            DependencyProperty.Register(
                "ScA", GetType(Single), GetType(ColorPicker),
                New PropertyMetadata(CSng(1),
                    AddressOf scAChanged
                ))

        ''' <summary>
        ''' The sc r property
        ''' </summary>
        Public Shared ReadOnly ScRProperty As DependencyProperty =
            DependencyProperty.Register(
                "ScR", GetType(Single), GetType(ColorPicker),
                New PropertyMetadata(CSng(1),
                    AddressOf scRChanged
                ))

        ''' <summary>
        ''' The sc g property
        ''' </summary>
        Public Shared ReadOnly ScGProperty As DependencyProperty =
            DependencyProperty.Register(
                "ScG", GetType(Single), GetType(ColorPicker),
                New PropertyMetadata(CSng(1),
                    AddressOf scGChanged
                ))

        ''' <summary>
        ''' The sc b property
        ''' </summary>
        Public Shared ReadOnly ScBProperty As DependencyProperty =
            DependencyProperty.Register(
                "ScB", GetType(Single), GetType(ColorPicker),
                New PropertyMetadata(CSng(1),
                    AddressOf scBChanged
                ))

        ''' <summary>
        ''' a property
        ''' </summary>
        Public Shared ReadOnly AProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(A), GetType(Byte), GetType(ColorPicker),
                New PropertyMetadata(CByte(255),
                    AddressOf aChanged
                ))

        ''' <summary>
        ''' The r property
        ''' </summary>
        Public Shared ReadOnly RProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(R), GetType(Byte), GetType(ColorPicker),
                New PropertyMetadata(CByte(255),
                    AddressOf rChanged
                ))

        ''' <summary>
        ''' The g property
        ''' </summary>
        Public Shared ReadOnly GProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(G), GetType(Byte), GetType(ColorPicker),
                New PropertyMetadata(CByte(255),
                    AddressOf gChanged
                ))

        ''' <summary>
        ''' The b property
        ''' </summary>
        Public Shared ReadOnly BProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(B), GetType(Byte), GetType(ColorPicker),
                New PropertyMetadata(CByte(255),
                    AddressOf bChanged
                ))

        ''' <summary>
        ''' The hexadecimal string property
        ''' </summary>
        Public Shared ReadOnly HexadecimalStringProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(HexadecimalString), GetType(String), GetType(ColorPicker),
                New PropertyMetadata("#FFFFFFFF",
                    AddressOf hexadecimalStringChanged
                ))

        ''' <summary>
        ''' The selected color changed event
        ''' </summary>
        Public Shared ReadOnly SelectedColorChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged",
            RoutingStrategy.Bubble,
            GetType(RoutedPropertyChangedEventHandler(Of Color)),
            GetType(ColorPicker))

        ''' <summary>
        ''' as the changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub aChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnAChanged(CByte(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [a changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnAChanged(newValue As Byte)
            _mColor.A = newValue
            SetValue(ScAProperty, _mColor.ScA)
            SetValue(SelectedColorProperty, _mColor)
        End Sub

        ''' <summary>
        ''' rs the changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub rChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnRChanged(CByte(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [r changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnRChanged(newValue As Byte)
            _mColor.R = newValue
            SetValue(ScRProperty, _mColor.ScR)
            SetValue(SelectedColorProperty, _mColor)
        End Sub

        ''' <summary>
        ''' gs the changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub gChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnGChanged(CByte(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [g changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnGChanged(newValue As Byte)
            _mColor.G = newValue
            SetValue(ScGProperty, _mColor.ScG)
            SetValue(SelectedColorProperty, _mColor)
        End Sub

        ''' <summary>
        ''' bs the changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub bChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnBChanged(CByte(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [b changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnBChanged(newValue As Byte)
            _mColor.B = newValue
            SetValue(ScBProperty, _mColor.ScB)
            SetValue(SelectedColorProperty, _mColor)
        End Sub

        ''' <summary>
        ''' Scs a changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub scAChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnScAChanged(CSng(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [sc a changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnScAChanged(newValue As Single)
            _isAlphaChange = True

            If _shouldFindPoint Then
                _mColor.ScA = newValue
                SetValue(AProperty, _mColor.A)
                SetValue(SelectedColorProperty, _mColor)
                SetValue(HexadecimalStringProperty, _mColor.ToString())
            End If

            _isAlphaChange = False
        End Sub

        ''' <summary>
        ''' Scs the r changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub scRChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnScRChanged(CSng(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [sc r changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnScRChanged(newValue As Single)
            If Not _shouldFindPoint Then
                Return
            End If

            _mColor.ScR = newValue
            SetValue(RProperty, _mColor.R)
            SetValue(SelectedColorProperty, _mColor)
            SetValue(HexadecimalStringProperty, _mColor.ToString())
        End Sub

        ''' <summary>
        ''' Scs the g changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub scGChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnScGChanged(CSng(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [sc g changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnScGChanged(newValue As Single)
            If Not _shouldFindPoint Then
                Return
            End If

            _mColor.ScG = newValue
            SetValue(GProperty, _mColor.G)
            SetValue(SelectedColorProperty, _mColor)
            SetValue(HexadecimalStringProperty, _mColor.ToString())
        End Sub

        ''' <summary>
        ''' Scs the b changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub scBChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnScBChanged(CSng(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [sc b changed].
        ''' </summary>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnScBChanged(newValue As Single)
            If Not _shouldFindPoint Then
                Return
            End If

            _mColor.ScB = newValue
            SetValue(BProperty, _mColor.B)
            SetValue(SelectedColorProperty, _mColor)
            SetValue(HexadecimalStringProperty, _mColor.ToString())
        End Sub

        ''' <summary>
        ''' Hexadecimals the string changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub hexadecimalStringChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim c As ColorPicker = CType(d, ColorPicker)
            c.OnHexadecimalStringChanged(CStr(e.OldValue), CStr(e.NewValue))
        End Sub

        ''' <summary>
        ''' Called when [hexadecimal string changed].
        ''' </summary>
        ''' <param name="oldValue">The old value.</param>
        ''' <param name="newValue">The new value.</param>
        Protected Overridable Sub OnHexadecimalStringChanged(oldValue As String, newValue As String)
            Try
                If _shouldFindPoint Then
                    Dim convertFromString As Object = ColorConverter.ConvertFromString(newValue)
                    If convertFromString IsNot Nothing Then
                        _mColor = CType(convertFromString, Color)
                    End If
                End If

                SetValue(AProperty, _mColor.A)
                SetValue(RProperty, _mColor.R)
                SetValue(GProperty, _mColor.G)
                SetValue(BProperty, _mColor.B)

                If _shouldFindPoint AndAlso Not _isAlphaChange AndAlso _templateApplied Then
                    updateMarkerPosition(_mColor)
                End If
            Catch e As FormatException
                SetValue(HexadecimalStringProperty, oldValue)
            End Try
        End Sub

        ''' <summary>
        ''' Selecteds the color_changed.
        ''' </summary>
        ''' <param name="d">The d.</param>
        ''' <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        Private Shared Sub selectedColor_changed(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim cPicker As ColorPicker = CType(d, ColorPicker)
            cPicker.OnSelectedColorChanged(CType(e.OldValue, Color), CType(e.NewValue, Color))
        End Sub

        ''' <summary>
        ''' Called when [selected color changed].
        ''' </summary>
        ''' <param name="oldColor">The old color.</param>
        ''' <param name="newColor">The new color.</param>
        Protected Overridable Sub OnSelectedColorChanged(oldColor As Color, newColor As Color)
            Dim newEventArgs As New RoutedPropertyChangedEventArgs(Of Color)(oldColor, newColor) With {.RoutedEvent = SelectedColorChangedEvent}
            RaiseEvent SelectedColorChanged(Me, newEventArgs)
        End Sub

        ''' <summary>
        ''' Called whenever the control's template changes.
        ''' </summary>
        ''' <param name="oldTemplate">The old template.</param>
        ''' <param name="newTemplate">The new template.</param>
        Protected Overrides Sub OnTemplateChanged(oldTemplate As ControlTemplate, newTemplate As ControlTemplate)
            _templateApplied = False

            If oldTemplate IsNot Nothing Then
                RemoveHandler _mColorSlider.ValueChanged, AddressOf baseColorChanged
                RemoveHandler _mColorDetail.MouseLeftButtonDown, AddressOf onMouseLeftButtonDown
                RemoveHandler _mColorDetail.PreviewMouseMove, AddressOf onMouseMove
                RemoveHandler _mColorDetail.SizeChanged, AddressOf colorDetailSizeChanged
                _mColorDetail = Nothing
                _mColorMarker = Nothing
                _mColorSlider = Nothing
            End If

            MyBase.OnTemplateChanged(oldTemplate, newTemplate)
        End Sub

        ''' <summary>
        ''' Bases the color changed.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="Double"/> instance containing the event data.</param>
        Private Sub baseColorChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
            If _mColorPosition IsNot Nothing Then
                determineColor(CType(_mColorPosition, Point))
            End If
        End Sub

        ''' <summary>
        ''' Ons the mouse left button down.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        Private Shadows Sub onMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
            Dim p As Point = e.GetPosition(_mColorDetail)
            updateMarkerPosition(p)
        End Sub

        ''' <summary>
        ''' Ons the mouse move.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        Private Shadows Sub onMouseMove(sender As Object, e As MouseEventArgs)
            If e.LeftButton <> MouseButtonState.Pressed Then
                Return
            End If

            Dim p As Point = e.GetPosition(_mColorDetail)
            updateMarkerPosition(p)
            Mouse.Synchronize()
        End Sub

        ''' <summary>
        ''' Colors the detail size changed.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="args">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        Private Sub colorDetailSizeChanged(sender As Object, args As SizeChangedEventArgs)
            If args.PreviousSize <> Size.Empty AndAlso
                args.PreviousSize.Width <> 0 AndAlso args.PreviousSize.Height <> 0 Then
                Dim widthDifference As Double = args.NewSize.Width / args.PreviousSize.Width
                Dim heightDifference As Double = args.NewSize.Height / args.PreviousSize.Height
                _markerTransform.X *= widthDifference
                _markerTransform.Y *= heightDifference
            ElseIf _mColorPosition IsNot Nothing Then
                _markerTransform.X = CType(_mColorPosition, Point).X * args.NewSize.Width
                _markerTransform.Y = CType(_mColorPosition, Point).Y * args.NewSize.Height
            End If
        End Sub

        ''' <summary>
        ''' Sets the color.
        ''' </summary>
        ''' <param name="theColor">The color.</param>
        Private Sub setColor(theColor As Color)
            _mColor = theColor

            If Not _templateApplied Then
                Return
            End If

            SetValue(AProperty, _mColor.A)
            SetValue(RProperty, _mColor.R)
            SetValue(GProperty, _mColor.G)
            SetValue(BProperty, _mColor.B)
            updateMarkerPosition(theColor)
        End Sub

        ''' <summary>
        ''' Updates the marker position.
        ''' </summary>
        ''' <param name="p">The p.</param>
        Private Sub updateMarkerPosition(p As Point)
            _markerTransform.X = p.X
            _markerTransform.Y = p.Y
            p.X /= _mColorDetail.ActualWidth
            p.Y /= _mColorDetail.ActualHeight
            _mColorPosition = p
            determineColor(p)
        End Sub

        ''' <summary>
        ''' Updates the marker position.
        ''' </summary>
        ''' <param name="theColor">The color.</param>
        Private Sub updateMarkerPosition(theColor As Color)
            _mColorPosition = Nothing

            Dim hsv As HsvColor = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B)

            _mColorSlider.Value = hsv.H

            Dim p As New Point(hsv.S, 1 - hsv.V)

            _mColorPosition = p
            p.X *= _mColorDetail.ActualWidth
            p.Y *= _mColorDetail.ActualHeight
            _markerTransform.X = p.X
            _markerTransform.Y = p.Y
        End Sub

        ''' <summary>
        ''' Determines the color.
        ''' </summary>
        ''' <param name="p">The p.</param>
        Private Sub determineColor(p As Point)
            Dim hsv As New HsvColor(360 - _mColorSlider.Value, 1, 1) With {.S = p.X, .V = 1 - p.Y}

            _mColor = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V)
            _shouldFindPoint = False
            _mColor.ScA = CSng(GetValue(ScAProperty))
            SetValue(HexadecimalStringProperty, _mColor.ToString())
            _shouldFindPoint = True
        End Sub
    End Class
End Namespace
