using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Items
{
   public abstract class FoodItem : Item
   {
      public abstract int FoodPoints { get; }
      public abstract float Saturation { get; }

      public override void OnItemUsed(World world, Entity usedBy)
      {
         var player = usedBy as PlayerEntity;
         if (player != null)
            player.OnStartEating();
         }
      }
}