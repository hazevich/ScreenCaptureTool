using ScreenCaptureTool.Models.Hotkeys;
using System.Collections.Generic;

namespace ScreenCaptureTool.DataAccess
{
    public class HotkeysFactory : SingletonBase<HotkeysFactory>
    {
        private Dictionary<string, Hotkey> _hotkeys = new Dictionary<string, Hotkey>();

        public void RegisterHotKey(HotkeyArgs hotkeyInitModel)
        {
            var hotKey = new Hotkey(hotkeyInitModel.ModKeys, hotkeyInitModel.Key, hotkeyInitModel.Window);
            hotKey.OnHotkeyPressed += hotkeyInitModel.Action;
            hotKey.Register();
            _hotkeys.Add(hotkeyInitModel.Name, hotKey);
        }

        public void UnregisterHotKey(string name)
        {
            _hotkeys[name].Dispose();
            _hotkeys.Remove(name);
        }

        public void UnregisterAllHotKeys()
        {
            foreach (KeyValuePair<string, Hotkey> hk in _hotkeys)
                hk.Value.Dispose();

            _hotkeys.Clear();
        }
    }
}
