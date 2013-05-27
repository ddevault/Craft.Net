using Craft.Net.Data.Entities;
using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Items
{
    public class SignItem : Item
    {
        public override short Id
        {
            get
            {
                return 323;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            if (clickedSide == Vector3.Up)
            {
                // Floor sign
                byte metadata = (byte)((usedBy.Yaw % 360 / 360 * 16) + 8 % 16);
                world.SetBlock(clickedBlock + clickedSide, new SignPostBlock(metadata));
            }
            else if (clickedSide != Vector3.Down)
            {
                // Wall sign
                world.SetBlock(clickedBlock + clickedSide, new WallSignBlock(MathHelper.DirectionByRotationFlat(usedBy, true)));
            }
        }
    }
}
