using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// An abstract model for strongly-typed
    /// representation of blocks
    /// </summary>
    public abstract class Block
    {
        #region Block list

        private static Block[] Blocks = new Block[]
        {
            new AirBlock(),
            new StoneBlock(),
            new GrassBlock(),
            new DirtBlock(),
            new CobblestoneBlock(),
            new WoodPlankBlock(),
            new SaplingBlock(), //saplings (4 types)
            new BedrockBlock(),
            new WaterFlowingBlock(),
            new WaterStillBlock(),
            new LavaFlowingBlock(),
            new LavaStillBlock(),
            new SandBlock(),
            new GravelBlock(),
            new GoldOreBlock(),
            new IronOreBlock(),
            new CoalOreBlock(),
            new LogBlock(), //logs (4 types)
            new LeafBlockBlock(), //leaves (4 or 5 types)
            new SpongeBlock(),
            new GlassBlock(),
            new LapisLazuliOreBlock(),
            new LapisLazuliBlock(),
            new DispenserBlock(),
            new SandstoneBlock(),
            new NoteBlock(),
            new BedBlock(),
            new PoweredRailBlock(),
            new DetectorRailBlock(),
            new StickyPistonBlock(),
            new CobwebBlock(),
            new TallGrassBlock(),
            new DeadBushBlock(),
            new PistonBlock(),
            new PistonPlungerBlock(),
            new WoolBlock(), //wools
            new MovedByPistonBlock(), //moved by piston
            new DandelionBlock(),
            new RoseBlock(),
            new BrownMushroomBlock(),
            new RedMushroomBlock(),
            new GoldBlock(),
            new IronBlock(),
            new SlabBlock(),
            new HalfSlabBlock(), // 4 different textures
            new BricksBlock(), // 4 different textures
            new TNTBlock(),
            new BookshelfBlock(),
            new MossStoneBlock(),
            new ObsidianBlock(),
            new TorchBlock(),
            new FireBlock(), //fire
            new MonsterSpawnerBlock(),
            new WoodenStairsBlock(),
            new ChestBlock(),
            new RedstoneWireBlock(),
            new DiamondOreBlock(),
            new DiamondBlock(),
            new CraftingTableBlock(),
            new SeedBlock(),
            new FarmlandBlock(), //farmland
            new FurnaceBlock(),
            new ActiveFurnaceBlock(),
            new SignPostBlock(),
            new WoodenDoorBlock(),
            new LadderBlock(),
            new RailBlock(),
            new StoneStairsBlock(),
            new WallSignBlock(),
            new LeverBlock(),
            new StonePressurePlateBlock(),
            new IronDoorBlock(),
            new WoodPressurePlateBlock(),
            new RedstoneOreBlock(),
            new RedstoneOreActiveBlock(),
            new RedstoneTorchBlock(),
            new RedstoneTorchActiveBlock(),
            new ButtonBlock(),
            new SnowCapBlock(),
            new IceBlock(),
            new SnowBlock(),
            new CactusBlock(),
            new ClayBlock(),
            new SugarcaneBlock(),
            new JukeboxBlock(),
            new FenceBlock(),
            new PumpkinBlock(),
            new NetherrackBlock(),
            new SoulSandBlock(),
            new GlowstoneBlock(),
            new NetherPortalBlock(),
            new JackOLanternBlock(), //jack o lantern
            new CakeBlock(),
            new RepeaterBlock(),
            new RepeaterActiveBlock(),
            new LockedChestBlock(),
            new TrapdoorBlock(),
            new SilverfishBlock(),
            new StoneBrickBlock(),
            new HugeMushroomBlock(),
            new HugeMushroomRedBlock(),
            new IronBarsBlock(),
            new GlassPaneBlock(),
            new MelonBlock(),
            new PumpkinStemBlock(),
            new MelonStemBlock(),
            new VinesBlock(),
            new FenceGateBlock(),
            new BrickStairsBlock(),
            new StoneBrickStairsBlock(),
            new MyceliumBlock(),
            new LilyPadBlock(),
            new NetherBrickBlock(),
            new NetherBrickFenceBlock(),
            new NetherBrickStairsBlock(),
            new NetherWartBlock(),
            new EnchantmentTableBlock(),
            new BrewingStandBlock(),
            new CauldronBlock(),
            new EndPortalBlock(),
            new EndPortalFrameBlock(),
            new EndStoneBlock(),
            new EnderDragonEggBlock(),
            new InactiveRedstonelampBlock(),
            new ActiveRedstonelampBlock(),
        };
        
        #endregion

        /// <summary>
        /// The Block ID for this block
        /// </summary>
        public abstract byte BlockID { get; }

        /// <summary>
        /// This block's friendly name for client inventories.
        /// By default, returns the name of the Type.
        /// </summary>
        public virtual string BlockName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// This block's metadata
        /// </summary>
        public virtual byte Metadata { get; set; }

        /// <summary>
        /// The skylight value of this block
        /// </summary>
        public virtual byte SkyLight { get; set; }

        /// <summary>
        /// The block light value of this block
        /// </summary>
        public virtual byte BlockLight { get; set; }

        /// <summary>
        /// Return the type of transparency of a block. The default is opaque.
        /// See types in BlockOpacity enum.
        /// </summary>
        public virtual BlockOpacity Transparent
        {
            get
            {
                return BlockOpacity.Opaque;
            }
        }

        /// <summary>
        /// Converts a Block to a byte
        /// </summary>
        public static implicit operator byte(Block b)
        {
            return b.BlockID;
        }

        /// <summary>
        /// Converts a byte to a Block
        /// </summary>
        public static implicit operator Block(byte b)
        {
            Block bl = Blocks[b];
            return (Block)Activator.CreateInstance(bl.GetType());
        }

        /// <summary>
        /// Allows libraries to implement their own version
        /// of any block's logic.
        /// </summary>
        public static void OverrideBlock(Block Block)
        {
            Blocks[Block.BlockID] = Block;
        }

        #region Overrides

        /// <summary>
        /// Called when this block is placed
        /// </summary>
        /// <param name="world">The world it was placed in</param>
        /// <param name="position">The position it was placed at</param>
        /// <param name="blockClicked">The location of the block left clicked upon placing</param>
        /// <param name="facing">The facing of the placement</param>
        /// <param name="placedBy">The entity who placed the block</param>
        /// <returns>False to override placement</returns>
        public virtual bool BlockPlaced(World world, Vector3 position, Vector3 blockClicked, byte facing, Entity placedBy)
        {
            return true;
        }

        /// <summary>
        /// Called when this block is destroyed
        /// </summary>
        /// <param name="world">The world the block used to exist in</param>
        /// <param name="position">The position the block used to occupy</param>
        /// <param name="destroyedBy">The entity that destroyed the block</param>
        public virtual void BlockDestroyed(World world, Vector3 position, Entity destroyedBy)
        {
        }

        /// <summary>
        /// Called when this block is right clicked by a player.
        /// </summary>
        /// <param name="world">The world in which the event occured</param>
        /// <param name="position">The location of the block being clicked</param>
        /// <param name="clickedBy">The player who clicked the block</param>
        /// <returns>False to override the default action (block placement)</returns>
        public virtual bool BlockRightClicked(World world, Vector3 position, PlayerEntity clickedBy)
        {
            return true;
        }

        /// <summary>
        /// Called when an action in the world causes this block to update.
        /// </summary>
        /// <param name="world">The world the update occured in</param>
        /// <param name="position">The location of the updated block</param>
        public virtual void BlockUpdate(World world, Vector3 position)
        {
        }

        #endregion
    }
}
