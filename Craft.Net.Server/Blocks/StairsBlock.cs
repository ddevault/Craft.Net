using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// The base class for Stair blocks
    /// </summary>
    /// <remarks></remarks>
    public abstract class StairsBlock : Block
    {
        public override bool BlockPlaced(World world, Vector3 position, Vector3 blockClicked, byte facing, Entity placedBy)
        {
            // TODO
            return true;
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}
