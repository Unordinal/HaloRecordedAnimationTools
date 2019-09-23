using System.Runtime.InteropServices;
using HaloRecordedAnimationTools.IO;

namespace HaloRecordedAnimationTools.Blam
{
    public struct Vector2
    {
        public readonly float x;
        public readonly float y;
        public Vector2 Zero => new Vector2(0f);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(float f) : this(f, f) { }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector3
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;
        public Vector3 Zero => new Vector3(0f);

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3(float f) : this(f, f, f) { }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }

    public struct RawdataRef
    {
        public enum External { No, Yes }

        public int size;
        public External external; // 0x1 - external (bitmaps.map, sounds.map, etc)
        public uint rawPointer;
        public uint pointer;
        public uint id;

        public RawdataRef(EndianReader r)
        {
            size = r.ReadInt32();
            external = (External)r.ReadInt32();
            rawPointer = r.ReadUInt32();
            pointer = r.ReadUInt32();
            id = r.ReadUInt32();
        }
    }
    public struct Reflexive
    {
        public int count;
        public uint pointer;
        public uint id;

        public Reflexive(EndianReader r)
        {
            count = r.ReadInt32();
            pointer = r.ReadUInt32();
            id = r.ReadUInt32();
        }

        public override string ToString() =>
            $"{count}, {pointer}, {id}";
    }

    /// <summary>
    /// A dependency on or reference to a tag.
    /// </summary>
    public struct TagRef
    {
        public const uint SIZE = 16;
        public TagClass tagClass;
        public int pathPointer;
        public int pathLength;
        public uint id;

        public TagRef(EndianReader r)
        {
            tagClass = (TagClass)r.ReadUInt32();
            pathPointer = r.ReadInt32();
            pathLength = r.ReadInt32();
            id = r.ReadUInt32();
        }

        public override string ToString() =>
            $"{tagClass}, {pathPointer}, {pathLength}, {id}";
    }

    public enum TagClass : uint
    {
        none = 0xFFFFFFFF,
        sbsp = 0x73627370
    }

    /*public struct TagClass
    {
        public static TagClass actr => new TagClass("actr", "actor");
        public static TagClass actv => new TagClass("actv", "actor_variant");

        public string Name { get; private set; }
        public string FullName { get; private set; }
        private TagClass(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }
    }*/
}
