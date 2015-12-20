namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    using Controls;
    using Pages;
    using Models;
    using Utilities.Notifications;

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
                MessageDialogNotifier.Notify("Please fill the reference's actual height field.");
                return;
            }

            Canvas canvas = control.Canvas;

            IList<double> tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count < 3)
            {
                MessageDialogNotifier.Notify("Please add the three reference points.");
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
