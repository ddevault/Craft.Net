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

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(DataUtility.CreateString(PlayerName))
                .Concat(DataUtility.CreateAbsoluteInteger((int)Position.X))
                .Concat(DataUtility.CreateAbsoluteInteger((int)Position.Y))
                .Concat(DataUtility.CreateAbsoluteInteger((int)Position.Z))
                .Concat(DataUtility.CreatePackedByte(Yaw))
                .Concat(DataUtility.CreatePackedByte(Pitch))
                .Concat(DataUtility.CreateInt16(CurrentItem))
                .Concat(new byte[] { 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x00, 0x7F }).ToArray(); // TODO: Metadata
            client.SendData(buffer);
        }
    }
}