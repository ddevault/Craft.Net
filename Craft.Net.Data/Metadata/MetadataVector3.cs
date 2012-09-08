using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public class MetadataVector3 : MetadataEntry
    {
        public override byte Identifier { get { return 2; } }
        public override string FriendlyName { get { return "float"; } }

        public Vector3 Value;

        public MetadataVector3(byte index) : base(index)
        {
        }

        public MetadataVector3(byte index, Vector3 value) : base(index)
        {
            Value = value;
        }

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 13)
                return false;
            offset++;
            int x, y, z;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out x))
                return false;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out y))
                return false;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out z))
                return false;
            Value = new Vector3(x, y, z);
            return true;
        }

        public override byte[] Encode()
        {
            byte[] data = new byte[13];
            data[0] = GetKey();
            Array.Copy(DataUtility.CreateInt32((int)Value.X), 0, data, 1, 4);
            Array.Copy(DataUtility.CreateInt32((int)Value.Y), 0, data, 5, 4);
            Array.Copy(DataUtility.CreateInt32((int)Value.Z), 0, data, 9, 4);
            return data;
        }
    }
}
