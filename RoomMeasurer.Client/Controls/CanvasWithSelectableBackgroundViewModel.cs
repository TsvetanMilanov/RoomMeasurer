namespace RoomMeasurer.Client.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.Media.Capture;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.Storage.Provider;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    using Utilities;
    using ViewModels;
    using Utilities.Notifications;

    public class CanvasWithSelectableBackgroundViewModel : BaseViewModel
    {
        private static Color DefaultEllipseColor = Colors.Red;
        private static Color TappedEllipseColor = Colors.Blue;

        private double calculatedAngle;

        public CanvasWithSelectableBackgroundViewModel()
        {
            this.BrowsePicturesCommand = new DelegateCommandWithParameter<Canvas>(this.ExecuteBrowseCommand);
            this.Tap = new DelegateCommandWithParameter<TappedRoutedEventArgs>(this.ExecuteTappedCommand);
            this.TakePhotoWithCameraCommand = new DelegateCommandWithParameter<Canvas>(this.ExecuteTakePhotoWithCameraCommand);
        }

        public double CalculatedAngle
        {
            get
            {
                return this.calculatedAngle;
            }
            set
            {
                this.calculatedAngle = value;
                this.RaisePropertyChanged("CalculatedAngle");
            }
        }

        public ICommand BrowsePicturesCommand { get; set; }

        public ICommand TakePhotoWithCameraCommand { get; set; }

        public ICommand Tap { get; set; }

        public int MaxTaps { get; set; }

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

            if (ellipsesCount >= MaxTaps)
            {
                return;
            }

            Ellipse circle = this.CreateEllipse(15, 15, DefaultEllipseColor);

            Point position = args.GetPosition(canvas);

            // Set the position of the new circles in the canvas.
            Canvas.SetLeft(circle, position.X - circle.Width / 2);
            Canvas.SetTop(circle, position.Y - circle.Height / 2);

            canvas.Children.Add(circle);
        }

        private async void ExecuteTakePhotoWithCameraCommand(Canvas canvas)
        {
            var camera = new CameraCaptureUI();

            var photo = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            // Create and configure the file picker.
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Image", new List<string>() { ".jpg" });
            savePicker.SuggestedFileName = "IMG_" + DateTime.Now;

            StorageFile file = null;

            // Need the check for exception because of random UnauthorizedAccessException (emulator restart fixes the problem).
            try
            {
                file = await savePicker.PickSaveFileAsync();
            }
            catch (UnauthorizedAccessException)
            {
                MessageDialogNotifier.Notify("You are not authorized to save files in this directory. Try different one.");
                return;
            }

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
                    MessageDialogNotifier.Notify("The photo cannot be saved. Please giv this application the required permissions..");
                }
            }

            this.SetAngle();
        }

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

            this.SetAngle();
        }

        private Ellipse CreateEllipse(double width, double height, Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.Fill = new SolidColorBrush(color);
            ellipse.ManipulationMode = ManipulationModes.All;
            ellipse.ManipulationStarted += HandleEllipseManipulationStarted;
            ellipse.ManipulationDelta += HandleEllipseManipulationDelta;
            ellipse.ManipulationCompleted += HandleEllipseManipulationCompleted;
            ellipse.ManipulationInertiaStarting += HandleEllipseInertionStarted;

            return ellipse;
        }

        private void HandleEllipseInertionStarted(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = int.MaxValue;

        }

        private void HandleEllipseManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Ellipse control = sender as Ellipse;

            control.Fill = new SolidColorBrush(DefaultEllipseColor);
        }

        private void HandleEllipseManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Ellipse control = sender as Ellipse;

            double top = Canvas.GetTop(control);
            double left = Canvas.GetLeft(control);

            Canvas.SetTop(control, top + e.Delta.Translation.Y);
            Canvas.SetLeft(control, left + e.Delta.Translation.X);
        }

        private void HandleEllipseManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Ellipse control = sender as Ellipse;

            control.Fill = new SolidColorBrush(TappedEllipseColor);
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

        private void SetAngle()
        {
            this.CalculatedAngle = AngleCalculator.CalculateAngle();
        }
    }
}
