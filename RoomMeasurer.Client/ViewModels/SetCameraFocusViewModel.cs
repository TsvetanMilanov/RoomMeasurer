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

        public string RealSize { get; set; }

        public string ProjectedSize { get; set; }

        public string Distance { get; set; }

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
            // TODO: error notifications for invalid input
            if (this.RealSize == null || this.ProjectedSize == null || this.Distance == null)
            {
                return;
            }

            double parsedRealSize = 0;
            double.TryParse(this.RealSize, out parsedRealSize);

            double parsedProjectedSize = 0;
            double.TryParse(this.ProjectedSize, out parsedProjectedSize);

            double parsedDistance = 0;
            double.TryParse(this.Distance, out parsedDistance);

            if (parsedRealSize == 0 || parsedProjectedSize == 0 || parsedDistance == 0)
            {
                return;
            }

            this.CalculatedFocus = Measurer.GetCameraFocalDistance(parsedDistance, parsedRealSize, parsedProjectedSize);
        }
    }
}