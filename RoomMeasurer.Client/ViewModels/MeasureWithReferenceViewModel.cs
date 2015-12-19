namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    using Logic;

    public class MeasureWithReferenceViewModel : CalculateBaseModel<Canvas>
    {
        private double calculatedHeight;

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

        protected override async void ExecuteCalculateCommand(Canvas canvas)
        {
            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                // TODO: Pop notification for the required reference height.
                return;
            }
            
            IList<double> tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count < 3)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double[] distances = this.PointsDistanceCalculator.CalculateEdgePointsWithReferenceDistances(tappedPointsTopOffsets);

            double projectedReferenceHeight = distances[0];
            double projectedEdgeHeight = distances[1];
            double actualReferenceHeight = double.Parse(this.ReferenceObjectHeight);

            this.CalculatedHeight = Measurer.GetRealHeight(projectedEdgeHeight, projectedReferenceHeight, actualReferenceHeight);

            // TODO: Create room model and add the initial information then pass it to the next page.
            var focusDistance = await this.Data.GetFoucsDistance();
            var distance = Measurer.GetEdgeDistances(new double[] { 155 }, focusDistance, projectedReferenceHeight, actualReferenceHeight);
        }
    }
}
