using ScreenCaptureTool.Commands;
using ScreenCaptureTool.DataAccess;
using ScreenCaptureTool.DataAccess.ImageProcessing;
using ScreenCaptureTool.Enums;
using ScreenCaptureTool.Helpers;
using ScreenCaptureTool.Helpers.IOHooks;
using ScreenCaptureTool.Helpers.WinApi;
using ScreenCaptureTool.Services;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace ScreenCaptureTool.ViewModels
{
    public class ScreenshotViewModel : ViewModelBase, IScreenBoundsProvider, ICloseableViewModel
    {
        private Rect _cutImageBounds;
        private Rect _screenBounds;
        private ImageSource _screenImage;
        private ImageProcessor _imageProcessor = new ImageProcessor();
        private bool _canExecutePostProcessingCommand;
        private bool _screenViewIsVisible;

        private IFileDialogService _fileDialog;

        private string _imagePath;

        public ScreenshotViewModel(IFileDialogService fileDialog)
        {
            _fileDialog = fileDialog;

            ToClipboardCommand = new RelayCommand(p => ToClipboardExecute(), p => _canExecutePostProcessingCommand);
            CancelCommand = new RelayCommand(p => CancelExecute(), p => true);
            SaveCommand = new RelayCommand(p => SaveExecute(), p => _canExecutePostProcessingCommand);
            UploadCommand = new RelayCommand(p => UploadExecute(), p => _canExecutePostProcessingCommand);
            CaptureWindowCommand = new RelayCommand(p => CaptureWindowExecute(), p => true);
        }

        public ImageSource ScreenImage
        {
            get
            {
                return _screenImage;
            }
            private set
            {
                _screenImage = value;
                OnPropertyChanged("ScreenImage");
            }
        }

        public Rect CutImageBounds
        {
            get
            {
                return _cutImageBounds;
            }
            set
            {
                _cutImageBounds = value;
                OnPropertyChanged("CutImageBounds");
            }
        }

        public Rect ScreenBounds
        {
            get
            {
                return _screenBounds;
            }
            set
            {
                _screenBounds = value;
                OnPropertyChanged("ScreenBounds");
            }
        }

        public bool ScreenViewIsVisible
        {
            get
            {
                return _screenViewIsVisible;
            }
            set
            {
                _screenViewIsVisible = value;
                OnPropertyChanged("ScreenViewIsVisible");
            }

        }

        public bool CanExecutePostProcessingAction
        {
            get
            {
                return _canExecutePostProcessingCommand;
            }
            set
            {
                _canExecutePostProcessingCommand = value;
                OnPropertyChanged("CanExecutePostProcessingAction");
            }
        }

        #region commands

        public ICommand ToClipboardCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand UploadCommand { get; private set; }

        public ICommand CaptureWindowCommand { get; private set; }

        #endregion

        #region command execute

        private void ToClipboardExecute()
        {
            var croppedImage = _imageProcessor.CropImage((BitmapSource)ScreenImage, CutImageBounds.ToInt32Rect());

            Clipboard.SetImage(croppedImage);

            RaiseCloseEvent();
        }

        private void CancelExecute()
        {
            RaiseCloseEvent();
        }

        private void SaveExecute()
        {
            CanExecutePostProcessingAction = false;

            var defaultExt = ".png";
            var filters = "PNG (*.png) | *.png";

            string filePath = _fileDialog.SaveFileDialog(defaultExt, filters);

            if (string.IsNullOrEmpty(filePath))
            {
                CanExecutePostProcessingAction = true;
                return;
            }

            using (var bmp = _imageProcessor.CroppedBitmap(_imagePath, CutImageBounds.ToDrawingRectangle()))
            {
                bmp.Save(filePath, ImageFormat.Png);
            }

            RaiseCloseEvent();
        }

        private async Task UploadExecute()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void DeleteImage()
        {
            File.Delete(_imagePath);
            ScreenImage = null;
        }

        private void CaptureWindowExecute()
        {
            MouseHooks.Instance.InstallHook();
            MouseHooks.Instance.MouseDown += OnMouseClick;
            ScreenViewIsVisible = false;
            File.Delete(_imagePath);
            ScreenImage = null;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "ScreenViewIsVisible")
            {
                ScreenViewLoaded();
            }

            base.OnPropertyChanged(propertyName);
        }

        private void ScreenViewLoaded()
        {
            if (ScreenViewIsVisible)
            {
                _imagePath = _imageProcessor.CaptureScreen(ScreenSizeResolver.GetSize(ScreenMode.AllScreens));
                ScreenBounds = ScreenSizeResolver.GetSize(ScreenMode.AllScreens);
                ScreenImage = _imageProcessor.ImageFromFile(_imagePath);
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            IntPtr window = User32.WindowFromPoint(e.X, e.Y);
            if (window != IntPtr.Zero)
            {
                User32.SetForegroundWindow(window);
                var uRect = new User32.Rect();
                User32.GetWindowRect(window, ref uRect);
                CutImageBounds = new Rect(uRect.Left + Math.Abs(ScreenBounds.X),
                    uRect.Top, uRect.Right - uRect.Left, uRect.Bottom - uRect.Top);
                MouseHooks.Instance.UnInstallHook();
                ScreenViewIsVisible = true;
                CanExecutePostProcessingAction = true;
                MouseHooks.Instance.MouseDown -= OnMouseClick;
            }
        }

        public event EventHandler CloseEvent;

        private void RaiseCloseEvent()
        {
            if (CloseEvent != null)
            {
                ScreenBounds = new Rect(0, 0, 0, 0);
                CutImageBounds = new Rect(0, 0, 0, 0);
                CloseEvent(this, EventArgs.Empty);
                CanExecutePostProcessingAction = false;
                DeleteImage();
            }
        }
    }
}
