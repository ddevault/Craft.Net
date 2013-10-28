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

    #region Login

    public struct LoginDisconnectPacket : IPacket
    {
        public LoginDisconnectPacket(string jsonData)
        {
            JsonData = jsonData;
        }

        /// <summary>
        /// Note: This will eventually be replaced with a strongly-typed represenation of this data.
        /// </summary>
        public string JsonData;

        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            JsonData = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(JsonData);
            return mode;
        }
    }

    public struct EncryptionKeyRequestPacket : IPacket
    {
        public EncryptionKeyRequestPacket(string serverId, byte[] publicKey, byte[] verificationToken)
        {
            ServerId = serverId;
            PublicKey = publicKey;
            VerificationToken = verificationToken;
        }

        public string ServerId;
        public byte[] PublicKey;
        public byte[] VerificationToken;

        public const long PacketId = 0x01;
        public long Id { get { return 0x01; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ServerId = stream.ReadString();
            var pkLength = stream.ReadInt16();
            PublicKey = stream.ReadUInt8Array(pkLength);
            var vtLength = stream.ReadInt16();
            VerificationToken = stream.ReadUInt8Array(vtLength);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(ServerId);
            stream.WriteInt16((short)PublicKey.Length);
            stream.WriteUInt8Array(PublicKey);
            stream.WriteInt16((short)VerificationToken.Length);
            stream.WriteUInt8Array(VerificationToken);
            return mode;
        }
    }

    public struct LoginSuccessPacket : IPacket
    {
        public LoginSuccessPacket(string uuid, string username)
        {
            UUID = uuid;
            Username = username;
        }

        public string UUID;
        public string Username;

        public const long PacketId = 0x02;
        public long Id { get { return 0x02; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            UUID = stream.ReadString();
            Username = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(UUID);
            stream.WriteString(Username);
            return mode;
        }
    }

    public struct LoginStartPacket : IPacket
    {
        public LoginStartPacket(string username)
        {
            Username = username;
        }

        public string Username;

        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Username = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(Username);
            return mode;
        }
    }

    public struct EncryptionKeyResponsePacket : IPacket
    {
        public EncryptionKeyResponsePacket(byte[] sharedSecret, byte[] verificationToken)
        {
            SharedSecret = sharedSecret;
            VerificationToken = verificationToken;
        }

        public byte[] SharedSecret;
        public byte[] VerificationToken;

        public const long PacketId = 0x01;
        public long Id { get { return 0x01; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            var ssLength = stream.ReadInt16();
            SharedSecret = stream.ReadUInt8Array(ssLength);
            var vtLength = stream.ReadInt16();
            VerificationToken = stream.ReadUInt8Array(vtLength);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt16((short)SharedSecret.Length);
            stream.WriteUInt8Array(SharedSecret);
            stream.WriteInt16((short)VerificationToken.Length);
            stream.WriteUInt8Array(VerificationToken);
            return mode;
        }
    }

    #endregion

    #region Play

    public struct KeepAlivePacket : IPacket
    {
        public KeepAlivePacket(int keepAlive)
        {
            KeepAlive = keepAlive;
        }

        public int KeepAlive;

        public const long PacketId = 0x00;
        public long Id { get { return 0x00; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            KeepAlive = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(KeepAlive);
            return mode;
        }
    }

    #endregion
}
/*
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
*/