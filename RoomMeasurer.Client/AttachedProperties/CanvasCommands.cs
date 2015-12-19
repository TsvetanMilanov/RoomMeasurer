namespace RoomMeasurer.Client.AttachedProperties
{
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

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
    }
}
