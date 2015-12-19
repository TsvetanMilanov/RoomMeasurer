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

        public CalculationResultViewModel()
        {
            this.GoToMeasureNextEdgeCommand = new DelegateCommand(this.HandleGoToMeasureNextEdge);
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
        
        public ICommand GoToMeasureNextEdgeCommand { get; set; }
        
        public void CalculateHeight(Room room)
        {
            Edge lastEdge = room.Edges.Last();

            this.CalculatedHeight = Measurer.GetRealHeight(lastEdge.ProjectedHeight, room.ProjectedReferenceHeight, room.ActualReferenceHeight);
        }

        private void HandleGoToMeasureNextEdge()
        {
            this.NavigationService.Navigate(typeof(MeasureWithoutReferencePage), this.Room);
        }
    }
}
