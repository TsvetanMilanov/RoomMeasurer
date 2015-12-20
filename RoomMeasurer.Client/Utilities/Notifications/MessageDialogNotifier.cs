namespace RoomMeasurer.Client.Utilities.Notifications
{
    using System;
    using Windows.UI.Popups;

    public static class MessageDialogNotifier
    {
        public async static void Notify(string message)
        {
            MessageDialog messageDialog = new MessageDialog(message);
            
            await messageDialog.ShowAsync();
        }
    }
}
