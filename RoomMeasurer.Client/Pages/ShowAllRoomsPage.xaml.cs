namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ShowAllRoomsPage : Page
    {
        public ShowAllRoomsPage()
        {
            this.InitializeComponent();
            this.ViewModel = new ShowAllRoomsPageViewModel();
        }

        public ShowAllRoomsPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as ShowAllRoomsPageViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ViewModel.GetAllRoomsAsync();
        }
    }
}
