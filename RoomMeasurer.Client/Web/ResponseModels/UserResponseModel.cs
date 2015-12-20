namespace RoomMeasurer.Client.Web.ResponseModels
{
    using System;
    using Newtonsoft.Json;

    [JsonObject]
    public class UserResponseModel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        [JsonProperty("registrationDate")]
        public DateTime RegistrationDate { get; set; }

    }
}
