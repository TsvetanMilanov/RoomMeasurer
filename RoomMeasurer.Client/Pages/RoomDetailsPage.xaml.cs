namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using Models;
    using ViewModels;

    public sealed partial class RoomDetailsPage : Page
    {
        public RoomDetailsPage()
        {
            this.InitializeComponent();
            this.ViewModel = new RoomDetailsPageViewModel();
        }

        public RoomDetailsPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as RoomDetailsPageViewModel;
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
