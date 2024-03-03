using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace CaptainCapslock.Keyboard
{
    internal class ModifierKeyState
    {
        public bool IsActive { get; set; }

        public static ModifierKeyState FromCurrentCapslockKeyState()
        {
            // Initialize the state of the modifier key based on the current capslock state.
            // This will usually be initialized as inactive but there's a chance it could be
            // active if this method was called while capslock was already being held down.
            return new ModifierKeyState
            {
                IsActive = PInvoke.GetAsyncKeyState((int)VIRTUAL_KEY.VK_CAPITAL) < 0
            };
        }
    }
}