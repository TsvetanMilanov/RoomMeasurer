namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;

    using ViewModels;

    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.ViewModel = new LoginViewModel();
        }

        public LoginViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }
    }
}