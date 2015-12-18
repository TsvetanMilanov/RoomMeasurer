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
            this.GoToMeasureFromExistingImageCommand = new DelegateCommand(this.HandleGoToMeasureFromExistingImage);
            this.GoToSetCameraFocusCommand = new DelegateCommand(this.HandleGoToSetCameraFocus);
            this.GoToStartMeasuringRoomCommand = new DelegateCommand(this.HandleGoToStartMeasuringRoom);
        }

        public ICommand GoToMeasureFromExistingImageCommand { get; set; }

        public ICommand GoToSetCameraFocusCommand { get; set; }

        public ICommand GoToStartMeasuringRoomCommand { get; set; }

        private void HandleGoToStartMeasuringRoom()
        {
            this.NavigationService.Navigate(typeof(MeasureWithReferencePage));
        }

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