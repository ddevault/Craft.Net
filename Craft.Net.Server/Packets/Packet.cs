using System;
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
            if (Text.Length > 240)
                throw new ArgumentOutOfRangeException("Text", "Text.Lenght can't be > 240."); //regarding to http://www.wiki.vg/Data_Types, 

            return CreateShort((short)Text.Length)
                .Concat(Encoding.BigEndianUnicode.GetBytes(Text)).ToArray();
        }

        protected internal static byte[] CreateBoolean(bool Value)
        {
            return new byte[]
            {
                unchecked((byte)(Value ? 1 : 0))
            };
        }

        protected internal static byte[] CreateUShort(ushort Value)
        {
            unchecked
            {
                return new byte[] { (byte)(Value >> 8), (byte)(Value) };
            }
//            return BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)Value));
        }

        protected internal static byte[] CreateShort(short Value)
        {
            unchecked
            {
                return new byte[] { (byte)(Value >> 8), (byte)(Value) };
            }

//            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Value));
        }

        protected internal static byte[] CreateInt(int Value)
        {
            unchecked
            {
                return new byte[] { (byte)(Value >> 24), (byte)(Value >> 16), 
                    (byte)(Value >> 8), (byte)(Value) };
            }
//            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Value));
        }

        protected internal static byte[] CreateLong(long Value)
        {
            unchecked
            {
                //byte[] v = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
                return new byte[] { (byte)(Value >> 56), (byte)(Value >> 48), 
                                    (byte)(Value >> 40), (byte)(Value >> 32),
                                    (byte)(Value >> 24), (byte)(Value >> 16),
                                    (byte)(Value >> 8), (byte)(Value) };
            }
        }

        protected internal unsafe static byte[] CreateFloat(float Value)
        {
            return CreateInt(*(int*)&Value);
        }

        protected internal static byte[] CreatePackedByte(float Value)
        {
            return new byte[] { (byte)(((Math.Floor(Value) % 360) / 360) * 256) };
        }

        protected internal unsafe static byte[] CreateDouble(double Value)
        {
            return CreateLong(*(long*)&Value);
            //for reference see Mono implementation of BitConverter.DoubleToInt64Bits(double)
        }

        #endregion

        #region Packet Reader Methods

        protected static short ReadShort(byte[] buffer, int offset)
        {
            return unchecked((short)(buffer [0 + offset] << 8 | buffer [1 + offset]));
//            return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        }

        protected internal static bool TryReadBoolean(byte[] buffer, ref int offset, out bool value)
        {
            value = false;
            if (buffer.Length - offset >= 1)
            {
                value = buffer [offset++] == 1;
                return true;
            } else
                return false;
        }

        protected internal static bool TryReadByte(byte[] buffer, ref int offset, out byte value)
        {
            value = 0;
            if (buffer.Length - offset >= 1)
            {
                value = buffer [offset++];
                return true;
            } else
                return false;
        }

        protected internal static bool TryReadShort(byte[] buffer, ref int offset, out short value)
        {
            value = -1;
            if (buffer.Length - offset >= 2)
            {
//                value = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
                value = unchecked((short)(buffer [0 + offset] << 8 | buffer [1 + offset]));
                offset += 2;
                return true;
            } else
                return false;
        }

        protected internal static bool TryReadInt(byte[] buffer, ref int offset, out int value)
        {
            value = -1;
            if (buffer.Length - offset >= 4)
            {
//                value = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, offset));
                value = unchecked(buffer [0 + offset] << 24 |
                    buffer [1 + offset] << 16 |
                    buffer [2 + offset] << 8 |
                    buffer [3 + offset]);
                offset += 4;
                return true;
            } else
                return false;
        }

        protected internal static bool TryReadLong(byte[] buffer, ref int offset, out long value)
        {
            if (buffer.Length - offset >= 4)
            {
                unchecked
                {
                    value = 0;
                    value |= (long)buffer [0 + offset] << 56;
                    value |= (long)buffer [1 + offset] << 48;
                    value |= (long)buffer [2 + offset] << 40;
                    value |= (long)buffer [3 + offset] << 32;
                    value |= (long)buffer [4 + offset] << 24;
                    value |= (long)buffer [5 + offset] << 16;
                    value |= (long)buffer [6 + offset] << 8;
                    value |= (long)buffer [7 + offset];
                }
                offset += 8;
                return true;
            } else
            {
                value = -1;
                return false;
            }
        }

        protected internal unsafe static bool TryReadFloat(byte[] buffer, ref int offset, out float value)
        {
            value = -1;
            int i;

            if (TryReadInt(buffer, ref offset, out i))
            {
                value = *(float*)&i;
                return true;
            }

            return false;
        }

        protected internal unsafe static bool TryReadDouble(byte[] buffer, ref int offset, out double value)
        {
            value = -1;
            long l;

            if (TryReadLong(buffer, ref offset, out l))
            {
                value = *(double*)&l;
                return true;
            }

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

