using Craft.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic.Blocks
{
    public class BlockCobble : Block
    {
        public static readonly short ID = 4;
        public override short BlockId { get {return ID;}}

        public BlockCobble()
            : base("minecraft:cobblestone", hardness: 0.8)
        {
            base.SetToolQuality(1);
            base.SetPlacementSoundEffect(SoundEffect.DigStone);
        }
    }
}
