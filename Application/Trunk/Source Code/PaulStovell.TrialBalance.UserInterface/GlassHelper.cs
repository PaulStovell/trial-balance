using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;
using PaulStovell.Common;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// A helper class that extends the "Aero Glass" effect onto the client area of a window.
    /// </summary>
    /// <remarks>
    /// Thanks to Adam Nathan for this code sample: http://blogs.msdn.com/adam_nathan/archive/2006/05/04/589686.aspx.
    /// </remarks>
    public class GlassHelper {
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();

        private struct MARGINS {
            public MARGINS(Thickness t) {
                Left = (int)t.Left;
                Right = (int)t.Right;
                Top = (int)t.Top;
                Bottom = (int)t.Bottom;
            }
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        /// <summary>
        /// Extends the Aero Glass area on a given window.
        /// </summary>
        /// <param name="window">The window to extend glass onto.</param>
        /// <param name="margin">The distances from the border to extend the glass by.</param>
        /// <returns>True if glass was extended successfully, otherwise false.</returns>
        public static bool ExtendGlassFrame(Window window, Thickness margin) {
            if (window == null) {
                throw new ArgumentNullException("window");
            }

            bool result = false;
            try {
                if (DwmIsCompositionEnabled()) {

                    IntPtr hwnd = new WindowInteropHelper(window).Handle;
                    if (hwnd == IntPtr.Zero)
                        throw new InvalidOperationException("The Window must be shown before extending glass.");

                    // Set the background to transparent from both the WPF and Win32 perspectives
                    window.Background = Brushes.Transparent;
                    HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                    MARGINS margins = new MARGINS(margin);
                    DwmExtendFrameIntoClientArea(hwnd, ref margins);
                    result = true;
                }
            } catch { }
            return result;
        }
    }
}
