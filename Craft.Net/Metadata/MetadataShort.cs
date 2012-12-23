using System;

namespace Craft.Net.Metadata
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

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadInt16();
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteInt16(Value);
        }
    }
}
