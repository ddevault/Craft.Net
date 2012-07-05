using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Craft.Net.Server.Packets;
using System.IO.Compression;
using LibNbt;

namespace Craft.Net.Server
{
    /// <summary>
    /// Represents an inventory slot.
    /// </summary>
    /// <remarks></remarks>
    public class Slot
    {
        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        /// <value>The item ID.</value>
        /// <remarks>This ID may be a block or an item.</remarks>
        public short Id;
        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        /// <value>The item count.</value>
        /// <remarks></remarks>
        public byte Count;
        /// <summary>
        /// Gets or sets the item metadata.
        /// </summary>
        /// <value>The item metadata.</value>
        /// <remarks></remarks>
        public short Metadata;
        /// <summary>
        /// Gets or sets the NBT data.
        /// </summary>
        /// <value>The NBT data.</value>
        /// <remarks>This is used for enchanting equipment</remarks>
        public NbtFile Nbt;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Slot()
        {
            this.Id = 0;
            this.Count = 1;
            this.Metadata = 0;
            this.Nbt = new NbtFile();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="Count">The count.</param>
        /// <remarks></remarks>
        public Slot(short ID, byte Count)
        {
            this.Id = ID;
            this.Count = Count;
            this.Metadata = 0;
            this.Nbt = new NbtFile();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slot"/> class.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="Count">The count.</param>
        /// <param name="Metadata">The metadata.</param>
        /// <remarks></remarks>
        public Slot(short ID, byte Count, short Metadata)
        {
            this.Id = ID;
            this.Count = Count;
            this.Metadata = Metadata;
            this.Nbt = new NbtFile();
        }

        /// <summary>
        /// Reads a slot from the given stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Slot ReadSlot(Stream stream)
        {
            Slot s = new Slot();
            s.Id = ReadShort(stream);
            if (s.Id == -1)
                return s;
            s.Count = (byte)stream.ReadByte();
            s.Metadata = ReadShort(stream);

            short length = ReadShort(stream);
            if (length != -1)
            {
                byte[] compressed = new byte[length];
                stream.Read(compressed, 0, length);
                MemoryStream output = new MemoryStream();
                GZipStream gzs = new GZipStream(new MemoryStream(compressed), CompressionMode.Decompress, false);
                gzs.CopyTo(output);
                gzs.Close();
                s.Nbt = new NbtFile();
                s.Nbt.LoadFile(output, false);
            }

            return s;
        }

        public static bool TryReadSlot(byte[] buffer, ref int offset, out Slot slot)
        {
            slot = new Slot();
            if (!Packet.TryReadShort(buffer, ref offset, out slot.Id))
                return false;
            if (slot.Id == -1)
                return true;
            if (!Packet.TryReadByte(buffer, ref offset, out slot.Count))
                return false;
            if (!Packet.TryReadShort(buffer, ref offset, out slot.Metadata))
                return false;
            short length = 0;
            if (!Packet.TryReadShort(buffer, ref offset, out length))
                return false;
            if (length == -1)
                return true;
            byte[] compressed = new byte[length];
            if (!Packet.TryReadArray(buffer, length, ref offset, out compressed))
                return false;
            if (length != -1)
            {
                MemoryStream output = new MemoryStream();
                GZipStream gzs = new GZipStream(new MemoryStream(compressed), CompressionMode.Decompress, false);
                gzs.CopyTo(output);
                gzs.Close();
                slot.Nbt = new NbtFile();
                slot.Nbt.LoadFile(output, false);
            }
            return true;
        }

        /// <summary>
        /// Gets the slot data.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetData()
        {
            byte[] data = new byte[0]
                .Concat(Packet.CreateShort(Id)).ToArray();
            if (Id == -1)
                return data;
            data = data.Concat(new byte[] { Count })
                .Concat(Packet.CreateShort(Metadata)).ToArray();

            // TODO: Confirm this works (needs to return -1?)
            MemoryStream ms = new MemoryStream();
            GZipStream gzs = new GZipStream(ms, CompressionMode.Compress, false);
            Nbt.SaveFile(gzs);
            gzs.Close();
            byte[] b = ms.GetBuffer();
            data = data.Concat(Packet.CreateShort((short)b.Length)).Concat(b).ToArray();
            return data;
        }

        /// <summary>
        /// Gets the slot data.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetFullData()
        {
            byte[] data = new byte[0]
                .Concat(Packet.CreateShort(Id)).ToArray();
            data = data.Concat(new byte[] { Count })
                .Concat(Packet.CreateShort(Metadata)).ToArray();

            MemoryStream ms = new MemoryStream();
            GZipStream gzs = new GZipStream(ms, CompressionMode.Compress, false);
            Nbt.SaveFile(gzs);
            gzs.Close();
            byte[] b = ms.GetBuffer();
            data = data.Concat(Packet.CreateShort((short)b.Length)).Concat(b).ToArray();

            return data;
        }

        static short ReadShort(Stream stream)
        {
            byte[] buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            buffer.Reverse();
            return BitConverter.ToInt16(buffer, 0);
        }
    }
}
