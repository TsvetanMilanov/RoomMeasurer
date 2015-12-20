namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Windows.Devices.Geolocation;

    using DB;
    using Logic;
    using Models;
    using Pages;
    using Utilities;

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
        }

        private async void ExecuteDrawRoomCommand()
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
            this.NavigationService.Navigate(typeof(RoomDrawingPage), roomGeometry);
        }
    }
}
