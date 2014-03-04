using System;
using System.IO;
using Craft.Net.Common;
using BufferedStream = Craft.Net.Networking.BufferedStream;
using System.Collections.Generic;

namespace Craft.Net.Networking
{
    public class NetworkManager
    {
        public const int ProtocolVersion = 4;
        public const string FriendlyVersion = "1.7.5";

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
        private static readonly Type[][] HandshakePackets;
        private static readonly Type[][] StatusPackets;
        private static readonly Type[][] LoginPackets;
        private static readonly Type[][] PlayPackets;

        private static readonly Type[][][] NetworkModes;

        static NetworkManager()
        {
            List<Type> serverbound = new List<Type>();
            List<Type> clientbound = new List<Type>();
            // Handshake packets
            serverbound.Add(typeof(HandshakePacket)); // Serverbound

            clientbound.Add(typeof(HandshakePacket)); // Clientbound
            HandshakePackets = Populate(serverbound, clientbound);
            // Status packets
            serverbound.Add(typeof(StatusRequestPacket)); // Serverbound
            serverbound.Add(typeof(StatusPingPacket));

            clientbound.Add(typeof(StatusResponsePacket)); // Clientbound
            clientbound.Add(typeof(StatusPingPacket));
            StatusPackets = Populate(serverbound, clientbound);
            // Login packets
            serverbound.Add(typeof(LoginStartPacket)); // Serverbound
            serverbound.Add(typeof(EncryptionKeyResponsePacket));

            clientbound.Add(typeof(LoginDisconnectPacket)); // Clientbound
            clientbound.Add(typeof(EncryptionKeyRequestPacket));
            clientbound.Add(typeof(LoginSuccessPacket));
            LoginPackets = Populate(serverbound, clientbound);
            // Play packets
            serverbound.Add(typeof(KeepAlivePacket)); // Serverbound
            serverbound.Add(typeof(ChatMessagePacket));
            serverbound.Add(typeof(UseEntityPacket));
            serverbound.Add(typeof(PlayerPacket));
            serverbound.Add(typeof(PlayerPositionPacket));
            serverbound.Add(typeof(PlayerLookPacket));
            serverbound.Add(typeof(PlayerPositionAndLookPacket));
            serverbound.Add(typeof(PlayerBlockActionPacket));
            serverbound.Add(typeof(RightClickPacket));
            serverbound.Add(typeof(HeldItemPacket));
            serverbound.Add(typeof(AnimationPacket));
            serverbound.Add(typeof(EntityActionPacket));
            serverbound.Add(typeof(SteerVehiclePacket));
            serverbound.Add(typeof(CloseWindowPacket));
            serverbound.Add(typeof(ClickWindowPacket));
            serverbound.Add(typeof(ConfirmTransactionPacket));
            serverbound.Add(typeof(CreativeInventoryActionPacket));
            serverbound.Add(typeof(EnchantItemPacket));
            serverbound.Add(typeof(UpdateSignPacket));
            serverbound.Add(typeof(PlayerAbilitiesPacket));
            serverbound.Add(typeof(TabCompletePacket));
            serverbound.Add(typeof(ClientSettingsPacket));
            serverbound.Add(typeof(ClientStatusPacket));
            serverbound.Add(typeof(PluginMessagePacket));

            clientbound.Add(typeof(KeepAlivePacket)); // Clientbound
            clientbound.Add(typeof(JoinGamePacket));
            clientbound.Add(typeof(ChatMessagePacket));
            clientbound.Add(typeof(TimeUpdatePacket));
            clientbound.Add(typeof(EntityEquipmentPacket));
            clientbound.Add(typeof(SpawnPositionPacket));
            clientbound.Add(typeof(UpdateHealthPacket));
            clientbound.Add(typeof(RespawnPacket));
            clientbound.Add(typeof(PlayerPositionAndLookPacket));
            clientbound.Add(typeof(HeldItemPacket));
            clientbound.Add(typeof(UseBedPacket));
            clientbound.Add(typeof(AnimationPacket));
            clientbound.Add(typeof(SpawnPlayerPacket));
            clientbound.Add(typeof(CollectItemPacket));
            clientbound.Add(typeof(SpawnObjectPacket));
            clientbound.Add(typeof(SpawnMobPacket));
            clientbound.Add(typeof(SpawnPaintingPacket));
            clientbound.Add(typeof(SpawnExperienceOrbPacket));
            clientbound.Add(typeof(EntityVelocityPacket));
            clientbound.Add(typeof(DestroyEntityPacket));
            clientbound.Add(typeof(EntityPacket));
            clientbound.Add(typeof(EntityRelativeMovePacket));
            clientbound.Add(typeof(EntityLookPacket));
            clientbound.Add(typeof(EntityLookAndRelativeMovePacket));
            clientbound.Add(typeof(EntityTeleportPacket));
            clientbound.Add(typeof(EntityHeadLookPacket));
            clientbound.Add(typeof(EntityStatusPacket));
            clientbound.Add(typeof(AttachEntityPacket));
            clientbound.Add(typeof(EntityMetadataPacket));
            clientbound.Add(typeof(EntityEffectPacket));
            clientbound.Add(typeof(RemoveEntityEffectPacket));
            clientbound.Add(typeof(SetExperiencePacket));
            clientbound.Add(typeof(EntityPropertiesPacket));
            clientbound.Add(typeof(ChunkDataPacket));
            clientbound.Add(typeof(MultipleBlockChangePacket));
            clientbound.Add(typeof(BlockChangePacket));
            clientbound.Add(typeof(BlockActionPacket));
            clientbound.Add(typeof(BlockBreakAnimationPacket));
            clientbound.Add(typeof(MapChunkBulkPacket));
            clientbound.Add(typeof(ExplosionPacket));
            clientbound.Add(typeof(EffectPacket));
            clientbound.Add(typeof(SoundEffectPacket));
            clientbound.Add(typeof(ParticleEffectPacket));
            clientbound.Add(typeof(ChangeGameStatePacket));
            clientbound.Add(typeof(SpawnGlobalEntityPacket));
            clientbound.Add(typeof(OpenWindowPacket));
            clientbound.Add(typeof(CloseWindowPacket));
            clientbound.Add(typeof(SetSlotPacket));
            clientbound.Add(typeof(SetWindowItemsPacket));
            clientbound.Add(typeof(UpdateWindowPropertyPacket));
            clientbound.Add(typeof(ConfirmTransactionPacket));
            clientbound.Add(typeof(UpdateSignPacket));
            clientbound.Add(typeof(MapDataPacket));
            clientbound.Add(typeof(UpdateTileEntityPacket));
            clientbound.Add(typeof(OpenSignEditorPacket));
            clientbound.Add(typeof(UpdateStatisticsPacket));
            clientbound.Add(typeof(PlayerListItemPacket));
            clientbound.Add(typeof(PlayerAbilitiesPacket));
            clientbound.Add(typeof(TabCompletePacket));
            clientbound.Add(typeof(ScoreboardObjectivePacket));
            clientbound.Add(typeof(UpdateScorePacket));
            clientbound.Add(typeof(DisplayScoreboardPacket));
            clientbound.Add(typeof(SetTeamsPacket));
            clientbound.Add(typeof(PluginMessagePacket));
            clientbound.Add(typeof(DisconnectPacket));
            PlayPackets = Populate(serverbound, clientbound);

            NetworkModes = new Type[][][]
            {
                HandshakePackets,
                StatusPackets,
                LoginPackets,
                PlayPackets
            };
        }

