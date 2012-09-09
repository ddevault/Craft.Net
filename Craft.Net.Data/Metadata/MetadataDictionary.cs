using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    /// <summary>
    /// Used to send metadata with entities
    /// </summary>
    public class MetadataDictionary
    {
        private Dictionary<int, MetadataEntry> entries;

        public MetadataDictionary()
        {
            entries = new Dictionary<int, MetadataEntry>();
        }

        public MetadataEntry this[int index]
        {
            get { return entries[index]; }
            set { entries[index] = value; }
        }

        public byte[] Encode()
        {
            IEnumerable<byte> data = new byte[0];
            foreach (var entry in entries)
                data = data.Concat(entry.Value.Encode());
            data = data.Concat(new byte[] {127});
            return data.ToArray();
        }

        public static bool TryReadMetadata(byte[] buffer, ref int offset, out MetadataDictionary value)
        {
            value = new MetadataDictionary();
            while (buffer[offset] != 127)
            {
                byte key = buffer[offset];
                byte type = (byte)((key & 0xE0) >> 5);
                byte index = (byte)(key & 0x1F);
                var entryType = EntryTypes[type];
                value[index] = (MetadataEntry)Activator.CreateInstance(entryType, index);
                if (!value[index].TryReadEntry(buffer, ref offset))
                    return false;
                if (offset >= buffer.Length)
                    return false;
            }
            offset++;
            return true;
        }

        private static Type[] EntryTypes = new Type[]
            {
                typeof(MetadataByte), // 0
                typeof(MetadataShort), // 1
                typeof(MetadataInt), // 2
                typeof(MetadataFloat), // 3
                typeof(MetadataString), // 4
                typeof(MetadataSlot), // 5
                typeof(MetadataVector3) // 6
            };

        public override string ToString()
        {
            var value = "";
            foreach (var entry in entries)
                value += entry.Value.ToString() + ", ";
            return value.Remove(value.Length - 2);
        }
    }
}
