using System.Linq;
using System.Text;
using HaloRecordedAnimationTools.Helpers;
using HaloRecordedAnimationTools.IO;

namespace HaloRecordedAnimationTools.Blam
{
    public interface IDataRefHolder
    {
        int DataLength { get; }
    }

    public struct Sky : IDataRefHolder
    {
        public const uint SIZE = 16;
        public TagRef sky;
        public Sky(EndianReader r)
        {
            sky = r.ReadTagRef();
        }

        public int DataLength => (sky.pathLength > 0) ? (sky.pathLength + 1) : 0;
    }

    public struct ChildScenario : IDataRefHolder
    {
        public const uint SIZE = 32;
        public TagRef childScenario;
        public ChildScenario(EndianReader r)
        {
            childScenario = r.ReadTagRef();
        }

        public int DataLength => (childScenario.pathLength > 0) ? (childScenario.pathLength + 1) : 0;
    }

    public struct PredictedResource
    {
        public const uint SIZE = 8;
    }

    public struct Function
    {
        public const uint SIZE = 120;
    }

    public struct ScenarioEditorData
    {
        public const uint SIZE = 20;
    }

    public struct Comment : IDataRefHolder
    {
        public const uint SIZE = 48;
        public Vector3 position;
        public byte[] pad0;
        public RawdataRef commentData;
        public Comment(EndianReader r)
        {
            position = r.ReadVector3();
            pad0 = r.ReadBytes(16);
            commentData = r.ReadRawdataRef();
        }

        public int DataLength => commentData.size;
    }

    public struct ObjectName
    {
        public const uint SIZE = 36;
    }

    public struct Scenery
    {
        public const uint SIZE = 72;
    }

    public struct SceneryPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef sceneryPalette;
        public byte[] pad0;
        public SceneryPalette(EndianReader r)
        {
            sceneryPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (sceneryPalette.pathLength > 0) ? (sceneryPalette.pathLength + 1) : 0;
    }

    public struct Biped
    {
        public const uint SIZE = 120;
    }

    public struct BipedPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef bipedPalette;
        public byte[] pad0;
        public BipedPalette(EndianReader r)
        {
            bipedPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (bipedPalette.pathLength > 0) ? (bipedPalette.pathLength + 1) : 0;
    }

    public struct Vehicle
    {
        public const uint SIZE = 120;
    }

    public struct VehiclePalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef vehiclePalette;
        public byte[] pad0;
        public VehiclePalette(EndianReader r)
        {
            vehiclePalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (vehiclePalette.pathLength > 0) ? (vehiclePalette.pathLength + 1) : 0;
    }

    public struct Equipment
    {
        public const uint SIZE = 40;
    }

    public struct EquipmentPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef equipmentPalette;
        public byte[] pad0;
        public EquipmentPalette(EndianReader r)
        {
            equipmentPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (equipmentPalette.pathLength > 0) ? (equipmentPalette.pathLength + 1) : 0;
    }

    public struct Weapon
    {
        public const uint SIZE = 92;
    }

    public struct WeaponPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef weaponPalette;
        public byte[] pad0;
        public WeaponPalette(EndianReader r)
        {
            weaponPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (weaponPalette.pathLength > 0) ? (weaponPalette.pathLength + 1) : 0;
    }

    public struct DeviceGroup
    {
        public const uint SIZE = 52;
    }

    public struct Machine
    {
        public const uint SIZE = 64;
    }

    public struct MachinePalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef machinePalette;
        public byte[] pad0;
        public MachinePalette(EndianReader r)
        {
            machinePalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (machinePalette.pathLength > 0) ? (machinePalette.pathLength + 1) : 0;
    }

    public struct Control
    {
        public const uint SIZE = 64;
    }

    public struct ControlPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef controlPalette;
        public byte[] pad0;
        public ControlPalette(EndianReader r)
        {
            controlPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (controlPalette.pathLength > 0) ? (controlPalette.pathLength + 1) : 0;
    }

