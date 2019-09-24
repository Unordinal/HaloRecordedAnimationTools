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
        public override short ReadInt16()
        {
            if (EndiansMatch())
                return base.ReadInt16();
            return ReadBytesRequired(sizeof(short)).Reverse().ToInt16();
        }
        public override ushort ReadUInt16()
        {
            if (EndiansMatch())
                return base.ReadUInt16();
            return ReadBytesRequired(sizeof(ushort)).Reverse().ToUInt16();
        }

        public override int ReadInt32()
        {
            if (EndiansMatch())
                return base.ReadInt32();
            return ReadBytesRequired(sizeof(int)).Reverse().ToInt32();
        }
        public override uint ReadUInt32()
        {
            if (EndiansMatch())
                return base.ReadUInt32();
            return ReadBytesRequired(sizeof(uint)).Reverse().ToUInt32();
        }

        public override long ReadInt64()
        {
            if (EndiansMatch())
                return base.ReadInt64();
            return ReadBytesRequired(sizeof(long)).Reverse().ToInt64();
        }
        public override ulong ReadUInt64()
        {
            if (EndiansMatch())
                return base.ReadUInt64();
            return ReadBytesRequired(sizeof(ulong)).Reverse().ToUInt64();
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