        private static Type[][] Populate(List<Type> serverbound, List<Type> clientbound)
        {
            var array = new Type[Math.Max(serverbound.Count, clientbound.Count)][];
            for (int i = 0; i < array.Length; i++)
                array[i] = new[] { i < serverbound.Count ? serverbound[i] : null, i < clientbound.Count ? clientbound[i] : null };
            serverbound.Clear();
            clientbound.Clear();
            return array;
        }
        #endregion

        public IPacket ReadPacket(PacketDirection direction)
        {
            lock (streamLock)
            {
                int idLength;
                long length = MinecraftStream.ReadVarInt();
                long id = MinecraftStream.ReadVarInt(out idLength);
                var data = MinecraftStream.ReadUInt8Array((int)(length - idLength));
                if (NetworkModes[(int)NetworkMode].Length < id || NetworkModes[(int)NetworkMode][id][(int)direction] == null)
                {
                    if (Strict)
                        throw new InvalidOperationException("Invalid packet ID: 0x" + id.ToString("X2"));
                    else
                    {
                        return new UnknownPacket
                        {
                            Id = id,
                            Data = data
                        };
                    }
                }
                var ms = new MinecraftStream(new MemoryStream(data));
                var packet = (IPacket)Activator.CreateInstance(NetworkModes[(int)NetworkMode][id][(int)direction]);
                NetworkMode = packet.ReadPacket(ms, NetworkMode, direction);
                if (ms.Position < data.Length)
                    Console.WriteLine("Warning: did not completely read packet: {0}", packet.GetType().Name); // TODO: Find some other way to warn about this
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
