using System;
using Craft.Net.Common;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    public class WorkBench : Block
    {
        public static readonly short Id = 58;
        public override short BlockId { get { return Id; } }

        public WorkBench() : base("minecraft:crafting_table", hardness: 2.5)
        {
            base.SetPlacementSoundEffect(SoundEffect.DigGrass);
            base.SetBlockRightClickedHandler(OnRightClcik);
        }

        public bool OnRightClcik(World world,PlayerEntity player, Coordinates3D coordinates, BlockInfo info, BlockFace face, Coordinates3D cursor, ItemInfo? item)
        {
            player.OnInteractBlock();
            return false;
        }
    }
}