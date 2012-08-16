using System;
using System.Linq;
using Craft.Net.Data;

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

        public override byte PacketId
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
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateInt(EntityId))
                .Concat(DataUtility.CreateString(PlayerName))
                .Concat(DataUtility.CreateInt((int)Position.X))
                .Concat(DataUtility.CreateInt((int)Position.Y))
                .Concat(DataUtility.CreateInt((int)Position.Z))
                .Concat(DataUtility.CreatePackedByte(Yaw))
                .Concat(DataUtility.CreatePackedByte(Pitch))
                .Concat(DataUtility.CreateShort(CurrentItem)).ToArray();
            client.SendData(buffer);
        }
    }
}