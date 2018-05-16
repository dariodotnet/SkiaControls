using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SkiaControls.Shapes
{
    /// <inheritdoc />
    [Preserve(AllMembers = true)]
    public class Rectangle : RoundedBase
    {
        private readonly SKPaint _fillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        public Rectangle()
        {
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
        }
    }
}