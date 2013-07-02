using System;

namespace Craft.Net.Client
{
    public class PacketEventArgs : EventArgs
    {
        public IPacket Packet { get; set; }

        public PacketEventArgs(IPacket packet)
        {
            Packet = packet;
        }
    }
}

