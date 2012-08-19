using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
    public abstract class HoeItem : Item
    {
        public override void OnItemUsed(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            var block = world.GetBlock(clickedBlock);
            if (block is DirtBlock || block is GrassBlock)
                world.SetBlock(clickedBlock, new FarmlandBlock());
            base.OnItemUsed(clickedBlock, clickedSide, cursorPosition, world, usedBy);
        }
    }
}
