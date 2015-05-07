using System.Windows;

namespace ScreenCaptureTool.DataAccess
{
    public interface IScreenBoundsProvider
    {
        Rect ScreenBounds { get; set; }
    }
}
