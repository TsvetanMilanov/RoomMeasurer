﻿namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Devices.Geolocation;
    using Windows.Storage.Streams;
    using Windows.Web.Http;


    using Newtonsoft.Json;

    using DB;
    using Logic;
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
                Geometry = await this.GenerateRoomGeometryViewModel(),
                Room = this.Room
            };

            string requestBody = JsonConvert.SerializeObject(roomDatabaseModel);
            HttpStringContent requestContent = new HttpStringContent(requestBody, UnicodeEncoding.Utf8, "application/json");

            Data data = new Data();

            string token = (await data.GetCurrentUser()).Token;

            Requester requester = new Requester();
            string serverResult = string.Empty;

            try
            {
                serverResult = await requester.PostJsonAsync("/api/roomGeometry", requestContent, token);
            }
            catch (COMException)
            {
                MessageDialogNotificator.Notify("There was an error on the server. Please contact the server administrators..");
            }

            RoomRequestModel result = JsonConvert.DeserializeObject<RoomRequestModel>(serverResult);

            if (result == null)
            {
                MessageDialogNotificator.Notify("The room information was not valid or you are not authenticated.");
            }
            else
            {
                MessageDialogNotificator.Notify("The room information was successfully saved in the database.");
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

            List<double> actualWallSizes = Measurer.GetActualWallSizes(distances, orientations);

            RoomGeometryViewModel roomGeometry = new RoomGeometryViewModel(distances, orientations, actualWallSizes);
            return roomGeometry;
        }
    }
}