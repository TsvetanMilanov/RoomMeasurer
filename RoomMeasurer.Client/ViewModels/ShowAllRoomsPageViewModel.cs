namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;

    using Newtonsoft.Json;

    using DB;
    using DB.Models;
    using Models;
    using Pages;
    using Utilities;
    using Web.ResponseModels;
    using Windows.UI.Xaml.Input;
    using Utilities.Notifications;
    public class ShowAllRoomsPageViewModel : BaseViewModel
    {
        private ObservableCollection<Room> allRooms;

        public ShowAllRoomsPageViewModel()
        {
            this.GetAllRoomsAsync();
            this.SelectedItem = new DelegateCommandWithParameter<Room>(this.ExecuteSelectedItemCommand);
        }
        
        public ICommand SelectedItem { get; set; }

        public IEnumerable<Room> AllRooms
        {
            get
            {
                if (this.allRooms == null)
                {
                    this.allRooms = new ObservableCollection<Room>();
                }

                return this.allRooms;
            }
            set
            {
                if (this.allRooms == null)
                {
                    this.allRooms = new ObservableCollection<Room>();
                }

                this.allRooms.Clear();

                foreach (var item in value)
                {
                    this.allRooms.Add(item);
                }

                this.RaisePropertyChanged("AllRooms");
            }
        }
        
        private void ExecuteSelectedItemCommand(Room obj)
        {
            this.NavigationService.Navigate(typeof(RoomDetailsPage), obj);
        }

        public async void GetAllRoomsAsync()
        {
            Requester requester = new Requester();

            Data data = new Data();

            UserDatabaseModel currentUser = await Data.GetCurrentUser();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.Token))
            {
                MessageDialogNotifier.Notify("You must be logged in to get all rooms.");
                return;
            }

            string result = string.Empty;

            try
            {
                result = await requester.GetJsonAsync("/api/roomGeometry", currentUser.Token);
            }
            catch (Exception)
            {
                MessageDialogNotifier.Notify("You must be logged in to get all rooms.");
                return;
            }

            IEnumerable<RoomResponseModel> allRoomsResponseModels = JsonConvert.DeserializeObject<IEnumerable<RoomResponseModel>>(result);

            if (allRoomsResponseModels == null || allRoomsResponseModels.Count() <= 0)
            {
                MessageDialogNotifier.Notify("You must be logged in to get all rooms.");
                return;
            }

            if (allRoomsResponseModels != null)
            {
                allRoomsResponseModels
                           .Select(r => r.Room)
                           .ToList()
                           .ForEach(r =>
                           {
                               this.allRooms.Add(r);
                           });
            }
        }
    }
}
