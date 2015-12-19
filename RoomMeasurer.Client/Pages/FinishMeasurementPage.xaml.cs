namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;
    using Windows.UI.Xaml.Navigation;
    using Models;
    public sealed partial class FinishMeasurementPage : Page
    {
        public FinishMeasurementPage()
        {
            this.InitializeComponent();
            this.ViewModel = new FinishMeasurementViewModel();
        }

        public FinishMeasurementViewModel ViewModel
        {
            get
            {
                return this.DataContext as FinishMeasurementViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.ViewModel.Room = e.Parameter as Room;
        }
    }
}
