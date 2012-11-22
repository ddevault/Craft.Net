using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
   public class WoodenDoorItem : Item
   {
      public override ushort Id
      {
         get
         {
            return 324;
         }
      }

      public override byte MaximumStack
      {
         get { return 1; }
      }

      public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
      {
         if (clickedSide != Vector3.Up)
            return;
         Vector3 away = DataUtility.FowardVector(usedBy, true);
         var near = world.GetBlock(clickedBlock + clickedSide);
         var far = world.GetBlock(clickedBlock + clickedSide + Vector3.Up);
         if (near is AirBlock && far is AirBlock)
         {
            // Place door
            world.SetBlock(clickedBlock + clickedSide,
               new WoodenDoorBlock(DoorBlock.Vector3ToDoorDirection(away), false));
            world.SetBlock(clickedBlock + clickedSide + Vector3.Up,
               new WoodenDoorBlock(DoorBlock.Vector3ToDoorDirection(away), true));
            }
         }
      }
}