    public struct LightFixture
    {
        public const uint SIZE = 88;
    }

    public struct LightFixturePalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef lightFixturePalette;
        public byte[] pad0;
        public LightFixturePalette(EndianReader r)
        {
            lightFixturePalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (lightFixturePalette.pathLength > 0) ? (lightFixturePalette.pathLength + 1) : 0;
    }

    public struct SoundScenery
    {
        public const uint SIZE = 40;
    }

    public struct SoundSceneryPalette : IDataRefHolder
    {
        public const uint SIZE = 48;
        public TagRef soundSceneryPalette;
        public byte[] pad0;
        public SoundSceneryPalette(EndianReader r)
        {
            soundSceneryPalette = r.ReadTagRef();
            pad0 = r.ReadBytes(32);
        }

        public int DataLength => (soundSceneryPalette.pathLength > 0) ? (soundSceneryPalette.pathLength + 1) : 0;
    }

    public struct PlayerStartingProfile : IDataRefHolder
    {
        public const uint SIZE = 104;

        public byte[] name;
        public float startingHealthModifier;
        public float startingShieldModifier;
        public TagRef primaryWeapon;
        public short primaryRoundsLoaded;
        public short primaryRoundsTotal;
        public TagRef secondaryWeapon;
        public short secondaryRoundsLoaded;
        public short secondaryRoundsTotal;
        public byte startingFragGrenadeCount;
        public byte startingPlasmaGrenadeCount;
        public byte[] pad0;
        public string Name => HaloConstants.Latin1.GetString(name);
        public PlayerStartingProfile(EndianReader r)
        {
            name = r.ReadBytes(32);
            startingHealthModifier = r.ReadSingle();
            startingShieldModifier = r.ReadSingle();
            primaryWeapon = r.ReadTagRef();
            primaryRoundsLoaded = r.ReadInt16();
            primaryRoundsTotal = r.ReadInt16();
            secondaryWeapon = r.ReadTagRef();
            secondaryRoundsLoaded = r.ReadInt16();
            secondaryRoundsTotal = r.ReadInt16();
            startingFragGrenadeCount = r.ReadByte();
            startingPlasmaGrenadeCount = r.ReadByte();
            pad0 = r.ReadBytes(22);
        }
        private int PrimaryPathLength => (primaryWeapon.pathLength > 0) ? (primaryWeapon.pathLength + 1) : 0;
        private int SecondaryPathLength => (secondaryWeapon.pathLength > 0) ? (secondaryWeapon.pathLength + 1) : 0;
        public int DataLength => PrimaryPathLength + SecondaryPathLength;
    }

    public struct PlayerStartingLocation
    {
        public const uint SIZE = 52;

        public Vector3 position;
        public float facing;
        public short teamIndex;
        public short bspIndex;
        public short type0;
        public short type1;
        public short type2;
        public short type3;
    }

    public struct TriggerVolume
    {
        public const uint SIZE = 96;
    }

    public struct RecordedAnimation : IDataRefHolder
    {
        public const uint SIZE = 64;
        public byte[] name;
        public Version version;
        public sbyte raw;
        public sbyte control;
        public byte[] pad0;
        public ushort length; // in ticks
        public byte[] pad1;
        public RawdataRef eventStream;

        public string Name => Encoding.ASCII.GetString(name.Where(x => x != 0).ToArray()).Fallback("<unnamed>");

        public RecordedAnimation(EndianReader r)
        {
            name = r.ReadBytes(32);
            version = (Version)r.ReadSByte();
            raw = r.ReadSByte();
            control = r.ReadSByte();
            pad0 = r.ReadBytes(1);
            length = r.ReadUInt16();
            pad1 = r.ReadBytes(6);
            eventStream = r.ReadRawdataRef();
        }

        public int DataLength => eventStream.size;
        public enum Version : sbyte { None, v1, v2, v3, v4 }
        public override string ToString() =>
            $"{Name}, {version}, {raw}, {control}, {length}, {eventStream}";
    }
}
