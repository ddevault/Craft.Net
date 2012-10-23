using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Craft.Net.Data.Metadata
{
    public abstract class MetadataEntry
    {
        public MetadataEntry(byte index)
        {
            Index = index;
            Index &= 0x1F;
        }

        public byte Index { get; set; }
        public abstract byte Identifier { get; }
        public abstract string FriendlyName { get; }

        /// <summary>
        /// Assume that offset points to the key.
        /// </summary>
        public abstract bool TryReadEntry(byte[] buffer, ref int offset);

        public abstract byte[] Encode();

        protected byte GetKey()
        {
            return (byte)((Identifier << 5) | Index);
        }

        public override string ToString()
        {
            Type type = GetType();
            FieldInfo[] fields = type.GetFields();
            string result = FriendlyName + "[" + Index + "]: ";
            if (fields.Length != 0)
                result += fields[0].GetValue(this).ToString();
            return result;
        }
    }
}
