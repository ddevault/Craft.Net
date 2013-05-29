using Craft.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Networking
{
    public static class PacketReader
    {
        public const int ProtocolVersion = 61;
        public const string FriendlyVersion = "1.5.2";

        #region Packet Types
        private static Type[] Packets = new[]
        {
            typeof(KeepAlivePacket), // 0x00
            typeof(LoginRequestPacket), // 0x01
            typeof(HandshakePacket), // 0x02
            typeof(ChatMessagePacket), // 0x03
            typeof(TimeUpdatePacket), // 0x04
            typeof(EntityEquipmentPacket), // 0x05
            typeof(SpawnPositionPacket), // 0x06
            typeof(UseEntityPacket), // 0x07
            typeof(UpdateHealthPacket), // 0x08
            typeof(RespawnPacket), // 0x09
            typeof(PlayerPacket), // 0x0A
            typeof(PlayerPositionPacket), // 0x0B
            typeof(PlayerLookPacket), // 0x0C
            typeof(PlayerPositionAndLookPacket), // 0x0D
            typeof(PlayerDiggingPacket), // 0x0E
            typeof(RightClickPacket), // 0x0F
            typeof(HeldItemChangePacket), // 0x10
            typeof(UseBedPacket), // 0x11
            typeof(AnimationPacket), // 0x12
            typeof(EntityActionPacket), // 0x13
            typeof(SpawnPlayerPacket), // 0x14
            null, // 0x15
            typeof(CollectItemPacket), // 0x16
            typeof(SpawnObjectPacket), // 0x17
            typeof(SpawnMobPacket), // 0x18
            typeof(SpawnPaintingPacket), // 0x19
            typeof(SpawnExperienceOrbPacket), // 0x1A
            null, // 0x1B
            typeof(EntityVelocityPacket), // 0x1C
            typeof(DestroyEntityPacket), // 0x1D
            typeof(EntityPacket), // 0x1E
            typeof(EntityRelativeMovePacket), // 0x1F
            typeof(EntityLookPacket), // 0x20
            typeof(EntityLookAndRelativeMovePacket), // 0x21
            typeof(EntityTeleportPacket), // 0x22
            typeof(EntityHeadLookPacket), // 0x23
            null, // 0x24
            null, // 0x25
            typeof(EntityStatusPacket), // 0x26
            typeof(AttachEntityPacket), // 0x27
            typeof(EntityMetadataPacket), // 0x28
            typeof(EntityEffectPacket), // 0x29
            typeof(RemoveEntityEffectPacket), // 0x2A
            typeof(SetExperiencePacket), // 0x2B
            null, // 0x2C
            null, // 0x2D
            null, // 0x2E
            null, // 0x2F
            null, // 0x30
            null, // 0x31
            null, // 0x32
            typeof(ChunkDataPacket), // 0x33
            typeof(MultipleBlockChangePacket), // 0x34
            typeof(BlockChangePacket), // 0x35
            typeof(BlockActionPacket), // 0x36
            typeof(BlockBreakAnimationPacket), // 0x37
            typeof(MapChunkBulkPacket), // 0x38
            null, // 0x39
            null, // 0x3A
            null, // 0x3B
            typeof(ExplosionPacket), // 0x3C
            typeof(SoundOrParticleEffectPacket), // 0x3D
            typeof(NamedSoundEffectPacket), // 0x3E
            null, // 0x3F
            null, // 0x40
            null, // 0x41
            null, // 0x42
            null, // 0x43
            null, // 0x44
            null, // 0x45
            typeof(ChangeGameStatePacket), // 0x46
            typeof(SpawnGlobalEntityPacket), // 0x47
            null, // 0x48
            null, // 0x49
            null, // 0x4A
            null, // 0x4B
            null, // 0x4C
            null, // 0x4D
            null, // 0x4E
            null, // 0x4F
            null, // 0x50
            null, // 0x51
            null, // 0x52
            null, // 0x53
            null, // 0x54
            null, // 0x55
            null, // 0x56
            null, // 0x57
            null, // 0x58
            null, // 0x59
            null, // 0x5A
            null, // 0x5B
            null, // 0x5C
            null, // 0x5D
            null, // 0x5E
            null, // 0x5F
            null, // 0x60
            null, // 0x61
            null, // 0x62
            null, // 0x63
            typeof(OpenWindowPacket), // 0x64
            typeof(CloseWindowPacket), // 0x65
            typeof(ClickWindowPacket), // 0x66
            typeof(SetSlotPacket), // 0x67
            typeof(SetWindowItemsPacket), // 0x68
            typeof(UpdateWindowPropertyPacket), // 0x69
            typeof(ConfirmTransactionPacket), // 0x6A
            typeof(CreativeInventoryActionPacket), // 0x6B
            typeof(EnchantItemPacket), // 0x6C
            null, // 0x6D
            null, // 0x6E
            null, // 0x6F
            null, // 0x70
            null, // 0x71
            null, // 0x72
            null, // 0x73
            null, // 0x74
            null, // 0x75
            null, // 0x76
            null, // 0x77
            null, // 0x78
            null, // 0x79
            null, // 0x7A
            null, // 0x7B
            null, // 0x7C
            null, // 0x7D
            null, // 0x7E
            null, // 0x7F
            null, // 0x80
            null, // 0x81
            typeof(UpdateSignPacket), // 0x82
            typeof(ItemDataPacket), // 0x83
            typeof(UpdateTileEntityPacket), // 0x84
            null, // 0x85
            null, // 0x86
            null, // 0x87
            null, // 0x88
            null, // 0x89
            null, // 0x8A
            null, // 0x8B
            null, // 0x8C
            null, // 0x8D
            null, // 0x8E
            null, // 0x8F
            null, // 0x90
            null, // 0x91
            null, // 0x92
            null, // 0x93
            null, // 0x94
            null, // 0x95
            null, // 0x96
            null, // 0x97
            null, // 0x98
            null, // 0x99
            null, // 0x9A
            null, // 0x9B
            null, // 0x9C
            null, // 0x9D
            null, // 0x9E
            null, // 0x9F
            null, // 0xA0
            null, // 0xA1
            null, // 0xA2
            null, // 0xA3
            null, // 0xA4
            null, // 0xA5
            null, // 0xA6
            null, // 0xA7
            null, // 0xA8
            null, // 0xA9
            null, // 0xAA
            null, // 0xAB
            null, // 0xAC
            null, // 0xAD
            null, // 0xAE
            null, // 0xAF
            null, // 0xB0
            null, // 0xB1
            null, // 0xB2
            null, // 0xB3
            null, // 0xB4
            null, // 0xB5
            null, // 0xB6
            null, // 0xB7
            null, // 0xB8
            null, // 0xB9
            null, // 0xBA
            null, // 0xBB
            null, // 0xBC
            null, // 0xBD
            null, // 0xBE
            null, // 0xBF
            null, // 0xC0
            null, // 0xC1
            null, // 0xC2
            null, // 0xC3
            null, // 0xC4
            null, // 0xC5
            null, // 0xC6
            null, // 0xC7
            typeof(IncrementStatisticPacket), // 0xC8
            typeof(PlayerListItemPacket), // 0xC9
            typeof(PlayerAbilitiesPacket), // 0xCA
            typeof(TabCompletePacket), // 0xCB
            typeof(ClientSettingsPacket), // 0xCC
            typeof(ClientStatusPacket), // 0xCD
            typeof(CreateScoreboardPacket), // 0xCE
            typeof(UpdateScorePacket), // 0xCF
            typeof(DisplayScoreboardPacket), // 0xD0
            null, // 0xD1
            null, // 0xD2
            null, // 0xD3
            null, // 0xD4
            null, // 0xD5
            null, // 0xD6
            null, // 0xD7
            null, // 0xD8
            null, // 0xD9
            null, // 0xDA
            null, // 0xDB
            null, // 0xDC
            null, // 0xDD
            null, // 0xDE
            null, // 0xDF
            null, // 0xE0
            null, // 0xE1
            null, // 0xE2
            null, // 0xE3
            null, // 0xE4
            null, // 0xE5
            null, // 0xE6
            null, // 0xE7
            null, // 0xE8
            null, // 0xE9
            null, // 0xEA
            null, // 0xEB
            null, // 0xEC
            null, // 0xED
            null, // 0xEE
            null, // 0xEF
            null, // 0xF0
            null, // 0xF1
            null, // 0xF2
            null, // 0xF3
            null, // 0xF4
            null, // 0xF5
            null, // 0xF6
            null, // 0xF7
            null, // 0xF8
            null, // 0xF9
            typeof(PluginMessagePacket), // 0xFA
            null, // 0xFB
            typeof(EncryptionKeyResponsePacket), // 0xFC
            typeof(EncryptionKeyRequestPacket), // 0xFD
            typeof(ServerListPingPacket), // 0xFE
            typeof(DisconnectPacket), // 0xFF
        };
        #endregion

        public static IPacket ReadPacket(MinecraftStream stream)
        {
            byte id = stream.ReadUInt8();
            var type = Packets[id];
            if (type == null)
                throw new InvalidOperationException("Invalid packet ID: 0x" + id.ToString("X2"));
            var packet = (IPacket)Activator.CreateInstance(type);
            packet.ReadPacket(stream);
            return packet;
        }

        /// <summary>
        /// Overrides the implementation for a certain packet.
        /// </summary>
        /// <param name="packetType">A Type that inherits from Craft.Net.IPacket</param>
        public static void OverridePacket(Type packetType)
        {
            if (!typeof(IPacket).IsAssignableFrom(packetType))
                throw new InvalidCastException("Type must inherit from IPacket.");
            var instance = (IPacket)Activator.CreateInstance(packetType);
            Packets[instance.Id] = packetType;
        }
    }
}
