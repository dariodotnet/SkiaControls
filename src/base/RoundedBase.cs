using Xamarin.Forms;

namespace SkiaControls
{
    /// <inheritdoc />
    public class RoundedBase : ShapeBase
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            "CornerRadius", typeof(Thickness), typeof(ShapeBase), default(Thickness));

        /// <summary>
        /// If you are drawing a triangle, only get Left, Top and Right values. You need declare Bottom value but it will be ignored.
        /// </summary>
        public Thickness CornerRadius
        {
            get => (Thickness)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}