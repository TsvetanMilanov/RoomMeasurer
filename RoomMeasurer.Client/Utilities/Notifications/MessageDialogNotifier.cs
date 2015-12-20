namespace RoomMeasurer.Client.Utilities.Notifications
{
    using System;
    using System.Threading.Tasks;
    using Windows.UI.Popups;

    public static class MessageDialogNotifier
    {
        private static bool hasOpenedMessageDialog = false;

        public async static void Notify(string message)
        {
            if (!hasOpenedMessageDialog)
            {
                hasOpenedMessageDialog = true;
                MessageDialog messageDialog = new MessageDialog(message);
                await messageDialog.ShowAsync();
                hasOpenedMessageDialog = false;
            }
        }
    }
}
