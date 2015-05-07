using ScreenCaptureTool.ViewModels.Upload;
using System;
using System.Threading.Tasks;

namespace ScreenCaptureTool.ViewModels
{
    public class UploadViewModel : ViewModelBase
    {
        private ViewModelBase _uploadViewModel;
        private UploadProgressViewModel _uploadProgressViewModel;
        private UploadResultViewModel _uploadResultViewModel;

        public UploadViewModel()
        {
            _uploadProgressViewModel = new UploadProgressViewModel();
            _uploadResultViewModel = new UploadResultViewModel();
            ViewModel = _uploadProgressViewModel;
        }

        public ViewModelBase ViewModel
        {
            get { return _uploadViewModel; }
            set
            {
                _uploadViewModel = value;
                OnPropertyChanged("ViewModel");
            }
        }

        public async Task<bool> Upload()
        {
            throw new NotImplementedException();
        }
    }
}
