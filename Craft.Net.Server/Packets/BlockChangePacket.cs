using System;
using System.Linq;
using Craft.Net.Server.Blocks;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Packets
{
    public class BlockChangePacket : Packet
    {
        public Vector3 Position;
        public Block Value;

        public BlockChangePacket(Vector3 position, Block value)
        {
            this.Position = position;
            this.Value = value;
        }

        public override byte PacketID
        {
            get { return 0x35; }
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
                .Concat(CreateInt((int)Position.X))
                .Concat(new[] {(byte)Position.Y})
                .Concat(CreateInt((int)Position.Z))
                .Concat(CreateShort(Value.BlockID))
                .Concat(new[] {Value.Metadata}).ToArray();
            client.SendData(buffer);
        }
    }
}