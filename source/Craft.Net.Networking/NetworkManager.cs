using System;
using System.IO;
using Craft.Net.Common;
using BufferedStream = Craft.Net.Networking.BufferedStream;

namespace Craft.Net.Networking
{
    public class NetworkManager
    {
        public const int ProtocolVersion = 4;
        public const string FriendlyVersion = "1.7.2";

        public NetworkMode NetworkMode { get; private set; }
        public bool Strict { get; set; }

        private object streamLock = new object();
        private Stream _baseStream;
        public Stream BaseStream
        {
            get { return _baseStream; }
            set
            {
                lock (streamLock)
                {
                    if (BufferedStream != null)
                        BufferedStream.Flush();
                    _baseStream = value;
                    BufferedStream = new BufferedStream(value);
                    MinecraftStream = new MinecraftStream(BufferedStream);
                }
            }
        }

        private BufferedStream BufferedStream { get; set; }
        private MinecraftStream MinecraftStream { get; set; }

        public NetworkManager(Stream stream)
        {
            NetworkMode = NetworkMode.Handshake;
            Strict = true;
            BaseStream = stream;
        }

        #region Packet Types
        private static readonly Type[][] HandshakePackets = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
            new Type[] { typeof(HandshakePacket), typeof(HandshakePacket) }, // 0x00
        };

