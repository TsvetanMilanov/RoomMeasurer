namespace RoomMeasurer.Client.Contracts
{
    using System;

    public interface INavigationService
    {
        void Navigate(Type sourcePageType);
        void Navigate(Type sourcePageType, object parameter);
        void GoBack();
    }
}
