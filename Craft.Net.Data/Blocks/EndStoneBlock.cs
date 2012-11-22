using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
   public class EndStoneBlock : Block
   {
      public override ushort Id
      {
         get { return 121; }
      }

      public override double Hardness
      {
         get { return 3; }
      }
   }
}