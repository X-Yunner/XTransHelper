using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using HookManager;

namespace XTranslation.Utils
{
    public class WinIterop
    {
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys modifierKeys, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);




        #region 鼠标穿透
        
        ///win32 api
        private const uint WS_EX_TRANSPARENT = 0x20;

        private const int GWL_EXSTYLE = -20;

        //设置窗口是否鼠标穿透
        public static uint SetWindowPenetrate(IntPtr hwnd, bool flag)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (flag)
                return SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            return SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
        }
        
        #endregion

        //设置窗口是否监听复制消息
        public static bool SetClipboardFormatListener(IntPtr hwnd, bool flag)
        {
            if (flag)
                return AddClipboardFormatListener(hwnd);
            return RemoveClipboardFormatListener(hwnd);
        }
        
        //热键消息
        const int WM_HOTKEY = 0x312;

        //窗口消息处理函数是否已添加
        private static bool WndProcIsAdd = false;
        
        //热键映射表 可以使用此实例查看已注册的快捷键
        public static Dictionary<int, Action<int>> HotKey_Map = new Dictionary<int, Action<int>>();
        
        public static bool RegisterHotKey(IntPtr hwnd, ModifierKeys modifierKeys, Key key, Action<int> action)
        {
            if (!RegisterHotKey(hwnd,(int)modifierKeys+(int)key , modifierKeys, (uint)KeyInterop.VirtualKeyFromKey(key)))
            {
                return false;
            }
            HotKey_Map.Add((int)modifierKeys+(int)key, action);
            if (!WndProcIsAdd)
            {
                WndProcIsAdd = true;
                HwndSource.FromHwnd(hwnd).AddHook(WndProc);
            }
            return true;
        }
        
        public static void UnregisterHotKey(IntPtr hwnd, ModifierKeys modifierKeys,Key key)
        {
            HotKey_Map.Remove((int)modifierKeys+(int)key);
            UnregisterHotKey(hwnd, (int)modifierKeys+(int)key);
            if (HotKey_Map.Count == 0)
            {
                WndProcIsAdd = false;
                HwndSource.FromHwnd(hwnd).RemoveHook(WndProc);
            }
        }

        static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                {
                    int id= wParam.ToInt32();
                    if (HotKey_Map.TryGetValue(id, out Action<int> action))
                    {
                        action(id);
                    }

                    break;
                }
            }

            return IntPtr.Zero;
        }
    }
}