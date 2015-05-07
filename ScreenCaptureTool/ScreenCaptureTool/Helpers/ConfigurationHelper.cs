using ScreenCaptureTool.DataAccess;
using System.Configuration;

namespace ScreenCaptureTool.Helpers
{
    public class ConfigurationHelper : SingletonBase<ConfigurationHelper>
    {
        public bool KeepSignedIn
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["KeepMeSignedIn"].ToLower()); }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["KeepMeSignedIn"].Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public bool ScreenCapture { get; set; }
    }
}
