using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Items
{

    public class FlintAndSteelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 259;
            }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            // TODO: Check flammability
            if (world.GetBlock(clickedBlock + clickedSide) == 0)
                world.SetBlock(clickedBlock + clickedSide, new FireBlock());
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Other; }
        }
    }
}
