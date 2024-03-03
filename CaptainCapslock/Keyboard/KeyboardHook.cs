using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace CaptainCapslock.Keyboard
{
    internal class KeyboardHook()
    {
        private static IDictionary<uint, IKeyHook>? keyHooks;

        public static void Install(IDictionary<uint, IKeyHook> keyHooks)
        {
            KeyboardHook.keyHooks = keyHooks;
            PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_KEYBOARD_LL, HookProc, null, 0);
        }

        public static unsafe LRESULT HookProc(int code, WPARAM wparam, LPARAM lparam)
        {
            Debug.Assert(keyHooks != null, "Key hooks should be initialized when installing keyboard hook");

            if (code != 0)
            {
                // Must continue as a previous hook has disallowed further hook processing
                PInvoke.CallNextHookEx(null, code, wparam, lparam);
            }

            var keyData = (KBDLLHOOKSTRUCT*)lparam.Value;
            if (keyHooks.TryGetValue(keyData->vkCode, out IKeyHook? keyHook))
            {
                if (GetKeyHookResult(keyHook, wparam) == KeyHookResult.SuppressKey)
                {
                    // Key hook want this key event supressed, returning 1 does so
                    return new LRESULT(1);
                }
            }

            // The key event should be dispatched, continue to the next hook
            return PInvoke.CallNextHookEx(null, code, wparam, lparam);
        }

        private static KeyHookResult GetKeyHookResult(IKeyHook keyHook, WPARAM wparam)
        {
            if (wparam.Value == PInvoke.WM_KEYDOWN || wparam.Value == PInvoke.WM_SYSKEYDOWN)
            {
                return keyHook.OnKeyDown();
            }
            else
            {
                Debug.Assert(wparam.Value == PInvoke.WM_KEYUP || wparam.Value == PInvoke.WM_SYSKEYUP, $"Unknown key hook message id {wparam.Value}");
                return keyHook.OnKeyUp();
            }
        }
    }
}