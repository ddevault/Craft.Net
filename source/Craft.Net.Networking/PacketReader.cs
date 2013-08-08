using System;
using Craft.Net.Common;

namespace Craft.Net.Networking
{
    public static class PacketReader
    {
        public const int ProtocolVersion = 74;
        public const string FriendlyVersion = "1.6.2";

        public delegate IPacket CreatePacketInstance();

        #region Packet Types
        private static readonly CreatePacketInstance[] Packets = new CreatePacketInstance[]
        {
            () => new KeepAlivePacket(), // 0x00
            () => new LoginRequestPacket(), // 0x01
            () => new HandshakePacket(), // 0x02
            () => new ChatMessagePacket(), // 0x03
            () => new TimeUpdatePacket(), // 0x04
            () => new EntityEquipmentPacket(), // 0x05
            () => new SpawnPositionPacket(), // 0x06
            () => new UseEntityPacket(), // 0x07
            () => new UpdateHealthPacket(), // 0x08
            () => new RespawnPacket(), // 0x09
            () => new PlayerPacket(), // 0x0A
            () => new PlayerPositionPacket(), // 0x0B
            () => new PlayerLookPacket(), // 0x0C
            () => new PlayerPositionAndLookPacket(), // 0x0D
            () => new PlayerDiggingPacket(), // 0x0E
            () => new RightClickPacket(), // 0x0F
            () => new HeldItemChangePacket(), // 0x10
            () => new UseBedPacket(), // 0x11
            () => new AnimationPacket(), // 0x12
            () => new EntityActionPacket(), // 0x13
            () => new SpawnPlayerPacket(), // 0x14
            null, // 0x15
            () => new CollectItemPacket(), // 0x16
            () => new SpawnObjectPacket(), // 0x17
            () => new SpawnMobPacket(), // 0x18
            () => new SpawnPaintingPacket(), // 0x19
            () => new SpawnExperienceOrbPacket(), // 0x1A
            () => new SteerVehiclePacket(), // 0x1B
            () => new EntityVelocityPacket(), // 0x1C
            () => new DestroyEntityPacket(), // 0x1D
            () => new EntityPacket(), // 0x1E
            () => new EntityRelativeMovePacket(), // 0x1F
            () => new EntityLookPacket(), // 0x20
            () => new EntityLookAndRelativeMovePacket(), // 0x21
            () => new EntityTeleportPacket(), // 0x22
            () => new EntityHeadLookPacket(), // 0x23
            null, // 0x24
            null, // 0x25
            () => new EntityStatusPacket(), // 0x26
            () => new AttachEntityPacket(), // 0x27
            () => new EntityMetadataPacket(), // 0x28
            () => new EntityEffectPacket(), // 0x29
            () => new RemoveEntityEffectPacket(), // 0x2A
            () => new SetExperiencePacket(), // 0x2B
            () => new EntityPropertiesPacket(), // 0x2C
            null, // 0x2D
            null, // 0x2E
            null, // 0x2F
            null, // 0x30
            null, // 0x31
            null, // 0x32
            () => new ChunkDataPacket(), // 0x33
            () => new MultipleBlockChangePacket(), // 0x34
            () => new BlockChangePacket(), // 0x35
            () => new BlockActionPacket(), // 0x36
            () => new BlockBreakAnimationPacket(), // 0x37
            () => new MapChunkBulkPacket(), // 0x38
            null, // 0x39
            null, // 0x3A
            null, // 0x3B
            () => new ExplosionPacket(), // 0x3C
            () => new SoundOrParticleEffectPacket(), // 0x3D
            () => new NamedSoundEffectPacket(), // 0x3E
            null, // 0x3F
            null, // 0x40
            null, // 0x41
            null, // 0x42
            null, // 0x43
            null, // 0x44
            null, // 0x45
            () => new ChangeGameStatePacket(), // 0x46
            () => new SpawnGlobalEntityPacket(), // 0x47
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
            () => new OpenWindowPacket(), // 0x64
            () => new CloseWindowPacket(), // 0x65
            () => new ClickWindowPacket(), // 0x66
            () => new SetSlotPacket(), // 0x67
            () => new SetWindowItemsPacket(), // 0x68
            () => new UpdateWindowPropertyPacket(), // 0x69
            () => new ConfirmTransactionPacket(), // 0x6A
            () => new CreativeInventoryActionPacket(), // 0x6B
            () => new EnchantItemPacket(), // 0x6C
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
            () => new UpdateSignPacket(), // 0x82
            () => new ItemDataPacket(), // 0x83
            () => new UpdateTileEntityPacket(), // 0x84
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
            () => new IncrementStatisticPacket(), // 0xC8
            () => new PlayerListItemPacket(), // 0xC9
            () => new PlayerAbilitiesPacket(), // 0xCA
            () => new TabCompletePacket(), // 0xCB
            () => new ClientSettingsPacket(), // 0xCC
            () => new ClientStatusPacket(), // 0xCD
            () => new CreateScoreboardPacket(), // 0xCE
            () => new UpdateScorePacket(), // 0xCF
            () => new DisplayScoreboardPacket(), // 0xD0
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
            () => new PluginMessagePacket(), // 0xFA
            null, // 0xFB
            () => new EncryptionKeyResponsePacket(), // 0xFC
            () => new EncryptionKeyRequestPacket(), // 0xFD
            () => new ServerListPingPacket(), // 0xFE
            () => new DisconnectPacket(), // 0xFF
        };
        #endregion

        public static IPacket ReadPacket(MinecraftStream stream)
        {
            byte id = stream.ReadUInt8();
            if (Packets[id] == null)
                throw new InvalidOperationException("Invalid packet ID: 0x" + id.ToString("X2"));
            var packet = Packets[id]();
            packet.ReadPacket(stream);
            return packet;
        }

        /// <summary>
        /// Overrides the implementation for a certain packet.
        /// </summary>
        /// <param name="factory">A method that returns a new instance of the packet for populating later.</param>
        public static void OverridePacket(CreatePacketInstance factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            var packet = factory();
            if (packet == null)
                throw new NullReferenceException("Factory must not return null packet.")
            Packets[packet.Id] = factory;
        }
    }
}
