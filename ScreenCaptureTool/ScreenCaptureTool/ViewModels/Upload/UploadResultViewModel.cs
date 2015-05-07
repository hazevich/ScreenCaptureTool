using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ScreenCaptureTool.Commands;

namespace ScreenCaptureTool.ViewModels.Upload
{
    public class UploadResultViewModel : ViewModelBase, ICloseableViewModel
    {
        public event EventHandler CloseEvent;
        private string _imageUrl;

        public UploadResultViewModel()
        {
            CopyCommand = new RelayCommand(p => CopyExecute(), p => true);
            OpenCommand = new RelayCommand(p => OpenExecute(), p => true);
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        public ICommand CopyCommand
        {
            get;
            private set;
        }

        public ICommand OpenCommand
        {
            get;
            private set;
        }

        private void CopyExecute()
        {
            Clipboard.SetText(ImageUrl);
            OnCloseEvent();
        }

        private void OpenExecute()
        {
            Process.Start(ImageUrl);
            OnCloseEvent();
        }

        private void OnCloseEvent()
        {
            var handler = CloseEvent;

            if (handler != null)
            {
                CloseEvent(this, EventArgs.Empty);
            }
        }
    }
}
