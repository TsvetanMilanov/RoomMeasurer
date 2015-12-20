namespace RoomMeasurer.Client.AttachedProperties
{
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Shapes;

    public class ScaleShapeCommand
    {
        public static ICommand GetScale(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ScaleProperty);
        }

        public static void SetScale(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ScaleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.RegisterAttached("Scale", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleScaleChanged));

        private static void HandleScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Shape control = d as Shape;

            control.ManipulationMode = ManipulationModes.All;
            control.ManipulationDelta += (sender, args) =>
            {
                ICommand command = e.NewValue as ICommand;

                command.Execute(args);
            };
        }
    }
}
