using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinMergeSync {
    public static class Win32 {
        //---------------------------------------------------------------
        // EnumWindows
        //---------------------------------------------------------------
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

        delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        static bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam) {
            return Win32.enumWindowsCallback(hWnd);
        }

        static Func<IntPtr, bool> enumWindowsCallback;

        public static bool EnumWindowsEach(Func<IntPtr, bool> enumWindowsCallback) {
            Win32.enumWindowsCallback = enumWindowsCallback;
            return Win32.EnumWindows(EnumWindowsProc, IntPtr.Zero);
        }

        //---------------------------------------------------------------
        // GetClassName
        //---------------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static string GetClassNameWrapper(IntPtr hWnd) {
            StringBuilder className = new StringBuilder(1000);
            int nRet = Win32.GetClassName(hWnd, className, className.Capacity);
            if (nRet != 0) {
                return className.ToString();
            }
            else {
                return null;
            }
        }

        //---------------------------------------------------------------
        // FindWindowEx
        //---------------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        //---------------------------------------------------------------
        // IsWindow
        //---------------------------------------------------------------
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int IsWindow(IntPtr hWnd);

        //---------------------------------------------------------------
        // SendMessage / PostMessage
        //---------------------------------------------------------------
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, UIntPtr lParam);

        public static uint WM_KEYDOWN = 0x0100;
        public static uint WM_KEYUP = 0x0101;
        public static uint WM_CHAR = 0x0102;
        public static uint WM_SYSKEYDOWN = 0x0104;
        public static uint WM_SYSKEYUP = 0x0105;
        public static uint WM_COMMAND = 0x0111;
        public static uint WM_MOUSEWHEEL = 0x020A;

        public static uint VK_MENU = 0x12;
        public static uint VK_DOWN = 0x28;
    }
}
