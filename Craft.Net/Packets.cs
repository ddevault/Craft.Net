using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibNbt;
using Craft.Net.Data;

namespace Craft.Net
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
            
        }

        public int EntityId;
        public string LevelType;
        public GameMode GameMode;
        public Dimension Dimension;
        public Difficulty Difficulty;
        public byte Discarded;
        public byte MaxPlayers;

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
        public byte ProtocolVersion;
        public string Username;
        public string ServerHostname;
        public int ServerPort;

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
        public string Message;

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
        public long WorldAge, TimeOfDay;

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
        public int EntityId;
        public short SlotIndex;
        public Slot Slot;

        public byte Id { get { return 0x05; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            SlotIndex = stream.ReadInt16();
            Slot = Slot.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt16(SlotIndex);
            Slot.WriteTo(stream);
        }
    }

    public struct SpawnPositionPacket : IPacket
    {
        public int X, Y, Z;

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
        public int User, Target;
        public bool MouseButton;

        public byte Id { get { return 0x07; } }

        public void ReadPacket(MinecraftStream stream)
        {
            User = stream.ReadInt32();
            Target = stream.ReadInt32();
            MouseButton = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(User);
            stream.WriteInt32(Target);
            stream.WriteBoolean(MouseButton);
        }
    }

    public struct UpdateHealthPacket : IPacket
    {
        public short Health, Food;
        public float FoodSaturation;

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
        public Dimension Dimension;
        public Difficulty Difficulty;
        public GameMode GameMode;
        public short WorldHeight;
        public string LevelType;

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
        public bool OnGround;

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
        public double X, Y, Stance, Z;
        public bool OnGround;

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
        public float Yaw, Pitch;
        public bool OnGround;

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
        public double X, Y, Stance, Z;
        public float Yaw, Pitch;
        public bool OnGround;

        public byte Id { get { return 0x0D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            // TODO: Investigate if Y/Stance are indeed swapped
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
        public byte Status;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public byte Id { get { return 0x0E; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Status = stream.ReadUInt8();
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Face = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(Status);
            stream.WriteInt32(X);
            stream.WriteUInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8(Face);
        }
    }

    public struct PlayerBlockPlacementPacket : IPacket
    {
        public int X;
        public byte Y;
        public int Z;
        public byte Direction;
        public Slot HeldItem;
        public byte CursorX;
        public byte CursorY;
        public byte CursorZ;

        public byte Id { get { return 0x0F; } }

        public void ReadPacket(MinecraftStream stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadUInt8();
            Z = stream.ReadInt32();
            Direction = stream.ReadUInt8();
            HeldItem = Slot.FromStream(stream);
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
        public short SlotIndex;

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
        public int EntityId;
        public byte Unknown;
        public int X;
        public byte Y;
        public int Z;

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
        public int EntityId;
        public byte Animation;

        public byte Id { get { return 0x12; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Animation = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Animation);
        }
    }

    public struct EntityActionPacket : IPacket
    {
        public int EntityId;
        public byte ActionId;

        public byte Id { get { return 0x13; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            ActionId = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(ActionId);
        }
    }

    public struct SpawnNamedEntityPacket : IPacket
    {
        public int EntityId;
        public string PlayerName;
        public int X, Y, Z;
        public byte Yaw, Pitch;
        public short HeldItem;
        public MetadataDictionary Metadata;

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
        public int ItemId;
        public int PlayerId;

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

    public struct SpawnObjectOrVehiclePacket : IPacket
    {
        public int EntityId;
        public byte Type;
        public int X, Y, Z;
        public int Data;
        public short? SpeedX, SpeedY, SpeedZ;
        public byte Yaw, Pitch; // TODO: Packed bytes

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
        public int EntityId;
        public byte Type;
        public int X, Y, Z;
        public byte Yaw, Pitch, HeadYaw;
        public short VelocityX, VelocityY, VelocityZ;
        public MetadataDictionary Metadata;

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
        public int EntityId;
        public string Title;
        public int X, Y, Z;
        public int Direction;

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
        public int EntityId;
        public int X, Y, Z;
        public short Count;

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
        public int EntityId;
        public short VelocityX, VelocityY, VelocityZ;

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

    public struct DestroyEntity : IPacket
    {
        public int[] EntityIds;

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
        public int EntityId;

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
        public int EntityId;
        public sbyte DeltaX, DeltaY, DeltaZ;

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
        public int EntityId;
        public byte Yaw, Pitch;

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
        public int EntityId;
        public sbyte DeltaX, DeltaY, DeltaZ;
        public byte Yaw, Pitch;

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
        public int EntityId;
        public int X, Y, Z;
        public byte Yaw, Pitch;

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

    public struct EntityHeadLook : IPacket
    {
        public int EntityId;
        public byte HeadYaw;

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
        public int EntityId;
        public byte Status;

        public byte Id { get { return 0x26; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            Status = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteUInt8(Status);
        }
    }

    public struct AttachEntityPacket : IPacket
    {
        public int EntityId, VehicleId;

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
        public int EntityId;
        public MetadataDictionary Metadata;

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
        public int EntityId;
        public byte EffectId;
        public byte Amplifier;
        public short Duration;

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

    public struct RemoveEntityEffect : IPacket
    {
        public int EntityId;
        public byte EffectId;

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
        public float ExperienceBar;
        public short Level;
        public short TotalExperience;

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
        //public static bool DecompressChunks = false; // TODO

        public int X, Z;
        public bool GroundUpContinuous;
        [LogDisplay(LogDisplayType.Binary)]
        public ushort PrimaryBitMap;
        [LogDisplay(LogDisplayType.Binary)]
        public ushort AddBitMap;
        public byte[] Data;

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
        public int ChunkX, ChunkZ;
        public short RecordCount;
        public int[] Data; // TODO: Display this in a friendly manner

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
        public int X;
        public byte Y;
        public int Z;
        public short BlockType;
        public byte BlockMetadata;

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
        public int X;
        public short Y;
        public int Z;
        public byte Data1;
        public byte Data2; // TODO: Perhaps expand on this
        public short BlockId;

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
        public int EntityId;
        public int X, Y, Z;
        public byte DestroyStage;

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
        // TODO: See about making this packet more detailed in logs
        public short ChunkCount;
        public bool Unknown;
        public byte[] ChunkData;
        public byte[] ChunkMetadata;

        public byte Id { get { return 0x38; } }

        public void ReadPacket(MinecraftStream stream)
        {
            ChunkCount = stream.ReadInt16();
            var length = stream.ReadInt32();
            Unknown = stream.ReadBoolean();
            ChunkData = stream.ReadUInt8Array(length);
            ChunkMetadata = stream.ReadUInt8Array(ChunkCount * 12);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt16(ChunkCount);
            stream.WriteInt32(ChunkData.Length);
            stream.WriteBoolean(Unknown);
            stream.WriteUInt8Array(ChunkData);
            stream.WriteUInt8Array(ChunkMetadata);
        }
    }

    public struct ExplosionPacket : IPacket
    {
        public double X, Y, Z;
        public float Radius;
        public int RecordCount;
        public byte[] Records; // TODO: Consider making more detailed
        public float PlayerVelocityX, PlayerVelocityY, PlayerVelocityZ;

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
        public int EntityId;
        public int X;
        public sbyte Y;
        public int Z;
        public int Data;
        public bool DisableRelativeVolume;

        public byte Id { get { return 0x3D; } }

        public void ReadPacket(MinecraftStream stream)
        {
            EntityId = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt8();
            Z = stream.ReadInt32();
            Data = stream.ReadInt32();
            DisableRelativeVolume = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteInt32(EntityId);
            stream.WriteInt32(X);
            stream.WriteInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Data);
            stream.WriteBoolean(DisableRelativeVolume);
        }
    }

    public struct NamedSoundEffectPacket : IPacket
    {
        public string SoundName;
        public int X, Y, Z;
        public float Volume;
        public byte Pitch;

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

    public struct ChangeGameStatePacket : IPacket
    {
        // TODO: Expand upon this, list reason from enum
        public byte State, GameMode;

        public byte Id { get { return 0x46; } }

        public void ReadPacket(MinecraftStream stream)
        {
            State = stream.ReadUInt8();
            GameMode = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(State);
            stream.WriteUInt8(GameMode);
        }
    }

    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityId;
        public byte Type;
        public int X, Y, Z;

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
        public byte WindowId;
        public byte InventoryType;
        public string WindowTitle;
        public byte SlotCount;

        public byte Id { get { return 0x64; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            InventoryType = stream.ReadUInt8();
            WindowTitle = stream.ReadString();
            SlotCount = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteUInt8(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteUInt8(SlotCount);
        }
    }

    public struct CloseWindowPacket : IPacket
    {
        public byte WindowId;

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
        public byte WindowId;
        public short SlotIndex;
        public byte MouseButton;
        public short ActionNumber;
        public bool Shift;
        public Slot ClickedItem;

        public byte Id { get { return 0x66; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            SlotIndex = stream.ReadInt16();
            MouseButton = stream.ReadUInt8();
            ActionNumber = stream.ReadInt16();
            Shift = stream.ReadBoolean();
            ClickedItem = Slot.FromStream(stream);
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
        public byte WindowId;
        public short SlotIndex;
        public Slot Item;

        public byte Id { get { return 0x67; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            SlotIndex = stream.ReadInt16();
            Item = Slot.FromStream(stream);
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
        public byte WindowId;
        public short Count;
        public Slot[] Items;

        public byte Id { get { return 0x68; } }

        public void ReadPacket(MinecraftStream stream)
        {
            WindowId = stream.ReadUInt8();
            Count = stream.ReadInt16();
            Items = new Slot[Count];
            for (int i = 0; i < Count; i++)
                Items[i] = Slot.FromStream(stream);
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(WindowId);
            stream.WriteInt16(Count);
            for (int i = 0; i < Count; i++)
                Items[i].WriteTo(stream);
        }
    }

    public struct UpdateWindowPropertyPacket : IPacket
    {
        public byte WindowId;
        public short PropertyId;
        public short Value;

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
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

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
        public short SlotIndex;
        public Slot Item;

        public byte Id { get { return 0x6B; } }

        public void ReadPacket(MinecraftStream stream)
        {
            SlotIndex = stream.ReadInt16();
            Item = Slot.FromStream(stream);
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
        public byte WindowId;
        public byte Enchantment;

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
        public int X;
        public short Y;
        public int Z;
        public string Text1, Text2, Text3, Text4;

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
        public short ItemType, ItemId;
        public string Text;

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
        public int X;
        public short Y;
        public int Z;
        public byte Action;
        public NbtFile Nbt;

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
        public int StatisticId;
        public byte Amount;

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
        public string PlayerName;
        public bool Online;
        public short Ping;

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
        [LogDisplay(LogDisplayType.Binary)]
        public byte Flags;
        public byte FlyingSpeed, WalkingSpeed;

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
        public string Text;

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

    public struct ClientSettingsPacket : IPacket
    {
        public string Locale;
        public byte ViewDistance;
        [LogDisplay(LogDisplayType.Binary)]
        public byte ChatFlags;
        public byte Difficulty;
        public bool ShowCape;

        public byte Id { get { return 0xCC; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Locale = stream.ReadString();
            ViewDistance = stream.ReadUInt8();
            ChatFlags = stream.ReadUInt8();
            Difficulty = stream.ReadUInt8();
            ShowCape = stream.ReadBoolean();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteString(Locale);
            stream.WriteUInt8(ViewDistance);
            stream.WriteUInt8(ChatFlags);
            stream.WriteUInt8(Difficulty);
            stream.WriteBoolean(ShowCape);
        }
    }

    public struct ClientStatusPacket : IPacket
    {
        public byte Payload;

        public byte Id { get { return 0xCD; } }

        public void ReadPacket(MinecraftStream stream)
        {
            Payload = stream.ReadUInt8();
        }

        public void WritePacket(MinecraftStream stream)
        {
            stream.WriteUInt8(Id);
            stream.WriteUInt8(Payload);
        }
    }

    public struct PluginMessage : IPacket
    {
        public string Channel;
        public byte[] Data; // TODO: For known channels, elaborate on the data

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
        public byte[] SharedSecret;
        public byte[] VerificationToken;

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
        public string ServerId;
        public byte[] PublicKey;
        public byte[] VerificationToken;

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
        public byte Magic;

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
        public string Reason;

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
