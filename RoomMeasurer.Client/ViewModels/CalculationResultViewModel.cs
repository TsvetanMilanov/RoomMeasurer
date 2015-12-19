namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Logic;
    using Models;
    using Pages;
    using Utilities;

    public class CalculationResultViewModel : BaseViewModel
    {
        private double calculatedHeight;
        private Edge edge;

        public CalculationResultViewModel()
        {
            this.GoToMeasureNextEdgeCommand = new DelegateCommand(this.HandleGoToMeasureNextEdge);
            this.Finish = new DelegateCommand(this.HandleGoToFinishPage);
        }

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

        public Room Room { get; internal set; }

        public Edge Edge
        {
            get
            {
                return this.edge;
            }
            set
            {
                this.edge = value;
                this.RaisePropertyChanged("Edge");
            }
        }
        
        public ICommand GoToMeasureNextEdgeCommand { get; set; }

        public ICommand Finish { get; set; }

        public void CalculateHeight(Room room)
        {
            this.Edge = room.Edges.Last();

            this.CalculatedHeight = Measurer.GetRealHeight(this.Edge.ProjectedHeight, room.ProjectedReferenceHeight, room.ActualReferenceHeight);
        }

        private void HandleGoToMeasureNextEdge()
        {
            this.NavigationService.Navigate(typeof(MeasureWithoutReferencePage), this.Room);
        }
        
        private void HandleGoToFinishPage()
        {
            this.NavigationService.Navigate(typeof(FinishMeasurementPage), this.Room);
        }
    }
}
