namespace RoomMeasurer.Client.ViewModels
{
    using System.Windows.Input;
    using Pages;
    using Utilities;

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.GoToMeasureFromExistingImageCommand = new DelegateCommand(this.HandleGoToMeasureFromExistingImage);
            this.GoToSetCameraFocusCommand = new DelegateCommand(this.HandleGoToSetCameraFocus);
        }

        public ICommand GoToMeasureFromExistingImageCommand { get; set; }

        public ICommand GoToSetCameraFocusCommand { get; set; }

        private void HandleGoToSetCameraFocus()
        {
            this.NavigationService.Navigate(typeof(SetCameraFocusPage));
        }

        private void HandleGoToMeasureFromExistingImage()
        {
            this.NavigationService.Navigate(typeof(MeasureFromExistingImagePage));
        }

        private double savedFocus;

        public double SavedFocus
        {
            get
            {
                return savedFocus;
            }
            set
            {
                savedFocus = value;
                this.RaisePropertyChanged("SavedFocus");
            }
        }

    }
}