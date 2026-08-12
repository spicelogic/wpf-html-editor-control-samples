Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Media
Imports System.Windows.Shapes

Namespace Global.CustomDialog.Dialogs.ColorPickerDialog

#Region "SpectrumSlider"

    <DesignTimeVisible(False)>
    Public Class SpectrumSlider
        Inherits Slider

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(SpectrumSlider),
                New FrameworkPropertyMetadata(GetType(SpectrumSlider)))
        End Sub

#Region "Public Properties"

        Public Property SelectedColor As Color
            Get
                Return CType(GetValue(SelectedColorProperty), Color)
            End Get
            Set(value As Color)
                SetValue(SelectedColorProperty, value)
            End Set
        End Property

#End Region ' Public Properties

#Region "Dependency Property Fields"

        Public Shared ReadOnly SelectedColorProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(SelectedColor), GetType(Color), GetType(SpectrumSlider),
                New PropertyMetadata(Colors.Transparent))

#End Region ' Dependency Property Fields

#Region "Public Methods"

        Public Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            _mSpectrumDisplay = TryCast(GetTemplateChild(SpectrumDisplayName), Rectangle)
            updateColorSpectrum()
            OnValueChanged(Double.NaN, Value)
        End Sub

#End Region ' Public Methods

#Region "Protected Methods"

        Protected Overrides Sub OnValueChanged(oldValue As Double, newValue As Double)
            MyBase.OnValueChanged(oldValue, newValue)
            Dim theColor As Color = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1)
            SetValue(SelectedColorProperty, theColor)
        End Sub

#End Region ' Protected Methods

#Region "Private Methods"

        Private Sub updateColorSpectrum()
            If _mSpectrumDisplay IsNot Nothing Then
                createSpectrum()
            End If
        End Sub

        Private Sub createSpectrum()
            _pickerBrush = New LinearGradientBrush With {
                .StartPoint = New Point(0.5, 0),
                .EndPoint = New Point(0.5, 1),
                .ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation
            }

            Dim colorsList As List(Of Color) = ColorUtilities.GenerateHsvSpectrum()
            Dim stopIncrement As Double = CDbl(1) / colorsList.Count

            Dim i As Integer

            For i = 0 To colorsList.Count - 1
                _pickerBrush.GradientStops.Add(New GradientStop(colorsList(i), i * stopIncrement))
            Next

            _pickerBrush.GradientStops(i - 1).Offset = 1.0
            _mSpectrumDisplay.Fill = _pickerBrush
        End Sub

#End Region ' Private Methods

#Region "Private Fields"

        Private Const SpectrumDisplayName As String = "PART_SpectrumDisplay"
        Private _mSpectrumDisplay As Rectangle
        Private _pickerBrush As LinearGradientBrush

#End Region ' Private Fields
    End Class

#End Region ' SpectrumSlider

#Region "ColorUtilities"

    Friend Module ColorUtilities
        ' Converts an RGB color to an HSV color.
        Public Function ConvertRgbToHsv(r As Integer, b As Integer, g As Integer) As HsvColor
            Dim h As Double = 0, s As Double

            Dim min As Double = Math.Min(Math.Min(r, g), b)
            Dim v As Double = Math.Max(Math.Max(r, g), b)
            Dim delta As Double = v - min

            If v = 0.0 Then
                s = 0
            Else
                s = delta / v
            End If

            If s = 0 Then
                h = 0.0
            Else
                If r = v Then
                    h = (g - b) / delta
                ElseIf g = v Then
                    h = 2 + (b - r) / delta
                ElseIf b = v Then
                    h = 4 + (r - g) / delta
                End If

                h *= 60
                If h < 0.0 Then
                    h += 360
                End If
            End If

            Dim hsvColor As New HsvColor With {.H = h, .S = s, .V = v / 255}

            Return hsvColor
        End Function

        ' Converts an HSV color to an RGB color.
        Public Function ConvertHsvToRgb(h As Double, s As Double, v As Double) As Color
            Dim r As Double, g As Double, b As Double

            If s = 0 Then
                r = v
                g = v
                b = v
            Else
                Dim f As Double, p As Double, q As Double, t As Double

                If h = 360 Then
                    h = 0
                Else
                    h /= 60
                End If

                Dim i As Integer = CInt(Math.Truncate(h))
                f = h - i

                p = v * (1.0 - s)
                q = v * (1.0 - (s * f))
                t = v * (1.0 - (s * (1.0 - f)))

                Select Case i
                    Case 0
                        r = v
                        g = t
                        b = p

                    Case 1
                        r = q
                        g = v
                        b = p

                    Case 2
                        r = p
                        g = v
                        b = t

                    Case 3
                        r = p
                        g = q
                        b = v

                    Case 4
                        r = t
                        g = p
                        b = v

                    Case Else
                        r = v
                        g = p
                        b = q
                End Select
            End If

            Return Color.FromArgb(255, CByte(r * 255), CByte(g * 255), CByte(b * 255))
        End Function

        ' Generates a list of colors with hues ranging from 0 360
        ' and a saturation and value of 1.
        Public Function GenerateHsvSpectrum() As List(Of Color)
            Dim colorsList As New List(Of Color)(8)

            For i As Integer = 0 To 28
                colorsList.Add(ConvertHsvToRgb(i * 12, 1, 1))
            Next

            colorsList.Add(ConvertHsvToRgb(0, 1, 1))

            Return colorsList
        End Function
    End Module

#End Region ' ColorUtilities

    ' Describes a color in terms of
    ' Hue, Saturation, and Value (brightness)

#Region "HsvColor"

    Friend Structure HsvColor
        Public H As Double
        Public S As Double
        Public V As Double

        Public Sub New(h As Double, s As Double, v As Double)
            Me.H = h
            Me.S = s
            Me.V = v
        End Sub
    End Structure

#End Region ' HsvColor

#Region "ColorThumb"

    <DesignTimeVisible(False)>
    Public Class ColorThumb
        Inherits Thumb

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(ColorThumb),
                New FrameworkPropertyMetadata(GetType(ColorThumb)))
        End Sub

        Public Shared ReadOnly ThumbColorProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(ThumbColor), GetType(Color), GetType(ColorThumb),
                New FrameworkPropertyMetadata(Colors.Transparent))

        Public Shared ReadOnly PointerOutlineThicknessProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(PointerOutlineThickness), GetType(Double), GetType(ColorThumb),
                New FrameworkPropertyMetadata(1.0))

        Public Shared ReadOnly PointerOutlineBrushProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(PointerOutlineBrush), GetType(Brush), GetType(ColorThumb),
                New FrameworkPropertyMetadata(CType(Nothing, Brush)))

        Public Property ThumbColor As Color
            Get
                Return CType(GetValue(ThumbColorProperty), Color)
            End Get
            Set(value As Color)
                SetValue(ThumbColorProperty, value)
            End Set
        End Property

        Public Property PointerOutlineThickness As Double
            Get
                Return CDbl(GetValue(PointerOutlineThicknessProperty))
            End Get
            Set(value As Double)
                SetValue(PointerOutlineThicknessProperty, value)
            End Set
        End Property

        Public Property PointerOutlineBrush As Brush
            Get
                Return CType(GetValue(PointerOutlineBrushProperty), Brush)
            End Get
            Set(value As Brush)
                SetValue(PointerOutlineBrushProperty, value)
            End Set
        End Property
    End Class

#End Region ' ColorThumb

End Namespace
