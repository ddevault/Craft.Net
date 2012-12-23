using System;

namespace Craft.Net.Metadata
{
    public class MetadataFloat : MetadataEntry
    {
        public override byte Identifier { get { return 3; } }
        public override string FriendlyName { get { return "float"; } }

        public float Value;

        public MetadataFloat(byte index) : base(index)
        {
        }

        public MetadataFloat(byte index, float value) : base(index)
        {
            Value = value;
        }

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadSingle();
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteSingle(Value);
        }
    }
}
