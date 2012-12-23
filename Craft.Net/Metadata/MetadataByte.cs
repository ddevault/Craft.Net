namespace Craft.Net.Metadata
{
    public class MetadataByte : MetadataEntry
    {
        public override byte Identifier { get { return 0; } }
        public override string FriendlyName { get { return "byte"; } }

        public byte Value;

        public MetadataByte(byte index) : base(index)
        {
        }

        public MetadataByte(byte index, byte value) : base(index)
        {
            Value = value;
        }

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadUInt8();
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteUInt8(Value);
        }
    }
}
