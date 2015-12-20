namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;
    using Windows.UI.Xaml.Controls;

    using Utilities;
    using Pages;

    public class RoomDrawingViewModel : BaseViewModel
    {
        private PointCollection roomCorners;

        public RoomDrawingViewModel()
        {
            this.roomCorners = new PointCollection();
            this.Translate = new DelegateCommandWithParameter<ManipulationDeltaRoutedEventArgs>(this.ExecuteTranslateCommand);
            this.DisableInertia = new DelegateCommandWithParameter<ManipulationInertiaStartingRoutedEventArgs>(this.ExecuteDisableInertiaCommand);
            this.Scale = new DelegateCommandWithParameter<ManipulationDeltaRoutedEventArgs>(this.ExecuteScaleCommand);
            this.BackToMainMenu = new DelegateCommand(this.ExecuteBackToMainMenuCommand);
        }

        public ICommand Translate { get; set; }

        public ICommand DisableInertia { get; set; }

        public ICommand Scale { get; set; }

        public ICommand BackToMainMenu { get; set; }

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

        private void ExecuteBackToMainMenuCommand()
        {
            this.NavigationService.Navigate(typeof(MainPage));
        }

        private void ExecuteScaleCommand(ManipulationDeltaRoutedEventArgs obj)
        {
            // TODO: Check how to scale Polygon
            Shape control = obj.OriginalSource as Shape;

            if (control == null)
            {
                return;
            }

            control.Width *= obj.Delta.Scale;
            control.Height *= obj.Delta.Scale;
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
            //room = new RoomGeometryViewModel(new System.Collections.Generic.List<double>
            //{
            //    50, 50, 50
            //}, new System.Collections.Generic.List<double>
            //{
            //    0, 120, 240
            //});

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
