﻿namespace RoomMeasurer.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Shapes;

    using Logic;
    using Utilities;
    using ViewModels;
    using System;

    public class MeasureFromExistingImageViewModel : CalculateBaseModel<Canvas>
    {
        private double calculatedHeight;

        public double CalculatedHeight
        {
            get
            {
                return this.calculatedHeight;
            }
            set
            {
                this.calculatedHeight = value;
                this.RaisePropertyChanged("CalculatedHeight");
            }
        }

        public string ReferenceObjectHeight { get; set; }

        protected override async void ExecuteCalculateCommand(Canvas canvas)
        {
            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                // TODO: Pop notification for the required reference height.
                return;
            }

            // Order the top offsets descending because the first point needs to be the closest to the "ground".
            IList<double> tappedPointsTopOffsets = canvas.Children
                .Where(p => p.GetType() == typeof(Ellipse))
                .Select(p => Canvas.GetTop(p as Ellipse))
                .OrderByDescending(p => p)
                .ToList();

            if (tappedPointsTopOffsets.Count < 3)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];
            double thirdPointOffset = tappedPointsTopOffsets[2];

            double projectedReferenceHeight = firstPointOffset - secondPointOffset;
            double projectedEdgeHeight = firstPointOffset - thirdPointOffset;
            double actualReferenceHeight = double.Parse(this.ReferenceObjectHeight);

            this.CalculatedHeight = Measurer.GetRealHeight(projectedEdgeHeight, projectedReferenceHeight, actualReferenceHeight);

            var focusDistance = await this.Data.GetFoucsDistance();
            var distance = Measurer.GetEdgeDistances(new double[] { 155 }, focusDistance, projectedReferenceHeight, actualReferenceHeight);
        }

    }
}
