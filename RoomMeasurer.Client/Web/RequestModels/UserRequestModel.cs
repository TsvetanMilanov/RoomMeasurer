namespace RoomMeasurer.Client.Web.RequestModels
{
    using Newtonsoft.Json;

    [JsonObject]
    public class UserRequestModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
