using System;
using System.IO;
using System.Linq;
using System.Text;
using HaloRecordedAnimationTools.Helpers;

namespace HaloRecordedAnimationTools.IO
{
    public class EndianReader : BinaryReader
    {
        #region Properties
        /// <summary>
        /// Defines endianness constants.
        /// </summary>
        public enum Endian { Little, Big }
        /// <summary>
        /// The underlying stream's endianness.
        /// </summary>
        public Endian Endianness { get; set; } = Endian.Little;
        /// <summary>
        /// This machine's endianness. Returns an <see cref="Endian"/> value based on <see cref="BitConverter.IsLittleEndian"/>.
        /// </summary>
        public Endian HostEndianness { get; } = BitConverter.IsLittleEndian ? Endian.Little : Endian.Big;
        /// <summary>
        /// Returns <see langword="true"/> if the endianness of the host matches the reader.
        /// </summary>
        private bool EndiansMatch() => Endianness == HostEndianness;
        #endregion

        #region Constructors
        public EndianReader(Stream input, Endian endianness, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { Endianness = endianness; }
        public EndianReader(Stream input, Endian endianness, Encoding encoding) : base(input, encoding) { Endianness = endianness; }
        public EndianReader(Stream input, Endian endianness) : base(input) { Endianness = endianness; }
        public EndianReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }
        public EndianReader(Stream input, Encoding encoding) : base(input, encoding) { }
        public EndianReader(Stream input) : base(input) { }
        #endregion

        #region Read Values
        public override ushort ReadUInt16()
        {
            if (EndiansMatch())
                return base.ReadUInt16();
            return BitConverter.ToUInt16(ReadBytesRequired(sizeof(ushort)).Reverse(), 0);
        }
        public override short ReadInt16()
        {
            if (EndiansMatch())
                return base.ReadInt16();
            return BitConverter.ToInt16(ReadBytesRequired(sizeof(short)).Reverse(), 0);
        }

        public override uint ReadUInt32()
        {
            if (EndiansMatch())
                return base.ReadUInt32();
            return BitConverter.ToUInt32(ReadBytesRequired(sizeof(uint)).Reverse(), 0);
        }
        public override int ReadInt32()
        {
            if (EndiansMatch())
                return base.ReadInt32();
            return BitConverter.ToInt32(ReadBytesRequired(sizeof(int)).Reverse(), 0);
        }

        public override ulong ReadUInt64()
        {
            if (EndiansMatch())
                return base.ReadUInt64();
            return BitConverter.ToUInt64(ReadBytesRequired(sizeof(ulong)).Reverse(), 0);
        }
        public override long ReadInt64()
        {
            if (EndiansMatch())
                return base.ReadInt64();
            return BitConverter.ToInt64(ReadBytesRequired(sizeof(long)).Reverse(), 0);
        }

        public override float ReadSingle()
        {
            if (EndiansMatch())
                return base.ReadSingle();
            return BitConverter.ToSingle(ReadBytesRequired(sizeof(float)).Reverse(), 0);
        }
        public override double ReadDouble()
        {
            if (EndiansMatch())
                return base.ReadDouble();
            return BitConverter.ToDouble(ReadBytesRequired(sizeof(double)).Reverse(), 0);
        }

        protected byte[] ReadBytesRequired(int byteCount)
        {
            var result = base.ReadBytes(byteCount);
            var byteDiff = byteCount - result.Length;
            if (byteDiff != 0)
                throw new EndOfStreamException($"End of stream reached with {byteDiff} byte{(byteDiff > 1 ? "s" : "")} left to read.");
            return result;
        }
        #endregion
    }
}
