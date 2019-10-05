using System;
using HaloRecordedAnimationTools.Blam;
using HaloRecordedAnimationTools.IO;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class EndianReaderExtensions
    {
        public static Vector3 ReadVector3(this EndianReader r)
        {
            return new Vector3(
                r.ReadSingle(),
                r.ReadSingle(),
                r.ReadSingle()
                );
        }

        public static RawdataRef ReadRawdataRef(this EndianReader r) => 
            new RawdataRef(r);

        public static Reflexive ReadReflexive(this EndianReader r) =>
            new Reflexive(r);

        public static RecordedAnimation ReadRecordedAnimation(this EndianReader r) =>
            new RecordedAnimation(r);

        public static TagRef ReadTagRef(this EndianReader r) =>
            new TagRef(r);

        public static int ReadPaletteDataLength<T>(this EndianReader r, int blockCount)
        {
            Console.WriteLine($"\tBlock Count: {blockCount}");
            int dataLength = 0;
            for (int i = 0; i < blockCount; i++)
            {
                IDataRefHolder palette = (IDataRefHolder)Activator.CreateInstance(typeof(T), r);
                dataLength += palette?.DataLength ?? 0;
                Console.WriteLine($"\t[{i}]: Size: {palette?.DataLength}");
            }
            Console.WriteLine($"\tTotal Block Size: {dataLength}");
            return dataLength;
        }

        /*public static int ReadDataLength<T>(this EndianReader r, int blockCount, uint refSize)
        {
            int dataLength = 0;
            T te = (T)Activator.CreateInstance(typeof(T), r);
            for (int i = 0; i < blockCount; i++)
            {
                TagRef tagRef = r.ReadTagRef();
                Console.WriteLine("PL: " + tagRef.pathLength);
                if (tagRef.pathLength != 0)
                    dataLength += tagRef.pathLength + 1;
                r.BaseStream.Position += refSize - TagRef.SIZE;
            }
            return dataLength;
        }*/
    }
}
