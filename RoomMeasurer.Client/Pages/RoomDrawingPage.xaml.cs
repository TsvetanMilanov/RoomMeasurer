using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using RoomMeasurer.Client.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RoomMeasurer.Client.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomDrawingPage : Page
    {
        public RoomDrawingPage()
        {
            this.InitializeComponent();
            this.ViewModel = new RoomDrawingViewModel();
        }

        public RoomDrawingViewModel ViewModel
        {
            get
            {
                return this.DataContext as RoomDrawingViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var room = e.Parameter as RoomGeometryViewModel;
            this.ViewModel.CalculateRoomCorners(room);
            base.OnNavigatedTo(e);
        }
    }
}
