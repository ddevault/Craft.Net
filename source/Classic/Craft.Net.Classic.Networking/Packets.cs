using System;
using Craft.Net.Classic.Common;

namespace Craft.Net.Classic.Networking
{
    public interface IPacket
    {
        byte Id { get; }
        void ReadPacket(MinecraftStream stream);
        void WritePacket(MinecraftStream stream);
    }

    public struct HandshakePacket : IPacket
    {
        public HandshakePacket(byte protocolVersion, string partyName, string keyOrMOTD, bool isOP)
        {
            ProtocolVersion = protocolVersion;
            PartyName = partyName;
            KeyOrMOTD = keyOrMOTD;
            IsOP = isOP;
        }

        public byte ProtocolVersion;
        /// <summary>
        /// The name of the sender, either the server name or username.
        /// </summary>
        public string PartyName;
        /// <summary>
        /// From a server, this is the MOTD. From a client, this is the verification key.
        /// </summary>
        public string KeyOrMOTD;
        public bool IsOP;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ProtocolVersion = stream.ReadUInt8();
            PartyName = stream.ReadString();
            KeyOrMOTD = stream.ReadString();
            IsOP = stream.ReadUInt8() == 0x64;
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteUInt8(ProtocolVersion);
            stream.WriteString(PartyName);
            stream.WriteString(KeyOrMOTD);
            stream.WriteUInt8((byte)(IsOP ? 0x64 : 0x00));
        }
    }

    public struct PingPacket : IPacket
    {
        public const byte PacketId = 0x01;
        public byte Id { get { return 0x01; } }

        public void ReadPacket(MinecraftStream stream)
        {
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
        }
    }
    
    public struct LevelInitializePacket : IPacket
    {
        public const byte PacketId = 0x02;
        public byte Id { get { return 0x02; } }

        public void ReadPacket(MinecraftStream stream)
        {
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
        }
    }

    public struct LevelDataPacket : IPacket
    {
        public LevelDataPacket(byte[] data, double complete)
        {
            Data = data;
            Complete = complete;
        }

        public byte[] Data;
        public double Complete;

        public const byte PacketId = 0x03;
        public byte Id { get { return 0x03; } }

        public void ReadPacket(MinecraftStream stream)
        {
            var length = stream.ReadInt16();
            var data = stream.ReadArray();
            Data = new byte[length];
            Array.Copy(data, Data, length);
            Complete = stream.ReadUInt8() / 100.0;
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt16((short)Data.Length);
            stream.WriteArray(Data);
            stream.WriteUInt8((byte)(Complete * 100));
        }
    }

    public struct LevelFinalizePacket : IPacket
    {
        public LevelFinalizePacket(short xSize, short ySize, short zSize)
        {
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;
        }

        public short XSize, YSize, ZSize;

        public const byte PacketId = 0x04;
        public byte Id { get { return 0x04; } }

        public void ReadPacket(MinecraftStream stream)
        {
            XSize = stream.ReadInt16();
            YSize = stream.ReadInt16();
            ZSize = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
        }
    }

    public struct ClientSetBlockPacket : IPacket
    {
        public ClientSetBlockPacket(short x, short y, short z, bool destroy, byte blockType)
        {
            X = x;
            Y = y;
            Z = z;
            Destroy = destroy;
            BlockType = blockType;
        }

        public short X, Y, Z;
        public bool Destroy;
        public byte BlockType;

        public const byte PacketId = 0x05;
        public byte Id { get { return 0x05; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt16();
            Y = stream.ReadInt16();
            Z = stream.ReadInt16();
            Destroy = stream.ReadUInt8() == 0x00;
            BlockType = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt16(X);
            stream.WriteInt16(Y);
            stream.WriteInt16(Z);
            stream.WriteUInt8((byte)(Destroy ? 0x00 : 0x01));
            stream.WriteUInt8(BlockType);
        }
    }

    public struct ServerSetBlockPacket : IPacket
    {
        public ServerSetBlockPacket(short x, short y, short z, byte blockType)
        {
            X = x;
            Y = y;
            Z = z;
            BlockType = blockType;
        }

        public short X, Y, Z;
        public byte BlockType;

        public const byte PacketId = 0x06;
        public byte Id { get { return 0x06; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt16();
            Y = stream.ReadInt16();
            Z = stream.ReadInt16();
            BlockType = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt16(X);
            stream.WriteInt16(Y);
            stream.WriteInt16(Z);
            stream.WriteUInt8(BlockType);
        }
    }

    public struct SpawnPlayerPacket : IPacket
    {
        public SpawnPlayerPacket(sbyte playerId, string username, short x, short y, short z, byte yaw, byte pitch)
        {
            PlayerID = playerId;
            Username = username;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public sbyte PlayerID;
        public string Username;
        public short X, Y, Z;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x07;
        public byte Id { get { return 0x07; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerID = stream.ReadInt8();
            Username = stream.ReadString();
            X = stream.ReadInt16();
            Y = stream.ReadInt16();
            Z = stream.ReadInt16();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerID);
            stream.WriteString(Username);
            stream.WriteInt16(X);
            stream.WriteInt16(Y);
            stream.WriteInt16(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
        }
    }

    public struct PositionAndOrientationPacket : IPacket
    {
        public PositionAndOrientationPacket(sbyte playerId, short x, short y, short z, byte yaw, byte pitch)
        {
            PlayerID = playerId;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public sbyte PlayerID;
        public short X, Y, Z;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x08;
        public byte Id { get { return 0x08; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerID = stream.ReadInt8();
            X = stream.ReadInt16();
            Y = stream.ReadInt16();
            Z = stream.ReadInt16();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerID);
            stream.WriteInt16(X);
            stream.WriteInt16(Y);
            stream.WriteInt16(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
        }
    }

    public struct RelativePositionAndOrientationPacket : IPacket
    {
        public RelativePositionAndOrientationPacket(sbyte playerId, sbyte deltaX, sbyte deltaY, sbyte deltaZ, byte yaw, byte pitch)
        {
            PlayerID = playerId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            Yaw = yaw;
            Pitch = pitch;
        }

        public sbyte PlayerID;
        public sbyte DeltaX, DeltaY, DeltaZ;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x09;
        public byte Id { get { return 0x09; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerID = stream.ReadInt8();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerID);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
        }
    }

    public struct RelativePositionPacket : IPacket
    {
        public RelativePositionPacket(sbyte playerId, sbyte deltaX, sbyte deltaY, sbyte deltaZ)
        {
            PlayerID = playerId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
        }

        public sbyte PlayerID;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public const byte PacketId = 0x0A;
        public byte Id { get { return 0x0A; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerID = stream.ReadInt8();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerID);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
        }
    }

    public struct OrientationPacket : IPacket
    {
        public OrientationPacket(sbyte playerId, byte yaw, byte pitch)
        {
            PlayerID = playerId;
            Yaw = yaw;
            Pitch = pitch;
        }

        public sbyte PlayerID;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x0B;
        public byte Id { get { return 0x0B; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerID = stream.ReadInt8();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerID);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
        }
    }

    public struct DespawnPlayerPacket : IPacket
    {
        public DespawnPlayerPacket(sbyte playerId)
        {
            PlayerId = playerId;
        }

        public sbyte PlayerId;

        public const byte PacketId = 0x0C;
        public byte Id { get { return 0x0C; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerId = stream.ReadInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerId);
        }
    }

    public struct ChatMessagePacket : IPacket
    {
        public ChatMessagePacket(string message, sbyte playerId)
        {
            Message = message;
            PlayerId = playerId;
        }

        public string Message;
        public sbyte PlayerId;

        public const byte PacketId = 0x0D;
        public byte Id { get { return 0x0D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerId = stream.ReadInt8();
            Message = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteInt8(PlayerId);
            stream.WriteString(Message);
        }
    }

    public struct DisconnectPlayerPacket : IPacket
    {
        public DisconnectPlayerPacket(string reason)
        {
            Reason = reason;
        }

        public string Reason;

        public const byte PacketId = 0x0E;
        public byte Id { get { return 0x0E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteString(Reason);
        }
    }

    public struct ChangePlayerPermissionPacket : IPacket
    {
        public ChangePlayerPermissionPacket(bool isOP)
        {
            IsOP = isOP;
        }

        public bool IsOP;

        public const byte PacketId = 0x0E;
        public byte Id { get { return 0x0E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            IsOP = stream.ReadUInt8() == 0x64;
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteUInt8((byte)(IsOP ? 0x64 : 0x00));
        }
    }
}