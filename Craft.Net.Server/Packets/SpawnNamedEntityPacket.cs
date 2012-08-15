using System;
using System.Linq;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Packets
{
    public class SpawnNamedEntityPacket : Packet
    {
        public short CurrentItem;
        public int EntityId;
        public float Pitch;
        public string PlayerName;
        public Vector3 Position;
        public float Yaw;

        public SpawnNamedEntityPacket(MinecraftClient Client)
        {
            EntityId = Client.Entity.Id;
            PlayerName = Client.Username;
            Position = Client.Entity.Position;
            Yaw = Client.Entity.Yaw;
            Pitch = Client.Entity.Pitch;
            CurrentItem = 0; // TODO
        }

        public override byte PacketID
        {
            get { return 0x14; }
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
            byte[] buffer = new[] {PacketID}
                .Concat(CreateInt(EntityId))
                .Concat(CreateString(PlayerName))
                .Concat(CreateInt((int) Position.X))
                .Concat(CreateInt((int) Position.Y))
                .Concat(CreateInt((int) Position.Z))
                .Concat(CreatePackedByte(Yaw))
                .Concat(CreatePackedByte(Pitch))
                .Concat(CreateShort(CurrentItem)).ToArray();
            Client.SendData(buffer);
        }
    }
}