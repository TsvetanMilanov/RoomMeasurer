namespace RoomMeasurer.Client.DB.Models
{
    using Client.Models;
    using ViewModels;

    public class RoomDatabaseModel
    {
        public Room Room { get; set; }

        public RoomGeometryViewModel Geometry { get; set; }
    }
}
