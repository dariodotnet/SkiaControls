using SkiaControls.Enums;
using SkiaControls.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaControls
{
    /// <inheritdoc />
    public class ShapeBase : ControlBase
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            "Color", typeof(Color), typeof(ShapeBase), Color.LightSlateGray);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty GradientsProperty = BindableProperty.Create(
            "Gradients", typeof(Gradient[]), typeof(ShapeBase), default(Gradient[]));

        public Gradient[] Gradients
        {
            get => (Gradient[])GetValue(GradientsProperty);
            set => SetValue(GradientsProperty, value);
        }

        public static readonly BindableProperty GradientOrientationProperty = BindableProperty.Create(
            "GradientOrientation", typeof(Orientation), typeof(ShapeBase), default(Orientation));

        public Orientation GradientOrientation
        {
            get => (Orientation)GetValue(GradientOrientationProperty);
            set => SetValue(GradientOrientationProperty, value);
        }

        protected SKShader GetGradient(SKImageInfo info, Gradient[] gradients, Orientation orientation)
        {
            var colors = new SKColor[gradients.Length];
            for (var i = 0; i < gradients.Length; i++)
            {
                colors[i] = gradients[i].Color.ToSKColor();
            }

            var containPoints = false;
            var points = new float[gradients.Length];
            for (var i = 0; i < gradients.Length; i++)
            {
                points[i] = gradients[i].End;
                if (points[i] > 0)
                    containPoints = true;
            }

            var starPoint = new SKPoint(0, 0);
            var endPoint = new SKPoint();
            switch (orientation)
            {
                case Orientation.Vertical:
                    endPoint.X = 0;
                    endPoint.Y = info.Height;
                    break;
                case Orientation.Diagonal:
                    endPoint.X = info.Width;
                    endPoint.Y = info.Height;
                    break;
                default:
                    endPoint.X = info.Width;
                    endPoint.Y = 0;
                    break;
            }

            return SKShader.CreateLinearGradient(starPoint, endPoint, colors, containPoints ? points : null, SKShaderTileMode.Clamp);
        }
    }
}