        private static readonly Type[][] StatusPackets  = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
            new Type[] { typeof(StatusRequestPacket), typeof(StatusResponsePacket) }, // 0x00
            new Type[] { typeof(StatusPingPacket), typeof(StatusPingPacket) }, // 0x01
        };

        private static readonly Type[][] LoginPackets = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
            new Type[] { typeof(LoginStartPacket), typeof(LoginDisconnectPacket) }, // 0x00
            new Type[] { typeof(EncryptionKeyResponsePacket), typeof(EncryptionKeyRequestPacket) }, // 0x01
            new Type[] { null, typeof(LoginSuccessPacket) }, // 0x02
        };

        private static readonly Type[][] PlayPackets  = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
            new Type[] { typeof(KeepAlivePacket), typeof(KeepAlivePacket) }, // 0x00
            new Type[] { typeof(ChatMessagePacket), typeof(JoinGamePacket) }, // 0x01
            new Type[] { typeof(UseEntityPacket), typeof(ChatMessagePacket) }, // 0x02
            new Type[] { typeof(PlayerPacket), typeof(TimeUpdatePacket) }, // 0x03
            new Type[] { typeof(PlayerPositionPacket), typeof(EntityEquipmentPacket) }, // 0x04
            new Type[] { typeof(PlayerLookPacket), typeof(SpawnPositionPacket) }, // 0x05
            new Type[] { typeof(PlayerPositionAndLookPacket), typeof(UpdateHealthPacket) }, // 0x06
            new Type[] { typeof(PlayerBlockActionPacket), typeof(RespawnPacket) }, // 0x07
            new Type[] { typeof(RightClickPacket), typeof(PlayerPositionAndLookPacket) }, // 0x08
            new Type[] { typeof(HeldItemPacket), typeof(HeldItemPacket) }, // 0x09
            new Type[] { typeof(EntityActionPacket), typeof(UseBedPacket) }, // 0x0A
            new Type[] { typeof(SteerVehiclePacket), typeof(AnimationPacket) }, // 0x0B
            new Type[] { typeof(CloseWindowPacket), typeof(SpawnPlayerPacket) }, // 0x0C
            new Type[] { typeof(ClickWindowPacket), typeof(CollectItemPacket) }, // 0x0D
            new Type[] { typeof(ConfirmTransactionPacket), typeof(SpawnObjectPacket) }, // 0x0E
            new Type[] { typeof(CreativeInventoryActionPacket), typeof(SpawnMobPacket) }, // 0x0F
            new Type[] { typeof(EnchantItemPacket), typeof(SpawnPaintingPacket) }, // 0x10
            new Type[] { typeof(UpdateSignPacket), typeof(SpawnExperienceOrbPacket) }, // 0x11
            new Type[] { typeof(PlayerAbilitiesPacket), typeof(EntityVelocityPacket) }, // 0x12
            new Type[] { typeof(TabCompletePacket), typeof(DestroyEntityPacket) }, // 0x13
            new Type[] { typeof(ClientSettingsPacket), typeof(EntityPacket) }, // 0x14
            new Type[] { typeof(PluginMessagePacket), typeof(EntityRelativeMovePacket) }, // 0x15
            new Type[] { null, typeof(EntityLookPacket) }, // 0x16
            new Type[] { null, typeof(EntityLookAndRelativeMovePacket) }, // 0x17
            new Type[] { null, typeof(EntityTeleportPacket) }, // 0x18
            new Type[] { null, typeof(EntityHeadLookPacket) }, // 0x19
            new Type[] { null, typeof(EntityStatusPacket) }, // 0x1A
            new Type[] { null, typeof(AttachEntityPacket) }, // 0x1B
            new Type[] { null, typeof(EntityMetadataPacket) }, // 0x1C
            new Type[] { null, typeof(EntityEffectPacket) }, // 0x1D
            new Type[] { null, typeof(RemoveEntityEffectPacket) }, // 0x1E
            new Type[] { null, typeof(SetExperiencePacket) }, // 0x1F
            new Type[] { null, typeof(EntityPropertiesPacket) }, // 0x20
            new Type[] { null, typeof(ChunkDataPacket) }, // 0x21
            new Type[] { null, typeof(MultipleBlockChangePacket) }, // 0x22
            new Type[] { null, typeof(BlockChangePacket) }, // 0x23
            new Type[] { null, typeof(BlockActionPacket) }, // 0x24
            new Type[] { null, typeof(BlockBreakAnimationPacket) }, // 0x25
            new Type[] { null, typeof(MapChunkBulkPacket) }, // 0x26
            new Type[] { null, typeof(ExplosionPacket) }, // 0x27
            new Type[] { null, typeof(EffectPacket) }, // 0x28
            new Type[] { null, typeof(SoundEffectPacket) }, // 0x29
            new Type[] { null, typeof(ParticleEffectPacket) }, // 0x2A
            new Type[] { null, typeof(ChangeGameStatePacket) }, // 0x2B
            new Type[] { null, typeof(SpawnGlobalEntityPacket) }, // 0x2C
            new Type[] { null, typeof(OpenWindowPacket) }, // 0x2D
            new Type[] { null, typeof(CloseWindowPacket) }, // 0x2E
            new Type[] { null, typeof(SetWindowItemsPacket) }, // 0x2F
            new Type[] { null, typeof(UpdateWindowPropertyPacket) }, // 0x30
            new Type[] { null, typeof(UpdateSignPacket) }, // 0x31
            new Type[] { null, typeof(MapDataPacket) }, // 0x32
            new Type[] { null, typeof(UpdateTileEntityPacket) }, // 0x33
            new Type[] { null, typeof(OpenSignEditorPacket) }, // 0x34
            new Type[] { null, typeof(UpdateStatisticsPacket) }, // 0x35
            new Type[] { null, typeof(PlayerListItemPacket) }, // 0x36
            new Type[] { null, typeof(PlayerAbilitiesPacket) }, // 0x37
            new Type[] { null, typeof(TabCompletePacket) }, // 0x38
            new Type[] { null, typeof(ScoreboardObjectivePacket) }, // 0x39
            new Type[] { null, typeof(UpdateScorePacket) }, // 0x3A
            new Type[] { null, typeof(DisplayScoreboardPacket) }, // 0x3B
            new Type[] { null, typeof(SetTeamsPacket) }, // 0x3C
            new Type[] { null, typeof(PluginMessagePacket) }, // 0x3D
            new Type[] { null, typeof(DisconnectPacket) }, // 0x3E
        };

        private static readonly Type[][][] NetworkModes = new Type[][][]
        {
            HandshakePackets,
            StatusPackets,
            LoginPackets,
            PlayPackets
        };
        #endregion

        public IPacket ReadPacket(PacketDirection direction)
        {
            lock (streamLock)
            {
                int idLength;
                long length = MinecraftStream.ReadVarInt();
                long id = MinecraftStream.ReadVarInt(out idLength);
                if (NetworkModes[(int)NetworkMode].Length < id || NetworkModes[(int)NetworkMode][id][(int)direction] == null)
                {
                    if (Strict)
                        throw new InvalidOperationException("Invalid packet ID: 0x" + id.ToString("X2"));
                    else
                    {
                        return new UnknownPacket
                        {
                            Id = id,
                            Data = MinecraftStream.ReadUInt8Array((int)(length - idLength))
                        };
                    }
                }
                var packet = (IPacket)Activator.CreateInstance(NetworkModes[(int)NetworkMode][id][(int)direction]);
                NetworkMode = packet.ReadPacket(MinecraftStream, NetworkMode, direction);
                return packet;
            }
        }

        public void WritePacket(IPacket packet, PacketDirection direction)
        {
            lock (streamLock)
            {
                var newNetworkMode = packet.WritePacket(MinecraftStream, NetworkMode, direction);
                BufferedStream.WriteImmediately = true;
                int id = -1;
                var type = packet.GetType();
                // Find packet ID for this type
                for (int i = 0; i < NetworkModes[(int)NetworkMode].LongLength; i++)
                {
                    if (NetworkModes[(int)NetworkMode][i][(int)direction] == type)
                    {
                        id = i;
                        break;
                    }
                }
                if (id == -1)
                    throw new InvalidOperationException("Attempted to write invalid packet type.");
                MinecraftStream.WriteVarInt((int)BufferedStream.PendingWrites + MinecraftStream.GetVarIntLength(id));
                MinecraftStream.WriteVarInt(id);
                BufferedStream.WriteImmediately = false;
                BufferedStream.Flush();
                NetworkMode = newNetworkMode;
            }
        }
    }
}
