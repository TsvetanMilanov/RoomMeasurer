namespace RoomMeasurer.Client.ViewModels
{
    using System.ComponentModel;
    using Contracts;
    using DB;
    using Utilities;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            this.NavigationService = new NavigationService();
            this.Data = new Data();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected INavigationService NavigationService { get; private set; }
        protected Data Data { get; private set; }

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}