using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Client.Events
{
    public class ChunkRecievedEventArgs : EventArgs
    {
        public ReadOnlyChunk Chunk { get; set; }
        public Vector3 Position { get; set; }

        public ChunkRecievedEventArgs(Vector3 position, ReadOnlyChunk chunk)
        {
            Chunk = chunk;
            Position = position;
        }
    }
}
