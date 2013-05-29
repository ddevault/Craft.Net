using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Common;
using fNbt;

namespace Craft.Net.Networking
{
    public interface IPacket
    {
        byte Id { get; }
        void ReadPacket(MinecraftStream stream);
        void WritePacket(MinecraftStream stream);
    }

    public struct KeepAlivePacket : IPacket
    {
        public KeepAlivePacket(int keepAlive)
        {
            KeepAlive = keepAlive;
        }

        public int KeepAlive;

        public const byte PacketId = 0x00;
        public byte Id { get { return 0x00; } }

        public void ReadPacket(MinecraftStream stream)
        {
            KeepAlive = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(KeepAlive);
        }
    }

    public struct LoginRequestPacket : IPacket
    {
        public LoginRequestPacket(int entityId, string levelType, GameMode gameMode,
            Dimension dimension, Difficulty difficulty, byte maxPlayers)
        {
            EntityId = entityId;
            LevelType = levelType;
            GameMode = gameMode;
            Dimension = dimension;
            Difficulty = difficulty;
            MaxPlayers = maxPlayers;
            Discarded = 0;
        }

        public int EntityId;
        public string LevelType;
        public GameMode GameMode;
        public Dimension Dimension;
        public Difficulty Difficulty;
        public byte Discarded;
        public byte MaxPlayers;

        public const byte PacketId = 0x01;
        public byte Id { get { return 0x01; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            LevelType = stream.ReadString();
            GameMode = (GameMode)stream.ReadUInt8();
            Dimension = (Dimension)stream.ReadInt8();
            Difficulty = (Difficulty)stream.ReadUInt8();
            Discarded = stream.ReadUInt8();
            MaxPlayers = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteString(LevelType);
            stream.WriteUInt8((byte)GameMode);
            stream.WriteInt8((sbyte)Dimension);
            stream.WriteUInt8((byte)Difficulty);
            stream.WriteUInt8(Discarded);
            stream.WriteUInt8(MaxPlayers);
        }
    }

    public struct HandshakePacket : IPacket
    {
        public HandshakePacket(byte protocolVersion, string username, string hostname, 
            int port)
        {
            ProtocolVersion = protocolVersion;
            Username = username;
            ServerHostname = hostname;
            ServerPort = port;
        }

        public byte ProtocolVersion;
        public string Username;
        public string ServerHostname;
        public int ServerPort;

        public const byte PacketId = 0x02;
        public byte Id { get { return 0x02; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ProtocolVersion = stream.ReadUInt8();
            Username = stream.ReadString();
            ServerHostname = stream.ReadString();
            ServerPort = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(ProtocolVersion);
            stream.WriteString(Username);
            stream.WriteString(ServerHostname);
            stream.WriteInt32(ServerPort);
        }
    }

    public struct ChatMessagePacket : IPacket
    {
        public ChatMessagePacket(string message)
        {
            Message = message;
        }

        public string Message;

        public const byte PacketId = 0x03;
        public byte Id { get { return 0x03; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Message = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Message);
        }
    }

    public struct TimeUpdatePacket : IPacket
    {
        public TimeUpdatePacket(long worldAge, long timeOfDay)
        {
            WorldAge = worldAge;
            TimeOfDay = timeOfDay;
        }

        public long WorldAge, TimeOfDay;

        public const byte PacketId = 0x04;
        public byte Id { get { return 0x04; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WorldAge = stream.ReadInt64();
            TimeOfDay = stream.ReadInt64();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt64(WorldAge);
            stream.WriteInt64(TimeOfDay);
        }
    }

    public struct EntityEquipmentPacket : IPacket
    {
        public enum EntityEquipmentSlot
        {
            HeldItem = 0,
            Headgear = 1,
            Chestplate = 2,
            Pants = 3,
            Footwear = 4
        }

        public EntityEquipmentPacket(int entityId, EntityEquipmentSlot slotIndex, ItemStack slot)
        {
            EntityId = entityId;
            SlotIndex = slotIndex;
            Slot = slot;
        }

        public int EntityId;
        public EntityEquipmentSlot SlotIndex;
        public ItemStack Slot;

