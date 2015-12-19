namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Windows.Input;
    using Logic;
    using RoomMeasurer.Client.Utilities;
    using Windows.UI.Xaml.Controls;

    public class SetCameraFocusViewModel : CalculateBaseModel<Canvas>
    {
        private double calculatedFocus;

        public SetCameraFocusViewModel()
        {
            this.CalculateFocusCommand = new DelegateCommand(this.CalculateFocus);
            GetSavedFocus();
        }

        public string RealSize { get; set; }

        public string ProjectedSize { get; set; }

        public string Distance { get; set; }

        public double CalculatedFocus
        {
            get { return this.calculatedFocus; }

            set
            {
                this.calculatedFocus = value;
                this.RaisePropertyChanged("CalculatedFocus");
            }
        }

        public ICommand CalculateFocusCommand { get; private set; }

        private void CalculateFocus()
        {
            // TODO: error notifications for invalid input
            if (this.Distance == null || this.RealSize == null)
            {
                throw new NullReferenceException("Distance or RealSize not set.");
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

            this.Data.SaveFocusDistance(this.CalculatedFocus);
        }

        protected override void ExecuteCalculateCommand(Canvas canvas)
        {
            if (this.Distance == null)
            {
                throw new NullReferenceException("Canvas cannot be null.");
            }

            var tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count != 2)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];

            this.ProjectedSize = (firstPointOffset - secondPointOffset).ToString();
            this.CalculateFocus();
        }

        private async void GetSavedFocus()
        {
            this.CalculatedFocus = await this.Data.GetFoucsDistance();
        }
    }
}