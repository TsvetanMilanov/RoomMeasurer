using System.Collections.Generic;

namespace RoomMeasurer.Logic
{
    public class Measurer
    {
        public List<double> GetEdgeDistances(
            double[] projectedEdgeHeights,
            double focalDistance,
            double projectedReferenceHeight,
            double actualReferenceHeight
            )
        {
            double realEdgeHeight = GetRealHeight(projectedEdgeHeights[0], projectedReferenceHeight, actualReferenceHeight);

            var edgeDistances = new List<double>();
            foreach (var projectedHeight in projectedEdgeHeights)
            {
                double scale = realEdgeHeight / projectedHeight;
                var distance = scale * focalDistance;
                edgeDistances.Add(distance);
            }

            return edgeDistances;
        }

        public double GetCameraFocalDistance(double distance, double actualHeight, double projectedHeight)
        {
            return distance * projectedHeight / actualHeight;
        }

        private static double GetRealHeight(double projectedHeight, double projectedReferenceHeight, double actualReferenceHeight)
        {
            var scale = actualReferenceHeight / projectedReferenceHeight;
            var realHeight = projectedHeight * scale;
            return realHeight;
        }
    }
}