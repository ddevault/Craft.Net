using System;
using System.Net;
using System.Text;
using System.Linq;

namespace Craft.Net.Server
{
    public abstract class Packet
    {
        public abstract byte PacketID { get; }
        public abstract int TryReadPacket(byte[] Buffer, int Length);
        public abstract void HandlePacket(MinecraftServer Server, ref MinecraftClient Client);
        public abstract void SendPacket(MinecraftServer Server, MinecraftClient Client);

        #region Packet Builder Methods

        protected static byte[] CreateString(string Text)
        {
            return CreateShort((short)Text.Length)
                .Concat(Encoding.BigEndianUnicode.GetBytes(Text)).ToArray();
        }

        protected static byte[] CreateShort(short Value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Value));
        }

        #endregion

        #region Packet Reader Methods

        protected static short ReadShort(byte[] buffer, int offset)
        {
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        }

        protected static bool TryReadByte(byte[] buffer, ref int offset, out byte value)
        {
            value = 0;
            if (buffer.Length - offset >= 1)
                value = buffer[offset++];
            else
                return false;
            return true;
        }

        protected static bool TryReadShort(byte[] buffer, ref int offset, out short value)
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

        protected static bool TryReadInt(byte[] buffer, ref int offset, out int value)
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

        protected static bool TryReadString(byte[] buffer, ref int offset, out string value)
        {
            value = null;
            short length;
            if (buffer.Length - offset >= 2)
                length = ReadShort(buffer, offset);
            else
                return false;
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

        protected static bool TryReadArray(byte[] buffer, short length, ref int offset, out byte[] value)
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

