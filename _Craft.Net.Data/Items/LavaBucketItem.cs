using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
namespace Craft.Net.Data.Items
{
    public class LavaBucketItem : Item
    {
        public override short Id
        {
            get
            {
                return 327;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var entity = (PlayerEntity)usedBy;
            if (entity.GameMode == GameMode.Creative)
            {
                // TODO: survival
                world.SetBlock(clickedBlock + clickedSide, new LavaFlowingBlock());
            }
        }
    }
}
