using System;

namespace Craft.Net.Server.Packets
{
    public class LoginPacket : Packet
    {
        public LoginPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x1;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            throw new System.NotImplementedException();
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            throw new System.NotImplementedException();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            if (this.PacketContext == PacketContext.ClientToServer)
                throw new InvalidOperationException();
            else
                Client.SendData(new byte[] { PacketID });
        }
    }
}

