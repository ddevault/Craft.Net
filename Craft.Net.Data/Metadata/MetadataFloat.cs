using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public class MetadataFloat : MetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "float"; } }

        public float Value;

        public MetadataFloat(byte index) : base(index)
        {
        }

        public MetadataFloat(byte index, float value) : base(index)
        {
            Value = value;
        }

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 5)
                return false;
            offset++;
            if (!DataUtility.TryReadFloat(buffer, ref offset, out Value))
                return false;
            return true;
        }

        public override byte[] Encode()
        {
            byte[] data = new byte[5];
            data[0] = GetKey();
            Array.Copy(DataUtility.CreateFloat(Value), 0, data, 1, 4);
            return data;
        }
    }
}
