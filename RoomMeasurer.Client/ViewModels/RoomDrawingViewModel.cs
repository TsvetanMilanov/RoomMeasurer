using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace RoomMeasurer.Client.ViewModels
{
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
            var roomCorners = new PointCollection();
            for (int i = 0; i < room.Distances.Count; i++)
            {
                var line = new Line();

                line.X1 = 0;
                line.Y1 = 0;

                line.X2 = 0;
                line.Y2 = room.Distances[i];

                line.RenderTransformOrigin = new Point(0,0);

                var rotation = new RotateTransform();
                rotation.CenterX = 0;
                rotation.CenterY = 0;
                rotation.Angle = room.Yaws[i];
                line.RenderTransform = rotation;

                roomCorners.Add(new Point(line.X2, line.Y2));
            }

            this.RoomCorners = roomCorners;
        }
    }
}
