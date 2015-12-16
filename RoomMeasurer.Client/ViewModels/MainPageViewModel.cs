namespace RoomMeasurer.Client.ViewModels
{
    using System.Windows.Input;
    using Contracts;
    using Pages;
    using Utilities;

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.NavigationService = new NavigationService();
            this.GoToMeasureFromExistingImageCommand = new DelegateCommand(this.HandleGoToMeasureFromExistingImage);
            this.GoToSetCameraFocusCommand = new DelegateCommand(this.HandleGoToSetCameraFocus);
        }

        public ICommand GoToMeasureFromExistingImageCommand { get; set; }

        public ICommand GoToSetCameraFocusCommand { get; set; }

        public INavigationService NavigationService { get; private set; }

        private void HandleGoToSetCameraFocus()
        {
            this.NavigationService.Navigate(typeof(SetCameraFocusPage));
        }

        private void HandleGoToMeasureFromExistingImage()
        {
            this.NavigationService.Navigate(typeof(MeasureFromExistingImagePage));
        }
    }
}