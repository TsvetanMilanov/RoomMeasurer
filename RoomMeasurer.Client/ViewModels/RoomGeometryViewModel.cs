namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DB;
    using Logic;
    using Models;

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

        public async static Task<RoomGeometryViewModel> CreateFromRoom(Room room)
        {
            Data data = new Data();

            double[] projectedEdgeHeights = room.Edges.Select(e => e.ProjectedHeight).ToArray();
            double focalDistance = await data.GetFoucsDistance();

            List<double> distances = Measurer.GetEdgeDistances(
                projectedEdgeHeights,
                focalDistance,
                room.ProjectedReferenceHeight,
                room.ActualReferenceHeight);

            List<double> orientations = room.Edges.Select(e => e.ZRotation).ToList();

            List<double> actualWallSizes = Measurer.GetActualWallSizes(distances, orientations);

            RoomGeometryViewModel roomGeometry = new RoomGeometryViewModel(distances, orientations, actualWallSizes);

            return roomGeometry;
        }
    }
}