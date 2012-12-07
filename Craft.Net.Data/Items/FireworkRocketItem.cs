using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public class FireworkRocketItem : Item
    {
        public override ushort Id
        {
            get { return 401; }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            // TODO: Spawn entity
            base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
        }
    }
}
