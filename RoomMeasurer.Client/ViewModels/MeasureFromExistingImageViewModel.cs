namespace RoomMeasurer.Client.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    using Utilities;
    using System.Collections.Generic;
    using Logic;
    using Windows.Media.Capture;
    using Windows.Storage.Provider;
    public class MeasureFromExistingImageViewModel : BaseViewModel
    {
        private double calculatedHeight;

        public MeasureFromExistingImageViewModel()
        {
            this.BrowsePicturesCommand = new DelegateCommandWithParameter<Canvas>(this.ExecuteBrowseCommand);
            this.Tap = new DelegateCommandWithParameter<TappedRoutedEventArgs>(this.ExecuteTappedCommand);
            this.CalculateHeight = new DelegateCommandWithParameter<Canvas>(this.ExecuteCalculateHeightCommand);
            this.TakePhotoWithCameraCommand = new DelegateCommandWithParameter<Canvas>(this.ExecuteTakePhotoWithCameraCommand);
        }

        public ICommand BrowsePicturesCommand { get; set; }

        public ICommand Tap { get; set; }

        public ICommand CalculateHeight { get; set; }

        public ICommand TakePhotoWithCameraCommand { get; set; }

        public double CalculatedHeight
        {
            get
            {
                return this.calculatedHeight;
            }
            set
            {
                this.calculatedHeight = value;
                this.RaisePropertyChanged("CalculatedHeight");
            }
        }

        public string ReferenceObjectHeight { get; set; }

        private async void ExecuteBrowseCommand(Canvas canvas)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".bmp");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                this.SetCanvasBackgroundToImage(file, canvas);
            }
        }

        private async void SetCanvasBackgroundToImage(IStorageFile file, Canvas canvas)
        {
            Image openedImage = await this.CreateImageFromStorageFileAsync(file);
            if (openedImage == null)
            {
                return;
            }

            openedImage.MaxHeight = canvas.ActualHeight;
            openedImage.MaxWidth = canvas.ActualWidth;

            // Clear the Canvas when new image for the background is selected.
            canvas.Children.Clear();

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = openedImage.Source;
            imageBrush.Stretch = Stretch.Uniform;
            canvas.Background = imageBrush;
        }

        private void ExecuteTappedCommand(TappedRoutedEventArgs args)
        {
            Canvas canvas = args.OriginalSource as Canvas;

            if (canvas == null)
            {
                return;
            }

            int ellipsesCount = canvas.Children
                .Where(c => c.GetType() == typeof(Ellipse))
                .Count();

            if (ellipsesCount >= 3)
            {
                return;
            }

            Ellipse circle = this.CreateEllipse(15, 15, Colors.Red);

            Point position = args.GetPosition(canvas);

            // Set the position of the new circles in the canvas.
            Canvas.SetLeft(circle, position.X - circle.Width / 2);
            Canvas.SetTop(circle, position.Y - circle.Height / 2);

            canvas.Children.Add(circle);
        }

        private void ExecuteCalculateHeightCommand(Canvas Canvas)
        {
            if (string.IsNullOrEmpty(this.ReferenceObjectHeight))
            {
                // TODO: Pop notification for the required reference height.
                return;
            }

            // Order the top offsets descending because the first point needs to be the closest to the "ground".
            IList<double> tappedPointsTopOffsets = Canvas.Children
                .Where(p => p.GetType() == typeof(Ellipse))
                .Select(p => Canvas.GetTop(p as Ellipse))
                .OrderByDescending(p => p)
                .ToList();

            if (tappedPointsTopOffsets.Count < 3)
            {
                // TODO: Pop notification to add more points to the canvas.
                return;
            }

            double firstPointOffset = tappedPointsTopOffsets[0];
            double secondPointOffset = tappedPointsTopOffsets[1];
            double thirdPointOffset = tappedPointsTopOffsets[2];

            double projectedReferenceHeight = firstPointOffset - secondPointOffset;
            double projectedEdgeHeight = firstPointOffset - thirdPointOffset;
            double actualReferenceHeight = double.Parse(this.ReferenceObjectHeight);

            this.CalculatedHeight = Measurer.GetRealHeight(projectedEdgeHeight, projectedReferenceHeight, actualReferenceHeight);
        }
        
        private async void ExecuteTakePhotoWithCameraCommand(Canvas canvas)
        {
            var camera = new CameraCaptureUI();

            var photo = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            
            // Create and configure the file picker.
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".jpg" });
            savePicker.SuggestedFileName = "IMG_" + DateTime.Now;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Lock the file so that other apps can't change it.
                CachedFileManager.DeferUpdates(file);

                // Copy the temp image to the new file.
                await FileIO.WriteBufferAsync(file, await FileIO.ReadBufferAsync(photo));

                // Unlock the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                if (status == FileUpdateStatus.Complete)
                {
                    this.SetCanvasBackgroundToImage(file, canvas);
                }
                else
                {
                    // TODO: Notification for error.
                }
            }
        }

        private Ellipse CreateEllipse(double width, double height, Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.Fill = new SolidColorBrush(color);

            return ellipse;
        }

        private async Task<Image> CreateImageFromStorageFileAsync(IStorageFile file)
        {
            var image = new Image();

            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                image.Source = bitmapImage;
            }

            return image;
        }
    }
}