        public const byte PacketId = 0x05;
        public byte Id { get { return 0x05; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            SlotIndex = (EntityEquipmentSlot)stream.ReadInt16();
            Slot = ItemStack.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt16((short)SlotIndex);
            Slot.WriteTo(stream);
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

        public const byte PacketId = 0x06;
        public byte Id { get { return 0x06; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
        }
    }

    public struct UseEntityPacket : IPacket
    {
        public UseEntityPacket(int user, int target, bool mouseButton)
        {
            User = user;
            Target = target;
            LeftClick = mouseButton;
        }

        public int User, Target;
        public bool LeftClick;

        public const byte PacketId = 0x07;
        public byte Id { get { return 0x07; } }

        public void ReadPacket(MinecraftStream stream)
        {
            User = stream.ReadInt32();
            Target = stream.ReadInt32();
            LeftClick = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(User);
            stream.WriteInt32(Target);
            stream.WriteBoolean(LeftClick);
        }
    }

    public struct UpdateHealthPacket : IPacket
    {
        public UpdateHealthPacket(short health, short food, float saturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = saturation;
        }

        public short Health, Food;
        public float FoodSaturation;

        public const byte PacketId = 0x08;
        public byte Id { get { return 0x08; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Health = stream.ReadInt16();
            Food = stream.ReadInt16();
            FoodSaturation = stream.ReadSingle();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16(Health);
            stream.WriteInt16(Food);
            stream.WriteSingle(FoodSaturation);
        }
    }

    public struct RespawnPacket : IPacket
    {
        public RespawnPacket(Dimension dimension, Difficulty difficulty, GameMode gameMode,
            short worldHeight, string levelType)
        {
            Dimension = dimension;
            Difficulty = difficulty;
            GameMode = gameMode;
            WorldHeight = worldHeight;
            LevelType = levelType;
        }

        public Dimension Dimension;
        public Difficulty Difficulty;
        public GameMode GameMode;
        public short WorldHeight;
        public string LevelType;

        public const byte PacketId = 0x09;
        public byte Id { get { return 0x09; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Dimension = (Dimension)stream.ReadInt32();
            Difficulty = (Difficulty)stream.ReadInt8();
            GameMode = (GameMode)stream.ReadInt8();
            WorldHeight = stream.ReadInt16();
            LevelType = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32((int)Dimension);
            stream.WriteInt8((sbyte)Difficulty);
            stream.WriteInt8((sbyte)GameMode);
            stream.WriteInt16(WorldHeight);
            stream.WriteString(LevelType);
        }
    }

    public struct PlayerPacket : IPacket
    {
        public PlayerPacket(bool onGround)
        {
            OnGround = onGround;
        }

        public bool OnGround;

        public const byte PacketId = 0x0A;
        public byte Id { get { return 0x0A; } }

        public void ReadPacket(MinecraftStream stream)
        {
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteBoolean(OnGround);
        }
    }

    public struct PlayerPositionPacket : IPacket
    {
        public PlayerPositionPacket(double x, double y, double z, double stance, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Stance = stance;
            OnGround = onGround;
        }

        public double X, Y, Stance, Z;
        public bool OnGround;

        public const byte PacketId = 0x0B;
        public byte Id { get { return 0x0B; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Stance = stream.ReadDouble();
            Z = stream.ReadDouble();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Stance);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
        }
    }

    public struct PlayerLookPacket : IPacket
    {
        public PlayerLookPacket(float yaw, float pitch, bool onGround)
        {
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketId = 0x0C;
        public byte Id { get { return 0x0C; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteSingle(Yaw);
            stream.WriteSingle(Pitch);
            stream.WriteBoolean(OnGround);
        }
    }

    public struct PlayerPositionAndLookPacket : IPacket
    {
        public PlayerPositionAndLookPacket(double x, double y, double z, double stance,
            float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Stance = stance;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public double X, Y, Stance, Z;
        public float Yaw, Pitch;
        public bool OnGround;

        public const byte PacketId = 0x0D;
        public byte Id { get { return 0x0D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Stance = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadSingle();
            Pitch = stream.ReadSingle();
            OnGround = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Stance);
            stream.WriteDouble(Z);
            stream.WriteSingle(Yaw);
            stream.WriteSingle(Pitch);
            stream.WriteBoolean(OnGround);
        }
    }

    public struct PlayerDiggingPacket : IPacket
    {
        public enum PlayerAction
        {
            StartedDigging = 0,
            FinishedDigging = 2,
            DropStack = 3,
            DropItem = 4,
            ShootArrow = 5
        }

        public PlayerDiggingPacket(PlayerAction action, int x, byte y,
            int z, byte face)
        {
            Action = action;
            X = x;
            Y = y;
            Z = z;
            Face = face;
        }

        public PlayerAction Action;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public const byte PacketId = 0x0E;
        public byte Id { get { return 0x0E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Action = (PlayerAction)stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Face = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8((byte)Action);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Face);
        }
    }

    public struct RightClickPacket : IPacket
    {
        public RightClickPacket(int x, byte y, int z,
            byte direction, ItemStack heldItem, byte cursorX,
            byte cursorY, byte cursorZ)
        {
            X = x;
            Y = y;
            Z = z;
            Direction = direction;
            HeldItem = heldItem;
            CursorX = cursorX;
            CursorY = cursorY;
            CursorZ = cursorZ;
        }

        public int X;
        public byte Y;
        public int Z;
        public byte Direction;
        public ItemStack HeldItem;
        public byte CursorX;
        public byte CursorY;
        public byte CursorZ;

        public const byte PacketId = 0x0F;
        public byte Id { get { return 0x0F; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Direction = stream.ReadUInt8();
            HeldItem = ItemStack.FromStream(stream);
            CursorX = stream.ReadUInt8();
            CursorY = stream.ReadUInt8();
            CursorZ = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Direction);
            HeldItem.WriteTo(stream);
            stream.WriteUInt8(CursorX);
            stream.WriteUInt8(CursorY);
            stream.WriteUInt8(CursorZ);
        }
    }

    public struct HeldItemChangePacket : IPacket
    {
        public HeldItemChangePacket(short slotIndex)
        {
            SlotIndex = slotIndex;
        }

        public short SlotIndex;

        public const byte PacketId = 0x10;
        public byte Id { get { return 0x10; } }

        public void ReadPacket(MinecraftStream stream)
        {
            SlotIndex = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16(SlotIndex);
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
            Unknown = 0;
        }

        public int EntityId;
        public byte Unknown;
        public int X;
        public byte Y;
        public int Z;

        public const byte PacketId = 0x11;
        public byte Id { get { return 0x11; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Unknown = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Unknown);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
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

        public AnimationPacket(int entityId, AnimationType animation)
        {
            EntityId = entityId;
            Animation = animation;
        }

        public int EntityId;
        public AnimationType Animation;

        public const byte PacketId = 0x12;
        public byte Id { get { return 0x12; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Animation = (AnimationType)stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8((byte)Animation);
        }
    }

    public struct EntityActionPacket : IPacket
    {
        public enum EntityAction
        {
            Crouch = 1,
            Uncrouch = 2,
            LeaveBed = 3,
            StartSprinting = 4,
            StopSprinting = 5
        }

        public EntityActionPacket(int entityId, EntityAction action)
        {
            EntityId = entityId;
            Action = action;
        }

        public int EntityId;
        public EntityAction Action;

        public const byte PacketId = 0x13;
        public byte Id { get { return 0x13; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Action = (EntityAction)stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8((byte)Action);
        }
    }

    public struct SpawnPlayerPacket : IPacket
    {
        public SpawnPlayerPacket(int entityId, string playerName, int x,
            int y, int z, byte yaw,
            byte pitch, short heldItem, MetadataDictionary metadata)
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

        public int EntityId;
        public string PlayerName;
        public int X, Y, Z;
        public byte Yaw, Pitch;
        public short HeldItem;
        public MetadataDictionary Metadata;

        public const byte PacketId = 0x14;
        public byte Id { get { return 0x14; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            PlayerName = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
            HeldItem = stream.ReadInt16();
            Metadata = MetadataDictionary.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteString(PlayerName);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
            stream.WriteInt16(HeldItem);
            Metadata.WriteTo(stream);
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

        public const byte PacketId = 0x16;
        public byte Id { get { return 0x16; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ItemId = stream.ReadInt32();
            PlayerId = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(ItemId);
            stream.WriteInt32(PlayerId);
        }
    }

    public struct SpawnObjectPacket : IPacket
    {
        public SpawnObjectPacket(int entityId, byte type, int x,
            int y, int z, byte yaw, byte pitch)
        {
            EntityId = entityId;
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Data = 0;
            SpeedX = SpeedY = SpeedZ = null;
        }

        public SpawnObjectPacket(int entityId, byte type, int x,
            int y, int z, byte yaw, byte pitch,
            int data, short speedX, short speedY, short speedZ)
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

        public int EntityId;
        public byte Type;
        public int X, Y, Z;
        public int Data;
        public short? SpeedX, SpeedY, SpeedZ;
        public byte Yaw, Pitch;

        public const byte PacketId = 0x17;
        public byte Id { get { return 0x17; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
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
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
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
        }
    }

    public struct SpawnMobPacket : IPacket
    {
        public SpawnMobPacket(int entityId, byte type, int x,
            int y, int z, byte yaw,
            byte pitch, byte headYaw, short velocityX,
            short velocityY, short velocityZ,
            MetadataDictionary metadata)
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

        public int EntityId;
        public byte Type;
        public int X, Y, Z;
        public byte Yaw, Pitch, HeadYaw;
        public short VelocityX, VelocityY, VelocityZ;
        public MetadataDictionary Metadata;

        public const byte PacketId = 0x18;
        public byte Id { get { return 0x18; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
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
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
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
        }
    }

    public struct SpawnPaintingPacket : IPacket
    {
        public SpawnPaintingPacket(int entityId, string title, int x,
            int y, int z, int direction)
        {
            EntityId = entityId;
            Title = title;
            X = x;
            Y = y;
            Z = z;
            Direction = direction;
        }

        public int EntityId;
        public string Title;
        public int X, Y, Z;
        public int Direction;

        public const byte PacketId = 0x19;
        public byte Id { get { return 0x19; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Title = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Direction = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteString(Title);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Direction);
        }
    }

    public struct SpawnExperienceOrbPacket : IPacket
    {
        public SpawnExperienceOrbPacket(int entityId, int x, int y,
            int z, short count)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Count = count;
        }

        public int EntityId;
        public int X, Y, Z;
        public short Count;

        public const byte PacketId = 0x1A;
        public byte Id { get { return 0x1A; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Count = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteInt16(Count);
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

        public const byte PacketId = 0x1C;
        public byte Id { get { return 0x1C; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            VelocityX = stream.ReadInt16();
            VelocityY = stream.ReadInt16();
            VelocityZ = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt16(VelocityX);
            stream.WriteInt16(VelocityY);
            stream.WriteInt16(VelocityZ);
        }
    }

    public struct DestroyEntityPacket : IPacket
    {
        public DestroyEntityPacket(int[] entityIds)
        {
            EntityIds = entityIds;
        }

        public int[] EntityIds;

        public const byte PacketId = 0x1D;
        public byte Id { get { return 0x1D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            var length = stream.ReadUInt8();
            EntityIds = stream.ReadInt32Array(length);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8((byte)EntityIds.Length);
            stream.WriteInt32Array(EntityIds);
        }
    }

    public struct EntityPacket : IPacket
    {
        public EntityPacket(int entityId)
        {
            EntityId = entityId;
        }

        public int EntityId;

        public const byte PacketId = 0x1E;
        public byte Id { get { return 0x1E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
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

        public const byte PacketId = 0x1F;
        public byte Id { get { return 0x1F; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
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

        public const byte PacketId = 0x20;
        public byte Id { get { return 0x20; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
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

        public const byte PacketId = 0x21;
        public byte Id { get { return 0x21; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
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

        public const byte PacketId = 0x22;
        public byte Id { get { return 0x22; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadUInt8();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Yaw);
            stream.WriteUInt8(Pitch);
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

        public const byte PacketId = 0x23;
        public byte Id { get { return 0x23; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            HeadYaw = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(HeadYaw);
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

        public const byte PacketId = 0x26;
        public byte Id { get { return 0x26; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Status = (EntityStatus)stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8((byte)Status);
        }
    }

    public struct AttachEntityPacket : IPacket
    {
        public AttachEntityPacket(int entityId, int vehicleId)
        {
            EntityId = entityId;
            VehicleId = vehicleId;
        }

        public int EntityId, VehicleId;

        public const byte PacketId = 0x27;
        public byte Id { get { return 0x27; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            VehicleId = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(VehicleId);
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

        public const byte PacketId = 0x28;
        public byte Id { get { return 0x28; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Metadata = MetadataDictionary.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            Metadata.WriteTo(stream);
        }
    }

    public struct EntityEffectPacket : IPacket
    {
        public EntityEffectPacket(int entityId, byte effectId, byte amplifier,
            short duration)
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

        public const byte PacketId = 0x29;
        public byte Id { get { return 0x29; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            EffectId = stream.ReadUInt8();
            Amplifier = stream.ReadUInt8();
            Duration = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(EffectId);
            stream.WriteUInt8(Amplifier);
            stream.WriteInt16(Duration);
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

        public const byte PacketId = 0x2A;
        public byte Id { get { return 0x2A; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            EffectId = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(EffectId);
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

        public const byte PacketId = 0x2B;
        public byte Id { get { return 0x2B; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ExperienceBar = stream.ReadSingle();
            Level = stream.ReadInt16();
            TotalExperience = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteSingle(ExperienceBar);
            stream.WriteInt16(Level);
            stream.WriteInt16(TotalExperience);
        }
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

        public const byte PacketId = 0x33;
        public byte Id { get { return 0x33; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Z = stream.ReadInt32();
            GroundUpContinuous = stream.ReadBoolean();
            PrimaryBitMap = stream.ReadUInt16();
            AddBitMap = stream.ReadUInt16();
            var length = stream.ReadInt32();
            Data = stream.ReadUInt8Array(length);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteInt32(Z);
            stream.WriteBoolean(GroundUpContinuous);
            stream.WriteUInt16(PrimaryBitMap);
            stream.WriteUInt16(AddBitMap);
            stream.WriteInt32(Data.Length);
            stream.WriteUInt8Array(Data);
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

        public const byte PacketId = 0x34;
        public byte Id { get { return 0x34; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ChunkX = stream.ReadInt32();
            ChunkZ = stream.ReadInt32();
            RecordCount = stream.ReadInt16();
            stream.ReadInt32();
            Data = stream.ReadInt32Array(RecordCount);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(ChunkX);
            stream.WriteInt32(ChunkZ);
            stream.WriteInt16(RecordCount);
            stream.WriteInt32(RecordCount * 4);
            stream.WriteInt32Array(Data);
        }
    }

    public struct BlockChangePacket : IPacket
    {
        public BlockChangePacket(int x, byte y, int z,
            short blockType, byte blockMetadata)
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
        public short BlockType;
        public byte BlockMetadata;

        public const byte PacketId = 0x35;
        public byte Id { get { return 0x35; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            BlockType = stream.ReadInt16();
            BlockMetadata = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteInt16(BlockType);
            stream.WriteUInt8(BlockMetadata);
        }
    }

    public struct BlockActionPacket : IPacket
    {
        public BlockActionPacket(int x, short y, int z,
            byte data1, byte data2, short blockId)
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
        public short BlockId;

        public const byte PacketId = 0x36;
        public byte Id { get { return 0x36; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Data1 = stream.ReadUInt8();
            Data2 = stream.ReadUInt8();
            BlockId = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Data1);
            stream.WriteUInt8(Data2);
            stream.WriteInt16(BlockId);
        }
    }

    public struct BlockBreakAnimationPacket : IPacket
    {
        public BlockBreakAnimationPacket(int entityId, int x, int y,
            int z, byte destroyStage)
        {
            // TODO: Use this packet when mining begins
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            DestroyStage = destroyStage;
        }

        public int EntityId;
        public int X, Y, Z;
        public byte DestroyStage;

        public const byte PacketId = 0x37;
        public byte Id { get { return 0x37; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            DestroyStage = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(DestroyStage);
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

        public const byte PacketId = 0x38;
        public byte Id { get { return 0x38; } }

        public void ReadPacket(MinecraftStream stream)
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
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
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
        }
    }

    public struct ExplosionPacket : IPacket
    {
        public ExplosionPacket(double x, double y, double z,
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

        public double X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records;
        public float PlayerVelocityX, PlayerVelocityY, PlayerVelocityZ;

        public const byte PacketId = 0x3C;
        public byte Id { get { return 0x3C; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Radius = stream.ReadSingle();
            RecordCount = stream.ReadInt32();
            Records = stream.ReadUInt8Array(RecordCount * 3);
            PlayerVelocityX = stream.ReadSingle();
            PlayerVelocityY = stream.ReadSingle();
            PlayerVelocityZ = stream.ReadSingle();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
            stream.WriteSingle(Radius);
            stream.WriteInt32(RecordCount);
            stream.WriteUInt8Array(Records);
            stream.WriteSingle(PlayerVelocityX);
            stream.WriteSingle(PlayerVelocityY);
            stream.WriteSingle(PlayerVelocityZ);
        }
    }

    public struct SoundOrParticleEffectPacket : IPacket
    {
        public SoundOrParticleEffectPacket(int entityId, int x, byte y, int z,
            int data, bool disableRelativeVolume)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Data = data;
            DisableRelativeVolume = disableRelativeVolume;
        }

        public int EntityId;
        public int X;
        public byte Y;
        public int Z;
        public int Data;
        public bool DisableRelativeVolume;

        public const byte PacketId = 0x3D;
        public byte Id { get { return 0x3D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Data = stream.ReadInt32();
            DisableRelativeVolume = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Data);
            stream.WriteBoolean(DisableRelativeVolume);
        }
    }

    public struct NamedSoundEffectPacket : IPacket
    {
        public NamedSoundEffectPacket(string soundName, int x, int y,
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

        public const byte PacketId = 0x3E;
        public byte Id { get { return 0x3E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            SoundName = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Volume = stream.ReadSingle();
            Pitch = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(SoundName);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteSingle(Volume);
            stream.WriteUInt8(Pitch);
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

        public const byte PacketId = 0x3F;

        public byte Id { get { return 0x3F; } }

        public void ReadPacket(MinecraftStream stream)
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
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
            stream.WriteString(EffectName);
            stream.WriteSingle(X);
            stream.WriteSingle(Y);
            stream.WriteSingle(Z);
            stream.WriteSingle(OffsetX);
            stream.WriteSingle(OffsetY);
            stream.WriteSingle(OffsetZ);
            stream.WriteSingle(ParticleSpeed);
            stream.WriteInt32(ParticleCount);
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

        public const byte PacketId = 0x46;
        public byte Id { get { return 0x46; } }

        public void ReadPacket(MinecraftStream stream)
        {
            State = (GameState)stream.ReadUInt8();
            GameMode = (GameMode)stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8((byte)State);
            stream.WriteUInt8((byte)GameMode);
        }
    }

    public struct SpawnGlobalEntityPacket : IPacket
    {
        public SpawnGlobalEntityPacket(int entityId, byte type, int x,
            int y, int z)
        {
            EntityId = entityId;
            Type = type;
            X = x;
            Y = y;
            Z = z;
        }

        public int EntityId;
        public byte Type;
        public int X, Y, Z;

        public const byte PacketId = 0x47;
        public byte Id { get { return 0x47; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Type = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Type);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
        }
    }

    public struct OpenWindowPacket : IPacket
    {
        public OpenWindowPacket(byte windowId, byte inventoryType, string windowTitle,
            byte slotCount, bool useProvidedTitle) // TODO: InventoryTypes enum
        {
            WindowId = windowId;
            InventoryType = inventoryType;
            WindowTitle = windowTitle;
            SlotCount = slotCount;
            UseProvidedTitle = useProvidedTitle;
        }
        
        public byte WindowId;
        public byte InventoryType;
        public string WindowTitle;
        public byte SlotCount;
        public bool UseProvidedTitle;

        public const byte PacketId = 0x64;
        public byte Id { get { return 0x64; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            InventoryType = stream.ReadUInt8();
            WindowTitle = stream.ReadString();
            SlotCount = stream.ReadUInt8();
            UseProvidedTitle = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteUInt8(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteUInt8(SlotCount);
            stream.WriteBoolean(UseProvidedTitle);
        }
    }

    public struct CloseWindowPacket : IPacket
    {
        public CloseWindowPacket(byte windowId)
        {
            WindowId = windowId;
        }

        public byte WindowId;

        public const byte PacketId = 0x65;
        public byte Id { get { return 0x65; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
        }
    }

    public struct ClickWindowPacket : IPacket
    {
        public ClickWindowPacket(byte windowId, short slotIndex, byte mouseButton,
            short actionNumber, bool shift, ItemStack clickedItem)
        {
            WindowId = windowId;
            SlotIndex = slotIndex;
            MouseButton = mouseButton;
            ActionNumber = actionNumber;
            Shift = shift;
            ClickedItem = clickedItem;
        }

        public byte WindowId;
        public short SlotIndex;
        public byte MouseButton;
        public short ActionNumber;
        public bool Shift;
        public ItemStack ClickedItem;

        public const byte PacketId = 0x66;
        public byte Id { get { return 0x66; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            SlotIndex = stream.ReadInt16();
            MouseButton = stream.ReadUInt8();
            ActionNumber = stream.ReadInt16();
            Shift = stream.ReadBoolean();
            ClickedItem = ItemStack.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(SlotIndex);
            stream.WriteUInt8(MouseButton);
            stream.WriteInt16(ActionNumber);
            stream.WriteBoolean(Shift);
            ClickedItem.WriteTo(stream);
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

        public const byte PacketId = 0x67;
        public byte Id { get { return 0x67; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            SlotIndex = stream.ReadInt16();
            Item = ItemStack.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(SlotIndex);
            Item.WriteTo(stream);
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

        public const byte PacketId = 0x68;
        public byte Id { get { return 0x68; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            short count = stream.ReadInt16();
            Items = new ItemStack[count];
            for (int i = 0; i < count; i++)
                Items[i] = ItemStack.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16((short)Items.Length);
            for (int i = 0; i < Items.Length; i++)
                Items[i].WriteTo(stream);
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

        public const byte PacketId = 0x69;
        public byte Id { get { return 0x69; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            PropertyId = stream.ReadInt16();
            Value = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(PropertyId);
            stream.WriteInt16(Value);
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

        public const byte PacketId = 0x6A;
        public byte Id { get { return 0x6A; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            ActionNumber = stream.ReadInt16();
            Accepted = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(ActionNumber);
            stream.WriteBoolean(Accepted);
        }
    }

    public struct CreativeInventoryActionPacket : IPacket
    {
        public CreativeInventoryActionPacket(short slotIndex, ItemStack item)
        {
            SlotIndex = slotIndex;
            Item = item;
        }

        public short SlotIndex;
        public ItemStack Item;

        public const byte PacketId = 0x6B;
        public byte Id { get { return 0x6B; } }

        public void ReadPacket(MinecraftStream stream)
        {
            SlotIndex = stream.ReadInt16();
            Item = ItemStack.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16(SlotIndex);
            Item.WriteTo(stream);
        }
    }

    public struct EnchantItemPacket : IPacket
    {
        public EnchantItemPacket(byte windowId, byte enchantment)
        {
            WindowId = windowId;
            Enchantment = enchantment;
        }

        public byte WindowId;
        public byte Enchantment;

        public const byte PacketId = 0x6C;
        public byte Id { get { return 0x6C; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            Enchantment = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteUInt8(Enchantment);
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

        public const byte PacketId = 0x82;
        public byte Id { get { return 0x82; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Text1 = stream.ReadString();
            Text2 = stream.ReadString();
            Text3 = stream.ReadString();
            Text4 = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteString(Text1);
            stream.WriteString(Text2);
            stream.WriteString(Text3);
            stream.WriteString(Text4);
        }
    }

    public struct ItemDataPacket : IPacket
    {
        public ItemDataPacket(short itemType, short itemId, string text)
        {
            ItemType = itemType;
            ItemId = itemId;
            Text = text;
        }
        
        public short ItemType, ItemId;
        public string Text;

        public const byte PacketId = 0x83;
        public byte Id { get { return 0x83; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ItemType = stream.ReadInt16();
            ItemId = stream.ReadInt16();
            var length = stream.ReadInt16();
            Text = Encoding.ASCII.GetString(stream.ReadUInt8Array(length));
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16(ItemType);
            stream.WriteInt16(ItemId);
            stream.WriteInt16((short)Text.Length);
            stream.WriteUInt8Array(Encoding.ASCII.GetBytes(Text));
        }
    }

    public struct UpdateTileEntityPacket : IPacket
    {
        public UpdateTileEntityPacket(int x, short y, int z,
            byte action, NbtFile nbt)
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

        public const byte PacketId = 0x84;
        public byte Id { get { return 0x84; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadInt16();
            Z = stream.ReadInt32();
            Action = stream.ReadUInt8();
            var length = stream.ReadInt16();
            var data = stream.ReadUInt8Array(length);
            Nbt = new NbtFile();
            Nbt.LoadFromBuffer(data, 0, length, NbtCompression.GZip, null);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(X);
            stream.WriteInt16(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Action);
            var tempStream = new MemoryStream();
            Nbt.SaveToStream(tempStream, NbtCompression.GZip);
            var buffer = tempStream.GetBuffer();
            stream.WriteInt16((short)buffer.Length);
            stream.WriteUInt8Array(buffer);
        }
    }

    public struct IncrementStatisticPacket : IPacket
    {
        public IncrementStatisticPacket(int statisticId, byte amount)
        {
            StatisticId = statisticId;
            Amount = amount;
        }

        public int StatisticId;
        public byte Amount;

        public const byte PacketId = 0xC8;
        public byte Id { get { return 0xC8; } }

        public void ReadPacket(MinecraftStream stream)
        {
            StatisticId = stream.ReadInt32();
            Amount = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(StatisticId);
            stream.WriteUInt8(Amount);
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

        public const byte PacketId = 0xC9;
        public byte Id { get { return 0xC9; } }

        public void ReadPacket(MinecraftStream stream)
        {
            PlayerName = stream.ReadString();
            Online = stream.ReadBoolean();
            Ping = stream.ReadInt16();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(PlayerName);
            stream.WriteBoolean(Online);
            stream.WriteInt16(Ping);
        }
    }

    public struct PlayerAbilitiesPacket : IPacket
    {
        public PlayerAbilitiesPacket(byte flags, byte flyingSpeed, byte walkingSpeed)
        {
            Flags = flags;
            FlyingSpeed = flyingSpeed;
            WalkingSpeed = walkingSpeed;
        }

        public byte Flags;
        public byte FlyingSpeed, WalkingSpeed;

        public const byte PacketId = 0xCA;
        public byte Id { get { return 0xCA; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Flags = stream.ReadUInt8();
            FlyingSpeed = stream.ReadUInt8();
            WalkingSpeed = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(Flags);
            stream.WriteUInt8(FlyingSpeed);
            stream.WriteUInt8(WalkingSpeed);
        }
    }

    public struct TabCompletePacket : IPacket
    {
        public TabCompletePacket(string text)
        {
            Text = text;
        }

        public string Text;

        public const byte PacketId = 0xCB;
        public byte Id { get { return 0xCB; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Text = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Text);
        }
    }

    public enum ChatMode // TODO: Move this?
    {
        Hidden = 2,
        CommandsOnly = 1,
        Enabled = 0
    }

    public struct ClientSettingsPacket : IPacket
    {
        public ClientSettingsPacket(string locale, byte viewDistance, ChatMode chatFlags,
            Difficulty difficulty, bool showCape)
        {
            Locale = locale;
            ViewDistance = viewDistance;
            ChatFlags = chatFlags;
            Difficulty = difficulty;
            ShowCape = showCape;
        }

        public string Locale;
        public byte ViewDistance;
        public ChatMode ChatFlags;
        public Difficulty Difficulty;
        public bool ShowCape;

        public const byte PacketId = 0xCC;
        public byte Id { get { return 0xCC; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Locale = stream.ReadString();
            ViewDistance = stream.ReadUInt8();
            ChatFlags = (ChatMode)(stream.ReadUInt8() & 0x3);
            Difficulty = (Difficulty)stream.ReadUInt8();
            ShowCape = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Locale);
            stream.WriteUInt8(ViewDistance);
            stream.WriteUInt8((byte)ChatFlags);
            stream.WriteUInt8((byte)Difficulty);
            stream.WriteBoolean(ShowCape);
        }
    }

    public struct ClientStatusPacket : IPacket
    {
        public enum ClientStatus
        {
            InitialSpawn = 0,
            Respawn = 1
        }

        public ClientStatusPacket(ClientStatus status)
        {
            Status = status;
        }

        public ClientStatus Status;

        public const byte PacketId = 0xCD;
        public byte Id { get { return 0xCD; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Status = (ClientStatus)stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8((byte)Status);
        }
    }

    public struct CreateScoreboardPacket : IPacket
    {
        public CreateScoreboardPacket(string name, string displayName)
            : this(name, displayName, false)
        {
        }

        public CreateScoreboardPacket(string name, string displayName, bool remove)
        {
            Name = name;
            DisplayName = displayName;
            RemoveBoard = remove;
        }

        public string Name;
        public string DisplayName;
        public bool RemoveBoard;

        public const byte PacketId = 0xCE;
        public byte Id { get { return 0xCE; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Name = stream.ReadString();
            DisplayName = stream.ReadString();
            RemoveBoard = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Name);
            stream.WriteString(DisplayName);
            stream.WriteBoolean(RemoveBoard);
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

        public const byte PacketId = 0xCF;
        public byte Id { get { return 0xCF; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ItemName = stream.ReadString();
            RemoveItem = stream.ReadBoolean();
            if (!RemoveItem)
            {
                ScoreName = stream.ReadString();
                Value = stream.ReadInt32();
            }
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(ItemName);
            stream.WriteBoolean(RemoveItem);
            if (!RemoveItem)
            {
                stream.WriteString(ScoreName);
                stream.WriteInt32(Value.Value);
            }
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

        public const byte PacketId = 0xD0;
        public byte Id { get { return 0xD0; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Position = (ScoreboardPosition)stream.ReadUInt8();
            ScoreName = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8((byte)Position);
            stream.WriteString(ScoreName);
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

        public const byte PacketId = 0xD1;
        public byte Id { get { return 0xD1; } }

        public void ReadPacket(MinecraftStream stream)
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
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(PacketId);
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

        public const byte PacketId = 0xFA;
        public byte Id { get { return 0xFA; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Channel = stream.ReadString();
            var length = stream.ReadInt16();
            Data = stream.ReadUInt8Array(length);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Channel);
            stream.WriteInt16((short)Data.Length);
            stream.WriteUInt8Array(Data);
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

        public const byte PacketId = 0xFC;
        public byte Id { get { return 0xFC; } }

        public void ReadPacket(MinecraftStream stream)
        {
            var ssLength = stream.ReadInt16();
            SharedSecret = stream.ReadUInt8Array(ssLength);
            var vtLength = stream.ReadInt16();
            VerificationToken = stream.ReadUInt8Array(vtLength);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16((short)SharedSecret.Length);
            stream.WriteUInt8Array(SharedSecret);
            stream.WriteInt16((short)VerificationToken.Length);
            stream.WriteUInt8Array(VerificationToken);
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

        public const byte PacketId = 0xFD;
        public byte Id { get { return 0xFD; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ServerId = stream.ReadString();
            var pkLength = stream.ReadInt16();
            PublicKey = stream.ReadUInt8Array(pkLength);
            var vtLength = stream.ReadInt16();
            VerificationToken = stream.ReadUInt8Array(vtLength);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(ServerId);
            stream.WriteInt16((short)PublicKey.Length);
            stream.WriteUInt8Array(PublicKey);
            stream.WriteInt16((short)VerificationToken.Length);
            stream.WriteUInt8Array(VerificationToken);
        }
    }

    public struct ServerListPingPacket : IPacket
    {
        public ServerListPingPacket(byte magic)
        {
            Magic = magic;
        }

        public byte Magic;

        public const byte PacketId = 0xFE;
        public byte Id { get { return 0xFE; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Magic = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(Magic);
        }
    }

    public struct DisconnectPacket : IPacket
    {
        public DisconnectPacket(string reason)
        {
            Reason = reason;
        }

        public string Reason;

        public const byte PacketId = 0xFF;
        public byte Id { get { return 0xFF; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Reason);
        }
    }
}
