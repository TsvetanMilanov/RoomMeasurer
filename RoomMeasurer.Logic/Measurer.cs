using System;
using System.Collections.Generic;

namespace RoomMeasurer.Logic
{
    public static class Measurer
    {
        public static List<double> GetEdgeDistances(
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

        public static double GetCameraFocalDistance(double distance, double actualHeight, double projectedHeight)
        {
            return distance * projectedHeight / actualHeight;
        }

        public static double GetRealHeight(double projectedHeight, double projectedReferenceHeight, double actualReferenceHeight)
        {
            var scale = actualReferenceHeight / projectedReferenceHeight;
            var realHeight = projectedHeight * scale;
            return realHeight;
        }

        public static List<double> GetActualWallSizes(List<double> distances, List<double> orientations)
        {
            List<double> result = new List<double>();

            for (int i = 0; i < distances.Count; i++)
            {
                double leftDistance = distances[i];
                double rightDistance = -1;
                double angle = -1;

                if (i + 1 >= distances.Count)
                {
                    rightDistance = distances[0];
                    angle = (Math.Abs(360 - orientations[i])) * Math.PI / 180;
                }
                else
                {
                    rightDistance = distances[i + 1];
                    angle = (Math.Abs(orientations[i] - orientations[i + 1])) * Math.PI / 180;
                }


                double distance = Math.Sqrt(Math.Pow(leftDistance, 2) + Math.Pow(rightDistance, 2) - (2 * (leftDistance * rightDistance * Math.Cos(angle))));

                result.Add(distance);
            }

            return result;
        }
    }
}