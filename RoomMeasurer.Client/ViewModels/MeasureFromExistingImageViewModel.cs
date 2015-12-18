namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;
    using Windows.Media.Capture;
    using Windows.Storage.Provider;

    using Logic;
    using Utilities;

    public class MeasureFromExistingImageViewModel : BaseViewModel
    {
        private double calculatedHeight;

        public MeasureFromExistingImageViewModel()
        {
            this.CalculateHeight = new DelegateCommandWithParameter<Canvas>(this.ExecuteCalculateHeightCommand);
        }

        public ICommand CalculateHeight { get; set; }

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

        private void ExecuteCalculateHeightCommand(Canvas Canvas)
        {
            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                // TODO: Pop notification for the required reference height.
                return;
            }

            // Order the top offsets descending because the first point needs to be the closest to the "ground".
            IList<double> tappedPointsTopOffsets = Canvas.Children
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
        }
    }
}
