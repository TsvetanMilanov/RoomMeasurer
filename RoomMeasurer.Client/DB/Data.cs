namespace RoomMeasurer.Client.DB
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite.Net;
    using SQLite.Net.Platform.WinRT;
    using Windows.Storage;

    public class Data
    {
        public const string SettingsFileName = "focus.txt";
        private readonly string dbFilePath;
        private SQLiteConnectionWithLock dbContext;
        private StorageFolder localFolder;

        public Data()
        {
            // will be created if non-existent
            this.dbFilePath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                "db.sqlite");

            this.dbContext = new SQLiteConnectionWithLock(
                         new SQLitePlatformWinRT(),
                         new SQLiteConnectionString(
                             dbFilePath,
                             storeDateTimeAsTicks: false)
                         );

            this.localFolder = ApplicationData.Current.LocalFolder;

        }

        public async void SaveFocusDistance(double f)
        {
            var files = await this.localFolder.GetFilesAsync();
            var textFile = files.FirstOrDefault(x => x.Name == SettingsFileName);
            if (textFile == null)
            {
                textFile = await this.localFolder.CreateFileAsync(SettingsFileName);
            }

            await FileIO.WriteTextAsync(textFile, f.ToString());
        }

        /// <summary>
        /// Gets the previously saved focus distance.
        /// </summary>
        /// <returns>Returns the focus distance or -1 if not found.</returns>
        public async Task<double> GetFoucsDistance()
        {
            var files = await this.localFolder.GetFilesAsync();
            var desiredFile = files.FirstOrDefault(x => x.Name == SettingsFileName);
            if (desiredFile == null)
            {
                return -1;
            }

            var textContent = await FileIO.ReadTextAsync(desiredFile);
            var focus = double.Parse(textContent);

            return focus;
        }
    }
}