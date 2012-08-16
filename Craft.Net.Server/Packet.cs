using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Craft.Net.Server
{
    public enum PacketContext
    {
        ClientToServer,
        ServerToClient
    }

    public abstract class Packet
    {
        public PacketContext PacketContext { get; set; }
        public abstract byte PacketId { get; }

        public event EventHandler OnPacketSent;

        public void FirePacketSent()
        {
            if (OnPacketSent != null)
                OnPacketSent(this, null);
        }

        public abstract int TryReadPacket(byte[] buffer, int length);
        public abstract void HandlePacket(MinecraftServer server, ref MinecraftClient client);
        public abstract void SendPacket(MinecraftServer server, MinecraftClient client);

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