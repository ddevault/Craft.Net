using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public class MetadataByte : MetadataEntry
    {
        public override byte Identifier { get { return 0; } }
        public override string FriendlyName { get { return "byte"; } }

        public byte Value;

        public MetadataByte(byte index) : base(index)
        {
        }

        public MetadataByte(byte index, byte value) : base(index)
        {
            Value = value;
        }

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 2)
                return false;
            Value = buffer[offset++];
            offset++;
            return true;
        }

        public override byte[] Encode()
        {
            return new byte[] { GetKey(), Value };
        }
    }
}
