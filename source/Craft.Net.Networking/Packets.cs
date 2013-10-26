using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Common;
using fNbt;
using Newtonsoft.Json;

namespace Craft.Net.Networking
{
    public interface IPacket
    {
        long Id { get; }
        /// <summary>
        /// Reads this packet data from the stream, not including its length or packet ID, and returns
        /// the new network state (if it has changed).
        /// </summary>
        NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode);
        /// <summary>
        /// Writes this packet data to the stream, not including its length or packet ID, and returns
        /// the new network state (if it has changed).
        /// </summary>
        NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode);
    }

    public struct UnknownPacket : IPacket
    {
        public long Id { get; set; }
        public byte[] Data { get; set; }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            throw new NotImplementedException();
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(Data.LongLength);
            stream.WriteUInt8Array(Data);
            return mode;
        }
    }

    #region Handshake

    public struct HandshakePacket : IPacket
    {
        public HandshakePacket(long protocolVersion, string hostname, ushort port, NetworkMode nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerHostname = hostname;
            ServerPort = port;
            NextState = nextState;
        }

        public long ProtocolVersion;
        public string ServerHostname;
        public ushort ServerPort;
        public NetworkMode NextState;

        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ProtocolVersion = stream.ReadVarInt();
            ServerHostname = stream.ReadString();
            ServerPort = stream.ReadUInt16();
            NextState = (NetworkMode)stream.ReadVarInt();
            return NextState;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerHostname);
            stream.WriteUInt16(ServerPort);
            stream.WriteVarInt((long)NextState);
            return NextState;
        }
    }

    #endregion

    #region Status

    public struct StatusRequestPacket : IPacket
    {
        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            return mode;
        }
    }

    public struct StatusResponsePacket : IPacket
    {
        public StatusResponsePacket(ServerStatus status)
        {
            Status = status;
        }

        public ServerStatus Status;

        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Status = JsonConvert.DeserializeObject<ServerStatus>(stream.ReadString());
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(JsonConvert.SerializeObject(Status));
            return mode;
        }
    }

    public struct StatusPingPacket : IPacket
    {
        public StatusPingPacket(long time)
        {
            Time = time;
        }

        public long Time;

        public const long PacketId = 0x01;
        public long Id { get { return 0x01; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Time = stream.ReadInt64();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt64(Time);
            return mode;
        }
    }

    #endregion
}