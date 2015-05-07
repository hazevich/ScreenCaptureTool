using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ScreenCaptureTool.DataAccess.ImageProcessing
{
    public class ImageProcessor
    {
        /// <summary>
        /// Captures image from screen by specified bounds
        /// </summary>
        /// <param name="bounds">Bounds</param>
        /// <returns><see cref="System.Windows.Media.Imaging.BitmapSource"/></returns>
        public string CaptureScreen(Rect bounds)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");

            using (var screenBitmap = new Bitmap((int)bounds.Width, (int)bounds.Height))
            {
                using (var graphics = Graphics.FromImage(screenBitmap))
                {
                    graphics.CopyFromScreen((int)bounds.X, (int)bounds.Y, 0, 0, screenBitmap.Size);
                    screenBitmap.Save(tempPath, ImageFormat.Png);
                }
            }

            return tempPath;
        }

        /// <summary>
        /// Crops Bitmap source by specified bounds
        /// </summary>
        /// <param name="image"><see cref="System.Windows.Media.Imaging.BitmapSource"/> image</param>
        /// <param name="bounds">Crop bounds</param>
        /// <returns><see cref="System.Windows.Media.Imaging.BitmapSource"/></returns>
        public CroppedBitmap CropImage(BitmapSource image, Int32Rect bounds)
        {
            var croppedBitmap = new CroppedBitmap(image, bounds);
            croppedBitmap.Freeze();

            return croppedBitmap;
        }

        public string CropImage(string imagePath, Rectangle bounds)
        {
            using (Bitmap croppedBmp = this.CroppedBitmap(imagePath, bounds))
            {
                string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
                croppedBmp.Save(tempPath, ImageFormat.Png);

                return tempPath;
            }
        }

        public Bitmap CroppedBitmap(string imagePath, Rectangle bounds)
        {
            using (Bitmap bmp = (Bitmap)Image.FromFile(imagePath))
            {
                return bmp.Clone(bounds, PixelFormat.Format32bppRgb);
            }
        }


        public BitmapImage ImageFromFile(string imagePath)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(imagePath);
            image.EndInit();
            image.Freeze();

            return image;
        }
    }
}
