using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class GlassBlock : Block
    {
        public static readonly short Id = 20;
        public override short BlockId { get { return Id; } }

        public GlassBlock() : base("minecraft:glass", hardness: 0.3)
        {
            base.SetToolQuality(0);
            base.SetPlacementSoundEffect(SoundEffect.RandomGlass);            
            base.SetDropHandler((world, coordinates, info) => new ItemStack[] { });
        }
    }
}
