namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;

    public class RoomGeometryViewModel
    {
        public RoomGeometryViewModel(List<double> distances, List<double> yaws, List<double> actualWallSizes)
        {
            this.Distances = distances;
            this.Yaws = yaws;
            this.ActualWallsSizes = actualWallSizes;
        }

        public List<double> Distances { get; set; }

        public List<double> Yaws { get; set; }

        public List<double> ActualWallsSizes { get; set; }
    }
}