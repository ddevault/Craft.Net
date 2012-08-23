using System;
using System.Linq;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class PlayerListItemPacket : Packet
    {
        public bool Online;
        public short Ping;
        public string PlayerName;

        public PlayerListItemPacket()
        {
        }

        public PlayerListItemPacket(string playerName, bool online, short ping)
        {
            this.PlayerName = playerName;
            this.Online = online;
            this.Ping = ping;
        }

        public override byte PacketId
        {
            get { return 0xC9; }
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
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateString(PlayerName))
                .Concat(DataUtility.CreateBoolean(Online))
                .Concat(DataUtility.CreateInt16(Ping)).ToArray();
            client.SendData(buffer);
        }
    }
}