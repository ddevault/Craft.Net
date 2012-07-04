using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class PlayerListItemPacket : Packet
    {
        public string PlayerName;
        public bool Online;
        public short Ping;

        public PlayerListItemPacket()
        {
        }

        public PlayerListItemPacket(string PlayerName, bool Online, short Ping)
        {
            this.PlayerName = PlayerName;
            this.Online = Online;
            this.Ping = Ping;
        }

        public override byte PacketID
        {
            get
            {
                return 0xC9;
            }
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
            byte[] buffer = new byte[] { PacketID }
                .Concat(CreateString(PlayerName))
                .Concat(CreateBoolean(Online))
                .Concat(CreateShort(Ping)).ToArray();
            Client.SendData(buffer);
        }
    }
}

