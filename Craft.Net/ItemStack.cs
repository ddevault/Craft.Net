using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using fNbt;

namespace Craft.Net
{
    public struct ItemStack : ICloneable
    {
        public ItemStack(short id)
        {
            _Id = id;
            _Count = 1;
            Metadata = 0;
            Nbt = null;
            Index = 0;
        }

        public ItemStack(short id, sbyte count) : this(id)
        {
            Count = count;
        }

        public ItemStack(short id, sbyte count, short metadata) : this(id, count)
        {
            Metadata = metadata;
        }

        public ItemStack(short id, sbyte count, short metadata, NbtFile nbt) : this(id, count, metadata)
        {
            Nbt = nbt;
            if (Count == 0)
            {
                Id = -1;
                Metadata = 0;
                Nbt = null;
            }
        }

        public static ItemStack FromStream(MinecraftStream stream)
        {
            var slot = ItemStack.EmptyStack;
            slot.Id = stream.ReadInt16();
            if (slot.Empty)
                return slot;
            slot.Count = stream.ReadInt8();
            slot.Metadata = stream.ReadInt16();
            var length = stream.ReadInt16();
            if (length == -1)
                return slot;
            slot.Nbt = new NbtFile();
            var buffer = stream.ReadUInt8Array(length);
            slot.Nbt.LoadFromBuffer(buffer, 0, length, NbtCompression.GZip, null);
            return slot;
        }

        public void WriteTo(MinecraftStream stream)
        {
            stream.WriteInt16(Id);
            if (Empty)
                return;
            stream.WriteInt8(Count);
            stream.WriteInt16(Metadata);
            if (Nbt == null)
            {
                stream.WriteInt16(-1);
                return;
            }
            var mStream = new MemoryStream();
            Nbt.SaveToStream(mStream, NbtCompression.GZip);
            var buffer = mStream.GetBuffer();
            stream.WriteInt16((short)buffer.Length);
            stream.WriteUInt8Array(buffer);
        }

        public static ItemStack FromNbt(NbtCompound compound)
        {
            var s = ItemStack.EmptyStack;
            s.Id = compound.Get<NbtShort>("id").Value;
            s.Metadata = compound.Get<NbtShort>("Damage").Value;
            s.Count = (sbyte)compound.Get<NbtByte>("Count").Value;
            s.Index = compound.Get<NbtByte>("Slot").Value;
            if (compound.Get<NbtCompound>("tag") != null)
            {
                s.Nbt = new NbtFile();
                s.Nbt.RootTag = compound.Get<NbtCompound>("tag");
            }
            return s;
        }

        public NbtCompound ToNbt()
        {
            var c = new NbtCompound();
            c.Add(new NbtShort("id", Id));
            c.Add(new NbtShort("Damage", Metadata));
            c.Add(new NbtByte("Count", (byte)Count));
            c.Add(new NbtByte("Slot", (byte)Index));
            if (Nbt != null && Nbt.RootTag != null)
            {
                Nbt.RootTag = new NbtCompound("tag");
                c.Add(Nbt.RootTag);
            }
            return c;
        }

        public bool Empty
        {
            get { return Id == -1; }
        }

        public short Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                if (_Id == -1)
                {
                    _Count = 0;
                    Metadata = 0;
                    Nbt = null;
                }
            }
        }

        public sbyte Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                if (_Count == 0)
                {
                    _Id = -1;
                    Metadata = 0;
                    Nbt = null;
                }
            }
        }

        public short _Id;
        public sbyte _Count;
        public short Metadata;
        public NbtFile Nbt;
        public int Index;

        public override string ToString()
        {
            if (Empty)
                return "(Empty)";
            string result = "ID: " + Id;
            if (Count != 1) result += "; Count: " + Count;
            if (Metadata != 0) result += "; Metadata: " + Metadata;
            if (Nbt != null) result += Environment.NewLine + Nbt.ToString();
            return "(" + result + ")";
        }

        public object Clone()
        {
            return new ItemStack(Id, Count, Metadata, Nbt);
        }

        public static ItemStack EmptyStack
        {
            get
            {
                return new ItemStack(-1);
            }
        }
    }
}
