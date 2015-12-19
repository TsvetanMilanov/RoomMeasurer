namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;
    
    using Controls;
    using Pages;
    using Models;

    public class MeasureWithReferenceViewModel : CalculateBaseModel<CanvasWithSelectableBackground>
    {
        public string ReferenceObjectHeight { get; set; }

        protected override void ExecuteCalculateCommand(CanvasWithSelectableBackground control)
        {
            if (this.CalculatedAngle == double.MinValue)
            {
                this.CalculatedAngle = control.CalculatedAngle;
            }

            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                // TODO: Pop notification for the required reference height.
                return;
            }

            Canvas canvas = control.Canvas;

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
            
            Room room = new Room(projectedReferenceHeight, actualReferenceHeight);
            
            Edge edge = new Edge(projectedEdgeHeight, this.CalculatedAngle);
            room.Edges.Add(edge);

            this.NavigationService.Navigate(typeof(CalculationResultPage), room);
        }
    }
}
