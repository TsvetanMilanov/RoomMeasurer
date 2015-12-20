namespace RoomMeasurer.Client.ViewModels
{
    using System.Windows.Input;
    using Windows.Web.Http;
    using Windows.Storage.Streams;

    using Newtonsoft.Json;

    using DB;
    using DB.Models;
    using Web.RequestModels;
    using Web.ResponseModels;
    using Utilities;
    using Utilities.Notifications;
    using System;
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            this.Login = new DelegateCommand(this.ExecuteLoginCommand);
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICommand Login { get; set; }

        private async void ExecuteLoginCommand()
        {
            Requester requester = new Requester();

            UserRequestModel requestModel = new UserRequestModel
            {
                UserName = this.UserName,
                Password = Password
            };

            string requestBody = JsonConvert.SerializeObject(requestModel);

            HttpStringContent requestContent = new HttpStringContent(requestBody, UnicodeEncoding.Utf8, "application/json");

            string response = string.Empty;

            try
            {
                response = await requester.PutJsonAsync("/api/users/token", requestContent);
            }
            catch (Exception)
            {
                MessageDialogNotifier.Notify("There was an error on the server. Please contact the server administrators.");
            }

            UserResponseModel user = JsonConvert.DeserializeObject<UserResponseModel>(response);

            if (string.IsNullOrEmpty(user.UserName) || 
                string.IsNullOrEmpty(user.Token))
            {
                MessageDialogNotifier.Notify("Invalid username or password.");
            }
            else
            {
                Data data = new Data();

                UserDatabaseModel databaseUser = new UserDatabaseModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    RegistrationDate = user.RegistrationDate,
                    UserName = user.UserName,
                    Token = user.Token
                };

                await data.UpdateCurrentUserAsync(databaseUser);

                UserDatabaseModel currentUser = await data.GetCurrentUser();

                MessageDialogNotifier.Notify(string.Format("Hello {0} {1}!\nYou are now logged in.", currentUser.FirstName, currentUser.LastName));
            }
        }
    }
}
