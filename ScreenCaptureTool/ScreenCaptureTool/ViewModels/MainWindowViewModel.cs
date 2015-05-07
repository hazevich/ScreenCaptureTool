using ScreenCaptureTool.Commands;
using ScreenCaptureTool.DataAccess;
using ScreenCaptureTool.Models.Hotkeys;
using ScreenCaptureTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenCaptureTool.ViewModels
{
    public class MainWindowViewModel
    {
        private IScreenCaptureService _screenCaptureService;

        public MainWindowViewModel(IScreenCaptureService screenCaptureService)
        {
            _screenCaptureService = screenCaptureService;

            ExitCommand = new RelayCommand(p => ExitExecute(), p => true);
            OpenCommand = new RelayCommand(p => OpenExecute(), p => true);
            UploadImageCommand = new RelayCommand(p => UploadImageExecute(), p => true);
            CaptureScreenshotCommand = new RelayCommand(p => CaptureScreenshotExecute(), p => true);
        }

        public ICommand ExitCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand UploadImageCommand { get; private set; }

        public ICommand CaptureScreenshotCommand { get; private set; }

        private void UploadImageExecute()
        {
            throw new NotImplementedException();
        }

        private void CaptureScreenshotExecute()
        {
            _screenCaptureService.Capture(true);
        }

        private void RegisterGlobalHotkeys()
        {
            var captureScreenArgs = new HotkeyArgs
            {
                Name = "CaptureScreen",
                Key = System.Windows.Forms.Keys.S,
                ModKeys = ModifierKeys.Alt,
                Window = Application.Current.MainWindow,
                Action = CaptureScreenshotExecute
            };

            HotkeysFactory.Instance.RegisterHotKey(captureScreenArgs);
        }

        private void ExitExecute()
        {
            Application.Current.Shutdown();
        }

        private void OpenExecute()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow.Visibility == Visibility.Hidden)
            {
                mainWindow.Show();
            }
            else if (mainWindow.Visibility == Visibility.Visible)
            {
                mainWindow.Hide();
            }
        }
    }
}
