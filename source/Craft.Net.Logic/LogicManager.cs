using System;
using System.Collections.Generic;
using Craft.Net.Physics;
using Craft.Net.Anvil;
using Craft.Net.Common;
using System.Reflection;

namespace Craft.Net.Logic
{
    public static class LogicManager
    {
        static LogicManager()
        {
            PhysicsProvider = new BlockPhysicsProvider();
            BoundingBoxes = new Dictionary<short, BoundingBox?>();
            BlockMinedHandlers = new Dictionary<short, BlockMinedHandler>();
            ReflectBlocks(typeof(LogicManager).Assembly);
        }

        public static void ReflectBlocks(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(Block).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    var block = (Block)Activator.CreateInstance(type);
                    block._Initialize();
                }
            }
        }

        public static void LoadBlock<T>() where T : Block
        {
            var block = default(T) as Block;
            block._Initialize();
        }

        public static void SetBoundingBox(short blockId, BoundingBox? boundingBox)
        {
            BoundingBoxes[blockId] = boundingBox;
        }

        public static void SetBlockMinedHandler(short blockId, BlockMinedHandler handler)
        {
            BlockMinedHandlers[blockId] = handler;
        }

        public static IBlockPhysicsProvider PhysicsProvider { get; private set; }
        private static Dictionary<short, BoundingBox?> BoundingBoxes { get; set; }
        private class BlockPhysicsProvider : IBlockPhysicsProvider
        {
            public BoundingBox? GetBoundingBox(World world, Coordinates3D coordinates)
            {
                // TODO: Consider passing block info to a handler to get a fancier bounding box
                var blockId = world.GetBlockId(coordinates);
                if (BoundingBoxes.ContainsKey(blockId))
                    return BoundingBoxes[blockId];
                return new BoundingBox(Vector3.Zero, Vector3.One);
            }
        }

        public delegate void BlockMinedHandler(World world, Coordinates3D coordinates, BlockInfo info);
        private static Dictionary<short, BlockMinedHandler> BlockMinedHandlers { get; set; }
        internal static void OnBlockMined(World world, Coordinates3D coordinates)
        {
            var info = world.GetBlockInfo(coordinates);
            if (BlockMinedHandlers.ContainsKey(info.BlockId))
                BlockMinedHandlers[info.BlockId](world, coordinates, info);
            else
                DefaultBlockMinedHandler(world, coordinates, info);
        }

        private static void DefaultBlockMinedHandler(World world, Coordinates3D coordinates, BlockInfo info)
        {
            world.SetBlockId(coordinates, 0);
            world.SetMetadata(coordinates, 0);
            world.OnSpawnEntityRequested(new ItemEntity((Vector3)coordinates + new Vector3(0.5),
                new ItemStack(info.BlockId, 1, info.Metadata)));
        }
    }
}