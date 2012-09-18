using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class RedstoneOreBlock : Block
    {
        public override ushort Id
        {
            get { return 73; }
        }

        public override double Hardness
        {
            get { return 3; }
        }

        public override void OnBlockWalkedOn(World world, Vector3 position, Entities.Entity entity)
        {
            world.SetBlock(position, new RedstoneOreActiveBlock());
        }
    }
}
