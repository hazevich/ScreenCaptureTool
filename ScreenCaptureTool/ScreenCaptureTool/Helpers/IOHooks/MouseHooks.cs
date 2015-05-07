using ScreenCaptureTool.DataAccess;
using ScreenCaptureTool.Enums;
using ScreenCaptureTool.Helpers.WinApi;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace ScreenCaptureTool.Helpers.IOHooks
{
    public class MouseHooks : SingletonBase<MouseHooks>
    {
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;

        [StructLayout(LayoutKind.Sequential)]
        public struct Msllhookstruct
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator POINT(Point p)
            {
                return new POINT((int)p.X, (int)p.Y);
            }
        }

        const int WmLButtonDown = 0x201;
        const int WmLButtonUp = 0x202;
        const int WmMouseMove = 0x0200;
        const int WmMouseWheel = 0x020A;
        const int WmRButtonDown = 0x0204;
        const int WmRButtonUp = 0x0205;
        const int WmMButtonUp = 0x208;
        const int WmMButtonDown = 0x207;

        private IntPtr _hHook = IntPtr.Zero;
        private IntPtr _hIntance = IntPtr.Zero;
        private User32.HookProc _hookProc;

        public bool IsInstalled { get; set; }

        public void InstallHook()
        {
            if (IsInstalled)
            {
                return;
            }

            _hIntance = Marshal.GetHINSTANCE(AppDomain.CurrentDomain.GetAssemblies()[0].GetModules()[0]);
            _hookProc = HookProcessMsg;

            _hHook = User32.SetWindowsHookEx(HookType.WhMouseLL, _hookProc, _hIntance, 0);

            if (_hHook != null)
            {
                IsInstalled = true;
            }
        }

        public void UnInstallHook()
        {
            if (!IsInstalled)
            {
                return;
            }

            User32.UnhookWindowsHookEx(_hHook);

            _hHook = IntPtr.Zero;
            _hIntance = IntPtr.Zero;
            _hookProc = null;
            IsInstalled = false;
        }

        public IntPtr HookProcessMsg(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                var mhs = (Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct));

                switch (wParam.ToInt32())
                {
                    case WmLButtonDown:
                        if (MouseDown != null)
                        {
                            MouseDown(null, new MouseEventArgs(MouseButtons.Left, 1, mhs.pt.X, mhs.pt.Y, 0));
                        }
                        break;
                    case WmMouseMove:
                        if (MouseMove != null)
                        {
                            MouseMove(null, new MouseEventArgs(MouseButtons.None, 1, mhs.pt.X, mhs.pt.Y, 0));
                        }
                        break;
                }
            }

            return User32.CallNextHookEx(_hHook, nCode, wParam, lParam);
        }
    }
}
