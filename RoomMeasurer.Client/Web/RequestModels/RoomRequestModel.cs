namespace RoomMeasurer.Client.Web.RequestModels
{
    using Client.Models;
    using ViewModels;

    public class RoomRequestModel
    {
        public Room Room { get; set; }

        public RoomGeometryViewModel Geometry { get; set; }
    }
}
