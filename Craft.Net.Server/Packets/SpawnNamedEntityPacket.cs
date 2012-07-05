using System;
using Craft.Net.Server.Worlds;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class SpawnNamedEntityPacket : Packet
    {
        public int EntityId;
        public string PlayerName;
        public Vector3 Position;
        public float Yaw, Pitch;
        public short CurrentItem;

        public SpawnNamedEntityPacket(MinecraftClient Client)
        {
            this.EntityId = Client.Entity.Id;
            this.PlayerName = Client.Username;
            this.Position = Client.Entity.Position;
            this.Yaw = Client.Entity.Yaw;
            this.Pitch = Client.Entity.Pitch;
            this.CurrentItem = 0; // TODO
        }

        public override byte PacketID
        {
            get
            {
                return 0x14;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }
                .Concat(CreateInt(EntityId))
                .Concat(CreateString(PlayerName))
                .Concat(CreateInt((int)Position.X))
                .Concat(CreateInt((int)Position.Y))
                .Concat(CreateInt((int)Position.Z))
                .Concat(CreatePackedByte(Yaw))
                .Concat(CreatePackedByte(Pitch))
                .Concat(CreateShort(CurrentItem)).ToArray();
            Client.SendData(buffer);
        }
    }
}

