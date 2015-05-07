using System;

namespace ScreenCaptureTool.ViewModels
{
    public interface ICloseableViewModel
    {
        event EventHandler CloseEvent;
    }
}
