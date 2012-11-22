using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
   public class SandBlock : Block
   {
      public override ushort Id
      {
         get { return 12; }
      }

      public override double Hardness
      {
         get { return 0.5; }
      }

      public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
      {
         if (world.GetBlock(updatedBlock + Vector3.Down) == 0)
         {
            world.OnSpawnEntity(new BlockEntity(updatedBlock, this));
            world.SetBlock(updatedBlock, new AirBlock());
         }
      }
   }
}