using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
   public class CobblestoneWallBlock : Block
   {
      public override ushort Id
      {
         get { return 139; }
      }

      public override double Hardness
      {
         get { return 2; } // TODO: Estimated, burger provides 0
      }
   }
}