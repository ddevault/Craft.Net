using System;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Packets
{
    public enum PlayerAction
    {
        StartedDigging = 0,
        FinishedDigging = 2,
        DropItem = 4,
        ShootArrow = 5
    }

    public class PlayerDiggingPacket : Packet
    {
        public PlayerAction Action;
        public Vector3 Position;
        public byte Face;

        public PlayerDiggingPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xE;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            byte action, y;
            int x, z;
            if (!TryReadByte(Buffer, ref offset, out action))
                return -1;
            if (!TryReadInt(Buffer, ref offset, out x))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out y))
                return -1;
            if (!TryReadInt(Buffer, ref offset, out z))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out Face))
                return -1;
            Position = new Vector3(x, y, z);
            Action = (PlayerAction)action;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            // TODO
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }
    }
}

