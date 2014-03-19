using System;
using Craft.Net.Common;
using Craft.Net.Anvil;

namespace Craft.Net.Logic
{
    public abstract class Block
    {
        public string Name { get; private set; }

        internal void _Initialize()
        {
            Name = Initialize();
        }

        protected abstract string Initialize();

        protected void SetDropHandler(short blockId, Func<World, Coordinates3D, BlockInfo, ItemStack[]> dropHandler, bool overrideSilkTouch = false)
        {
            LogicManager.SetBlockMinedHandler(blockId, (world, coordinates, info) =>
                {
                    world.SetBlockId(coordinates, 0);
                    world.SetMetadata(coordinates, 0);
                    foreach (var drop in dropHandler(world, coordinates, info))
                        world.OnSpawnEntityRequested(new ItemEntity((Vector3)coordinates + new Vector3(0.5), drop));
                });
        }
    }
}