using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data
{
    /// <summary>
    /// Provides several useful utilities for manipulating
    /// Minecraft data.
    /// </summary>
    public static class DataUtility
    {
        /// <summary>
        /// A global <see cref="System.Random"/> instance.
        /// </summary>
        public static Random Random = new Random();

        #region Math

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it, on a flat plane.
        /// </summary>
        public static Direction DirectionByRotationFlat(Entity p, bool invert = false)
        {
            byte direction = (byte)((int)Math.Floor((p.Yaw * 4F) / 360F + 0.5D) & 3);
            if (invert)
                switch (direction) // TODO: Don't cast these
                {
                    case 0: return (Direction)2;
                    case 1: return (Direction)5;
                    case 2: return (Direction)3;
                    case 3: return (Direction)4;
                }
            else
                switch (direction)
                {
                    case 0: return (Direction)3;
                    case 1: return (Direction)4;
                    case 2: return (Direction)2;
                    case 3: return (Direction)5;
                }
            return 0;
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it.
        /// </summary>
        public static Direction DirectionByRotation(PlayerEntity p, Vector3 position, bool invert = false)
        {
            // TODO: Figure out some algorithm based on player's look yaw
            double d = Math.Asin((p.Position.Y - position.Y) / position.DistanceTo(p.Position));
            if (d > (Math.PI / 4)) return invert ? (Direction)1 : (Direction)0;
            if (d < -(Math.PI / 4)) return invert ? (Direction)0 : (Direction)1;
            return DirectionByRotationFlat(p, invert);
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it.
        /// </summary>
        public static Vector3 FowardVector(Entity entity, bool invert = false)
        {
            Direction value = (Direction)DirectionByRotationFlat(entity, invert);
            switch (value)
            {
                case Direction.East:
                    return Vector3.East;
                case Direction.West:
                    return Vector3.West;
                case Direction.North:
                    return Vector3.North;
                case Direction.South:
                    return Vector3.South;
                default:
                    return Vector3.Zero;
            }
        }

        public static double Distance2D(double a1, double a2, double b1, double b2)
        {
            return Math.Sqrt(Math.Pow(b1 - a1, 2) + Math.Pow(b2 - a2, 2));
        }

        #endregion

        #region Logging/Debugging

        /// <summary>
        /// Turns the given byte array into a string of hexadecimal
        /// numbers in the format [01 23 45 67]
        /// </summary>
        public static string DumpArray(byte[] array)
        {
            if (array.Length == 0)
                return "[]";
            var sb = new StringBuilder((array.Length * 2) + 2);
            foreach (byte b in array)
                sb.AppendFormat("{0} ", b.ToString("X2"));
            return sb.ToString().Remove(sb.Length - 1) + "]";
        }

        #endregion

        #region Network Data

        /// <summary>
        /// Creates a Minecraft-style length-prefixed UCS-2 string.
        /// </summary>
        public static byte[] CreateString(string text)
        {
            return CreateInt16((short)text.Length)
                .Concat(Encoding.BigEndianUnicode.GetBytes(text)).ToArray();
        }

        /// <summary>
        /// Creates a Minecraft-style boolean whose value is false.
        /// </summary>
        public static byte[] CreateBoolean()
        {
            return CreateBoolean(false);
        }

        /// <summary>
        /// Creates a Minecraft-style boolean.
        /// </summary>
        public static byte[] CreateBoolean(bool value)
        {
            return new[]
                       {
                           unchecked((byte)(value ? 1 : 0))
                       };
        }

        /// <summary>
        /// Creates a Minecraft-style unsigned 16-bit integer.
        /// </summary>
        public static byte[] CreateUInt16(ushort value)
        {
            unchecked
            {
                return new[] { (byte)(value >> 8), (byte)(value) };
            }
        }

        /// <summary>
        /// Creates a Minecraft-style 16-bit integer.
        /// </summary>
        public static byte[] CreateInt16(short value)
        {
            unchecked
            {
                return new[] { (byte)(value >> 8), (byte)(value) };
            }
        }

        /// <summary>
        /// Creates a Minecraft-style unsigned 32-bit integer.
        /// </summary>
        public static byte[] CreateInt32(int value)
        {
            unchecked
            {
                return new[]
                           {
                               (byte)(value >> 24), (byte)(value >> 16),
                               (byte)(value >> 8), (byte)(value)
                           };
            }
        }

        public static byte[] CreateAbsoluteInteger(double value)
        {
            return CreateInt32((int)(value * 32));
        }

        /// <summary>
        /// Creates a Minecraft-style unsigned 64-bit integer.
        /// </summary>
        public static byte[] CreateInt64(long value)
        {
            unchecked
            {
                return new[]
                           {
                               (byte)(value >> 56), (byte)(value >> 48),
                               (byte)(value >> 40), (byte)(value >> 32),
                               (byte)(value >> 24), (byte)(value >> 16),
                               (byte)(value >> 8), (byte)(value)
                           };
            }
        }

        /// <summary>
        /// Creates a Minecraft-style 32-bit floating-point value.
        /// </summary>
        public static unsafe byte[] CreateFloat(float value)
        {
            return CreateInt32(*(int*)&value);
        }

        /// <summary>
        /// Creates a Minecraft-style "packed byte" to map degrees onto
        /// a byte.
        /// </summary>
        public static byte[] CreatePackedByte(float value)
        {
            return new[] { (byte)(((Math.Floor(value) % 360) / 360) * 256) };
        }

        /// <summary>
        /// Creates a Minecraft-style 64-bit floating-point value.
        /// </summary>
        public static unsafe byte[] CreateDouble(double value)
        {
            return CreateInt64(*(long*)&value);
        }

        /// <summary>
        /// Reads a Minecraft-style 16-bit integer from a buffer.
        /// </summary>
        public static short ReadInt16(byte[] buffer, int offset)
        {
            return unchecked((short)(buffer[0 + offset] << 8 | buffer[1 + offset]));
        }

        /// <summary>
        /// Attempts to read a Minecraft-style boolean value from the specified buffer.
        /// </summary>
        public static bool TryReadBoolean(byte[] buffer, ref int offset, out bool value)
        {
            value = false;
            if (buffer.Length - offset >= 1)
            {
                value = buffer[offset++] == 1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 8-bit unsigned integer from the given buffer.
        /// </summary>
        public static bool TryReadByte(byte[] buffer, ref int offset, out byte value)
        {
            value = 0;
            if (buffer.Length - offset >= 1)
            {
                value = buffer[offset++];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 16-bit integer from the given buffer.
        /// </summary>
        public static bool TryReadUInt16(byte[] buffer, ref int offset, out ushort value)
        {
            value = 0xFFFF;
            if (buffer.Length - offset >= 2)
            {
                value = unchecked((ushort)(buffer[0 + offset] << 8 | buffer[1 + offset]));
                offset += 2;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 16-bit integer from the given buffer.
        /// </summary>
        public static bool TryReadInt16(byte[] buffer, ref int offset, out short value)
        {
            value = -1;
            if (buffer.Length - offset >= 2)
            {
                value = unchecked((short)(buffer[0 + offset] << 8 | buffer[1 + offset]));
                offset += 2;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 32-bit integer from the given buffer.
        /// </summary>
        public static bool TryReadInt32(byte[] buffer, ref int offset, out int value)
        {
            value = -1;
            if (buffer.Length - offset >= 4)
            {
                value = unchecked(buffer[0 + offset] << 24 |
                                  buffer[1 + offset] << 16 |
                                  buffer[2 + offset] << 8 |
                                  buffer[3 + offset]);
                offset += 4;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 64-bit integer from the given buffer.
        /// </summary>
        public static bool TryReadInt64(byte[] buffer, ref int offset, out long value)
        {
            if (buffer.Length - offset >= 8)
            {
                unchecked
                {
                    value = 0;
                    value |= (long)buffer[0 + offset] << 56;
                    value |= (long)buffer[1 + offset] << 48;
                    value |= (long)buffer[2 + offset] << 40;
                    value |= (long)buffer[3 + offset] << 32;
                    value |= (long)buffer[4 + offset] << 24;
                    value |= (long)buffer[5 + offset] << 16;
                    value |= (long)buffer[6 + offset] << 8;
                    value |= buffer[7 + offset];
                }
                offset += 8;
                return true;
            }
            value = -1;
            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 32-bit floating-point value from the given buffer.
        /// </summary>
        public static unsafe bool TryReadFloat(byte[] buffer, ref int offset, out float value)
        {
            value = -1;
            int i;

            if (TryReadInt32(buffer, ref offset, out i))
            {
                value = *(float*)&i;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style 64-bit floating-point value from the given buffer.
        /// </summary>
        public static unsafe bool TryReadDouble(byte[] buffer, ref int offset, out double value)
        {
            value = -1;
            long l;

            if (TryReadInt64(buffer, ref offset, out l))
            {
                value = *(double*)&l;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to read a Minecraft-style length-prefixed UCS-2 string from the given buffer.
        /// </summary>
        public static bool TryReadString(byte[] buffer, ref int offset, out string value)
        {
            value = null;
            short length;

            if (buffer.Length - offset >= 2)
                length = ReadInt16(buffer, offset);
            else
                return false;

            if (length < 0)
                throw new ArgumentOutOfRangeException("value", "String length is less than zero");

            offset += 2;
            if (buffer.Length - offset >= length)
            {
                value = Encoding.BigEndianUnicode.GetString(buffer, offset, length * 2);
                offset += length * 2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to read an arbituary number of bytes from the given buffer.
        /// </summary>
        public static bool TryReadArray(byte[] buffer, short length, ref int offset, out byte[] value)
        {
            value = null;
            if (buffer.Length - offset < length)
                return false;
            value = new byte[length];
            Array.Copy(buffer, offset, value, 0, length);
            offset += length;
            return true;
        }

        #endregion

        #region Stream Utilities

        public static int ReadInt32(Stream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static void WriteInt32(Stream stream, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            stream.Write(buffer, 0, buffer.Length);
        }

        #endregion
    }

    public enum Direction
    {
        Bottom = 0,
        Top = 1, 
        North = 2, 
        South = 3,
        West = 4,
        East = 5 
    }
}
