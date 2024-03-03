using CaptainCapslock.Remote;

namespace CaptainCapslock.Keyboard
{
    internal class ApplicationToggleKeyHook(ModifierKeyState modifierKeyState, RemoteApplication application) : IKeyHook
    {
        public KeyHookResult OnKeyDown()
        {
            if (!modifierKeyState.IsActive)
            {
                return KeyHookResult.DispatchKey;
            }

            application.Toggle();
            return KeyHookResult.SuppressKey;
        }

        public KeyHookResult OnKeyUp()
        {
            return KeyHookResult.DispatchKey;
        }
    }
}