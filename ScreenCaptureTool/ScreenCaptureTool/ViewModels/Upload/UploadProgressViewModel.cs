using System;
using System.Threading.Tasks;

namespace ScreenCaptureTool.ViewModels.Upload
{
    public class UploadProgressViewModel : ViewModelBase
    {
        private bool _isUploading = true;
        private string _uploadingMessage = "Uploading please wait...";

        public UploadProgressViewModel()
        {
        }


        public bool IsUploading
        {
            get { return _isUploading; }
            set
            {
                _isUploading = value;
                OnPropertyChanged("IsUploading");
            }
        }

        public string UploadingMessage
        {
            get { return _uploadingMessage; }
            set
            {
                _uploadingMessage = value;
                OnPropertyChanged("UploadingMessage");
            }
        }


        public async Task Upload()
        {
            throw new NotImplementedException();
        }
    }
}
