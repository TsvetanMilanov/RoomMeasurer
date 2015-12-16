namespace RoomMeasurer.Client.ViewModels
{
    using System.Windows.Input;
    using Logic;
    using RoomMeasurer.Client.Utilities;

    public class SetCameraFocusViewModel : BaseViewModel
    {
        private NavigationService NavigationService;

        public SetCameraFocusViewModel()
        {
            this.NavigationService = new NavigationService();
            this.CalculateFocusCommand = new DelegateCommand(this.CalculateFocus);
        }

        public double RealSize { get; set; }

        public double ProjectedSize { get; set; }

        public double Distance { get; set; }

        private double calculatedFocus;

        public double CalculatedFocus
        {
            get { return this.calculatedFocus; }
            set { this.calculatedFocus = value;
                this.RaisePropertyChanged("CalculatedFocus");
            }
        }


        public ICommand CalculateFocusCommand { get; private set; }

        internal void CalculateFocus()
        {
            if (this.RealSize == 0 || this.ProjectedSize == 0 || this.Distance == 0)
            {
                return;
            }

            this.CalculatedFocus = Measurer.GetCameraFocalDistance(this.Distance, this.RealSize, this.ProjectedSize);
        }
    }
}