namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using RoomMeasurer.Client.Models;
    using RoomMeasurer.Client.ViewModels;

    public sealed partial class CalculationResultPage : Page
    {
        public CalculationResultPage()
        {
            this.InitializeComponent();
            this.ViewModel = new CalculationResultViewModel();
        }

        public CalculationResultViewModel ViewModel
        {
            get
            {
                return this.DataContext as CalculationResultViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Room room = e.Parameter as Room;

            this.ViewModel.CalculateHeight(room);
            this.ViewModel.Room = room;
        }
    }
}
