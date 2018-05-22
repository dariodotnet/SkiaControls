using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkiaControls
{
    /// <summary>
    /// This control is type of ContentView. Yo can animate, rotate, scale or make with it all you can imagine like a Xamarin Forms ContentView
    /// </summary>
    public class ControlBase : ContentView
    {
        protected readonly TapGestureRecognizer Tap;

        protected ControlBase()
        {
            Tap = new TapGestureRecognizer();
            GestureRecognizers.Add(Tap);
        }

        private async void TapOnTapped(object sender, EventArgs e)
        {
            if (AnimateClick)
            {
                await this.ScaleTo(0.95, 50, Easing.CubicIn);
                await this.ScaleTo(1, 50, Easing.CubicOut);
            }
            Command?.Execute(CommandParameter);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            "Command", typeof(ICommand), typeof(ControlBase), default(ICommand));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            "CommandParameter", typeof(object), typeof(ControlBase), default(object));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty AnimateClickProperty = BindableProperty.Create(
            "AnimateClick", typeof(bool), typeof(ControlBase), default(bool));

        public bool AnimateClick
        {
            get => (bool)GetValue(AnimateClickProperty);
            set => SetValue(AnimateClickProperty, value);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent == null)
                Tap.Tapped -= TapOnTapped;
            else
                Tap.Tapped += TapOnTapped;
        }

        protected float ConvertToDeviceScaleFactor(int value)
        {
            return value * (float)Device.info.ScalingFactor;
        }

        protected float ConvertToDeviceScaleFactor(float value)
        {
            return value * (float)Device.info.ScalingFactor;
        }

        protected float ConvertToDeviceScaleFactor(double value)
        {
            return (float)value * (float)Device.info.ScalingFactor;
        }
    }
}