using HaloRecordedAnimationTools.Blam;
using HaloRecordedAnimationTools.IO;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class EndianWriterExtensions
    {
        public static void Write(this EndianWriter w, RecordedAnimation anim)
        {
            w.Write(anim.name, 0, anim.name.Length);
            w.Write((sbyte)anim.version);
            w.Write(anim.raw);
            w.Write(anim.control);
            w.Write(new byte[1]);
            w.Write(anim.length);
            w.Write(new byte[6]);
            w.Write(anim.eventStream.size);
        }

        public static void Write(this EndianWriter w, RawdataRef dataRef)
        {
            w.Write(dataRef.size);
            w.Write((int)dataRef.external);
            w.Write(dataRef.rawPointer);
            w.Write(dataRef.pointer);
            w.Write(dataRef.id);
        }
    }
}
