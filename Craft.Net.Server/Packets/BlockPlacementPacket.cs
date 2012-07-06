using System;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Packets
{
    public class BlockPlacementPacket : Packet
    {
        public Vector3 Position;
        public byte Direction;
        public Slot HeldItem;

        public BlockPlacementPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xF;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            int x = 0, z = 0;
            byte y = 0;
            if (!TryReadInt(Buffer, ref offset, out x))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out y))
                return -1;
            if (!TryReadInt(Buffer, ref offset, out z))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out Direction))
                return -1;
            if (!Slot.TryReadSlot(Buffer, ref offset, out HeldItem))
                return -1;
            Position = new Vector3(x, y, z);
            Console.WriteLine(Position.ToString() + ":" + Direction + ";" +
                ((Block)((byte)HeldItem.Id)).GetType().Name + "[" +
                HeldItem.Count + ":" + HeldItem.Metadata + "]");
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            if (HeldItem.Id < 256)
            {
                Block block = (Block)HeldItem.Id;
                if (block != null)
                {
                }
            }
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new System.NotImplementedException();
        }
    }
}

