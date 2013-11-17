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
        private static readonly Type[][][] NetworkModes = new Type[][][]
        {
            HandshakePackets,
            StatusPackets,
            LoginPackets,
            PlayPackets
        };

        private static readonly Type[][] HandshakePackets = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) },
            new Type[] { typeof(HandshakePacket), typeof(HandshakePacket) } // 0x00
        };

        private static readonly Type[][] StatusPackets  = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) },
            new Type[] { typeof(StatusRequestPacket), typeof(StatusResponsePacket) }, // 0x00
            new Type[] { typeof(StatusPingPacket), typeof(StatusPingPacket) }, // 0x01
        };

        private static readonly Type[][] LoginPackets = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
            new Type[] { typeof(LoginStartPacket), typeof(LoginDisconnectPacket) }, // 0x00
            new Type[] { typeof(EncryptionKeyRequestPacket), typeof(EncryptionKeyResponsePacket) }, // 0x01
        };

        private static readonly Type[][] PlayPackets  = new Type[][]
        {
            // { typeof(serverbound), typeof(clientbound) }, // 0xID
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
                NetworkMode = packet.ReadPacket(MinecraftStream, NetworkMode);
                return packet;
            }
        }

        public void WritePacket(IPacket packet, PacketDirection direction)
        {
            lock (streamLock)
            {
                NetworkMode = packet.WritePacket(MinecraftStream, NetworkMode);
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
            }
        }

        /// <summary>
        /// Overrides the implementation for a certain packet.
        /// </summary>
        /// <param name="factory">A method that returns a new instance of the packet for populating later.</param>
//        public void OverridePacket(CreatePacketInstance factory)
//        {
//            if (factory == null)
//                throw new ArgumentNullException("factory");
//            var packet = factory(PacketDirection.Serverbound);
//            if (packet == null)
//                throw new NullReferenceException("Factory must not return null packet.");
//            packet = factory(PacketDirection.Clientbound);
//            if (packet == null)
//                throw new NullReferenceException("Factory must not return null packet.");
//            NetworkModes[(int)NetworkMode][packet.Id] = factory;
//        }
    }
}
