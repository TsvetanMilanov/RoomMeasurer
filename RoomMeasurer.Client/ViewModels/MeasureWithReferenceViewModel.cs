namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    using Logic;
    using Pages;
    using Models;
    public class MeasureWithReferenceViewModel : CalculateBaseModel<Canvas>
    {
        public string ReferenceObjectHeight { get; set; }

        protected override void ExecuteCalculateCommand(Canvas canvas)
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
            
            // TODO: Move in final calculations page.
            // var focusDistance = await this.Data.GetFoucsDistance();
            // var distance = Measurer.GetEdgeDistances(new double[] { 155 }, focusDistance, projectedReferenceHeight, actualReferenceHeight);

            Room room = new Room(projectedReferenceHeight, actualReferenceHeight);

            // TODO: Get the z index from accelerometer.
            double zRotation = 0.5;
            Edge edge = new Edge(projectedEdgeHeight, zRotation);
            room.Edges.Add(edge);

            this.NavigationService.Navigate(typeof(CalculationResultPage), room);
        }
    }
}
