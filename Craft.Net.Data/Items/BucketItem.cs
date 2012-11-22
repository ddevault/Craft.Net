using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
namespace Craft.Net.Data.Items
{
   public class BucketItem : Item
   {
      public override ushort Id
      {
         get
         {
            return 325;
         }
      }

      public override byte MaximumStack
      {
         get { return 16; }
      }

      public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
      {
         var entity = (PlayerEntity)usedBy;
         // TODO: Check for source block
         // TODO: Survival
         if (world.GetBlock(clickedBlock + clickedSide) is WaterFlowingBlock || world.GetBlock(clickedBlock + clickedSide) is WaterStillBlock)
         {
            world.SetBlock(clickedBlock + clickedSide, new AirBlock());
         }
         if (world.GetBlock(clickedBlock + clickedSide) is LavaFlowingBlock || world.GetBlock(clickedBlock + clickedSide) is LavaStillBlock)
         {
            world.SetBlock(clickedBlock + clickedSide, new AirBlock());
         }
      }
   }
}