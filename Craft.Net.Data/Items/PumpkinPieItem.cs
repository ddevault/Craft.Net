using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public class PumpkinPieItem : FoodItem
    {
        public override ushort Id
        {
            get { return 400; }
        }

        public override int FoodPoints
        {
            get { return 8; }
        }

        public override float Saturation
        {
            get { return 4.8f; }
        }
    }
}