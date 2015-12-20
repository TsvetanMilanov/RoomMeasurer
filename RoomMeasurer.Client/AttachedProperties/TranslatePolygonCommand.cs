namespace RoomMeasurer.Client.AttachedProperties
{
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Shapes;

    public class TranslatePolygonCommand
    {
        public static ICommand GetTranslate(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(TranslateProperty);
        }

        public static void SetTranslate(DependencyObject obj, ICommand value)
        {
            obj.SetValue(TranslateProperty, value);
        }

        // Using a DependencyProperty as the backing store for Translate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.RegisterAttached("Translate", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleTranslateChanged));

        private static void HandleTranslateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Polygon control = d as Polygon;
            control.ManipulationMode = ManipulationModes.All;
            control.ManipulationDelta += (sender, args) =>
            {
                ICommand command = e.NewValue as ICommand;

                command.Execute(args);
            };
        }
    }
}
