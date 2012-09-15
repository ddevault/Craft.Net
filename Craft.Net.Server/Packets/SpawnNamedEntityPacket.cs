using System;
using Craft.Net.Data;
using Craft.Net.Data.Metadata;

namespace Craft.Net.Server.Packets
{
    public sealed class SpawnNamedEntityPacket : Packet
    {
        private short CurrentItem;
        private int EntityId;
        private float Pitch;
        private string PlayerName;
        private Vector3 Position;
        private float Yaw;
        private MetadataDictionary Metadata;

        public SpawnNamedEntityPacket(MinecraftClient client)
        {
            EntityId = client.Entity.Id;
            PlayerName = client.Username;
            Position = client.Entity.Position;
            Yaw = client.Entity.Yaw;
            Pitch = client.Entity.Pitch;
            CurrentItem = 0; // TODO
            Metadata = client.Entity.Metadata;
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
            client.SendData(CreateBuffer(
                DataUtility.CreateInt32(EntityId),
                DataUtility.CreateString(PlayerName),
                DataUtility.CreateAbsoluteInteger((int)Position.X),
                DataUtility.CreateAbsoluteInteger((int)Position.Y),
                DataUtility.CreateAbsoluteInteger((int)Position.Z),
                DataUtility.CreatePackedByte(Yaw),
                DataUtility.CreatePackedByte(Pitch),
                DataUtility.CreateInt16(CurrentItem),
                Metadata.Encode()));
        }
    }
}