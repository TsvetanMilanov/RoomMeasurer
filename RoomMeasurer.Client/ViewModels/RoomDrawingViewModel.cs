namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using Windows.Foundation;
    using Windows.UI.Xaml.Media;

    public class RoomDrawingViewModel : BaseViewModel
    {
        private PointCollection roomCorners;

        public RoomDrawingViewModel()
        {
            this.roomCorners = new PointCollection();
        }

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

        internal void CalculateRoomCorners(RoomGeometryViewModel room)
        {
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
