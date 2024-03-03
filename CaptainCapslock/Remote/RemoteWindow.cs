using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;

namespace CaptainCapslock.Remote
{
    internal class RemoteWindow(HWND hwnd)
    {
        public static IEnumerable<RemoteWindow> EnumerateMainWindows()
        {
            return EnumerateAllWindows().Where(window => window.IsMain);
        }

        private static IEnumerable<RemoteWindow> EnumerateAllWindows()
        {
            var windows = new List<RemoteWindow>();
            PInvoke.EnumWindows((hwnd, param) =>
            {
                windows.Add(new RemoteWindow(hwnd));
                return true;
            }, 0);

            return windows;
        }

        // A main window is defined as a visible window without an owner
        public bool IsMain => PInvoke.GetWindow(hwnd, GET_WINDOW_CMD.GW_OWNER).IsNull && PInvoke.IsWindowVisible(hwnd);

        // Check if the window is focused. Note that when we minimize a window programmatically,
        // it will still be the foreground window, so also check it's not minimized.
        public bool IsFocused => PInvoke.GetForegroundWindow() == hwnd && !PInvoke.IsIconic(hwnd);

        public unsafe uint GetProcessId()
        {
            uint processId;
            PInvoke.GetWindowThreadProcessId(hwnd, &processId);
            return processId;
        }

        public void Focus()
        {
            // Restore minimized window if minimized
            if (PInvoke.IsIconic(hwnd))
            {
                PInvoke.ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_RESTORE);
            }

            // Send a dummy input event first so this process is allowed to SetForegroundWindow
            Span<INPUT> inputs = [new INPUT { type = INPUT_TYPE.INPUT_MOUSE }];
            PInvoke.SendInput(inputs, Marshal.SizeOf(typeof(INPUT)));

            PInvoke.SetForegroundWindow(hwnd);
        }

        public void Unfocus()
        {
            PInvoke.ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_MINIMIZE);

        }
    }
}