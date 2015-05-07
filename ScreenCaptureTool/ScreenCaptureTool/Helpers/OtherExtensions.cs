using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ScreenCaptureTool.Helpers
{
    public static class OtherExtensions
    {
        public static void Save(this BitmapSource image, string path)
        {
            var pngEncoder = new PngBitmapEncoder();

            pngEncoder.Frames.Add(BitmapFrame.Create(image));

            using (var fs = new FileStream(path, FileMode.Create))
                pngEncoder.Save(fs);
        }


        public static Int32Rect ToInt32Rect(this Rect rect)
        {
            return new Int32Rect
            {
                X = (int)rect.X,
                Y = (int)rect.Y,
                Width = (int)rect.Width,
                Height = (int)rect.Height
            };
        }

        public static Rectangle ToDrawingRectangle(this Rect rect)
        {
            return new Rectangle
            {
                X = (int)rect.X,
                Y = (int)rect.Y,
                Width = (int)rect.Width,
                Height = (int)rect.Height
            };
        }
    }
}
