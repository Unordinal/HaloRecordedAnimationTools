using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloRecordedAnimationTools.Blam
{
    public class AnimationDataOld
    {
        [Flags]
        public enum EventFlags : byte
        {
            OneTick = 1,
            ByteTime,
            ShortTime,
            End,
            MoveIndex = 8,
            Bool = 12,
            Stance = 16,
            Weapon = 20,
            Speed = 24,
            ByteRotate = 28,
            ShortRotate = 60,
            Feet = 4,
            Body = 8,
            Head = 16
        }

        [Flags]
        public enum MoveFlags : byte
        {
            LookOnly = 1,
            Walk,
            ExecuteInit = 4,
            Shoot = 8,
            Grenade = 48,
            ExecuteHold = 64
        }

        [Flags]
        public enum StanceFlags : byte
        {
            Crouch = 1,
            Jump,
            Melee = 16,
            ExecuteInit = 64,
            Flashlight = 128
        }

        

        public struct Event
        {
            public EventFlags type;
            public int time;
            public int param1;
            public int param2;
            public float x;
            public float y;

            public Event(Event data)
            {
                type = data.type;
                time = data.time;
                param1 = data.param1;
                param2 = data.param2;
                x = data.x;
                y = data.y;
            }
        }

        public struct Block
        {
            public string name;
            public byte version;
            public byte raw;
            public byte control;
            public byte[] pad0;
            public ushort length;
            public byte[] pad1;
            public byte[] data;
            public uint size;
            public byte[] pad2;

            public Block(string block, byte version, byte raw, byte control)
            {
                name = block;
                this.version = version;
                this.raw = raw;
                this.control = control;
                pad0 = new byte[1];
                length = 0;
                pad1 = new byte[6];
                size = 0u;
                pad2 = new byte[0];
                data = new byte[0];
            }

            public Block(BinaryReader r)
            {
                name = Encoding.ASCII.GetString(r.ReadBytes(32));
                version = r.ReadByte();
                raw = r.ReadByte();
                control = r.ReadByte();
                pad0 = r.ReadBytes(1);
                length = r.ReadUInt16();
                pad1 = r.ReadBytes(6);
                size = r.ReadUInt32();
                pad2 = r.ReadBytes(1520); // wrong
                data = r.ReadBytes(-1); // wrong
            }

            public static Block FromFile(BinaryReader r)
            {
                Block animBlock = new Block();
                _ = r.ReadBytes(24); // Start position and aim vector
                animBlock.data = r.ReadBytes((int)r.BaseStream.Length - 26);
                animBlock.size = (uint)r.BaseStream.Length - 26;
                animBlock.length = r.ReadUInt16();
                return animBlock;
            }

            public override string ToString()
            {
                return 
                    $"Name: {name}\n" +
                    $"Version: {version}\n" +
                    $"Raw: {raw}\n" +
                    $"Control: {control}\n" +
                    $"Length: {length}\n" +
                    $"Size: {size}\n" +
                    $"Data: {data}";
            }
        }
    }
}
