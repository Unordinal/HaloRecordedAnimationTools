using System;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class ByteConversions
    {
        /// <summary>
        /// Converts the specified byte array into the given type in little endian.
        /// </summary>
        /// <typeparam name="T">
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>May be one of the following types:</description><br/>
        ///         </listheader>
        ///         <item>
        ///             <description><see cref="sbyte"/></description>
        ///         </item>
        ///         <item>
        ///             <description><see cref="byte"/></description><br/>
        ///         </item>
        ///         <item>
        ///             <description><see cref="short"/></description>
        ///         </item>
        ///         <item>
        ///             <description><see cref="ushort"/></description><br/>
        ///         </item>
        ///         <item>
        ///             <description><see cref="int"/></description>
        ///         </item>
        ///         <item>
        ///             <description><see cref="uint"/></description><br/>
        ///         </item>
        ///         <item>
        ///             <description><see cref="long"/></description>
        ///         </item>
        ///         <item>
        ///             <description><see cref="ulong"/></description>
        ///         </item>
        ///     </list>
        /// </typeparam>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static T To<T>(this byte[] buffer, int pos = 0) where T : struct
        {
            var type = typeof(T);
            if (type == typeof(sbyte))
                return (T)(object)(sbyte)buffer[pos];
            if (type == typeof(byte))
                return (T)(object)buffer[pos];
            if (type == typeof(short))
                return (T)(object)buffer.ToInt16();
            if (type == typeof(ushort))
                return (T)(object)buffer.ToUInt16();
            if (type == typeof(int))
                return (T)(object)buffer.ToInt32();
            if (type == typeof(uint))
                return (T)(object)buffer.ToUInt32();
            if (type == typeof(long))
                return (T)(object)buffer.ToInt32();
            if (type == typeof(ulong))
                return (T)(object)buffer.ToInt32();
            throw new NotImplementedException($"{type} is not a valid type for .To<T>()");
        }

        public static short ToInt16(this byte[] buffer, int pos = 0) =>
            (short)
            ( buffer[pos + 1] << 8
            | buffer[pos]     << 0);
        public static ushort ToUInt16(this byte[] buffer, int pos = 0) =>
            (ushort)
            ( buffer[pos + 1] << 8
            | buffer[pos]     << 0);

        public static int ToInt32(this byte[] buffer, int pos = 0) =>
              buffer[pos + 3] << 24
            | buffer[pos + 2] << 16
            | buffer[pos + 1] << 8
            | buffer[pos] << 0;
        public static uint ToUInt32(this byte[] buffer, int pos = 0) =>
            (uint)
            ( buffer[pos + 3] << 24
            | buffer[pos + 2] << 16
            | buffer[pos + 1] << 8
            | buffer[pos] << 0);

        public static long ToInt64(this byte[] buffer, int pos = 0) =>
              buffer[pos + 7] << 56
            | buffer[pos + 6] << 48
            | buffer[pos + 5] << 40
            | buffer[pos + 4] << 32
            | buffer[pos + 3] << 24
            | buffer[pos + 2] << 16
            | buffer[pos + 1] << 8
            | buffer[pos]     << 0;
        public static ulong ToUInt64(this byte[] buffer, int pos = 0) =>
            (ulong)
            ( buffer[pos + 7] << 56
            | buffer[pos + 6] << 48
            | buffer[pos + 5] << 40
            | buffer[pos + 4] << 32
            | buffer[pos + 3] << 24
            | buffer[pos + 2] << 16
            | buffer[pos + 1] << 8
            | buffer[pos]     << 0);
    }
}
