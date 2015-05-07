using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace ScreenCaptureTool.Models.Hotkeys
{
    public class Hotkey
    {
        public event Action OnHotkeyPressed;

        private readonly int _id;
        private readonly ModifierKeys _modKeys;
        private readonly Keys _key;
        private readonly IntPtr _hWnd;
        private const uint ModNoRepeat = 0x4000;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int key);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Hotkey(ModifierKeys modKeys, Keys key, Window window)
        {
            _id = GetHashCode();
            _modKeys = modKeys;
            _key = key;
            _hWnd = new WindowInteropHelper(window).Handle;
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public int Id { get { return _id; } }

        public void Register()
        {
            RegisterHotKey(_hWnd, _id, (uint)_modKeys | ModNoRepeat, (int)_key);
        }

        public void Unregister()
        {
            UnregisterHotKey(_hWnd, _id);
        }

        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (!handled && msg.wParam.ToInt32() == _id)
            {
                HotKeyPressed();
            }
        }

        private void HotKeyPressed()
        {
            if (OnHotkeyPressed != null)
                OnHotkeyPressed();
        }

        public void Dispose()
        {
            Unregister();
            ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
        }
    }
}
