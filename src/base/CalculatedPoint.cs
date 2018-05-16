using SkiaSharp;
using Xamarin.Forms;

namespace SkiaControls
{
    public static class CalculatedPoint
    {
        public static SKPoint Center(SKImageInfo info)
        {
            return new SKPoint((float)info.Width / 2, (float)info.Height / 2);
        }
    }

    public static class CalculateScaled
    {
        public static float Size(float initialValue)
        {
            return initialValue * (float)Device.info.ScalingFactor;
        }
    }
}