using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SkiaControls.Controls
{
    /// <inheritdoc />
    [Preserve(AllMembers = true)]
    public class Button : RoundedBase
    {
        private readonly Grid _mainGrid;
        private readonly SKPaint _fillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        public Button()
        {
            _mainGrid = new Grid();
            var canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewOnPaintSurface;
            _mainGrid.Children.Add(canvasView);
            Content = _mainGrid;
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

            var topLeftRadius = ConvertToDeviceScaleFactor(CornerRadius.Left);
            var topRightRadius = ConvertToDeviceScaleFactor(CornerRadius.Top);
            var bottomRightRadius = ConvertToDeviceScaleFactor(CornerRadius.Right);
            var bottomLeftRadius = ConvertToDeviceScaleFactor(CornerRadius.Bottom);

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

                canvas.ClipPath(path);

                if (IsOnlyBorder)
                {
                    var border = ConvertToDeviceScaleFactor(BorderWidth) * 2;

                    var scalaX = (info.Width - border) / info.Width;
                    var scalaY = (info.Height - border) / info.Height;

                    canvas.Translate(ConvertToDeviceScaleFactor(BorderWidth), ConvertToDeviceScaleFactor(BorderWidth));

                    canvas.Scale(scalaX, scalaY);

                    using (var center = new SKPath())
                    {
                        center.MoveTo(bottomLeft);
                        center.ArcTo(topLeft, topRight, topLeftRadius);
                        center.ArcTo(topRight, bottomRight, topRightRadius);
                        center.ArcTo(bottomRight, bottomLeft, bottomRightRadius);
                        center.ArcTo(bottomLeft, topLeft, bottomLeftRadius);
                        center.Close();

                        canvas.ClipPath(center, SKClipOperation.Difference);
                    }


                }
            }

            canvas.DrawPaint(_fillPaint);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Label", typeof(Label), typeof(Button), default(Label), propertyChanged: TextChanged);

        private static void TextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == null)
                return;
            var button = (Button)bindable;
            if (button._mainGrid.Children.Count > 1)
                button._mainGrid.Children.Remove(button._mainGrid.Children[1]);

            button._mainGrid.Children.Add((Label)newvalue);
        }

        public Label Label
        {
            get => (Label)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}