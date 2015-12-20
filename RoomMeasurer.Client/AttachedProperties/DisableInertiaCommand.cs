namespace RoomMeasurer.Client.AttachedProperties
{
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;

    public class DisableInertiaCommand
    {
        public static ICommand GetDisableInertia(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DisableInertiaProperty);
        }

        public static void SetDisableInertia(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DisableInertiaProperty, value);
        }

        // Using a DependencyProperty as the backing store for DisableInertia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisableInertiaProperty =
            DependencyProperty.RegisterAttached("DisableInertia", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleDisableInertiaChanged));

        private static void HandleDisableInertiaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
