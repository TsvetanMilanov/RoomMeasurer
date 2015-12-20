namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Windows.Input;
    using Pages;
    using Utilities;

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.GoToMeasureFromExistingImageCommand = new DelegateCommand(this.ExecuteGoToMeasureFromExistingImageCommand);
            this.GoToSetCameraFocusCommand = new DelegateCommand(this.ExecuteGoToSetCameraFocusCommand);
            this.GoToStartMeasuringRoomCommand = new DelegateCommand(this.ExecuteGoToStartMeasuringRoomCommand);
            this.GoToLoginPage = new DelegateCommand(this.ExecuteGoToLoginPageCommand);
            this.GoToShowAllRoomsPage = new DelegateCommand(this.ExecuteGoToShowAllRoomsCommand);
        }

        public ICommand GoToMeasureFromExistingImageCommand { get; set; }

        public ICommand GoToSetCameraFocusCommand { get; set; }

        public ICommand GoToStartMeasuringRoomCommand { get; set; }

        public ICommand GoToLoginPage { get; set; }

        public ICommand GoToShowAllRoomsPage { get; set; }

        private void ExecuteGoToShowAllRoomsCommand()
        {
            this.NavigationService.Navigate(typeof(ShowAllRoomsPage));
        }

        private void ExecuteGoToLoginPageCommand()
        {
            this.NavigationService.Navigate(typeof(LoginPage));
        }

        private void ExecuteGoToStartMeasuringRoomCommand()
        {
            this.NavigationService.Navigate(typeof(MeasureWithReferencePage));
        }

        private void ExecuteGoToSetCameraFocusCommand()
        {
            this.NavigationService.Navigate(typeof(SetCameraFocusPage));
        }

        private void ExecuteGoToMeasureFromExistingImageCommand()
        {
            this.NavigationService.Navigate(typeof(MeasureHeightPage));
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