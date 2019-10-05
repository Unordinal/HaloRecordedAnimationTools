using System;
using System.Collections.Generic;
using System.IO;
using HaloRecordedAnimationTools.Helpers;
using HaloRecordedAnimationTools.IO;

namespace HaloRecordedAnimationTools.Blam
{
    public class Scenario
    {
        private const uint rawDataPtr = 0x5F0;
        private Reflexive recordedAnimations;
        private int recordedAnimationsSize;
        private int recordedAnimationsDataSize;
        private uint recordedAnimBlockPtr;
        public uint RecordedAnimBlockPtr
        {
            get => recordedAnimBlockPtr;
            private set
            {
                if (recordedAnimBlockPtr != value)
                    Console.WriteLine("\tPointer: " + recordedAnimBlockPtr + " => " + value);
                else
                    Console.WriteLine(value);
                recordedAnimBlockPtr = value;
            }
        }

        public readonly string scnrPath;
        public uint RecordedAnimDataPtr { get; private set; }

        public List<RecordedAnimation> RecordedAnimations { get; } = new List<RecordedAnimation>();
        public List<byte[]> RecordedAnimationsData { get; } = new List<byte[]>();

        public Scenario(string scnrPath, EndianReader r)
        {
            this.scnrPath = scnrPath;
            FindPointerToAnims(r);
            ReadRecordedAnimations(r);
        }

        /* Palette Reflexives and similar point to TagRefs, which have a path pointer and length - for each 
         * Reflexive (the Reflexive count) we use the added pointer value we have to jump to the location
         * of the start of that Reflexive raw data block, read the TagRef from it, read the path length 
         * (+1 as it's null-terminated) and then skip to the next TagRef and do the same until we've read 
         * to the total count of the reflexives. Then we go back to reading reflexives and do the above 
         * again if we encounter another Reflexive with a TagRef.
         */
        public void FindPointerToAnims(EndianReader r)
        {
            RecordedAnimBlockPtr = rawDataPtr;
            Reflexive currRefl;

            Console.WriteLine("Skies");
            r.BaseStream.Position = ReflexiveOffsets.Sky;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * Sky.SIZE) + r.ReadPaletteDataLength<Sky>(currRefl.count));

