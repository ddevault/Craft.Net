using System;

namespace Craft.Net
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

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadInt32();
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteInt32(Value);
        }
    }
}
