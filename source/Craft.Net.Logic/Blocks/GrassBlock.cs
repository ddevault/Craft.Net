using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class GrassBlock : Block
    {
        public static readonly short Id = 2;
        public override short BlockId { get { return Id; } }

        public GrassBlock() : base("minecraft:grass", hardness: 0.6)
        {
            base.SetPlacementSoundEffect(SoundEffect.DigGrass);
            base.SetDropHandler((world, coordinates, info) => new[] { new ItemStack(DirtBlock.Id) });
        }
    }
}