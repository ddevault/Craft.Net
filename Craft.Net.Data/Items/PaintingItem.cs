using Craft.Net.Data.Entities;
namespace Craft.Net.Data.Items
{
    public class PaintingItem : Item
    {
        public override short Id
        {
            get
            {
                return 321;
            }
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            var entity = PaintingEntity.CreateEntity(world, clickedBlock, clickedSide);
            if (entity != null)
                world.OnSpawnEntity(entity);
            // TODO: Remove items like this from the inventory after use
            base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
        }
    }
}
