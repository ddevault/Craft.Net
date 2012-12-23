using System;
using System.Text;

namespace Craft.Net.Metadata
{
    public class MetadataString : MetadataEntry
    {
        public override byte Identifier { get { return 4; } }
        public override string FriendlyName { get { return "string"; } }

        public string Value;

        public MetadataString(byte index) : base(index)
        {
        }

        public MetadataString(byte index, string value) : base(index)
        {
            if (value.Length > 16)
                throw new ArgumentOutOfRangeException("value", "Maximum string length is 16 characters");
            while (value.Length < 16)
                value = value + "\0";
            Value = value;
        }

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadString();
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteUInt8Array(Encoding.ASCII.GetBytes(Value));
        }
    }
}
