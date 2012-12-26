using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Items
{
    public class ItemFrameItem : Item
    {
        public override short Id
        {
            get { return 389; }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide,
            Vector3 cursorPosition, Entity usedBy)
        {
            var direction = ItemFrameEntity.Vector3ToDirection(clickedSide);
            if (direction.HasValue)
            {
                var entity = new ItemFrameEntity(new ItemStack(new DiamondPickaxeItem().Id),
                    direction.Value, clickedBlock);
                world.OnSpawnEntity(entity);
            }
            base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
        }
    }
}
