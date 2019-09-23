using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class MiscExtensions
    {
        /// <summary>
        /// Returns the specified bit from a <see cref="byte"/>.
        /// </summary>
        /// <param name="b">The <see cref="byte"/> to check in.</param>
        /// <param name="bit">The bit to check.</param>
        /// <returns>A <see cref="bool"/> containing the bit.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static bool GetBit(this byte b, int bit)
        {
            if (bit < 0 || bit > 7)
                throw new ArgumentOutOfRangeException("bit", bit, "The specified bit was not within the valid range of a byte (0-7).");
            return (b & (1 << bit)) != 0;
        }
        
        /// <summary>
        /// Returns the specified bit from a <see cref="ushort"/>.
        /// </summary>
        /// <param name="us">The <see cref="ushort"/> to check in.</param>
        /// <param name="bit">The bit to check.</param>
        /// <returns>A <see cref="bool"/> containing the bit.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static bool GetBit(this ushort us, int bit)
        {
            if (bit < 0 || bit > 15)
                throw new ArgumentOutOfRangeException("us", us, "The specified bit was not within the valid range of a ushort (0-15).");
            return (us & (1 << bit)) != 0;
        }

        public static byte[] Reverse(this byte[] bytes)
        {
            Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// Returns a fallback string if the specified string is <see langword="null"/> or <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The given string, or the fallback if the string is <see langword="null"/> or <see cref="string.Empty"/>.</returns>
        public static string Fallback(this string str, string fallback = "<empty>")
        {
            if (string.IsNullOrWhiteSpace(str))
                return fallback;
            return str;
        }

        // https://stackoverflow.com/a/7162873/
        public static bool ApplicationHasFocus()
        {
            IntPtr activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
                return false;

            int procId = Process.GetCurrentProcess().Id;
            GetWindowThreadProcessId(activatedHandle, out int activeProcId);
            return procId == activeProcId;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
    }
}
