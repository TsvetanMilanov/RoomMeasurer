namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using Logic;
    using Windows.UI.Xaml.Controls;

    public class MeasureHeightViewModel : CalculateBaseModel<Canvas>
    {
        private double calculatedHeight;

        private double distance;

        public double CalculatedHeight
        {
            get
            {
                return this.calculatedHeight;
            }

            set
            {
                this.calculatedHeight = value;
                this.RaisePropertyChanged("CalculatedHeight");
            }
        }

        public string ReferenceObjectHeight { get; set; }

        public double Distance
        {
            get { return distance; }
            set { distance = value; this.RaisePropertyChanged("Distance"); }
        }

        protected override async void ExecuteCalculateCommand(Canvas canvas)
        {
            var focusDistance = await this.Data.GetFoucsDistance();
            if (focusDistance == -1)
            {
                this.Instruction = "You must calibrate first.";
                return;
            }

            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                this.Instruction = "You must enter reference object height";
                return;
            }

            var tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count < 3)
            {
                this.Instruction = "You must tap three points - objects base, reference top, real object top";
                return;
            }

            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];
            double thirdPointOffset = tappedPointsTopOffsets[2];

            double projectedReferenceHeight = firstPointOffset - secondPointOffset;
            double projectedEdgeHeight = firstPointOffset - thirdPointOffset;

            double actualReferenceHeight = 0;
            double.TryParse(this.ReferenceObjectHeight, out actualReferenceHeight);
            if (actualReferenceHeight == 0)
            {
                this.Instruction = "Real reference height must be valid number";
                return;
            }

            this.CalculatedHeight = Measurer.GetRealHeight(projectedEdgeHeight, projectedReferenceHeight, actualReferenceHeight);

            this.Distance = Measurer.GetEdgeDistances(new double[] { projectedEdgeHeight }, focusDistance, projectedReferenceHeight, actualReferenceHeight)[0];

            this.Instruction = "";
        }
    }
}