using System;
using System.IO;
using LibNbt;

namespace Craft.Net.Metadata
{
    public class MetadataSlot : MetadataEntry
    {
        public override byte Identifier { get { return 5; } }
        public override string FriendlyName { get { return "slot"; } }

        public Slot Value;

        public MetadataSlot(byte index) : base(index)
        {
        }

        public MetadataSlot(byte index, Slot value) : base(index)
        {
            Value = value;
        }

        public override void FromStream(MinecraftStream stream)
        {
            Value = Slot.FromStream(stream);
        }

        public override void WriteTo(MinecraftStream stream)
        {
            stream.WriteUInt8(GetKey());
            stream.WriteInt16(Value.Id);
            if (Value.Id != -1)
            {
                stream.WriteInt8(Value.Count);
                stream.WriteInt16(Value.Metadata);
                if (Value.Nbt != null)
                {
                    var data = Value.Nbt.SaveToBuffer(NbtCompression.GZip);
                    stream.WriteInt16((short)data.Length);
                    stream.WriteUInt8Array(data);
                }
                else
                    stream.WriteInt16(-1);
            }
        }
    }
}
