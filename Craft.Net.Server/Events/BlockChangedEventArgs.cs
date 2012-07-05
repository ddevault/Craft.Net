using System;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Events
{
    public class BlockChangedEventArgs : EventArgs
    {
        public World World;
        public Vector3 Position;
        public Block Value;

        public BlockChangedEventArgs(World World, Vector3 Position, Block Value)
        {
            this.World = World;
            this.Position = Position;
            this.Value = Value;
        }
    }
}

