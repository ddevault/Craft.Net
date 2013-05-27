using Craft.Net.Data;
using fNbt.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.World
{
    public abstract class TileEntity
    {
        [NbtIgnore]
        public Vector3 Position
        {
            get
            {
                return new Vector3(X, Y, Z);
            }
        }

        [TagName("id")]
        public abstract string Id { get; }
        [TagName("x")]
        public int X { get; set; }
        [TagName("y")]
        public int Y { get; set; }
        [TagName("z")]
        public int Z { get; set; }
    }
}
