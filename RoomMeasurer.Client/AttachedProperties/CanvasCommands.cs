namespace RoomMeasurer.Client.AttachedProperties
{
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    public class CanvasCommands
    {
        public static ICommand GetTap(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(TapProperty);
        }

        public static void SetTap(DependencyObject obj, ICommand value)
        {
            obj.SetValue(TapProperty, value);
        }
        
        public static readonly DependencyProperty TapProperty =
            DependencyProperty.RegisterAttached("Tap", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleTappedEvent));

        private static void HandleTappedEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as Canvas;

            if (canvas == null)
            {
                return;
            }

            canvas.Tapped += (obj, args) =>
            {
                ICommand command = e.NewValue as ICommand;

                command.Execute(args);
            };
        }



        public static ICommand GetStopInertion(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(StopInertionProperty);
        }

        public static void SetStopInertion(DependencyObject obj, ICommand value)
        {
            obj.SetValue(StopInertionProperty, value);
        }

        // Using a DependencyProperty as the backing store for StopInertion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopInertionProperty =
            DependencyProperty.RegisterAttached("StopInertion", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleStopInertionChanged));

        private static void HandleStopInertionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement control = d as UIElement;

            control.ManipulationMode = ManipulationModes.All;
            control.ManipulationInertiaStarting += (sender, args) =>
            {
                ICommand command = e.NewValue as ICommand;

                command.Execute(args);
            };
        }
    }
}
