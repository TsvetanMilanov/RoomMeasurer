namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Devices.Geolocation;
    using Windows.Storage.Streams;
    using Windows.Web.Http;


    using Newtonsoft.Json;

    using DB;
    using DB.Models;
    using Logic;
    using Models;
    using Pages;
    using Utilities;
    using Windows.Web.Http.Headers;
    using System.Runtime.InteropServices;
    public class FinishMeasurementViewModel : BaseViewModel
    {
        public FinishMeasurementViewModel()
        {
            this.DrawRoom = new DelegateCommand(this.ExecuteDrawRoomCommand);
            this.SendToServer = new DelegateCommand(this.ExecuteSendToServerCommand);
        }

        public Room Room { get; internal set; }

        public ICommand DrawRoom { get; set; }

        public ICommand SendToServer { get; set; }

        private async void ExecuteSendToServerCommand()
        {
            GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Denied)
            {
                return;
            }

            Geolocator geolocator = new Geolocator();
            Geoposition position = await geolocator.GetGeopositionAsync();

            double latitude = position.Coordinate.Latitude;
            double longitude = position.Coordinate.Longitude;

            this.Room.Latitude = latitude;
            this.Room.Longitude = longitude;

            RoomDatabaseModel roomDatabaseModel = new RoomDatabaseModel
            {
                Geometry = await this.GenerateRoomGeometryViewModel(),
                Room = this.Room
            };

            string requestBody = JsonConvert.SerializeObject(roomDatabaseModel);
            HttpStringContent requestContent = new HttpStringContent(requestBody, UnicodeEncoding.Utf8, "application/json");

            // TODO: Get token from database.
            string token = "0jk2vreqJ7meGRfNf2rvfP0a0ts8xzEAxEnxGwgLGaG4FKCwMZdtDE5N7wCAlFbY3Pp55u3mIEcrEcFLWJbKxZMoGXJMkQC6EBBsVOGm5x9UJHwRAzQmO1VAqXXvNSumvKlo3f4zidH302p0RS9an6TUES5c44SWS86pLTsViiRbeh1GrfzcOq5kOZW9GMYvC5s45R0d";

            Requester requester = new Requester();
            string serverResult = string.Empty;

            try {
                 serverResult = await requester.PostJsonAsync("/api/roomGeometry", requestContent, token);
            } catch(COMException exception)
            {
                // TODO: Notify the user with error.
            }

            RoomDatabaseModel result = JsonConvert.DeserializeObject<RoomDatabaseModel>(serverResult);

            if (result == null)
            {
                // TODO: Notify bad request.
            }
            else
            {
                // TODO: Notify success.
            }
        }

        private async void ExecuteDrawRoomCommand()
        {
            RoomGeometryViewModel roomGeometry = await GenerateRoomGeometryViewModel();
            this.NavigationService.Navigate(typeof(RoomDrawingPage), roomGeometry);
        }

        private async Task<RoomGeometryViewModel> GenerateRoomGeometryViewModel()
        {
            Data data = new Data();

            double[] projectedEdgeHeights = this.Room.Edges.Select(e => e.ProjectedHeight).ToArray();
            double focalDistance = await data.GetFoucsDistance();

            List<double> distances = Measurer.GetEdgeDistances(
                projectedEdgeHeights,
                focalDistance,
                this.Room.ProjectedReferenceHeight,
                this.Room.ActualReferenceHeight);

            List<double> orientations = this.Room.Edges.Select(e => e.ZRotation).ToList();
            RoomGeometryViewModel roomGeometry = new RoomGeometryViewModel(distances, orientations);
            return roomGeometry;
        }
    }
}
