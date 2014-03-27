using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Items
{
    public class FlintAndSteelItem : Item
    {
        public static readonly short Id = 259;
        public override short ItemId { get { return Id; } }
        
        public FlintAndSteelItem() : base("minecraft:flint_and_steel")
        {
            SetItemUsedOnBlockHandler((world, coordinates, face, cursor, item) => world.SetBlockId(coordinates + MathHelper.BlockFaceToCoordinates(face), 1));
        }
    }
}