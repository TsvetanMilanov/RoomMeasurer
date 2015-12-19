namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    using Models;
    using Pages;
    using Controls;
    public class MeasureWithoutReferenceViewModel : CalculateBaseModel<CanvasWithSelectableBackground>
    {
        public Room Room { get; set; }

        protected override void ExecuteCalculateCommand(CanvasWithSelectableBackground control)
        {
            Canvas canvas = control.Canvas;

            IList<double> tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count < 2)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            if (this.CalculatedAngle == double.MinValue)
            {
                this.CalculatedAngle = control.CalculatedAngle;
            }

            double projectedEdgeHeight = this.PointsDistanceCalculator.CalculateEdgePointsDistance(tappedPointsTopOffsets);
            
            Edge edge = new Edge(projectedEdgeHeight, this.CalculatedAngle);
            this.Room.Edges.Add(edge);

            this.NavigationService.Navigate(typeof(CalculationResultPage), this.Room);
        }
    }
}
