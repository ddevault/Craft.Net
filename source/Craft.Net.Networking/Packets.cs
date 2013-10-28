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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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
        public long Id { get { return PacketId; } }

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

    public struct JoinGamePacket : IPacket
    {
        public JoinGamePacket(int entityId, GameMode gameMode, Dimension dimension,
            Difficulty difficulty, byte maxPlayers, string levelType)
        {
            EntityId = entityId;
            GameMode = gameMode;
            Dimension = dimension;
            Difficulty = difficulty;
            MaxPlayers = maxPlayers;
            LevelType = levelType;
        }

        public int EntityId;
        public GameMode GameMode;
        public Dimension Dimension;
        public Difficulty Difficulty;
        public byte MaxPlayers;
        public string LevelType;

        public const long PacketId = 0x01;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            GameMode = (GameMode)stream.ReadUInt8();
            Dimension = (Dimension)stream.ReadInt8();
            Difficulty = (Difficulty)stream.ReadUInt8();
            MaxPlayers = stream.ReadUInt8();
            LevelType = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8((byte)GameMode);
            stream.WriteInt8((sbyte)Dimension);
            stream.WriteUInt8((byte)Difficulty);
            stream.WriteUInt8(MaxPlayers);
            stream.WriteString(LevelType);
            return mode;
        }
    }

    public struct ChatMessagePacket : IPacket
    {
        public ChatMessagePacket(string message)
        {
            Message = message;
        }

        public string Message; // TODO: Deserialize JSON and strongly type this

        public const long PacketId = 0x02;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Message = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(Message);
            return mode;
        }
    }

    public struct TimeUpdatePacket : IPacket
    {
        public TimeUpdatePacket(long worldAge, long timeOfDay)
        {
            WorldAge = worldAge;
            TimeOfDay = timeOfDay;
        }

        public long WorldAge;
        public long TimeOfDay;

        public const long PacketId = 0x03;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WorldAge = stream.ReadInt64();
            TimeOfDay = stream.ReadInt64();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt64(WorldAge);
            stream.WriteInt64(TimeOfDay);
            return mode;
        }
    }

    public struct EntityEquipmentPacket : IPacket
    {
        public EntityEquipmentPacket(int entityId, short slot, ItemStack item)
        {
            EntityId = entityId;
            Slot = slot;
            Item = item;
        }

        public int EntityId;
        public short Slot;
        public ItemStack Item;

        public const long PacketId = 0x04;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            Slot = stream.ReadInt16();
            Item = ItemStack.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt16(Slot);
            Item.WriteTo(stream);
            return mode;
        }
    }

    public struct SpawnPositionPacket : IPacket
    {
        public SpawnPositionPacket(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X, Y, Z;

        public const long PacketId = 0x05;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            return mode;
        }
    }

    public struct UpdateHealthPacket : IPacket
    {
        public UpdateHealthPacket(float health, short food, float foodSaturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }

        public float Health;
        public short Food;
        public float FoodSaturation;

        public const long PacketId = 0x06;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Health = stream.ReadSingle();
            Food = stream.ReadInt16();
            FoodSaturation = stream.ReadSingle();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteSingle(Health);
            stream.WriteInt16(Food);
            stream.WriteSingle(FoodSaturation);
            return mode;
        }
    }

    public struct RespawnPacket : IPacket
    {
        public RespawnPacket(Dimension dimension, Difficulty difficulty, GameMode gameMode, string levelType)
        {
            Dimension = dimension;
            Difficulty = difficulty;
            GameMode = gameMode;
            LevelType = levelType;
        }

        public Dimension Dimension;
        public Difficulty Difficulty;
        public GameMode GameMode;
        public string LevelType;

        public const long PacketId = 0x07;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Dimension = (Dimension)stream.ReadInt32();
            Difficulty = (Difficulty)stream.ReadUInt8();
            GameMode = (GameMode)stream.ReadUInt8();
            LevelType = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32((int)Dimension);
            stream.WriteUInt8((byte)Difficulty);
            stream.WriteUInt8((byte)GameMode);
            stream.WriteString(LevelType);
            return mode;
        }
    }

    public struct PlayerPositionAndLookPacket : IPacket
    {
        public PlayerPositionAndLookPacket(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public double X, Y, Z;
        public float Yaw, Pitch;
        public bool OnGround;

        public const long PacketId = 0x08;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
            stream.WriteSingle(Yaw);
            stream.WriteSingle(Pitch);
            stream.WriteBoolean(OnGround);
            return mode;
        }
    }

    public struct HeldItemPacket : IPacket
    {
        public HeldItemPacket(sbyte slot)
        {
            Slot = slot;
        }

        public sbyte Slot;

        public const long PacketId = 0x09;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Slot = stream.ReadInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt8(Slot);
            return mode;
        }
    }

    public struct UseBedPacket : IPacket
    {
        public UseBedPacket(int entityId, int x, byte y, int z)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
        }

        public int EntityId;
        public int X;
        public byte Y;
        public int Z;

        public const long PacketId = 0x0A;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            return mode;
        }
    }

    public struct AnimationPacket : IPacket
    {
        public enum AnimationType
        {
            NoAnimation = 0,
            SwingArm = 1,
            Damage = 2,
            LeaveBed = 3,
            EatFood = 4,
            Crouch = 104,
            Uncrouch = 105
        }

        public AnimationPacket(long entityId, AnimationType animation)
        {
            EntityId = entityId;
            Animation = animation;
        }

        public long EntityId; // Seriously, Mojang?
        public AnimationType Animation;

        public const long PacketId = 0x0B;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            Animation = (AnimationType)stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteUInt8((byte)Animation);
            return mode;
        }
    }

    #endregion
}
/*
    public struct KeepAlivePacket : IPacket
    {
        public KeepAlivePacket()
        {
        }

        public const long PacketId = 0x00;
        public long Id { get { return PacketId; } }

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