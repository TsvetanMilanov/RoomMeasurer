namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;

    public class RoomGeometryViewModel
    {
        public RoomGeometryViewModel(List<double> distances, List<double> yaws)
        {
            this.Distances = distances;
            this.Yaws = yaws;
        }

        public List<double> Distances { get; set; }

        public List<double> Yaws { get; set; }
    }
}