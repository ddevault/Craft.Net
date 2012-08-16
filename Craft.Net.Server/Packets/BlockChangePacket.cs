using System;
using System.Linq;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;

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

        public override byte PacketId
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
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateInt((int)Position.X))
                .Concat(new[] {(byte)Position.Y})
                .Concat(DataUtility.CreateInt((int)Position.Z))
                .Concat(DataUtility.CreateShort(Value.BlockID))
                .Concat(new[] {Value.Metadata}).ToArray();
            client.SendData(buffer);
        }
    }
}