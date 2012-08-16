using System;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data
{
    public class BlockChangedEventArgs : EventArgs
    {
        public Vector3 Position;
        public Block Value;
        public World World;

        public BlockChangedEventArgs(World world, Vector3 position, Block value)
        {
            this.World = world;
            this.Position = position;
            this.Value = value;
        }
    }
}