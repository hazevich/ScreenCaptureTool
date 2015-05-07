using System.ComponentModel;

namespace ScreenCaptureTool.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                handler(this, eventArgs);
            }
        }
    }
}
