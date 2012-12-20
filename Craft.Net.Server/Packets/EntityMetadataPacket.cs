using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class EntityMetadataPacket : Packet
    {
        public EntityMetadataPacket()
        {
        }

        public EntityMetadataPacket(Entity entity)
        {
            Entity = entity;
        }

        public Entity Entity { get; set; }

        public override byte PacketId
        {
            get { return 0x28; }
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
                DataUtility.CreateInt32(Entity.Id),
                Entity.Metadata.Encode()));
        }
    }
}
