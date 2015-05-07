using ScreenCaptureTool.Enums;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ScreenCaptureTool.DataAccess.ImageProcessing
{
    /// <summary>
    /// Resolvses capture mode by providing correct screen size
    /// </summary>
    public class ScreenSizeResolver
    {
        /// <summary>
        /// Gets the screen size according to capture mode
        /// </summary>
        /// <param name="mode">capture mode</param>
        /// <returns>Screen size</returns>
        public static Rect GetSize(ScreenMode mode)
        {
            switch (mode)
            {
                case ScreenMode.PrimaryScreen:
                    return PrimaryScreenSize;
                case ScreenMode.AllScreens:
                    return AllScreensSize;
                default:
                    throw new ArgumentException(string.Format("Uknown ScreenMode - {0}", mode.ToString()));
            }
        }

        /// <summary>
        /// Size of primary screen
        /// </summary>
        private static Rect PrimaryScreenSize
        {
            get
            {
                return new Rect
                {
                    X = 0,
                    Y = 0,
                    Width = (int)SystemParameters.PrimaryScreenWidth,
                    Height = (int)SystemParameters.PrimaryScreenHeight
                };
            }
        }

        /// <summary>
        /// Size of all screens
        /// </summary>
        private static Rect AllScreensSize
        {
            get
            {
                var rect = new Rect();
                Screen[] allScreens = Screen.AllScreens;

                foreach (Screen screen in allScreens)
                {
                    rect.X = Math.Min(rect.X, screen.Bounds.X);
                    rect.Y = Math.Min(rect.Y, screen.Bounds.Y);
                    rect.Width += screen.Bounds.Width;
                    rect.Height += screen.Bounds.Height;
                }

                return rect;
            }
        }
    }
}
