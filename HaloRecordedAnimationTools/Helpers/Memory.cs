using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class Memory // Probably could be non-static, but this works fine.
    {
        public enum Flags
        {
            PROCESS_WM_READ = 0x10,
            PROCESS_WM_WRITE = 0x20,
            PROCESS_WM_OPERATION = 0x08
        }

        public static Process Process { get; private set; }
        public static IntPtr ProcessHandle { get; private set; }
        private static int m_iBytesWritten;
        private static int m_iBytesRead;

        public static bool Attach(string procName)
        {
            var processes = Process.GetProcessesByName(procName);
            if (processes.Any())
            {
                Process = processes[0];
                ProcessHandle = OpenProcess((int)(Flags.PROCESS_WM_OPERATION | Flags.PROCESS_WM_READ | Flags.PROCESS_WM_WRITE), false, Process.Id);
                return true;
            }
            return false;
        }
        public static bool Detach()
        {
            if (Process != null && ProcessHandle != IntPtr.Zero - 1)
            {
                var HRESULT = CloseHandle(ProcessHandle);
                if (HRESULT)
                {
                    Process = null;
                    ProcessHandle = IntPtr.Zero - 1;
                }
                return HRESULT;
            }
            return true;
        }

        public static T ReadMemory<T>(int address) where T : struct
        {
            Type type = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
            int size = Marshal.SizeOf(type);
            byte[] buffer = new byte[size];

            ReadProcessMemory(ProcessHandle, (IntPtr)address, buffer, size, ref m_iBytesRead);
            return ByteArrayToStruct<T>(buffer);
        }
        public static bool WriteMemory<T>(int address, T value) where T : struct
        {
            byte[] buffer = StructToByteArray(value);
            return WriteProcessMemory(ProcessHandle, (IntPtr)address, ref buffer, buffer.Length, out m_iBytesWritten);
        }
        public static IntPtr GetModuleAddress(string moduleName)
        {
            try
            {
                foreach (ProcessModule module in Process.Modules)
                    if (moduleName == module.ModuleName)
                        return module.BaseAddress;
            }
            catch { }
            return IntPtr.Zero - 1;
        }

        private static T ByteArrayToStruct<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                Type type = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            }
            finally
            {
                handle.Free();
            }
        }
        private static byte[] StructToByteArray(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] arr = new byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);
    }
}
