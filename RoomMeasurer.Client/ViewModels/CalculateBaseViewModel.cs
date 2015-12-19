namespace RoomMeasurer.Client.ViewModels
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Shapes;

    using Utilities;

    public abstract class CalculateBaseModel<T> : BaseViewModel
    {
        public CalculateBaseModel()
        {
            this.PointsDistanceCalculator = new PointsDistanceCalculator();
            this.Calculate = new DelegateCommandWithParameter<T>(this.ExecuteCalculateCommand);
        }

        public PointsDistanceCalculator PointsDistanceCalculator { get; set; }

        public ICommand Calculate { get; set; }

        protected abstract void ExecuteCalculateCommand(T param);

        protected IList<double> GetTappedPointsTopOffsets(Canvas canvas)
        {
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
