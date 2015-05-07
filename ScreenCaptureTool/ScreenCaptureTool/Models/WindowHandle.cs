using System;
using System.Windows;

namespace ScreenCaptureTool.Models
{
    public class WindowHandle
    {
        public string Name { get; set; }
        public string ProcessName { get; set; }
        public IntPtr Handle { get; set; }
        public Rect Rect { get; set; }
    }
}
