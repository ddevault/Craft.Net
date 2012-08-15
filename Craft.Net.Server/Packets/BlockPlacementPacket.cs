using System;
using Craft.Net.Server.Blocks;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Packets
{
    public class BlockPlacementPacket : Packet
    {
        public Vector3 CursorPosition;
        public byte Direction;
        public Slot HeldItem;
        public Vector3 Position;

        public override byte PacketID
        {
            get { return 0xF; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            int x = 0, z = 0;
            byte y = 0;
            byte curX, curY, curZ;
            if (!TryReadInt(buffer, ref offset, out x))
                return -1;
            if (!TryReadByte(buffer, ref offset, out y))
                return -1;
            if (!TryReadInt(buffer, ref offset, out z))
                return -1;
            if (!TryReadByte(buffer, ref offset, out Direction))
                return -1;
            if (!Slot.TryReadSlot(buffer, ref offset, out HeldItem))
                return -1;
            if (!TryReadByte(buffer, ref offset, out curX))
                return -1;
            if (!TryReadByte(buffer, ref offset, out curY))
                return -1;
            if (!TryReadByte(buffer, ref offset, out curZ))
                return -1;
            Position = new Vector3(x, y, z);
            CursorPosition = new Vector3(curX, curY, curZ);
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (client.Entity.Position.DistanceTo(Position) > 6)
                return;
            if (HeldItem.Id < 0x80)
            {
                var block = (Block)HeldItem.Id;
                Vector3 clickedBlock = Position;
                Vector3 placedBlock = Position;
                placedBlock += AdjustByDirection(Direction);
                if (block != null)
                {
                    // TODO: More stuff here
                    server.GetClientWorld(client).SetBlock(placedBlock, block);
                }
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new NotImplementedException();
        }

        private static Vector3 AdjustByDirection(byte direction)
        {
            switch (direction)
            {
                case 0:
                    return Vector3.Down;
                case 1:
                    return Vector3.Up;
                case 2:
                    return Vector3.Backwards;
                case 3:
                    return Vector3.Forwards;
                case 4:
                    return Vector3.Left;
                default:
                    return Vector3.Right;
            }
        }
    }
}