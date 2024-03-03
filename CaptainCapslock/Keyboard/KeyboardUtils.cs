using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace CaptainCapslock.Keyboard
{
    internal static class KeyboardUtils
    {
        /// <summary>
        /// If capslock is currently on, turn it off. If it's not current only, do nothing.
        /// </summary>
        /// <returns>
        /// True if capslock was on and was turned off, or false if capslock was already off.
        /// </returns>
        public static bool TurnOffCapslockIfOn()
        {
            if ((PInvoke.GetKeyState((int)VIRTUAL_KEY.VK_CAPITAL) & 1) != 1)
            {
                return false;
            }

            ToggleKey(VIRTUAL_KEY.VK_CAPITAL);
            return true;
        }

        /// <summary>
        /// Sends key down and key up for the given virtual key
        /// </summary>
        /// <param name="key">The virtual code of the key to toggle</param>
        private static void ToggleKey(VIRTUAL_KEY key)
        {
            Span<INPUT> keyDownInputs =
            [
                new INPUT
                {
                    type = INPUT_TYPE.INPUT_KEYBOARD,
                    Anonymous =
                    {
                        ki = { wVk = key }
                    }
                }
            ];
            PInvoke.SendInput(keyDownInputs, Marshal.SizeOf(typeof(INPUT)));

            Span<INPUT> keyUpInputs =
            [
                new INPUT
                {
                    type = INPUT_TYPE.INPUT_KEYBOARD,
                    Anonymous =
                    {
                        ki = { wVk = key, dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP }
                    }
                }
            ];
            PInvoke.SendInput(keyUpInputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}
