namespace CaptainCapslock.Keyboard
{
    internal class ModifierToggleKeyHook(ModifierKeyState modifierKeyState) : IKeyHook
    {
        public KeyHookResult OnKeyDown()
        {
            modifierKeyState.IsActive = true;
            return KeyHookResult.SuppressKey;
        }

        public KeyHookResult OnKeyUp()
        {
            modifierKeyState.IsActive = false;
            return KeyHookResult.SuppressKey;
        }
    }
}