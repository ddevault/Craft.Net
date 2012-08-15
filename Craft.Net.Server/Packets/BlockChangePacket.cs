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

        public BlockChangePacket(Vector3 Position, Block Value)
        {
            this.Position = Position;
            this.Value = Value;
        }

        public override byte PacketID
        {
            get { return 0x35; }
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
                .Concat(CreateInt((int) Position.X))
                .Concat(new[] {(byte) Position.Y})
                .Concat(CreateInt((int) Position.Z))
                .Concat(CreateShort(Value.BlockID))
                .Concat(new[] {Value.Metadata}).ToArray();
            Client.SendData(buffer);
        }
    }
}