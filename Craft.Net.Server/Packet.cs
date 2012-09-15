using System;
using System.Linq;
using System.Reflection;
using System.Text;
using LibNbt;

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
        /// Creates the buffer to send to the client. 
        /// You don't have to pass the PacketID, it will be used automatically.
        /// </summary>
        /// <returns>
        /// A buffer with all the bytes contained as params.
        /// </returns>
        /// <param name='byteArrays'>
        /// Byte arrays that should be inside of the buffer.
        /// </param>
        protected byte[] CreateBuffer(params byte[][] byteArrays)
        {
            byte[] result = new byte[1 + byteArrays.Sum(x => x.Length)];
            int resultOffset = 1;
            
            result [0] = PacketId;

            int byteLength = byteArrays.Length;
            for (int i = 0; i < byteLength; i++)
            {
                Buffer.BlockCopy(byteArrays [i], 0, result, resultOffset, byteArrays [i].Length);
                resultOffset += byteArrays [i].Length;
            }
            
            return result;
        }

        /// <summary>
        /// Attempts to read a packet from the given buffer. Returns the length of
        /// the packet if successful, or -1 if the packet is incomplete.
        /// </summary>
        public abstract int TryReadPacket(byte[] buffer, int length);
        /// <summary>
        /// Handles the server-side logic for recieving the packet.
        /// </summary>
        public abstract void HandlePacket(MinecraftServer server, MinecraftClient client);
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
                var fieldValue = field.GetValue(this);
                if (fieldValue is NbtFile)
                    fieldValue = ((NbtFile)fieldValue).RootTag;
                value += "\t" + field.Name + ": " + fieldValue + "\n";
            }
            return value.Remove(value.Length - 1);
        }
    }
}