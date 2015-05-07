using Microsoft.Practices.Unity;
using ScreenCaptureTool.Services;


namespace ScreenCaptureTool.ViewModels.Locators
{
    public class ViewModelLocator
    {
        private UploadViewModel _uploadViewModel;
        private ScreenshotViewModel _screenViewModel;
        private MainWindowViewModel _mainViewModel;

        private readonly IUnityContainer _unityContainer = new UnityContainer();


        public ViewModelLocator()
        {
            ConfigureUnityContainer();
        }

        public ScreenshotViewModel ScreenshotViewModel
        {
            get { return _screenViewModel ?? (_screenViewModel = _unityContainer.Resolve<ScreenshotViewModel>()); }
        }

        public UploadViewModel UploadViewModel
        {
            get { return _uploadViewModel ?? (_uploadViewModel = _unityContainer.Resolve<UploadViewModel>()); }
        }

        public MainWindowViewModel MainViewModel
        {
            get { return _mainViewModel ?? (_mainViewModel = _unityContainer.Resolve<MainWindowViewModel>()); }
        }

        private void ConfigureUnityContainer()
        {
            _unityContainer.RegisterType<IFileDialogService, FileDialogService>();
            _unityContainer.RegisterType<IScreenCaptureService, ScreenCaptureService>();
        }
    }
}
