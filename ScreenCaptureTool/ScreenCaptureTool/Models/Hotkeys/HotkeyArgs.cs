using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
namespace ScreenCaptureTool.Models.Hotkeys
{
    public class HotkeyArgs
    {
        public string Name { get; set; }
        public ModifierKeys ModKeys { get; set; }
        public Keys Key { get; set; }
        public Window Window { get; set; }
        public Action Action { get; set; }
    }
}
