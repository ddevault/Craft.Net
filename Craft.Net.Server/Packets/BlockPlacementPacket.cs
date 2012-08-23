using System;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Server.Packets
{
    public class BlockPlacementPacket : Packet
    {
        public Vector3 CursorPosition;
        public byte Direction;
        public Slot HeldItem;
        public Vector3 Position;

        public override byte PacketId
        {
            get { return 0xF; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            int x = 0, z = 0;
            byte y = 0;
            byte curX, curY, curZ;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out x))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out y))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out z))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out Direction))
                return -1;
            if (!Slot.TryReadSlot(buffer, ref offset, out HeldItem))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out curX))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out curY))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out curZ))
                return -1;
            Position = new Vector3(x, y, z);
            CursorPosition = new Vector3(curX, curY, curZ);
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (client.Entity.Position.DistanceTo(Position) > 6) // TODO: Use client.Reach
                return;
            Item item = client.Entity.Inventory[client.Entity.SelectedSlot].Item;
            if (item != null)
            {
                item.OnItemUsed(Position, AdjustByDirection(Direction), CursorPosition, server.GetClientWorld(client), client.Entity);
                if (client.Entity.GameMode != GameMode.Creative)
                    client.Entity.Inventory[client.Entity.SelectedSlot].Count--;
            }
            else
            {
                client.SendPacket(new DisconnectPacket("Unrecognized item!"));
                server.ProcessSendQueue();
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