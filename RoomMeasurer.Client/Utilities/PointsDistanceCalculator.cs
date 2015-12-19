namespace RoomMeasurer.Client.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PointsDistanceCalculator
    {
        public double CalculateEdgePointsDistance(IList<double> tappedPointsTopOffsets)
        {
            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];

            return firstPointOffset - secondPointOffset;
        }

        public double[] CalculateEdgePointsWithReferenceDistances(IList<double> tappedPointsTopOffsets)
        {
            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];
            double thirdPointOffset = tappedPointsTopOffsets[2];

            double[] distances = new double[2];

            distances[0] = firstPointOffset - secondPointOffset;
            distances[1] = firstPointOffset - thirdPointOffset;

            return distances;
        }
    }
}
