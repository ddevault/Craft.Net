using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
namespace Craft.Net.Data.Items
{
    
    public class WaterBucketItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 326;
            }
        }

        public override void OnItemUsed(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            PlayerEntity entity = (PlayerEntity)usedBy;
            if (entity.GameMode == GameMode.Creative)
            {
                // TODO: survival
                world.SetBlock(clickedBlock + clickedSide, new WaterFlowingBlock());
            }
        }
    }
}
