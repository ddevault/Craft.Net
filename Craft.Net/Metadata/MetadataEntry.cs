using System;
using System.Reflection;

namespace Craft.Net.Metadata
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

        public abstract void FromStream(MinecraftStream stream);
        public abstract void WriteTo(MinecraftStream stream);

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
