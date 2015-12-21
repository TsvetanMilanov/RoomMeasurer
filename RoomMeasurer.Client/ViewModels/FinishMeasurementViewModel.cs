namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Input;
    using Windows.Devices.Geolocation;
    using Windows.Storage.Streams;
    using Windows.Web.Http;


    using Newtonsoft.Json;

    using DB;
    using DB.Models;
    using Models;
    using Pages;
    using Web.RequestModels;
    using Utilities;
    using Utilities.Notifications;

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

            RoomRequestModel roomDatabaseModel = new RoomRequestModel
            {
                Geometry = await RoomGeometryViewModel.CreateFromRoom(this.Room),
                Room = this.Room
            };

            string requestBody = JsonConvert.SerializeObject(roomDatabaseModel);
            HttpStringContent requestContent = new HttpStringContent(requestBody, UnicodeEncoding.Utf8, "application/json");

            Data data = new Data();

            UserDatabaseModel currentUser = await data.GetCurrentUser();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.Token))
            {
                MessageDialogNotifier.Notify("You must be logged in to send room information on the server.");
                return;
            }

            string token = currentUser.Token;

            Requester requester = new Requester();
            string serverResult = string.Empty;

            try
            {
                serverResult = await requester.PostJsonAsync("/api/roomGeometry", requestContent, token);
            }
            catch (COMException)
            {
                MessageDialogNotifier.Notify("There was an error on the server. Please contact the server administrators..");
            }

            RoomRequestModel result = JsonConvert.DeserializeObject<RoomRequestModel>(serverResult);

            if (result == null)
            {
                MessageDialogNotifier.Notify("The room information was not valid or you are not authenticated.");
            }
            else
            {
                MessageDialogNotifier.Notify("The room information was successfully saved in the database.");
            }
        }

        private async void ExecuteDrawRoomCommand()
        {
            RoomGeometryViewModel roomGeometry = await RoomGeometryViewModel.CreateFromRoom(this.Room);
            this.NavigationService.Navigate(typeof(RoomDrawingPage), roomGeometry);
        }
    }
}
