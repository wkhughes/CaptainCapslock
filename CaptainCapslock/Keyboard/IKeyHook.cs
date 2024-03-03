namespace CaptainCapslock.Keyboard
{
    internal enum KeyHookResult
    {
        DispatchKey,
        SuppressKey,
    }

    internal interface IKeyHook
    {
        public KeyHookResult OnKeyDown();
        public KeyHookResult OnKeyUp();
    }
}