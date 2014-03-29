using System;
using Craft.Net.Common;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    public class DirtBlock : Block
    {
        public static readonly short Id = 3;
        public override short BlockId { get { return Id; } }

        public DirtBlock() : base("minecraft:dirt", hardness: 0.5)
        {
            base.SetPlacementSoundEffect(SoundEffect.DigGrass);
        }
    }
}