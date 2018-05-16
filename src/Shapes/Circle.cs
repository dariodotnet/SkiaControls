using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SkiaControls.Shapes
{
    /// <inheritdoc />
    [Preserve(AllMembers = true)]
    public class Circle : ShapeBase
    {
        private readonly SKPaint _fillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        public Circle()
        {
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewOnPaintSurface;
            Content = canvasView;
        }

        private void OnCanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            if (Gradients != null && Gradients.Any())
                _fillPaint.Shader = GetGradient(info, Gradients, GradientOrientation);
            else
                _fillPaint.Color = Color.ToSKColor();

            var radius = (float)(info.Width >= info.Height ? info.Height : info.Width) / 2;

            if (IsOnlyBorder)
            {
                radius = radius - CalculateScaled.Size(BorderWidth);
                _fillPaint.Style = SKPaintStyle.Stroke;
                _fillPaint.StrokeWidth = CalculateScaled.Size(BorderWidth);
            }

            canvas.DrawCircle((float)info.Width / 2, (float)info.Height / 2, radius, _fillPaint);
        }
    }
}