using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
namespace RoomMeasurer.Client.Pages
{
    using Windows.UI.Xaml.Controls;
    
    using ViewModels;

    public sealed partial class MeasureFromExistingImagePage : Page
    {
        public MeasureFromExistingImagePage()
        {
            this.InitializeComponent();
            this.ViewModel = new MeasureFromExistingImageViewModel();
        }

        public MeasureFromExistingImageViewModel ViewModel
        {
            get
            {
                return this.DataContext as MeasureFromExistingImageViewModel;
            }
            private set
            {
                this.DataContext = value;
            }
        }
    }
}
