using Microsoft.Win32;

namespace ScreenCaptureTool.Services
{
    public class FileDialogService : IFileDialogService
    {

        /// <summary>
        /// Opens open file dialog
        /// </summary>
        /// <param name="defaultExt">Example: ".txt"</param>
        /// <param name="filters">Example: "PNG (*.png) | *.png"</param>
        /// <returns>Returns full path if selected, either null</returns>
        public string OpenFileDialog(string defaultExt, string filters)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = filters,
                DefaultExt = defaultExt
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Opens save file dialog
        /// </summary>
        /// <param name="defaultExt">Example: ".txt"</param>
        /// <param name="filters">Example: "PNG (*.png) | *.png"</param>
        /// <returns>Returns full path if selected, either null</returns>
        public string SaveFileDialog(string defaultExt, string filters)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = filters,
                DefaultExt = defaultExt
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
