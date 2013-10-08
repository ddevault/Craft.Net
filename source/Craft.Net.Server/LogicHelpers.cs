using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Entities;
using Craft.Net.Logic.Blocks;
using Craft.Net.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
    public static class LogicHelpers
    {
        private static bool Registered = false;

        public static void Register()
        {
            if (Registered)
                return;
            Registered = true;
            Block.GlobalDefaultBlockMinedHandler = DefaultBlockMinedHandler;
        }

        public static void DefaultBlockMinedHandler(BlockDescriptor block, World world, Coordinates3D destroyedBlock, ItemDescriptor? tool)
        {
            var drops = Block.GetBlockDrop(block, world, destroyedBlock);
            world.SetBlockId(destroyedBlock, 0);
            world.SetMetadata(destroyedBlock, 0);
            foreach (var drop in drops)
                world.OnSpawnEntityRequested(new ItemEntity((Vector3)destroyedBlock + new Vector3(0.5), drop));
        }
    }
}
