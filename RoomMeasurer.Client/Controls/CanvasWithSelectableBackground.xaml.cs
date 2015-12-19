namespace RoomMeasurer.Client.Controls
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;
    using Windows.UI.Xaml;
    using System;

    public sealed partial class CanvasWithSelectableBackground : UserControl
    {
        public CanvasWithSelectableBackground()
        {
            this.InitializeComponent();
            this.Canvas = this.canvasImageContainer;
            this.ViewModel = new CanvasWithSelectableBackgroundViewModel();
        }

        public CanvasWithSelectableBackgroundViewModel ViewModel
        {
            get
            {
                return this.DataContext as CanvasWithSelectableBackgroundViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public Canvas Canvas
        {
            get { return (Canvas)GetValue(CanvasProperty); }
            set { SetValue(CanvasProperty, value); }
        }

        public static readonly DependencyProperty CanvasProperty =
            DependencyProperty.Register("Canvas", typeof(Canvas), typeof(CanvasWithSelectableBackground), new PropertyMetadata(null));
        
        public int MaxTaps
        {
            get { return (int)GetValue(MaxTapsProperty); }
            set { SetValue(MaxTapsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxTaps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxTapsProperty =
            DependencyProperty.Register("MaxTaps", typeof(int), typeof(CanvasWithSelectableBackground), new PropertyMetadata(0, HandleMaxTapsChanged));


        public double CalculatedAngle
        {
            get
            {
                return this.ViewModel.CalculatedAngle;
            }
        }

        private static void HandleMaxTapsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CanvasWithSelectableBackground;
            control.ViewModel.MaxTaps = (int)e.NewValue;
        }
    }
}
