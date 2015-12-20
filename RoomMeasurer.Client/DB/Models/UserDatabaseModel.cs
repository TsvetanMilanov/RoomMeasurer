namespace RoomMeasurer.Client.DB.Models
{
    using System;

    using SQLite.Net.Attributes;

    public class UserDatabaseModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public DateTime RegistrationDate { get; set; }
    }
}
