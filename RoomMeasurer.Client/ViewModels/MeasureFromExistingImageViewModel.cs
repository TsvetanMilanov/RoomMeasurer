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

    public class MeasureFromExistingImageViewModel : BaseViewModel
    {
        public MeasureFromExistingImageViewModel()
        {
            this.BrowsePicturesCommand = new DelegateCommandWithParameter<Canvas>(this.ExecuteBrowseCommand);
            this.Tap = new DelegateCommandWithParameter<TappedRoutedEventArgs>(this.ExecuteTappedCommand);
        }

        public ICommand BrowsePicturesCommand { get; set; }

        public ICommand Tap { get; set; }

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

        private Ellipse CreateEllipse(double width, double height, Color color)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.Fill = new SolidColorBrush(color);

            return ellipse;
        }

        private async Task<Image> CreateImageFromStorageFileAsync(StorageFile file)
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
