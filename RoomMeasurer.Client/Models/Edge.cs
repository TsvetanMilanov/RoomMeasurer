namespace RoomMeasurer.Client.Models
{
    public class Edge
    {
        public Edge(double projectedHeight, double zRotation)
        {
            this.ProjectedHeight = projectedHeight;
            this.ZRotation = zRotation;
        }

        public double ProjectedHeight { get; set; }

        public double ZRotation { get; set; }
    }
}