using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Craft.Net.Data.Entities
{
   public interface IBreedableEntity
   {
      Timer LoveTime { get; set; }
      int Age { get; set; }
   }
}