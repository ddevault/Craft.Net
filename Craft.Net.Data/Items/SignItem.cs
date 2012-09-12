using Craft.Net.Data.Entities;
using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Items
{
    public class SignItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 323;
            }
        }

        public override void OnItemUsed(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            if (clickedSide == Vector3.Up)
            {
                // Floor sign
            }
            else if (clickedSide != Vector3.Down)
            {
                // Wall sign
                world.SetBlock(clickedBlock + clickedSide, new WallSignBlock(DataUtility.DirectionByRotationFlat(usedBy, true)));
            }
        }
    }
}
