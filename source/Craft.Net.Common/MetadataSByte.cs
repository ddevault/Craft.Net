namespace Craft.Net.Common
{
    public class MetadataSByte : MetadataEntry
    {
        public override byte Identifier { get { return 0; } }
        public override string FriendlyName { get { return "sbyte"; } }

        public sbyte Value;

        public static implicit operator MetadataSByte(sbyte value)
        {
            return new MetadataSByte(value);
        }

        public MetadataSByte()
        {
        }

        public MetadataSByte(sbyte value)
        {
            Value = value;
        }

        public override void FromStream(MinecraftStream stream)
        {
            Value = stream.ReadInt8();
        }

        public override void WriteTo(MinecraftStream stream, byte index)
        {
            stream.WriteUInt8(GetKey(index));
            stream.WriteInt8(Value);
        }
    }
}
