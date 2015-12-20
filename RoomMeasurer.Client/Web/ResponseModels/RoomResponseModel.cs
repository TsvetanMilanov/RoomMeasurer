namespace RoomMeasurer.Client.Web.ResponseModels
{
    using Client.Models;
    using ViewModels;

    public class RoomResponseModel
    {
        public Room Room { get; set; }

        public RoomGeometryViewModel Geometry { get; set; }
    }
}
