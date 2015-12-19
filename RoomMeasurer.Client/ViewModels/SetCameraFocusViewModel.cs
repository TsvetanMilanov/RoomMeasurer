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
            this.Instruction = "Load reference image, tap object bottom and top points, enter object distance, relative to camera and object real height and press calculate.";
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

        protected override void ExecuteCalculateCommand(Canvas canvas)
        {
            if (this.Distance == null)
            {
                this.Instruction = "Distance not set.";
                return;
            }

            var tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count != 2)
            {
                this.Instruction = "You must tap two reference points in the image - object base and object top.";
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];

            this.ProjectedSize = (firstPointOffset - secondPointOffset).ToString();
            this.CalculateFocus();
        }

        private void CalculateFocus()
        {
            // TODO: error notifications for invalid input
            if (this.RealSize == null)
            {
                this.Instruction = "You must set the objects real size.";
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
                this.Instruction = "Real size and distance must be number values.";
                return;
            }

            this.CalculatedFocus = Measurer.GetCameraFocalDistance(parsedDistance, parsedRealSize, parsedProjectedSize);

            this.Data.SaveFocusDistance(this.CalculatedFocus);
            this.Instruction = "Calibration saved";
        }

        private async void GetSavedFocus()
        {
            this.CalculatedFocus = await this.Data.GetFoucsDistance();
        }
    }
}