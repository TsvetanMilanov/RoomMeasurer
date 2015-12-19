namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Shapes;

    using Utilities;
    using Windows.Devices.Sensors;

    public abstract class CalculateBaseModel<T> : BaseViewModel
    {
        public CalculateBaseModel()
        {
            this.PointsDistanceCalculator = new PointsDistanceCalculator();
            this.Calculate = new DelegateCommandWithParameter<T>(this.ExecuteCalculateCommand);
            this.SaveAngle = new DelegateCommandWithParameter<string>(this.ExecuteSaveAngleCommand);
            this.CalculatedAngle = double.MinValue;
        }

        private void ExecuteSaveAngleCommand(string angle)
        {
            this.CalculatedAngle = double.Parse(angle);
        }

        public PointsDistanceCalculator PointsDistanceCalculator { get; set; }

        public ICommand Calculate { get; set; }

        public ICommand SaveAngle { get; set; }

        public double CalculatedAngle { get; set; }

        protected abstract void ExecuteCalculateCommand(T param);

        protected IList<double> GetTappedPointsTopOffsets(Canvas canvas)
        {
            if (canvas == null)
            {
                throw new NullReferenceException();
            }
            // Order the top offsets descending because the first point needs to be the closest to the "ground".
            IList<double> tappedPointsTopOffsets = canvas.Children
                .Where(p => p.GetType() == typeof(Ellipse))
                .Select(p => Canvas.GetTop(p as Ellipse))
                .OrderByDescending(p => p)
                .ToList();

            return tappedPointsTopOffsets;
        }
    }
}
