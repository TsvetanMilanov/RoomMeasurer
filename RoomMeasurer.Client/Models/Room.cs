﻿namespace RoomMeasurer.Client.Models
{
    using System.Collections.Generic;

    public class Room
    {
        public Room(double projectedReferenceHeight, double actualReferenceHeight)
        {
            this.ProjectedReferenceHeight = projectedReferenceHeight;
            this.ActualReferenceHeight = actualReferenceHeight;
            this.Edges = new List<Edge>();
        }

        public ICollection<Edge> Edges { get; private set; }

        public double ProjectedReferenceHeight { get; set; }

        public double ActualReferenceHeight { get; set; }
    }
}
