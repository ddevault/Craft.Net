using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public class PoisonousPotatoItem : FoodItem
    {
        public override ushort Id
        {
            get { return 394; }
        }

        public override int FoodPoints
        {
            get { return 2; }
        }

        public override float Saturation
        {
            get { return 0.8f; } // Unknown
        }
    }
}
