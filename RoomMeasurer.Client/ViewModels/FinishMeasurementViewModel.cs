﻿namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using DB;
    using Logic;
    using Models;
    using Pages;
    using Utilities;

    public class FinishMeasurementViewModel : BaseViewModel
    {
        public FinishMeasurementViewModel()
        {
            this.DrawRoom = new DelegateCommand(this.HandleDrawRoomCommand);
        }

        public Room Room { get; internal set; }

        public ICommand DrawRoom { get; set; }

        private async void HandleDrawRoomCommand()
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

            this.NavigationService.Navigate(typeof(RoomDrawingPage), new RoomGeometryViewModel(distances, orientations));
        }
    }
}