            Console.WriteLine("Child Scenarios");
            r.BaseStream.Position = ReflexiveOffsets.ChildScenario;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * ChildScenario.SIZE) + r.ReadPaletteDataLength<ChildScenario>(currRefl.count));

            Console.WriteLine("Predicted Resources");
            r.BaseStream.Position = ReflexiveOffsets.PredictedResource;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * PredictedResource.SIZE);

            Console.WriteLine("Functions");
            r.BaseStream.Position = ReflexiveOffsets.Function;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Function.SIZE);

            Console.WriteLine("Scenario Editor Data");
            r.BaseStream.Position = ReflexiveOffsets.ScenarioEditorData;
            RecordedAnimBlockPtr += (uint)(r.ReadRawdataRef().size);

            Console.WriteLine("Comments");
            r.BaseStream.Position = ReflexiveOffsets.Comment;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * Comment.SIZE) + r.ReadPaletteDataLength<Comment>(currRefl.count));

            Console.WriteLine("Object Names");
            r.BaseStream.Position = ReflexiveOffsets.ObjectName;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * ObjectName.SIZE);

            Console.WriteLine("Scenery");
            r.BaseStream.Position = ReflexiveOffsets.Scenery;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Scenery.SIZE);

            Console.WriteLine("Scenery Palette");
            r.BaseStream.Position = ReflexiveOffsets.SceneryPalette; 
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * SceneryPalette.SIZE) + r.ReadPaletteDataLength<SceneryPalette>(currRefl.count));

            Console.WriteLine("Bipeds");
            r.BaseStream.Position = ReflexiveOffsets.Biped;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Biped.SIZE);

            Console.WriteLine("Biped Palette");
            r.BaseStream.Position = ReflexiveOffsets.BipedPalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * BipedPalette.SIZE) + r.ReadPaletteDataLength<BipedPalette>(currRefl.count));

            Console.WriteLine("Vehicles");
            r.BaseStream.Position = ReflexiveOffsets.Vehicle;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Vehicle.SIZE);

            Console.WriteLine("Vehicle Palette");
            r.BaseStream.Position = ReflexiveOffsets.VehiclePalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * VehiclePalette.SIZE) + r.ReadPaletteDataLength<VehiclePalette>(currRefl.count));

            Console.WriteLine("Equipment");
            r.BaseStream.Position = ReflexiveOffsets.Equipment;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Equipment.SIZE);

            Console.WriteLine("Equipment Palette");
            r.BaseStream.Position = ReflexiveOffsets.EquipmentPalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * EquipmentPalette.SIZE) + r.ReadPaletteDataLength<EquipmentPalette>(currRefl.count));

            Console.WriteLine("Weapons");
            r.BaseStream.Position = ReflexiveOffsets.Weapon;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Weapon.SIZE);

            Console.WriteLine("Weapon Palette");
            r.BaseStream.Position = ReflexiveOffsets.WeaponPalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * WeaponPalette.SIZE) + r.ReadPaletteDataLength<WeaponPalette>(currRefl.count));

            Console.WriteLine("Device Groups");
            r.BaseStream.Position = ReflexiveOffsets.DeviceGroup;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * DeviceGroup.SIZE);

            Console.WriteLine("Machines");
            r.BaseStream.Position = ReflexiveOffsets.Machine;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Machine.SIZE);

            Console.WriteLine("Machine Palette");
            r.BaseStream.Position = ReflexiveOffsets.MachinePalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * MachinePalette.SIZE) + r.ReadPaletteDataLength<MachinePalette>(currRefl.count));

            Console.WriteLine("Controls");
            r.BaseStream.Position = ReflexiveOffsets.Control;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * Control.SIZE);

            Console.WriteLine("Control Palette");
            r.BaseStream.Position = ReflexiveOffsets.ControlPalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * ControlPalette.SIZE) + r.ReadPaletteDataLength<ControlPalette>(currRefl.count));

            Console.WriteLine("Light Fixtures");
            r.BaseStream.Position = ReflexiveOffsets.LightFixture;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * LightFixture.SIZE);

            Console.WriteLine("Light Fixture Palette");
            r.BaseStream.Position = ReflexiveOffsets.LightFixturePalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * LightFixturePalette.SIZE) + r.ReadPaletteDataLength<LightFixturePalette>(currRefl.count));

            Console.WriteLine("Sound Scenery");
            r.BaseStream.Position = ReflexiveOffsets.SoundScenery;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * SoundScenery.SIZE);

            Console.WriteLine("Sound Scenery Palette");
            r.BaseStream.Position = ReflexiveOffsets.SoundSceneryPalette;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * SoundSceneryPalette.SIZE) + r.ReadPaletteDataLength<SoundSceneryPalette>(currRefl.count));

            Console.WriteLine("Player Starting Profiles");
            r.BaseStream.Position = ReflexiveOffsets.PlayerStartingProfile;
            currRefl = r.ReadReflexive();
            r.BaseStream.Position = RecordedAnimBlockPtr;
            RecordedAnimBlockPtr += (uint)((currRefl.count * PlayerStartingProfile.SIZE) + r.ReadPaletteDataLength<PlayerStartingProfile>(currRefl.count));

            Console.WriteLine($"Player Starting Locations");
            r.BaseStream.Position = ReflexiveOffsets.PlayerStartingLocation;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * PlayerStartingLocation.SIZE);

            Console.WriteLine("Trigger Volumes");
            r.BaseStream.Position = ReflexiveOffsets.TriggerVolume;
            RecordedAnimBlockPtr += (uint)(r.ReadReflexive().count * TriggerVolume.SIZE);

            Console.WriteLine("Recorded Animations");
            r.BaseStream.Position = ReflexiveOffsets.RecordedAnimation;
            recordedAnimations = r.ReadReflexive();

            RecordedAnimDataPtr = RecordedAnimBlockPtr + (uint)(recordedAnimations.count * RecordedAnimation.SIZE);


            Console.WriteLine($"Recorded Animations Block Pointer: {RecordedAnimBlockPtr}");
        }

        public void ReadRecordedAnimations(EndianReader r)
        {
            //r.BaseStream.Position = 0x17DFF; // b30.scenario anim blocks ptr for testing
            r.BaseStream.Position = RecordedAnimBlockPtr;

            for (int i = 0; i < recordedAnimations.count; i++)
            {
                var anim = r.ReadRecordedAnimation();
                RecordedAnimations.Add(anim);
            }

            foreach (var anim in RecordedAnimations)
            {
                RecordedAnimationsData.Add(r.ReadBytes(anim.DataLength));
            }

            foreach (var anim in RecordedAnimations)
            {
                recordedAnimationsSize += anim.length;
                recordedAnimationsDataSize += anim.DataLength;
            }
        }

        public void WriteAnimations()
        {
            byte[] before;
            byte[] after;

            using (var r = new EndianReader(new FileStream(scnrPath, FileMode.Open, FileAccess.Read), Endian.Big))
            {
                before = r.ReadBytes((int)RecordedAnimBlockPtr);

                int animsSize = recordedAnimationsSize + recordedAnimationsDataSize;
                r.BaseStream.Position = RecordedAnimBlockPtr + animsSize;

                after = r.ReadBytes((int)(r.BaseStream.Length - (RecordedAnimBlockPtr + animsSize)));
            }

            using (var w = new EndianWriter(new FileStream(scnrPath, FileMode.Create, FileAccess.ReadWrite), Endian.Big))
            {
                w.Write(before);

                foreach (var anim in RecordedAnimations)
                    w.Write(anim);

                foreach (var animData in RecordedAnimationsData) // Writes each animation's event stream data.
                    w.Write(animData);

                w.Write(after);
            }
        }

        public struct ReflexiveOffsets
        {
            public const long
                Sky = 112,
                ChildScenario = 128,
                PredictedResource = 300,
                Function = 312,
                ScenarioEditorData = 324, // technically a RawdataRef not a Reflexive but y'know
                Comment = 344,
                ObjectName = 580,
                Scenery = 592,
                SceneryPalette = 604,
                Biped = 616,
                BipedPalette = 628,
                Vehicle = 640,
                VehiclePalette = 652,
                Equipment = 664,
                EquipmentPalette = 676,
                Weapon = 688,
                WeaponPalette = 700,
                DeviceGroup = 712,
                Machine = 724,
                MachinePalette = 736,
                Control = 748,
                ControlPalette = 760,
                LightFixture = 772,
                LightFixturePalette = 784,
                SoundScenery = 796,
                SoundSceneryPalette = 808,
                PlayerStartingProfile = 904,
                PlayerStartingLocation = 916,
                TriggerVolume = 928,
                RecordedAnimation = 940;
        }
    }
}
