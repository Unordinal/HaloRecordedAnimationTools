using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace HaloRecordedAnimationTools.Helpers
{
    /// <summary>
    /// Provides methods for checking a <see cref="Key"/>'s state.
    /// </summary>
    public static class Keybinds
    {
        [Flags]
        public enum Flags
        {
            KeyDown = 0x8000,           // Key down: (Now: Yes, Last: No )
            KeyUp = 0x0001,           // Key down: (Now: No,  Last: Yes)
            KeyHeld = KeyDown | KeyUp   // Key down: (Now: Yes, Last: Yes)
        }

        public static bool IsKeyDown(Key key) =>
            (GetAsyncKeyState(KeyInterop.VirtualKeyFromKey(key)) & (int)Flags.KeyDown) != 0;

        public static bool IsKeyHeld(Key key) =>
            (GetAsyncKeyState(KeyInterop.VirtualKeyFromKey(key)) & (int)Flags.KeyHeld) != 0;

        public static bool KeyUp(Key key) =>
            (GetAsyncKeyState(KeyInterop.VirtualKeyFromKey(key)) & (int)Flags.KeyUp) != 0;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
    }
}
