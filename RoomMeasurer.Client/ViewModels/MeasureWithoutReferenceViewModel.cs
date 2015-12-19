namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using Windows.UI.Xaml.Controls;

    public class MeasureWithoutReferenceViewModel : CalculateBaseModel<Canvas>
    {
        protected override async void ExecuteCalculateCommand(Canvas canvas)
        {
            IList<double> tappedPointsTopOffsets = this.GetTappedPointsTopOffsets(canvas);

            if (tappedPointsTopOffsets.Count < 2)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double projectedEdgeHeight = this.PointsDistanceCalculator.CalculateEdgePointsDistance(tappedPointsTopOffsets);

            // TODO: Override on navigated to and update the room model.
            var focusDistance = await this.Data.GetFoucsDistance();
        }
    }
}
