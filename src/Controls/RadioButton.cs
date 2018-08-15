using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SkiaControls.Controls
{
    /// <inheritdoc />
    [Preserve(AllMembers = true)]
    public class RadioButton : ShapeBase
    {
        private TapGestureRecognizer _tap;
        private readonly SKCanvasView _canvasView;

        private readonly SKPaint _fillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        public RadioButton()
        {
            _canvasView = new SKCanvasView();
            _canvasView.PaintSurface += OnCanvasViewOnPaintSurface;
            Content = _canvasView;
            AnimateClick = true;
            _tap = new TapGestureRecognizer();
            _tap.Tapped += TapOnTapped;
            GestureRecognizers.Add(_tap);
        }

        private void TapOnTapped(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
        }

        private void OnCanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            _fillPaint.StrokeWidth = ConvertToDeviceScaleFactor(BorderWidth);
            _fillPaint.Color = Color.ToSKColor();
            var radius = ((float)(info.Width >= info.Height ? info.Height : info.Width) / 2) - ConvertToDeviceScaleFactor(BorderWidth);

            canvas.DrawCircle((float)info.Width / 2, (float)info.Height / 2, radius, _fillPaint);

            if (!IsChecked)
                return;

            radius = (float)(info.Width >= info.Height ? info.Height : info.Width) / 4;
            var center = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = Color.ToSKColor()
            };
            canvas.DrawCircle((float)info.Width / 2, (float)info.Height / 2, radius, center);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            "IsChecked", typeof(bool), typeof(RadioButton), default(bool), BindingMode.TwoWay,
            propertyChanged: IsCheckedChanged);

        private static void IsCheckedChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((RadioButton)bindable)._canvasView.InvalidateSurface();
        }

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
    }
}