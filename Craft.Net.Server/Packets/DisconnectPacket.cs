using System;
using System.Linq;
using System.Net.Sockets;

namespace Craft.Net.Server.Packets
{
    public class DisconnectPacket : Packet
    {
        public string Reason;
        
        public DisconnectPacket()
        {
        }
        
        public DisconnectPacket(string Reason)
        {
            this.Reason = Reason;
        }

        public override byte PacketID
        {
            get
            {
                return 0xFF;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadString(Buffer, ref offset, out Reason))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Server.Log(Client.Username + " disconnected (" + Reason + ")");
            Client.IsDisconnected = true;
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            if (!Reason.Contains("§"))
                Server.Log("Disconnected client: " + Reason);
            byte[] buffer = new byte[] { PacketID }.Concat(CreateString(Reason)).ToArray();
            Client.SendData(buffer);
            Client.IsDisconnected = true;
        }
    }
}

