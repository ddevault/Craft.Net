using System;
using Craft.Net.Server.Blocks;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Events
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