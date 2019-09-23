using System;
using System.Runtime.InteropServices;
using HaloRecordedAnimationTools.Helpers;

namespace HaloRecordedAnimationTools.Blam
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PlayerAnimInfo
    {
        [Flags]
        public enum PlayerActionStateFlags : ushort // Rename later
        {
            None = 0,

            Crouch = 1 << 0,
            Jump = 1 << 1,
            Flashlight = 1 << 4,
            Action = 1 << 6,
            Melee = 1 << 7,

            Reload = 1 << 10,
            Attack = 1 << 11,
            Grenade = 1 << 12,
            ActionSwap = 1 << 13
        }

        public const uint localPlayerPtr = 0x692DB0; // Local player
        public const uint playerControlsOffset = 0xF4; // Start of control presses (action, flashlight, etc)

        public PlayerActionStateFlags playerActionStateFlag;
        public float playerForward;
        public float playerLeft;
        public byte playerWeaponSlot;
        public Vector3 playerAimVector;
        public Vector3 playerPosition;

        public bool Crouch => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Crouch);
        public bool Jump => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Jump);
        public bool Flashlight => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Flashlight);
        public bool Action => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Action);
        public bool Melee => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Melee);
        public bool Reload => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Reload);
        public bool Attack => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Attack);
        public bool Grenade => playerActionStateFlag.HasFlag(PlayerActionStateFlags.Grenade);
        public bool ActionSwap => playerActionStateFlag.HasFlag(PlayerActionStateFlags.ActionSwap);

        public PlayerAnimInfo(PlayerActionStateFlags playerActionStateFlag, float playerForward, float playerLeft, byte playerWeaponSlot, Vector3 playerAimVector, Vector3 playerPosition)
        {
            this.playerActionStateFlag = playerActionStateFlag;
            this.playerForward = playerForward;
            this.playerLeft = playerLeft;
            this.playerWeaponSlot = playerWeaponSlot;
            this.playerAimVector = playerAimVector;
            this.playerPosition = playerPosition;
        }

        public static PlayerAnimInfo FromMemory()
        {
            PlayerAnimInfo animInfo = new PlayerAnimInfo
            {
                playerActionStateFlag = Memory.ReadMemory<PlayerActionStateFlags>((int)(localPlayerPtr + playerControlsOffset)),
                playerForward = Memory.ReadMemory<float>((int)(localPlayerPtr + 0x100)),
                playerLeft = Memory.ReadMemory<float>((int)(localPlayerPtr + 0x104)),
                playerWeaponSlot = Memory.ReadMemory<byte>((int)(localPlayerPtr + 0x10C)),
                playerAimVector = Memory.ReadMemory<Vector3>((int)(localPlayerPtr + 0x114)),
                playerPosition = Memory.ReadMemory<Vector3>((int)(localPlayerPtr + 0x164))
            };
            return animInfo;
        }
    }
}
