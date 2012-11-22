using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public class ObsidianBlock : Block
   {
      public override ushort Id
      {
         get { return 49; }
      }

      public override double Hardness
      {
         get { return 50; }
      }

      public override bool CanHarvest(ToolItem tool)
      {
         return tool is DiamondPickaxeItem;
      }
   }
}