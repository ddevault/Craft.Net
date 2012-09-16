using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Items
{
    public class CakeItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 354;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }

        public override void OnItemUsed(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (world.GetBlock(clickedBlock + clickedSide) == 0)
                world.SetBlock(clickedBlock + clickedSide, new CakeBlock());
        }
    }
}
