using System;
using System.IO;
using System.Text;

namespace Craft.Net.Classic.Common
{
    public partial class MinecraftStream : Stream
    {
        public byte ReadUInt8()
        {
            int value = BaseStream.ReadByte();
            if (value == -1)
                throw new EndOfStreamException();
            return (byte)value;
        }

        public void WriteUInt8(byte value)
        {
            WriteByte(value);
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        public void WriteInt8(sbyte value)
        {
            WriteUInt8((byte)value);
        }

        public ushort ReadUInt16()
        {
            return (ushort)(
                (ReadUInt8() << 8) |
                ReadUInt8());
        }

        public void WriteUInt16(ushort value)
        {
            Write(new[]
            {
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            }, 0, 2);
        }

        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        public bool ReadBoolean()
        {
            return ReadUInt8() != 0;
        }

        public void WriteBoolean(bool value)
        {
            if (value) WriteUInt8(1);
            else       WriteUInt8(0);
        }

        public void WriteArray(byte[] data)
        {
            if (data.Length != 1024)
                throw new ArgumentOutOfRangeException("data", "Array must be less than or equal to 1024 bytes in length.");
            var difference = 1024 - data.Length;
            Write(data, 0, data.Length);
            if (difference > 0)
                Write(new byte[difference], 0, difference);
        }

        public byte[] ReadArray()
        {
            var data = new byte[1024];
            Read(data, 0, data.Length);
            return data;
        }

        public string ReadString()
        {
            var raw = new byte[64];
            Read(raw, 0, 64);
            return Encoding.ASCII.GetString(raw).TrimEnd(' ');
        }

        public void WriteString(string value)
        {
            if (value.Length > 64)
                throw new ArgumentOutOfRangeException("value", "String must be less than or equal to 64 characters in length.");
            value = value.PadRight(64, ' ');
            var raw = Encoding.ASCII.GetBytes(value);
            Write(raw, 0, raw.Length);
        }
    }
}

