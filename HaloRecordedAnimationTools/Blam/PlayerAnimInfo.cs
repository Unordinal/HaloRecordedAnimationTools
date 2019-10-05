using System;
using System.Runtime.InteropServices;
using HaloRecordedAnimationTools.Helpers;

namespace HaloRecordedAnimationTools.Blam
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PlayerAnimInfo
    {
        /// <summary> The player's action state flags (crouching, attacking, throwing a grenade, etc). </summary>
        [Flags]
        public enum PlayerActionStateFlags : ushort
        {
            /// <summary> Player is not performing an action. </summary>
            None        = 0,

            /// <summary> Player is crouching. </summary>
            Crouch      = 1 << 0,
            /// <summary> Player is jumping. </summary>
            Jump        = 1 << 1,
            /// <summary> Player is pressing the flashlight button. </summary>
            Flashlight  = 1 << 4,
            /// <summary> Player is pressing the action button. </summary>
            Action      = 1 << 6,
            /// <summary> Player is meleeing. </summary>
            Melee       = 1 << 7,

            /// <summary> Player is pressing reload. </summary>
            Reload      = 1 << 10,
            /// <summary> Player is pressing their primary attack button. </summary>
            Attack      = 1 << 11,
            /// <summary> Player is pressing their grenade (secondary attack) button. </summary>
            Grenade     = 1 << 12,
            /// <summary> Player is pressing the action/swap button. </summary>
            ActionSwap  = 1 << 13
        }

        public const uint localPlayerPtr = 0x692DB0; // Local player
        public const uint playerControlsOffset = 0xF4; // Start of control presses (action, flashlight, etc)

        /// <summary> Player's current action states. </summary>
        public PlayerActionStateFlags playerActionStateFlag;
        /// <summary> Player's forward unit vector. </summary>
        public float playerForward;
        /// <summary> Player's left unit vector. </summary>
        public float playerLeft;
        /// <summary> Player's current weapon slot. </summary>
        public byte playerWeaponSlot;
        /// <summary> Player's aim vector. </summary>
        public Vector3 playerAimVector;
        /// <summary> Player's world position. </summary>
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
