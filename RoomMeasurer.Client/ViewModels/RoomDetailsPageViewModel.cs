namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using RoomMeasurer.Client.Models;
    public class RoomDetailsPageViewModel : BaseViewModel
    {
        private string edges;
        private Room room;
        
        public string Edges
        {
            get
            {
                return this.edges;
            }
            set
            {
                this.edges = value;
                this.RaisePropertyChanged("Edges");
            }
        }

        public Room Room
        {
            get
            {
                if (this.room == null)
                {
                    this.room = new Room(0, 0);
                }

                return this.room;
            }
            set
            {
                if (this.room == null)
                {
                    this.room = new Room(0, 0);
                }

                this.room.ActualReferenceHeight = value.ActualReferenceHeight;
                this.room.Edges = value.Edges;
                this.room.Latitude = value.Latitude;
                this.room.Longitude = value.Longitude;
                this.room.ProjectedReferenceHeight = value.ProjectedReferenceHeight;

                IEnumerable<string> formatedEdges = this.room.Edges
                .Select(e => string.Format("Projected height: {0}, ZRotation: {1}", e.ProjectedHeight, e.ZRotation));
                this.Edges = string.Format("{0}", string.Join(",\n\n", formatedEdges));

                this.RaisePropertyChanged("Room");
            }
        }
    }
}
