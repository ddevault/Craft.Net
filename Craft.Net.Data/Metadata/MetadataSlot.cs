using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Metadata
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

        public override bool TryReadEntry(byte[] buffer, ref int offset)
        {
            if (buffer.Length - offset < 6)
                return false;
            offset++;
            ushort id, metadata;
            byte count;
            if (!DataUtility.TryReadUInt16(buffer, ref offset, out id))
                return false;
            if (id == 0xFFFF)
                return true;
            if (!DataUtility.TryReadByte(buffer, ref offset, out count))
                return false;
            if (!DataUtility.TryReadUInt16(buffer, ref offset, out metadata))
                return false;
            Value = new Slot(id, count, metadata);
            return true;
        }

        public override byte[] Encode()
        {
            byte[] data = new byte[8];
            data[0] = GetKey();
            Array.Copy(DataUtility.CreateInt16((short)Value.Id), 0, data, 1, 2);
            if (Value.Id != 0xFFFF)
            {
                data[3] = Value.Count;
                Array.Copy(DataUtility.CreateInt16((short)Value.Metadata), 0, data, 4, 2);
                if (Value.Nbt == null && Value.Nbt.RootTag != null)
                {
                    MemoryStream ms = new MemoryStream();
                    Value.Nbt.SaveFile(ms, false);
                    var nbt = ms.GetBuffer();
                    Array.Resize<byte>(ref data, data.Length + nbt.Length + 2);
                    Array.Copy(DataUtility.CreateInt16((short)nbt.Length), 0, data, 6, 2);
                    Array.Copy(nbt, 0, data, 8, nbt.Length);
                }
                else
                {
                    Array.Copy(DataUtility.CreateInt16((short)-1), 0, data, 6, 2);
                }
            }
            return data;
        }
    }
}
