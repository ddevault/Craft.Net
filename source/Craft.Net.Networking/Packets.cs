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

    public struct SpawnPlayerPacket : IPacket
    {
        public SpawnPlayerPacket(long entityId, string playerName, int x,
            int y, int z, byte yaw, byte pitch, short heldItem, MetadataDictionary metadata)
        {
            EntityId = entityId;
            PlayerName = playerName;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            HeldItem = heldItem;
            Metadata = metadata;
        }

        public long EntityId;
        public string PlayerName;
        public int X, Y, Z;
        public byte Yaw, Pitch;
        public short HeldItem;
        public MetadataDictionary Metadata;

        public const long PacketId = 0x0C;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            PlayerName = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            HeldItem = stream.ReadInt16();
            Metadata = MetadataDictionary.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteString(PlayerName);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            stream.WriteInt16(HeldItem);
            Metadata.WriteTo(stream);
            return mode;
        }
    }

    public struct CollectItemPacket : IPacket
    {
        public CollectItemPacket(int itemId, int playerId)
        {
            ItemId = itemId;
            PlayerId = playerId;
        }
        
        public int ItemId;
        public int PlayerId;

        public const long PacketId = 0x0D;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ItemId = stream.ReadInt32();
            PlayerId = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(ItemId);
            stream.WriteInt32(PlayerId);
            return mode;
        }
    }

    public struct SpawnObjectPacket : IPacket
    {
        public SpawnObjectPacket(long entityId, byte type, int x,
             int y, int z, byte yaw, byte pitch,
             int data, short? speedX, short? speedY, short? speedZ)
        {
            EntityId = entityId;
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Data = data;
            SpeedX = speedX;
            SpeedY = speedY;
            SpeedZ = speedZ;
        }
        
        public long EntityId;
        public byte Type;
        public int X, Y, Z;
        public byte Yaw, Pitch;
        public int Data;
        public short? SpeedX, SpeedY, SpeedZ;

        public const long PacketId = 0x0E;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            Type = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            Data = stream.ReadInt32();
            if (Data != 0)
            {
                SpeedX = stream.ReadInt16();
                SpeedY = stream.ReadInt16();
                SpeedZ = stream.ReadInt16();
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteUInt8(Type);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            stream.WriteInt32(Data);
            if (Data != 0)
            {
                stream.WriteInt16(SpeedX.Value);
                stream.WriteInt16(SpeedY.Value);
                stream.WriteInt16(SpeedZ.Value);
            }
            return mode;
        }
    }

    public struct SpawnMobPacket : IPacket
    {
        public SpawnMobPacket(long entityId, byte type, int x,
            int y, int z, byte yaw, byte pitch, byte headYaw, short velocityX,
            short velocityY, short velocityZ, MetadataDictionary metadata)
        {
            EntityId = entityId;
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            HeadYaw = headYaw;
            VelocityX = velocityX;
            VelocityY = velocityY;
            VelocityZ = velocityZ;
            Metadata = metadata;
        }

        public long EntityId;
        public byte Type;
        public int X, Y, Z;
        public byte Yaw, Pitch, HeadYaw;
        public short VelocityX, VelocityY, VelocityZ;
        public MetadataDictionary Metadata;

        public const long PacketId = 0x0F;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            Type = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            HeadYaw = stream.ReadUInt8();
            VelocityX = stream.ReadInt16();
            VelocityY = stream.ReadInt16();
            VelocityZ = stream.ReadInt16();
            Metadata = MetadataDictionary.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteUInt8(Type);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            stream.WriteUInt8(HeadYaw);
            stream.WriteInt16(VelocityX);
            stream.WriteInt16(VelocityY);
            stream.WriteInt16(VelocityZ);
            Metadata.WriteTo(stream);
            return mode;
        }
    }

    public struct SpawnPaintingPacket : IPacket
    {
        public SpawnPaintingPacket(long entityId, string title, int x,
            int y, int z, int direction)
        {
            EntityId = entityId;
            Title = title;
            X = x;
            Y = y;
            Z = z;
            Direction = direction;
        }
        
        public long EntityId;
        public string Title;
        public int X, Y, Z;
        public int Direction;

        public const long PacketId = 0x10;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            Title = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Direction = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteString(Title);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Direction);
            return mode;
        }
    }

    public struct SpawnExperienceOrbPacket : IPacket
    {
        public SpawnExperienceOrbPacket(long entityId, int x, int y,
            int z, short count)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Count = count;
        }
        
        public long EntityId;
        public int X, Y, Z;
        public short Count;

        public const long PacketId = 0x11;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Count = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteInt16(Count);
            return mode;
        }
    }

    public struct EntityVelocityPacket : IPacket
    {
        public EntityVelocityPacket(int entityId, short velocityX, short velocityY,
            short velocityZ)
        {
            EntityId = entityId;
            VelocityX = velocityX;
            VelocityY = velocityY;
            VelocityZ = velocityZ;
        }

        public int EntityId;
        public short VelocityX, VelocityY, VelocityZ;

        public const long PacketId = 0x12;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            VelocityX = stream.ReadInt16();
            VelocityY = stream.ReadInt16();
            VelocityZ = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt16(VelocityX);
            stream.WriteInt16(VelocityY);
            stream.WriteInt16(VelocityZ);
            return mode;
        }
    }

    public struct DestroyEntityPacket : IPacket
    {
        public DestroyEntityPacket(int[] entityIds)
        {
            EntityIds = entityIds;
        }

        public int[] EntityIds;

        public const long PacketId = 0x13;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            var length = stream.ReadUInt8();
            EntityIds = stream.ReadInt32Array(length);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8((byte)EntityIds.Length);
            stream.WriteInt32Array(EntityIds);
            return mode;
        }
    }

    public struct EntityPacket : IPacket
    {
        public EntityPacket(int entityId)
        {
            EntityId = entityId;
        }

        public int EntityId;

        public const long PacketId = 0x14;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            return mode;
        }
    }

    public struct EntityRelativeMovePacket : IPacket
    {
        public EntityRelativeMovePacket(int entityId, sbyte deltaX, sbyte deltaY,
            sbyte deltaZ)
        {
            EntityId = entityId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
        }

        public int EntityId;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public const long PacketId = 0x15;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
            return mode;
        }
    }

    public struct EntityLookPacket : IPacket
    {
        public EntityLookPacket(int entityId, byte yaw, byte pitch)
        {
            EntityId = entityId;
            Yaw = yaw;
            Pitch = pitch;
        }

        public int EntityId;
        public byte Yaw, Pitch;

        public const long PacketId = 0x16;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            return mode;
        }
    }

    public struct EntityLookAndRelativeMovePacket : IPacket
    {
        public EntityLookAndRelativeMovePacket(int entityId, sbyte deltaX, sbyte deltaY,
            sbyte deltaZ, byte yaw, byte pitch)
        {
            EntityId = entityId;
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;
            Yaw = yaw;
            Pitch = pitch;
        }

        public int EntityId;
        public sbyte DeltaX, DeltaY, DeltaZ;
        public byte Yaw, Pitch;

        public const long PacketId = 0x17;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            return mode;
        }
    }

    public struct EntityTeleportPacket : IPacket
    {
        public EntityTeleportPacket(int entityId, int x, int y,
            int z, byte yaw, byte pitch)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public int EntityId;
        public int X, Y, Z;
        public byte Yaw, Pitch;

        public const long PacketId = 0x18;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            return mode;
        }
    }

    public struct EntityHeadLookPacket : IPacket
    {
        public EntityHeadLookPacket(int entityId, byte headYaw)
        {
            EntityId = entityId;
            HeadYaw = headYaw;
        }

        public int EntityId;
        public byte HeadYaw;

        public const long PacketId = 0x19;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            HeadYaw = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(HeadYaw);
            return mode;
        }
    }

    public struct EntityStatusPacket : IPacket
    {
        public enum EntityStatus
        {
            Hurt = 2,
            Dead = 3,
            WolfTaming = 6,
            WolfTamed = 7,
            /// <summary>
            /// Shaking water off the wolf's body
            /// </summary>
            WolfShaking = 8,
            EatingAccepted = 9,
            /// <summary>
            /// Sheep eating grass
            /// </summary>
            SheepEating = 10
        }

        public EntityStatusPacket(int entityId, EntityStatus status)
        {
            EntityId = entityId;
            Status = status;
        }

        public int EntityId;
        public EntityStatus Status;

        public const long PacketId = 0x1A;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            Status = (EntityStatus)stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8((byte)Status);
            return mode;
        }
    }

    public struct AttachEntityPacket : IPacket
    {
        public AttachEntityPacket(int entityId, int vehicleId, bool leash)
        {
            EntityId = entityId;
            VehicleId = vehicleId;
            Leash = leash;
        }

        public int EntityId, VehicleId;
        public bool Leash;

        public const long PacketId = 0x1B;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            VehicleId = stream.ReadInt32();
            Leash = stream.ReadBoolean();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt32(VehicleId);
            stream.WriteBoolean(Leash);
            return mode;
        }
    }

    public struct EntityMetadataPacket : IPacket
    {
        public EntityMetadataPacket(int entityId, MetadataDictionary metadata)
        {
            EntityId = entityId;
            Metadata = metadata;
        }

        public int EntityId;
        public MetadataDictionary Metadata;

        public const long PacketId = 0x1C;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            Metadata = MetadataDictionary.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            Metadata.WriteTo(stream);
            return mode;
        }
    }

    public struct EntityEffectPacket : IPacket
    {
        public EntityEffectPacket(int entityId, byte effectId, byte amplifier, short duration)
        {
            EntityId = entityId;
            EffectId = effectId;
            Amplifier = amplifier;
            Duration = duration;
        }

        public int EntityId;
        public byte EffectId;
        public byte Amplifier;
        public short Duration;

        public const long PacketId = 0x1D;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            EffectId = stream.ReadUInt8();
            Amplifier = stream.ReadUInt8();
            Duration = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(EffectId);
            stream.WriteUInt8(Amplifier);
            stream.WriteInt16(Duration);
            return mode;
        }
    }

    public struct RemoveEntityEffectPacket : IPacket
    {
        public RemoveEntityEffectPacket(int entityId, byte effectId)
        {
            EntityId = entityId;
            EffectId = effectId;
        }

        public int EntityId;
        public byte EffectId;

        public const long PacketId = 0x1F;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            EffectId = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(EffectId);
            return mode;
        }
    }

    public struct SetExperiencePacket : IPacket
    {
        public SetExperiencePacket(float experienceBar, short level, short totalExperience)
        {
            ExperienceBar = experienceBar;
            Level = level;
            TotalExperience = totalExperience;
        }
        
        public float ExperienceBar;
        public short Level;
        public short TotalExperience;

        public const long PacketId = 0x1F;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ExperienceBar = stream.ReadSingle();
            Level = stream.ReadInt16();
            TotalExperience = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteSingle(ExperienceBar);
            stream.WriteInt16(Level);
            stream.WriteInt16(TotalExperience);
            return mode;
        }
    }

    public struct EntityPropertiesPacket : IPacket
    {
        public EntityPropertiesPacket(int entityId, EntityProperty[] properties)
        {
            EntityId = entityId;
            Properties = properties;
        }

        public int EntityId;
        public EntityProperty[] Properties;

        public const long PacketId = 0x20;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadInt32();
            var count = stream.ReadInt32();
            if (count < 0)
                throw new InvalidOperationException("Cannot specify less than zero properties.");
            Properties = new EntityProperty[count];
            for (int i = 0; i < count; i++)
            {
                var property = new EntityProperty();
                property.Key = stream.ReadString();
                property.Value = stream.ReadDouble();
                var listLength = stream.ReadInt16();
                property.UnknownList = new EntityPropertyListItem[listLength];
                for (int j = 0; j < listLength; j++)
                {
                    var item = new EntityPropertyListItem();
                    item.UnknownMSB = stream.ReadInt64();
                    item.UnknownLSB = stream.ReadInt64();
                    item.UnknownDouble = stream.ReadDouble();
                    item.UnknownByte = stream.ReadUInt8();
                    property.UnknownList[j] = item;
                }
                Properties[i] = property;
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EntityId);
            stream.WriteInt32(Properties.Length);
            for (int i = 0; i < Properties.Length; i++)
            {
                stream.WriteString(Properties[i].Key);
                stream.WriteDouble(Properties[i].Value);
                stream.WriteInt16((short)Properties[i].UnknownList.Length);
                for (int j = 0; j < Properties[i].UnknownList.Length; j++)
                {
                    stream.WriteInt64(Properties[i].UnknownList[j].UnknownMSB);
                    stream.WriteInt64(Properties[i].UnknownList[j].UnknownLSB);
                    stream.WriteDouble(Properties[i].UnknownList[j].UnknownDouble);
                    stream.WriteUInt8(Properties[i].UnknownList[j].UnknownByte);
                }
            }
            return mode;
        }
    }

    public struct EntityProperty
    {
        public EntityProperty(string key, double value)
        {
            Key = key;
            Value = value;
            UnknownList = new EntityPropertyListItem[0];
        }

        public string Key;
        public double Value;
        public EntityPropertyListItem[] UnknownList;
    }

    public struct EntityPropertyListItem
    {
        public long UnknownMSB, UnknownLSB;
        public double UnknownDouble;
        public byte UnknownByte;
    }

    public struct ChunkDataPacket : IPacket
    {
        public ChunkDataPacket(int x, int z, bool groundUpContinuous,
            ushort primaryBitMap, ushort addBitMap, byte[] data)
        {
            X = x;
            Z = z;
            GroundUpContinuous = groundUpContinuous;
            PrimaryBitMap = primaryBitMap;
            AddBitMap = addBitMap;
            Data = data;
        }

        public int X, Z;
        public bool GroundUpContinuous;
        public ushort PrimaryBitMap;
        public ushort AddBitMap;
        public byte[] Data;

        public const long PacketId = 0x21;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Z = stream.ReadInt32();
            GroundUpContinuous = stream.ReadBoolean();
            PrimaryBitMap = stream.ReadUInt16();
            AddBitMap = stream.ReadUInt16();
            var length = stream.ReadInt32();
            Data = stream.ReadUInt8Array(length);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteInt32(Z);
            stream.WriteBoolean(GroundUpContinuous);
            stream.WriteUInt16(PrimaryBitMap);
            stream.WriteUInt16(AddBitMap);
            stream.WriteInt32(Data.Length);
            stream.WriteUInt8Array(Data);
            return mode;
        }
    }

    public struct MultipleBlockChangePacket : IPacket
    {
        public MultipleBlockChangePacket(int chunkX, int chunkZ, short recordCount, int[] data)
        {
            // TODO: Make this packet a little nicer
            ChunkX = chunkX;
            ChunkZ = chunkZ;
            RecordCount = recordCount;
            Data = data;
        }

        public int ChunkX, ChunkZ;
        public short RecordCount;
        public int[] Data;

        public const long PacketId = 0x22;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ChunkX = stream.ReadInt32();
            ChunkZ = stream.ReadInt32();
            RecordCount = stream.ReadInt16();
            stream.ReadInt32();
            Data = stream.ReadInt32Array(RecordCount);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(ChunkX);
            stream.WriteInt32(ChunkZ);
            stream.WriteInt16(RecordCount);
            stream.WriteInt32(RecordCount * 4);
            stream.WriteInt32Array(Data);
            return mode;
        }
    }

    public struct BlockChangePacket : IPacket
    {
        public BlockChangePacket(int x, byte y, int z,
            long blockType, byte blockMetadata)
        {
            X = x;
            Y = y;
            Z = z;
            BlockType = blockType;
            BlockMetadata = blockMetadata;
        }

        public int X;
        public byte Y;
        public int Z;
        public long BlockType;
        public byte BlockMetadata;

        public const long PacketId = 0x23;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            BlockType = stream.ReadVarInt();
            BlockMetadata = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteVarInt(BlockType);
            stream.WriteUInt8(BlockMetadata);
            return mode;
        }
    }

    public struct BlockActionPacket : IPacket
    {
        public BlockActionPacket(int x, short y, int z,
            byte data1, byte data2, long blockId)
        {
            X = x;
            Y = y;
            Z = z;
            Data1 = data1;
            Data2 = data2;
            BlockId = blockId;
        }

        public int X;
        public short Y;
        public int Z;
        public byte Data1;
        public byte Data2;
        public long BlockId;

        public const long PacketId = 0x24;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Data1 = stream.ReadUInt8();
            Data2 = stream.ReadUInt8();
            BlockId = stream.ReadVarInt();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Data1);
            stream.WriteUInt8(Data2);
            stream.WriteVarInt(BlockId);
            return mode;
        }
    }

    public struct BlockBreakAnimationPacket : IPacket
    {
        public BlockBreakAnimationPacket(long entityId, int x, int y,
            int z, byte destroyStage)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            DestroyStage = destroyStage;
        }

        public long EntityId;
        public int X, Y, Z;
        public byte DestroyStage;

        public const long PacketId = 0x25;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            DestroyStage = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(DestroyStage);
            return mode;
        }
    }

    public struct MapChunkBulkPacket : IPacket
    {
        public struct Metadata
        {
            public int ChunkX;
            public int ChunkZ;
            public ushort PrimaryBitMap;
            public ushort AddBitMap;
        }

        public MapChunkBulkPacket(short chunkCount, bool lightIncluded, byte[] chunkData, Metadata[] chunkMetadata)
        {
            ChunkCount = chunkCount;
            LightIncluded = lightIncluded;
            ChunkData = chunkData;
            ChunkMetadata = chunkMetadata;
        }

        public short ChunkCount;
        public bool LightIncluded;
        public byte[] ChunkData;
        public Metadata[] ChunkMetadata;

        public const long PacketId = 0x26;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ChunkCount = stream.ReadInt16();
            var length = stream.ReadInt32();
            LightIncluded = stream.ReadBoolean();
            ChunkData = stream.ReadUInt8Array(length);

            ChunkMetadata = new Metadata[ChunkCount];
            for (int i = 0; i < ChunkCount; i++)
            {
                var metadata = new Metadata();
                metadata.ChunkX = stream.ReadInt32();
                metadata.ChunkZ = stream.ReadInt32();
                metadata.PrimaryBitMap = stream.ReadUInt16();
                metadata.AddBitMap = stream.ReadUInt16();
                ChunkMetadata[i] = metadata;
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt16(ChunkCount);
            stream.WriteInt32(ChunkData.Length);
            stream.WriteBoolean(LightIncluded);
            stream.WriteUInt8Array(ChunkData);

            for (int i = 0; i < ChunkCount; i++)
            {
                stream.WriteInt32(ChunkMetadata[i].ChunkX);
                stream.WriteInt32(ChunkMetadata[i].ChunkZ);
                stream.WriteUInt16(ChunkMetadata[i].PrimaryBitMap);
                stream.WriteUInt16(ChunkMetadata[i].AddBitMap);
            }
            return mode;
        }
    }

    public struct ExplosionPacket : IPacket
    {
        public ExplosionPacket(float x, float y, float z,
            float radius, int recordCount, byte[] records,
            float playerVelocityX, float playerVelocityY, float playerVelocityZ)
        {
            // TODO: Improve this packet
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
            RecordCount = recordCount;
            Records = records;
            PlayerVelocityX = playerVelocityX;
            PlayerVelocityY = playerVelocityY;
            PlayerVelocityZ = playerVelocityZ;
        }
        
        public float X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records;
        public float PlayerVelocityX, PlayerVelocityY, PlayerVelocityZ;

        public const long PacketId = 0x27;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadSingle();
            Y = stream.ReadSingle();
            Z = stream.ReadSingle();
            Radius = stream.ReadSingle();
            RecordCount = stream.ReadInt32();
            Records = stream.ReadUInt8Array(RecordCount * 3);
            PlayerVelocityX = stream.ReadSingle();
            PlayerVelocityY = stream.ReadSingle();
            PlayerVelocityZ = stream.ReadSingle();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteSingle(X);
            stream.WriteSingle(Y);
            stream.WriteSingle(Z);
            stream.WriteSingle(Radius);
            stream.WriteInt32(RecordCount);
            stream.WriteUInt8Array(Records);
            stream.WriteSingle(PlayerVelocityX);
            stream.WriteSingle(PlayerVelocityY);
            stream.WriteSingle(PlayerVelocityZ);
            return mode;
        }
    }

    public struct EffectPacket : IPacket
    {
        public EffectPacket(int effectId, int x, byte y, int z,
            int data, bool disableRelativeVolume)
        {
            EffectId = effectId;
            X = x;
            Y = y;
            Z = z;
            Data = data;
            DisableRelativeVolume = disableRelativeVolume;
        }

        public int EffectId;
        public int X;
        public byte Y;
        public int Z;
        public int Data;
        public bool DisableRelativeVolume;

        public const long PacketId = 0x28;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EffectId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Data = stream.ReadInt32();
            DisableRelativeVolume = stream.ReadBoolean();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(EffectId);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Data);
            stream.WriteBoolean(DisableRelativeVolume);
            return mode;
        }
    }

    public struct SoundEffectPacket : IPacket
    {
        public SoundEffectPacket(string soundName, int x, int y,
            int z, float volume, byte pitch)
        {
            SoundName = soundName;
            X = x;
            Y = y;
            Z = z;
            Volume = volume;
            Pitch = pitch;
        }

        public string SoundName;
        public int X, Y, Z;
        public float Volume;
        public byte Pitch;

        public const long PacketId = 0x29;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            SoundName = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Volume = stream.ReadSingle();
            Pitch = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(SoundName);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteSingle(Volume);
            stream.WriteUInt8(Pitch);
            return mode;
        }
    }

    public struct ParticleEffectPacket : IPacket
    {
        public ParticleEffectPacket(string effectName, float x, float y, float z,
            float offsetX, float offsetY, float offsetZ, float particleSpeed,
            int particleCount)
        {
            EffectName = effectName;
            X = x;
            Y = y;
            Z = z;
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
            ParticleSpeed = particleSpeed;
            ParticleCount = particleCount;
        }
        
        public string EffectName;
        public float X, Y, Z;
        public float OffsetX, OffsetY, OffsetZ;
        public float ParticleSpeed;
        public int ParticleCount;

        public const long PacketId = 0x2A;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EffectName = stream.ReadString();
            X = stream.ReadSingle();
            Y = stream.ReadSingle();
            Z = stream.ReadSingle();
            OffsetX = stream.ReadSingle();
            OffsetY = stream.ReadSingle();
            OffsetZ = stream.ReadSingle();
            ParticleSpeed = stream.ReadSingle();
            ParticleCount = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(EffectName);
            stream.WriteSingle(X);
            stream.WriteSingle(Y);
            stream.WriteSingle(Z);
            stream.WriteSingle(OffsetX);
            stream.WriteSingle(OffsetY);
            stream.WriteSingle(OffsetZ);
            stream.WriteSingle(ParticleSpeed);
            stream.WriteInt32(ParticleCount);
            return mode;
        }
    }

    public struct ChangeGameStatePacket : IPacket
    {
        public enum GameState
        {
            InvalidBed = 0,
            BeginRaining = 1,
            EndRaining = 2,
            ChangeGameMode = 3,
            EnterCredits = 4
        }

        public ChangeGameStatePacket(GameState state)
        {
            State = state;
            GameMode = GameMode.Survival;
        }

        public ChangeGameStatePacket(GameMode gameMode)
        {
            State = GameState.ChangeGameMode;
            GameMode = gameMode;
        }
        
        public GameState State;
        public GameMode GameMode;

        public const long PacketId = 0x2B;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            State = (GameState)stream.ReadUInt8();
            GameMode = (GameMode)stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8((byte)State);
            stream.WriteUInt8((byte)GameMode);
            return mode;
        }
    }

    public struct SpawnGlobalEntityPacket : IPacket
    {
        public SpawnGlobalEntityPacket(long entityId, byte type, int x, int y, int z)
        {
            EntityId = entityId;
            Type = type;
            X = x;
            Y = y;
            Z = z;
        }
        
        public long EntityId;
        public byte Type;
        public int X, Y, Z;

        public const long PacketId = 0x2C;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            EntityId = stream.ReadVarInt();
            Type = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteUInt8(Type);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            return mode;
        }
    }

    public struct OpenWindowPacket : IPacket
    {
        public OpenWindowPacket(byte windowId, byte inventoryType, string windowTitle,
            byte slotCount, bool useProvidedTitle, int? entityId)
        {
            WindowId = windowId;
            InventoryType = inventoryType;
            WindowTitle = windowTitle;
            SlotCount = slotCount;
            UseProvidedTitle = useProvidedTitle;
            EntityId = entityId;
        }

        public byte WindowId;
        public byte InventoryType;
        public string WindowTitle;
        public byte SlotCount;
        public bool UseProvidedTitle;
        public int? EntityId;

        public const long PacketId = 0x2D;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            InventoryType = stream.ReadUInt8();
            WindowTitle = stream.ReadString();
            SlotCount = stream.ReadUInt8();
            UseProvidedTitle = stream.ReadBoolean();
            if (InventoryType == 11)
                EntityId = stream.ReadInt32();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            stream.WriteUInt8(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteUInt8(SlotCount);
            stream.WriteBoolean(UseProvidedTitle);
            if (InventoryType == 11)
                stream.WriteInt32(EntityId.GetValueOrDefault());
            return mode;
        }
    }

    public struct CloseWindowPacket : IPacket
    {
        public CloseWindowPacket(byte windowId)
        {
            WindowId = windowId;
        }

        public byte WindowId;

        public const long PacketId = 0x2E;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            return mode;
        }
    }

    public struct SetSlotPacket : IPacket
    {
        public SetSlotPacket(byte windowId, short slotIndex, ItemStack item)
        {
            WindowId = windowId;
            SlotIndex = slotIndex;
            Item = item;
        }
        
        public byte WindowId;
        public short SlotIndex;
        public ItemStack Item;

        public const long PacketId = 0x2F;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            SlotIndex = stream.ReadInt16();
            Item = ItemStack.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(SlotIndex);
            Item.WriteTo(stream);
            return mode;
        }
    }

    public struct SetWindowItemsPacket : IPacket
    {
        public SetWindowItemsPacket(byte windowId, ItemStack[] items)
        {
            WindowId = windowId;
            Items = items;
        }
        
        public byte WindowId;
        public ItemStack[] Items;

        public const long PacketId = 0x30;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            short count = stream.ReadInt16();
            Items = new ItemStack[count];
            for (int i = 0; i < count; i++)
                Items[i] = ItemStack.FromStream(stream);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            stream.WriteInt16((short)Items.Length);
            for (int i = 0; i < Items.Length; i++)
                Items[i].WriteTo(stream);
            return mode;
        }
    }

    public struct UpdateWindowPropertyPacket : IPacket
    {
        public UpdateWindowPropertyPacket(byte windowId, short propertyId, short value)
        {
            WindowId = windowId;
            PropertyId = propertyId;
            Value = value;
        }
        
        public byte WindowId;
        public short PropertyId;
        public short Value;

        public const long PacketId = 0x31;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            PropertyId = stream.ReadInt16();
            Value = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(PropertyId);
            stream.WriteInt16(Value);
            return mode;
        }
    }

    public struct ConfirmTransactionPacket : IPacket
    {
        public ConfirmTransactionPacket(byte windowId, short actionNumber, bool accepted)
        {
            WindowId = windowId;
            ActionNumber = actionNumber;
            Accepted = accepted;
        }
        
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

        public const long PacketId = 0x32;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            WindowId = stream.ReadUInt8();
            ActionNumber = stream.ReadInt16();
            Accepted = stream.ReadBoolean();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(ActionNumber);
            stream.WriteBoolean(Accepted);
            return mode;
        }
    }

    public struct UpdateSignPacket : IPacket
    {
        public UpdateSignPacket(int x, short y, int z,
            string text1, string text2, string text3, string text4)
        {
            X = x;
            Y = y;
            Z = z;
            Text1 = text1;
            Text2 = text2;
            Text3 = text3;
            Text4 = text4;
        }

        public int X;
        public short Y;
        public int Z;
        public string Text1, Text2, Text3, Text4;

        public const long PacketId = 0x33;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Text1 = stream.ReadString();
            Text2 = stream.ReadString();
            Text3 = stream.ReadString();
            Text4 = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteString(Text1);
            stream.WriteString(Text2);
            stream.WriteString(Text3);
            stream.WriteString(Text4);
            return mode;
        }
    }

    public struct MapDataPacket : IPacket
    {
        public MapDataPacket(long metadata, byte[] data)
        {
            Metadata = metadata;
            Data = data;
        }

        public long Metadata;
        public byte[] Data;

        public const long PacketId = 0x34;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Metadata = stream.ReadVarInt();
            var length = stream.ReadInt16();
            Data = stream.ReadUInt8Array(length);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(Metadata);
            stream.WriteInt16((short)Data.Length);
            stream.WriteUInt8Array(Data);
            return mode;
        }
    }

    public struct UpdateTileEntityPacket : IPacket
    {
        public UpdateTileEntityPacket(int x, short y, int z, byte action, NbtFile nbt)
        {
            X = x;
            Y = y;
            Z = z;
            Action = action;
            Nbt = nbt;
        }
        
        public int X;
        public short Y;
        public int Z;
        public byte Action;
        public NbtFile Nbt;

        public const long PacketId = 0x35;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Action = stream.ReadUInt8();
            var length = stream.ReadInt16();
            var data = stream.ReadUInt8Array(length);
            Nbt = new NbtFile();
            Nbt.LoadFromBuffer(data, 0, length, NbtCompression.GZip, null);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Action);
            var tempStream = new MemoryStream();
            Nbt.SaveToStream(tempStream, NbtCompression.GZip);
            var buffer = tempStream.GetBuffer();
            stream.WriteInt16((short)buffer.Length);
            stream.WriteUInt8Array(buffer);
            return mode;
        }
    }

    public struct OpenSignEditorPacket : IPacket
    {
        public OpenSignEditorPacket(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X, Y, Z;

        public const long PacketId = 0x36;
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

    public struct Statistic
    {
        public Statistic(string name, long value)
        {
            Name = name;
            Value = value;
        }

        public string Name;
        public long Value;
    }

    public struct UpdateStatisticsPacket : IPacket
    {
        public UpdateStatisticsPacket(Statistic[] statistics)
        {
            Statistics = statistics;
        }

        public Statistic[] Statistics;

        public const long PacketId = 0x37;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            var length = stream.ReadVarInt();
            Statistics = new Statistic[length];
            for (long i = 0; i < length; i++)
            {
                Statistics[i] = new Statistic(
                    stream.ReadString(),
                    stream.ReadVarInt());
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(Statistics.LongLength);
            for (long i = 0; i < Statistics.LongLength; i++)
            {
                stream.WriteString(Statistics[i].Name);
                stream.WriteVarInt(Statistics[i].Value);
            }
            return mode;
        }
    }

    public struct PlayerListItemPacket : IPacket
    {
        public PlayerListItemPacket(string playerName, bool online, short ping)
        {
            PlayerName = playerName;
            Online = online;
            Ping = ping;
        }

        public string PlayerName;
        public bool Online;
        public short Ping;

        public const long PacketId = 0x38;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            PlayerName = stream.ReadString();
            Online = stream.ReadBoolean();
            Ping = stream.ReadInt16();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(PlayerName);
            stream.WriteBoolean(Online);
            stream.WriteInt16(Ping);
            return mode;
        }
    }

    public struct PlayerAbilitiesPacket : IPacket
    {
        public PlayerAbilitiesPacket(byte flags, float flyingSpeed, float walkingSpeed)
        {
            Flags = flags;
            FlyingSpeed = flyingSpeed;
            WalkingSpeed = walkingSpeed;
        }

        public byte Flags;
        public float FlyingSpeed, WalkingSpeed;

        public const long PacketId = 0x39;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Flags = stream.ReadUInt8();
            FlyingSpeed = stream.ReadSingle();
            WalkingSpeed = stream.ReadSingle();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8(Flags);
            stream.WriteSingle(FlyingSpeed);
            stream.WriteSingle(WalkingSpeed);
            return mode;
        }
    }

    public struct TabCompletePacket : IPacket
    {
        public TabCompletePacket(string[] completions)
        {
            Completions = completions;
        }

        public string[] Completions;

        public const long PacketId = 0x3A;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            var count = stream.ReadVarInt();
            Completions = new string[count];
            for (long i = 0; i < Completions.LongLength; i++)
                Completions[i] = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteVarInt(Completions.LongLength);
            for (long i = 0; i < Completions.LongLength; i++)
                stream.WriteString(Completions[i]);
            return mode;
        }
    }

    public struct ScoreboardObjectivePacket : IPacket
    {
        public enum UpdateMode
        {
            Remove = 0,
            Create = 1,
            Update = 2
        }

        public ScoreboardObjectivePacket(string name, string displayName, UpdateMode mode)
        {
            Name = name;
            DisplayName = displayName;
            Mode = mode;
        }

        public string Name;
        public string DisplayName;
        public UpdateMode Mode;

        public const long PacketId = 0x3B;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Name = stream.ReadString();
            DisplayName = stream.ReadString();
            Mode = (UpdateMode)stream.ReadUInt8();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(Name);
            stream.WriteString(DisplayName);
            stream.WriteUInt8((byte)Mode);
            return mode;
        }
    }

    public struct UpdateScorePacket : IPacket
    {
        public UpdateScorePacket(string itemName)
        {
            ItemName = itemName;
            RemoveItem = true;
            ScoreName = null;
            Value = null;
        }

        public UpdateScorePacket(string itemName, string scoreName, int value)
        {
            ItemName = itemName;
            RemoveItem = false;
            ScoreName = scoreName;
            Value = value;
        }

        public string ItemName;
        public bool RemoveItem;
        public string ScoreName;
        public int? Value;

        public const long PacketId = 0x3C;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            ItemName = stream.ReadString();
            RemoveItem = stream.ReadBoolean();
            if (!RemoveItem)
            {
                ScoreName = stream.ReadString();
                Value = stream.ReadInt32();
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(ItemName);
            stream.WriteBoolean(RemoveItem);
            if (!RemoveItem)
            {
                stream.WriteString(ScoreName);
                stream.WriteInt32(Value.Value);
            }
            return mode;
        }
    }

    public struct DisplayScoreboardPacket : IPacket
    {
        public DisplayScoreboardPacket(ScoreboardPosition position, string scoreName)
        {
            Position = position;
            ScoreName = scoreName;
        }

        public enum ScoreboardPosition
        {
            PlayerList = 0,
            Sidebar = 1,
            BelowPlayerName = 2
        }

        public ScoreboardPosition Position;
        public string ScoreName;

        public const long PacketId = 0x3D;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Position = (ScoreboardPosition)stream.ReadUInt8();
            ScoreName = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteUInt8((byte)Position);
            stream.WriteString(ScoreName);
            return mode;
        }
    }

    public struct SetTeamsPacket : IPacket
    {
        public static SetTeamsPacket CreateTeam(string teamName, string displayName, string teamPrefix,
                                                string teamSuffix, bool enableFriendlyFire, string[] players)
        {
            var packet = new SetTeamsPacket();
            packet.PacketMode = TeamMode.CreateTeam;
            packet.TeamName = teamName;
            packet.DisplayName = displayName;
            packet.TeamPrefix = teamPrefix;
            packet.TeamSuffix = teamSuffix;
            packet.EnableFriendlyFire = enableFriendlyFire;
            packet.Players = players;
            return packet;
        }

        public static SetTeamsPacket UpdateTeam(string teamName, string displayName, string teamPrefix,
                                                string teamSuffix, bool enableFriendlyFire)
        {
            var packet = new SetTeamsPacket();
            packet.PacketMode = TeamMode.UpdateTeam;
            packet.TeamName = teamName;
            packet.DisplayName = displayName;
            packet.TeamPrefix = teamPrefix;
            packet.TeamSuffix = teamSuffix;
            packet.EnableFriendlyFire = enableFriendlyFire;
            return packet;
        }

        public static SetTeamsPacket RemoveTeam(string teamName)
        {
            var packet = new SetTeamsPacket();
            packet.PacketMode = TeamMode.RemoveTeam;
            packet.TeamName = teamName;
            return packet;
        }

        public static SetTeamsPacket AddPlayers(string teamName, string[] players)
        {
            var packet = new SetTeamsPacket();
            packet.PacketMode = TeamMode.AddPlayers;
            packet.TeamName = teamName;
            packet.Players = players;
            return packet;
        }

        public static SetTeamsPacket RemovePlayers(string teamName, string[] players)
        {
            var packet = new SetTeamsPacket();
            packet.PacketMode = TeamMode.RemovePlayers;
            packet.TeamName = teamName;
            packet.Players = players;
            return packet;
        }

        public enum TeamMode
        {
            CreateTeam = 0,
            RemoveTeam = 1,
            UpdateTeam = 2,
            AddPlayers = 3,
            RemovePlayers = 4
        }

        public string TeamName;
        public TeamMode PacketMode;
        public string DisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public bool? EnableFriendlyFire;
        public string[] Players;

        public const long PacketId = 0x3E;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            TeamName = stream.ReadString();
            PacketMode = (TeamMode)stream.ReadUInt8();
            if (PacketMode == TeamMode.CreateTeam || PacketMode == TeamMode.UpdateTeam)
            {
                DisplayName = stream.ReadString();
                TeamPrefix = stream.ReadString();
                TeamSuffix = stream.ReadString();
                EnableFriendlyFire = stream.ReadBoolean();
            }
            if (PacketMode == TeamMode.CreateTeam || PacketMode == TeamMode.AddPlayers ||
                PacketMode == TeamMode.RemovePlayers)
            {
                var playerCount = stream.ReadInt16();
                Players = new string[playerCount];
                for (int i = 0; i < playerCount; i++)
                    Players[i] = stream.ReadString();
            }
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(TeamName);
            stream.WriteUInt8((byte)PacketMode);
            if (PacketMode == TeamMode.CreateTeam || PacketMode == TeamMode.UpdateTeam)
            {
                stream.WriteString(DisplayName);
                stream.WriteString(TeamPrefix);
                stream.WriteString(TeamSuffix);
                stream.WriteBoolean(EnableFriendlyFire.Value);
            }
            if (PacketMode == TeamMode.CreateTeam || PacketMode == TeamMode.AddPlayers ||
                PacketMode == TeamMode.RemovePlayers)
            {
                stream.WriteInt16((short)Players.Length);
                for (int i = 0; i < Players.Length; i++)
                    stream.WriteString(Players[i]);
            }
            return mode;
        }
    }

    public struct PluginMessagePacket : IPacket
    {
        public PluginMessagePacket(string channel, byte[] data)
        {
            Channel = channel;
            Data = data;
        }

        public string Channel;
        public byte[] Data;

        public const long PacketId = 0x3F;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Channel = stream.ReadString();
            var length = stream.ReadInt16();
            Data = stream.ReadUInt8Array(length);
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(Channel);
            stream.WriteInt16((short)Data.Length);
            stream.WriteUInt8Array(Data);
            return mode;
        }
    }

    public struct DisconnectPacket : IPacket
    {
        public DisconnectPacket(string reason)
        {
            Reason = reason;
        }

        public string Reason;

        public const long PacketId = 0x40;
        public long Id { get { return PacketId; } }

        public NetworkMode ReadPacket(MinecraftStream stream, NetworkMode mode)
        {
            Reason = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(MinecraftStream stream, NetworkMode mode)
        {
            stream.WriteString(Reason);
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