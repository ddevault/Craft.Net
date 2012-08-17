using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Craft.Net.Server
{
    /// <summary>
    /// Describes the direction this packet moves across a TCP connection.
    /// </summary>
    public enum PacketContext
    {
        ClientToServer,
        ServerToClient
    }

    /// <summary>
    /// Describes a Minecraft packet that may be sent over a TCP connection.
    /// </summary>
    public abstract class Packet
    {
        /// <summary>
        /// The direction this packet travels.
        /// </summary>
        public PacketContext PacketContext { get; set; }
        /// <summary>
        /// This packet's packet ID.
        /// </summary>
        public abstract byte PacketId { get; }

        /// <summary>
        /// This event fires after the packet has been sent.
        /// </summary>
        public event EventHandler OnPacketSent;

        internal void FirePacketSent()
        {
            if (OnPacketSent != null)
                OnPacketSent(this, null);
        }

        /// <summary>
        /// Attempts to read a packet from the given buffer. Returns the length of
        /// the packet if successful, or -1 if the packet is incomplete.
        /// </summary>
        public abstract int TryReadPacket(byte[] buffer, int length);
        /// <summary>
        /// Handles the server-side logic for recieving the packet.
        /// </summary>
        public abstract void HandlePacket(MinecraftServer server, ref MinecraftClient client);
        /// <summary>
        /// Sends the packet to a client.
        /// </summary>
        public abstract void SendPacket(MinecraftServer server, MinecraftClient client);

        /// <summary>
        /// Converts the packet to a human-readable format.
        /// </summary>
        public override string ToString()
        {
            Type type = GetType();
            string value = type.Name + " (0x" + PacketId.ToString("x") + ")\n";
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                value += "\t" + field.Name + ": " + field.GetValue(this) + "\n";
            }
            return value.Remove(value.Length - 1);
        }
    }
}