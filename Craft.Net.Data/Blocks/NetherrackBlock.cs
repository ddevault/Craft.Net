using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
   public class NetherrackBlock : Block
   {
      public override ushort Id
      {
         get { return 87; }
      }

      public override double Hardness
      {
         get { return 0.4; }
      }
   }
}