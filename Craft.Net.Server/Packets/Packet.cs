using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Craft.Net.Server.Packets
{
    public enum PacketContext
    {
        ClientToServer,
        ServerToClient
    }

    public abstract class Packet
    {
        public PacketContext PacketContext { get; set; }

        public event EventHandler OnPacketSent;
        public void FirePacketSent()
        {
            if (OnPacketSent != null)
                OnPacketSent(this, null);
        }

        public abstract byte PacketID { get; }
        public abstract int TryReadPacket(byte[] Buffer, int Length);
        public abstract void HandlePacket(MinecraftServer Server, ref MinecraftClient Client);
        public abstract void SendPacket(MinecraftServer Server, MinecraftClient Client);

        public override string ToString()
        {
            Type type = this.GetType();
            string value = type.Name + " (0x" + this.PacketID.ToString("x") + ")\n";
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                value += "\t" + field.Name + ": " + field.GetValue(this).ToString() + "\n";
            }
            return value.Remove(value.Length - 1);
        }

        #region Packet Writer Methods

        protected internal static byte[] CreateString(string Text)
        {
            return CreateShort((short)Text.Length)
                .Concat(Encoding.BigEndianUnicode.GetBytes(Text)).ToArray();
        }

        protected internal static byte[] CreateBoolean(bool Value)
        {
            return new byte[]
            {
                (byte)(Value ? 1 : 0)
            };
        }

        protected internal static byte[] CreateUShort(ushort Value)
        {
            return BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)Value));
        }

        protected internal static byte[] CreateShort(short Value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Value));
        }

        protected internal static byte[] CreateInt(int Value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Value));
        }

        protected internal static byte[] CreateFloat(float Value)
        {
            return BitConverter.GetBytes(Value).Reverse().ToArray();
        }

        protected internal static byte[] CreatePackedByte(float Value)
        {
            return new byte[] { (byte)(((Math.Floor(Value) % 360) / 360) * 256) };
        }

        protected internal static byte[] CreateDouble(double Value)
        {
            return BitConverter.GetBytes(Value).Reverse().ToArray();
        }

        #endregion

        #region Packet Reader Methods

        protected static short ReadShort(byte[] buffer, int offset)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        }

        protected internal static bool TryReadBoolean(byte[] buffer, ref int offset, out bool value)
        {
            value = false;
            if (buffer.Length - offset >= 1)
                value = buffer[offset++] == 1;
            else
                return false;
            return true;
        }

        protected internal static bool TryReadByte(byte[] buffer, ref int offset, out byte value)
        {
            value = 0;
            if (buffer.Length - offset >= 1)
                value = buffer[offset++];
            else
                return false;
            return true;
        }

        protected internal static bool TryReadShort(byte[] buffer, ref int offset, out short value)
        {
            value = -1;
            if (buffer.Length - offset >= 2)
            {
                value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
                offset += 2;
                return true;
            }
            else
                return false;
        }

        protected internal static bool TryReadInt(byte[] buffer, ref int offset, out int value)
        {
            value = -1;
            if (buffer.Length - offset >= 4)
            {
                value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, offset));
                offset += 4;
                return true;
            }
            else
                return false;
        }

        protected internal static bool TryReadFloat(byte[] buffer, ref int offset, out float value)
        {
            value = -1;
            if (buffer.Length - offset >= 4)
            {
                Array.Reverse(buffer, offset, 4);
                value = BitConverter.ToSingle(buffer, offset);
                offset += 4;
                return true;
            }
            else
                return false;
        }

        protected internal static bool TryReadDouble(byte[] buffer, ref int offset, out double value)
        {
            value = -1;
            if (buffer.Length - offset >= 8)
            {
                Array.Reverse(buffer, offset, 8);
                value = BitConverter.ToDouble(buffer, offset);
                offset += 8;
                return true;
            }
            else
                return false;
        }

        protected internal static bool TryReadString(byte[] buffer, ref int offset, out string value)
        {
            value = null;
            short length;
            if (buffer.Length - offset >= 2)
                length = ReadShort(buffer, offset);
            else
                return false;
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "String length is less than zero");
            offset += 2;
            if (buffer.Length - offset >= length)
            {
                value = Encoding.BigEndianUnicode.GetString(buffer, offset, length * 2);
                offset += length * 2;
                return true;
            }
            else
                return false;
        }

        protected internal static bool TryReadArray(byte[] buffer, short length, ref int offset, out byte[] value)
        {
            value = null;
            if (buffer.Length - offset < length)
                return false;
            else
            {
                value = new byte[length];
                Array.Copy(buffer, offset, value, 0, length);
                offset += length;
                return true;
            }
        }

        #endregion
    }
}

