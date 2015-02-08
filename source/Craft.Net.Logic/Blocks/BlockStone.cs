using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Logic;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class BlockStone : Block
    {
        public static readonly short ID = 1;
        public override short BlockId { get { return ID; } }

        public BlockStone() : base("minecraft:stone", hardness : 1.0)
        {
            base.SetToolQuality(1);
            base.SetPlacementSoundEffect(SoundEffect.DigStone);
            base.SetDropHandler((world, coordinates, info) => new[] { new ItemStack(BlockCobble.ID) });
        }
    }
}
