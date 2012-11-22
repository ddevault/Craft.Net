using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public class MetadataShort : MetadataEntry
    {
        public override byte Identifier { get { return 1; } }
        public override string FriendlyName { get { return "short"; } }

        public short Value;

        public MetadataShort(byte index) : base(index)
        {
        }

        public MetadataShort(byte index, short value) : base(index)
        {
            Value = value;
        }

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 3)
                return false;
            offset++;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out Value))
                return false;
            return true;
        }

        public override byte[] Encode()
        {
            byte[] data = new byte[3];
            data[0] = GetKey();
            Array.Copy(DataUtility.CreateInt16(Value), 0, data, 1, 2);
            return data;
        }
    }
}