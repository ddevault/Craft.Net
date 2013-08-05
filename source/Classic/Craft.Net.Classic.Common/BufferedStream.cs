using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Craft.Net.Classic.Common
{
    // This is not strictly needed, but I prefer to write my packets all at once, instead
    // of field by field.
    // TODO: See if we can't just use System.IO.BufferedStream
    /// <summary>
    /// Queues all writes until Stream.Flush() is called.
    /// </summary>
    public class BufferedStream : Stream
    {
        public BufferedStream(Stream baseStream)
        {
            BaseStream = baseStream;
            PendingWrites = new List<byte>();
        }

        public Stream BaseStream { get; set; }
        public List<byte> PendingWrites { get; set; }

        public override bool CanRead { get { return BaseStream.CanRead; } }

        public override bool CanSeek { get { return BaseStream.CanSeek; } }

        public override bool CanWrite { get { return BaseStream.CanWrite; } }

        public override void Flush()
        {
            BaseStream.Write(PendingWrites.ToArray(), 0, PendingWrites.Count);
            PendingWrites.Clear();
        }

        public override long Length
        {
            get { return BaseStream.Length; }
        }

        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset != 0 || count != buffer.Length) // We check this to avoid allocating more memory for the copy
            {
                var value = new byte[count];
                Array.Copy(buffer, offset, value, 0, count);
                PendingWrites.AddRange(value);
            }
            else
                PendingWrites.AddRange(buffer);
        }
    }
}
