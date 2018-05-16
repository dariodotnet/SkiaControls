using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using Xamarin.Forms;

namespace SkiaControls.Controls
{
    /// <inheritdoc />
    public class Button : RoundedBase
    {
        private readonly SKPaint _fillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private readonly SKPaint _text = new SKPaint()
        {
            Color = SKColors.Black
        };

        public Button()
        {
            Padding = new Thickness(0);
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewOnPaintSurface;
            Content = canvasView;
        }

        protected virtual void OnCanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            if (Gradients != null && Gradients.Any())
                _fillPaint.Shader = GetGradient(info, Gradients, GradientOrientation);
            else
                _fillPaint.Color = Color.ToSKColor();

            var topLeftRadius = (float)CornerRadius.Left * (float)Device.info.ScalingFactor;
            var topRightRadius = (float)CornerRadius.Top * (float)Device.info.ScalingFactor;
            var bottomRightRadius = (float)CornerRadius.Right * (float)Device.info.ScalingFactor;
            var bottomLeftRadius = (float)CornerRadius.Bottom * (float)Device.info.ScalingFactor;

            var topLeft = new SKPoint(0, 0);
            var topRight = new SKPoint(info.Width, 0);
            var bottomRight = new SKPoint(info.Width, info.Height);
            var bottomLeft = new SKPoint(0, info.Height);

            using (var path = new SKPath())
            {
                path.MoveTo(bottomLeft);
                path.ArcTo(topLeft, topRight, topLeftRadius);
                path.ArcTo(topRight, bottomRight, topRightRadius);
                path.ArcTo(bottomRight, bottomLeft, bottomRightRadius);
                path.ArcTo(bottomLeft, topLeft, bottomLeftRadius);
                path.Close();

                canvas.DrawPath(path, _fillPaint);
            }

            if (string.IsNullOrEmpty(Text))
                return;
            var textWidth = _text.MeasureText(Text);
            var height = CalculateScaled.Size(TextSize) / 100 * info.Width * _text.TextSize / textWidth;
            _text.TextSize = height < info.Height - 10 ? height : info.Height - 10;
            var textBounds = new SKRect();
            _text.MeasureText(Text, ref textBounds);

            var xText = (float)info.Width / 2 - textBounds.MidX;
            var yText = (float)info.Height / 2 - textBounds.MidY;

            canvas.DrawText(Text, xText, yText, _text);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text", typeof(string), typeof(Button), default(string));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(
            "TextSize", typeof(float), typeof(Button), 20f);

        public float TextSize
        {
            get => (float)GetValue(TextSizeProperty);
            set => SetValue(TextSizeProperty, value);
        }
    }
}