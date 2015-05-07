
using ScreenCaptureTool.Views;
namespace ScreenCaptureTool.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        private ScreenshotView _screenshotView;

        public ScreenCaptureService(ScreenshotView screenshotView)
        {
            _screenshotView = screenshotView;
        }

        public void Capture(bool screenCapture)
        {
            _screenshotView.TakeScreenshot(screenCapture);
        }
    }
}
