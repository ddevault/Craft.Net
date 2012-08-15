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

        public BlockChangedEventArgs(World World, Vector3 Position, Block Value)
        {
            this.World = World;
            this.Position = Position;
            this.Value = Value;
        }
    }
}