using System;
using System.Windows.Input;

namespace ScreenCaptureTool.ViewModels
{
    public class ScreenRecorderViewModel : ViewModelBase
    {
        public ICommand StartRecordingCommand { get; private set; }
        public ICommand StopRecordingCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }

        private void StartRecordingExecute()
        {
            throw new NotImplementedException();
        }

        private void StopRecordingExecute()
        {
            throw new NotImplementedException();
        }
    }
}
