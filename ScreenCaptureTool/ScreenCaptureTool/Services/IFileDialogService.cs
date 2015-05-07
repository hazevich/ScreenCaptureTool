
namespace ScreenCaptureTool.Services
{
    public interface IFileDialogService
    {
        string OpenFileDialog(string defaultExt, string filters);
        string SaveFileDialog(string defaultExt, string filters);
    }
}
