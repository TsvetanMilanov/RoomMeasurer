namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;

    public sealed partial class MeasureWithReferencePage : Page
    {
        public MeasureWithReferencePage()
        {
            this.InitializeComponent();
            this.ViewModel = new MeasureWithReferenceViewModel();
        }

        public MeasureWithReferenceViewModel ViewModel
        {
            get
            {
                return this.DataContext as MeasureWithReferenceViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }
    }
}
