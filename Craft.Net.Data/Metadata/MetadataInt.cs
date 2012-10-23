using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public class MetadataInt : MetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "int"; } }

        public int Value;

        public MetadataInt(byte index) : base(index)
        {
        }

        public MetadataInt(byte index, int value) : base(index)
        {
            Value = value;
        }

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 5)
                return false;
            offset++;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out Value))
                return false;
            return true;
        }

        public override byte[] Encode()
        {
            byte[] data = new byte[5];
            data[0] = GetKey();
            Array.Copy(DataUtility.CreateInt32(Value), 0, data, 1, 4);
            return data;
        }
    }
}
