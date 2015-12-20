namespace RoomMeasurer.Client.AttachedProperties
{
    using System;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class ListBoxSelectedCommand
    {


        public static ICommand GetSelectedItem(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }
        
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(ICommand), typeof(object), new PropertyMetadata(null, HandleSelectedItemChanged));

        private static void HandleSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox control = d as ListBox;
                        
            control.SelectionChanged += (obj, args) =>
            {
                ICommand command = e.NewValue as ICommand;

                ListBox sender = obj as ListBox;

                command.Execute(sender.SelectedItem);
            }; ;
        }

        private static void Control_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
