namespace RoomMeasurer.Client.Controls
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;
    using Windows.UI.Xaml;

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
    }
}
