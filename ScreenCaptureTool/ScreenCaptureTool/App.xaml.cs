using Microsoft.Practices.Unity;
using ScreenCaptureTool.DataAccess;
using ScreenCaptureTool.Helpers.IOHooks;
using ScreenCaptureTool.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenCaptureTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            HotkeysFactory.Instance.UnregisterAllHotKeys();
            MouseHooks.Instance.UnInstallHook();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            var unityContainer = new UnityContainer();

            MainWindow mainView = unityContainer.Resolve<MainWindow>();
            mainView.Show();

            Current.MainWindow = mainView;
        }
    }
}
