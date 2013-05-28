using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Events
{
    public class HealthAndFoodEventArgs : EventArgs
    {
        public short OldHealth { get; set; }
        public short OldFood { get; set; }
        public float OldFoodSaturation { get; set; }
        public short Health { get; set; }
        public short Food { get; set; }
        public float FoodSaturation { get; set; }

        internal bool IsChanged()
        {
            return OldHealth != Health ||
                OldFood != Food ||
                OldFoodSaturation != FoodSaturation;
        }

        public HealthAndFoodEventArgs(short oldHealth, short oldFood, float oldFoodSaturation)
        {
            OldHealth = oldHealth;
            OldFood = oldFood;
            OldFoodSaturation = oldFoodSaturation;
        }
    }
}
