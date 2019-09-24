using System;
using System.IO;
using System.Linq;
using System.Text;
using HaloRecordedAnimationTools.Helpers;

namespace HaloRecordedAnimationTools.IO
{
    public class EndianWriter : BinaryWriter
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
        public EndianWriter(Stream input, Endian endianness, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { Endianness = endianness; }
        public EndianWriter(Stream input, Endian endianness, Encoding encoding) : base(input, encoding) { Endianness = endianness; }
        public EndianWriter(Stream input, Endian endianness) : base(input) { Endianness = endianness; }
        public EndianWriter(Endian endianness) : base() { Endianness = endianness; }
        public EndianWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }
        public EndianWriter(Stream input, Encoding encoding) : base(input, encoding) { }
        public EndianWriter(Stream input) : base(input) { }
        public EndianWriter() : base() { }
        #endregion

        #region Write Values
        public override void Write(short value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToInt16());
        }
        public override void Write(ushort value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToUInt16());
        }

        public override void Write(int value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToInt32());
        }
        public override void Write(uint value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToUInt32());
        }

        public override void Write(long value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToInt64());
        }
        public override void Write(ulong value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.GetBytes(value).Reverse().ToUInt64());
        }

        public override void Write(float value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.ToSingle(BitConverter.GetBytes(value).Reverse(), 0));
        }
        public override void Write(double value)
        {
            if (EndiansMatch())
                base.Write(value);
            base.Write(BitConverter.ToDouble(BitConverter.GetBytes(value).Reverse(), 0));
        }
        #endregion
    }
}
