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

        public SpawnNamedEntityPacket(MinecraftClient client)
        {
            EntityId = client.Entity.Id;
            PlayerName = client.Username;
            Position = client.Entity.Position;
            Yaw = client.Entity.Yaw;
            Pitch = client.Entity.Pitch;
            CurrentItem = 0; // TODO
        }

        public override byte PacketID
        {
            get { return 0x14; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] buffer = new[] {PacketID}
                .Concat(CreateInt(EntityId))
                .Concat(CreateString(PlayerName))
                .Concat(CreateInt((int)Position.X))
                .Concat(CreateInt((int)Position.Y))
                .Concat(CreateInt((int)Position.Z))
                .Concat(CreatePackedByte(Yaw))
                .Concat(CreatePackedByte(Pitch))
                .Concat(CreateShort(CurrentItem)).ToArray();
            client.SendData(buffer);
        }
    }
}