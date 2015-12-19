namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using Models;
    using ViewModels;

    public sealed partial class MeasureWithoutReferencePage : Page
    {
        public MeasureWithoutReferencePage()
        {
            this.InitializeComponent();
            this.ViewModel = new MeasureWithoutReferenceViewModel();
        }

        public MeasureWithoutReferenceViewModel ViewModel
        {
            get
            {
                return this.DataContext as MeasureWithoutReferenceViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Room room = e.Parameter as Room;

            this.ViewModel.Room = room;
        }
    }
}
