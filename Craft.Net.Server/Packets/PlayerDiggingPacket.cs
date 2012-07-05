using System;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Blocks;

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
        public static double MaxDigDistance = 6;

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
            if (Client.Entity.Position.DistanceTo(Position) <= MaxDigDistance)
            {
                switch (Action)
                {
                    case PlayerAction.StartedDigging:
                        // if (creative)
                        Server.GetClientWorld(Client).SetBlock(Position, new AirBlock());
                        break;
                    case PlayerAction.FinishedDigging:
                        Server.GetClientWorld(Client).SetBlock(Position, new AirBlock());
                        break;
                }
            }
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }
    }
}

