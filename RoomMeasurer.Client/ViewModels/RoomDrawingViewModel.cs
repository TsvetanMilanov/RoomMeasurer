﻿namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;
    using Windows.UI.Xaml.Controls;

    using Utilities;

    public class RoomDrawingViewModel : BaseViewModel
    {
        private PointCollection roomCorners;

        public RoomDrawingViewModel()
        {
            this.roomCorners = new PointCollection();
            this.Translate = new DelegateCommandWithParameter<ManipulationDeltaRoutedEventArgs>(this.ExecuteTranslateCommand);
            this.DisableInertia = new DelegateCommandWithParameter<ManipulationInertiaStartingRoutedEventArgs>(this.ExecuteDisableInertiaCommand);
        }

        public ICommand Translate { get; set; }

        public ICommand DisableInertia { get; set; }

        public PointCollection RoomCorners
        {
            get
            {
                return roomCorners;
            }
            set
            {
                if (this.roomCorners == null)
                {
                    this.roomCorners = new PointCollection();
                }

                roomCorners.Clear();

                foreach (var item in value)
                {
                    roomCorners.Add(item);
                }

                this.RaisePropertyChanged("RoomCorners");
            }
        }

        private void ExecuteDisableInertiaCommand(ManipulationInertiaStartingRoutedEventArgs obj)
        {
            obj.TranslationBehavior.DesiredDeceleration = int.MaxValue;
        }

        private void ExecuteTranslateCommand(ManipulationDeltaRoutedEventArgs obj)
        {
            Polygon control = obj.OriginalSource as Polygon;

            double top = Canvas.GetTop(control);
            double left = Canvas.GetLeft(control);

            Canvas.SetTop(control, top + obj.Delta.Translation.Y);
            Canvas.SetLeft(control, left + obj.Delta.Translation.X);
        }

        internal void CalculateRoomCorners(RoomGeometryViewModel room)
        {
            room = new RoomGeometryViewModel(new System.Collections.Generic.List<double>
            {
                50, 50, 50
            }, new System.Collections.Generic.List<double>
            {
                0, 120, 240
            });

            PointCollection roomCorners = new PointCollection();
            for (int i = 0; i < room.Yaws.Count; i++)
            {
                double angle = room.Yaws[i] * Math.PI / 180;
                double distance = room.Distances[i];
                double x = Math.Sin(angle) * distance;
                double y = Math.Cos(angle) * distance;

                roomCorners.Add(new Point(x, y));
            }

            this.RoomCorners = roomCorners;
        }
    }
}
