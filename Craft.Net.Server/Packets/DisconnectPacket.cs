using System;
using System.Linq;
using System.Net.Sockets;

namespace Craft.Net.Server
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
            throw new NotImplementedException();
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Client.IsDisconnected = true;
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }.Concat(CreateString(Reason)).ToArray();
            Client.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
            Client.IsDisconnected = true;
        }
    }
}

