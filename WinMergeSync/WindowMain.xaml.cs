using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WinMergeSync {
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window {
        public WindowMain() {
            InitializeComponent();
        }

        private IEnumerable<IntPtr> WinMergeWindows(bool child) {
            foreach (Process p in Process.GetProcesses()) {
                IntPtr hWnd = p.MainWindowHandle;
                if (hWnd == IntPtr.Zero) {
                    continue;
                }
                string className = Win32.GetClassNameWrapper(hWnd);
                if (className != "WinMergeWindowClassW") {
                    continue;
                }
                /*
                if (child) {
                    hWnd = Win32.FindWindowEx(hWnd, IntPtr.Zero, "MDIClient", null);
                    if (hWnd == IntPtr.Zero) {
                        continue;
                    }
                    hWnd = Win32.FindWindowEx(hWnd, IntPtr.Zero, "Afx:00400000:b:00010013:00000006:00020D41", null);
                    if (hWnd == IntPtr.Zero) {
                        continue;
                    }
                    hWnd = Win32.FindWindowEx(hWnd, IntPtr.Zero, "AfxMDIFrame100u", null);
                    if (hWnd == IntPtr.Zero) {
                        continue;
                    }
                }
                */
                yield return hWnd;
            }
        }

        private List<IntPtr> windowlist = new List<IntPtr>();
        private bool IsAlive() {
            if (this.windowlist.Count() == 0) {
                return false;
            }
            foreach (IntPtr hWnd in this.windowlist) {
                if (Win32.IsWindow(hWnd) == 0) {
                    return false;
                }
            }
            return true;
        }

        private void RescanWindows() {
            var ret = new List<IntPtr>();
            foreach (IntPtr hWnd in this.WinMergeWindows(false)) {
                ret.Add(hWnd);
            }
            this.windowlist = ret;
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e) {
            if (!this.IsAlive()) {
                this.RescanWindows();
            }
            foreach (IntPtr hWnd in this.windowlist) {
                Win32.SendMessage(hWnd, Win32.WM_COMMAND, new UIntPtr(0x18040), new UIntPtr(0));
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e) {
            if (!this.IsAlive()) {
                this.RescanWindows();
            }
            foreach (IntPtr hWnd in this.windowlist) {
                Win32.SendMessage(hWnd, Win32.WM_COMMAND, new UIntPtr(0x18041), new UIntPtr(0));
            }
        }

        private void ButtonReload_Click(object sender, RoutedEventArgs e) {
            this.RescanWindows();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
