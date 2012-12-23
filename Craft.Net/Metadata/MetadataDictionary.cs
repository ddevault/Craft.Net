using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Craft.Net.Metadata
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

        public static MetadataDictionary FromStream(MinecraftStream stream)
        {
            var value = new MetadataDictionary();
            byte key = 0;
            while (key != 127)
            {
                key = stream.ReadUInt8();
                if (key == 127) break;
                byte type = (byte)((key & 0xE0) >> 5);
                byte index = (byte)(key & 0x1F);
                var entryType = EntryTypes[type];
                value[index] = (MetadataEntry)Activator.CreateInstance(entryType, index);
                value[index].FromStream(stream);
            }
            return value;
        }

        public void WriteTo(MinecraftStream stream)
        {
            foreach (var entry in entries)
                entry.Value.WriteTo(stream);
            stream.WriteUInt8(0x7F);
        }

        private static Type[] EntryTypes = new[]
            {
                typeof(MetadataByte), // 0
                typeof(MetadataShort), // 1
                typeof(MetadataInt), // 2
                typeof(MetadataFloat), // 3
                typeof(MetadataString), // 4
                typeof(MetadataSlot), // 5
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
