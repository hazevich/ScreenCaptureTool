using ScreenCaptureTool.ViewModels;
using System;
using System.Windows;

namespace ScreenCaptureTool.Views
{
    public class WindowBase : Window
    {
        public WindowBase()
        {
            this.DataContextChanged += OnWindowBaseDataContextChanged;
        }

        private void OnWindowBaseDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var model = e.NewValue as ICloseableViewModel;
            if (model != null)
                model.CloseEvent += this.OnWindowBaseClosed;
        }

        protected virtual void OnWindowBaseClosed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
