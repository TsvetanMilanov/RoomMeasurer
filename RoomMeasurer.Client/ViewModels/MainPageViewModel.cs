namespace RoomMeasurer.Client.ViewModels
{
    using System.Windows.Input;
    using Utilities;

    using Contracts;
    using Pages;

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.NavigationService = new NavigationService();
            this.GoToMeasureFromExistingImageCommand = new DelegateCommand(this.HandleGoToMeasureFromExistingImage);
        }

        public ICommand GoToMeasureFromExistingImageCommand { get; set; }

        public INavigationService NavigationService { get; private set; }

        private void HandleGoToMeasureFromExistingImage()
        {
            this.NavigationService.Navigate(typeof(MeasureFromExistingImagePage));
        }
    }
}
