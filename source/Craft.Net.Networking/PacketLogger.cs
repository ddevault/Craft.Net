using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Craft.Net.Networking
{
    public static class PacketLogger
    {
        public static string LogPacket(IPacket packet, PacketDirection direction)
        {
            var memory = new MemoryStream();
            var stream = new StreamWriter(memory);
            var type = packet.GetType();
            var fields = type.GetFields();
            // Log time, direction, name
            stream.Write(DateTime.Now.ToString("{hh:mm:ss.fff} "));
            if (direction == PacketDirection.Serverbound)
                stream.Write("[CLIENT->SERVER] ");
            else
                stream.Write("[SERVER->CLIENT] ");
            stream.Write(FormatPacketName(type.Name));
            stream.WriteLine();

            // Log fields
            foreach (var field in fields)
            {
                var name = field.Name;
                if (field.Name == "PacketId")
                    continue;
                name = AddSpaces(name);
                var fValue = field.GetValue(packet);

                if (!(fValue is Array))
                    stream.Write(string.Format(" {0} ({1})", name, field.FieldType.Name));
                else
                {
                    var array = fValue as Array;
                    stream.Write(string.Format(" {0} ({1}[{2}])", name,
                        array.GetType().GetElementType().Name, array.Length));
                }

                if (fValue is byte[])
                    stream.Write(": " + DumpArray(fValue as byte[]) + "\n");
                else if (fValue is Array)
                {
                    stream.Write(": ");
                    var array = fValue as Array;
                    foreach (var item in array)
                        stream.Write(string.Format("{0}, ", item.ToString()));
                    stream.WriteLine();
                }
                else if (fValue is string)
                    stream.Write(": \"" + fValue + "\"\n");
                else
                    stream.Write(": " + fValue + "\n");
            }
            stream.WriteLine();
            stream.Flush();
            return Encoding.UTF8.GetString(memory.GetBuffer().Take((int)memory.Length).ToArray());
        }

        private static string FormatPacketName(string name)
        {
            if (name.EndsWith("Packet"))
                name = name.Remove(name.Length - "Packet".Length);
            // TODO: Consider adding spaces before capital letters
            return name;
        }

        private static string DumpArray(byte[] array)
        {
            if (array.Length == 0)
                return "[]";
            var sb = new StringBuilder((array.Length * 2) + 2);
            foreach (byte b in array)
                sb.AppendFormat("{0} ", b.ToString("X2"));
            return "[" + sb.ToString().Remove(sb.Length - 1) + "]";
        }

        private static string DumpArrayPretty(byte[] array)
        {
            if (array.Length == 0)
                return "[Empty arry]";
            int length = 5 * array.Length + (4 * (array.Length / 16)) + 2; // rough estimate of final length
            var sb = new StringBuilder(length);
            sb.AppendLine("[");
            for (int i = 0; i < array.Length; i += 16)
            {
                sb.Append(" ");
                // Hex dump
                int hexCount = 16;
                for (int j = i; j < array.Length && j < i + 16; j++, hexCount--)
                    sb.AppendFormat("{0} ", array[j].ToString("X2"));
                sb.Append(" ");
                for (; hexCount > 0; hexCount--)
                    sb.Append(" ");
                for (int j = i; j < array.Length && j < i + 16; j++)
                {
                    char value = Encoding.ASCII.GetString(new byte[] { array[j] })[0];
                    if (char.IsLetterOrDigit(value))
                        sb.AppendFormat("{0} ", value);
                    else
                        sb.Append(". ");
                }
                sb.AppendLine();
            }
            sb.AppendLine("]");
            string result = " " + sb.ToString().Replace("\n", "\n ");
            return result.Remove(result.Length - 2);
        }

        private static string AddSpaces(string value)
        {
            string newValue = "";
            foreach (char c in value)
            {
                if (char.IsLower(c))
                    newValue += c;
                else
                    newValue += " " + c;
            }
            return newValue.Substring(1);
        }
    }
}

