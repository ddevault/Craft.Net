using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
    
    public class PumpkinSeedsItem : Item
    {
        public override short Id
        {
            get
            {
                return 361;
            }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var block = new PumpkinStemBlock();
            if (block.OnBlockPlaced(world, clickedBlock + clickedSide, clickedBlock, clickedSide, cursorPosition, usedBy))
                world.SetBlock(clickedBlock + clickedSide, block);
            base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
        }
    }
}
