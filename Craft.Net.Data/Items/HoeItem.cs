using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
    public abstract class HoeItem : ToolItem
    {
        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var block = world.GetBlock(clickedBlock);
            if (block is DirtBlock || block is GrassBlock)
            {
                var farmland = new FarmlandBlock();
                if (farmland.OnBlockPlaced(world, clickedBlock, clickedBlock, clickedSide, cursorPosition, usedBy))
                    world.SetBlock(clickedBlock, farmland);
            }
            base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
        }
    }
}
