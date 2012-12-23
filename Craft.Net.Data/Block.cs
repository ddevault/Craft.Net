using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data
{
    public abstract class Block : Item
    {
        #region Properties

        private static Dictionary<short, Block> OverridenBlocks { get; set; }

        /// <summary>
        /// Gets or sets this block's metadata value as a
        /// 16-bit unsigned integer.
        /// </summary>
        public override short Data
        {
            get { return Metadata; }
            set { Metadata = (byte)(value & 0xF); }
        }

        /// <summary>
        /// This block's metadata value, stored as a 4-bit
        /// nibble.
        /// </summary>
        public byte Metadata { get; set; }

        /// <summary>
        /// The light on this block cast by other blocks.
        /// </summary>
        public byte BlockLight { get; set; }

        /// <summary>
        /// The light on this block cast by the sky.
        /// </summary>
        public byte SkyLight { get; set; }

        #endregion

        #region Overriden Members

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide,
                                               Vector3 cursorPosition, Entity usedBy)
        {
            var clicked = world.GetBlock(clickedBlock);
            if (clicked.OnBlockRightClicked(clickedBlock, clickedSide, cursorPosition, world, usedBy))
            {
                if (OnBlockPlaced(world, clickedBlock + clickedSide, clickedBlock, clickedSide, cursorPosition, usedBy))
                {
                    if ((clickedBlock + clickedSide).Y >= 0 && (clickedBlock + clickedSide).Y <= Chunk.Height)
                        world.SetBlock(clickedBlock + clickedSide, this);
                }
            }
        }

        #endregion

        #region Virtual Members

        public virtual double Hardness
        {
            get { return 0; }
        }

        /// <summary>
        /// The transparency of this block.
        /// </summary>
        public virtual Transparency Transparency
        {
            get { return Transparency.Opaque; }
        }

        /// <summary>
        /// True if this block should be destroyed when the block
        /// it rests upon is destroyed.
        /// </summary>
        public virtual bool RequiresSupport
        {
            get { return false; }
        }

        /// <summary>
        /// The direction of the block that provides support for
        /// this block.
        /// </summary>
        public virtual Vector3 SupportDirection
        {
            get { return Vector3.Down; }
        }

        public virtual TileEntity TileEntity
        {
            get { return null;  }
            set { }
        }

        public virtual Size Size
        {
            get { return new Size(1, 1, 1); } // TODO: Size.One
        }

        /// <summary>
        /// The bounding box, used for physics, that this block inhabits. Return null
        /// if entities may exist within the block.
        /// </summary>
        public virtual BoundingBox? BoundingBox
        {
            get { return new BoundingBox(Vector3.Zero, Vector3.Zero + Size); }
        }

        /// <summary>
        /// When the block is placed, this will be called. Return
        /// false to cancel block placement. The default behavoir
        /// is to simply place the block.
        /// </summary>
        public virtual bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            return true;
        }

        /// <summary>
        /// When the block is clicked, this will be called. Return
        /// false to cancel block placement if relevant.
        /// </summary>
        public virtual bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entity usedBy)
        {
            return true;
        }

        /// <summary>
        /// Called when an entity walks on top of the block.
        /// </summary>
        public virtual void OnBlockWalkedOn(World world, Vector3 position, Entity entity)
        {
        }

        /// <summary>
        /// Called when an entity walks inside of the block.
        /// </summary>
        public virtual void OnBlockWalkedIn(World world, Vector3 position, Entity entity)
        {
            // TODO: Default handler to kick the entity out
        }

        public virtual void OnBlockMined(World world, Vector3 destroyedBlock, PlayerEntity player)
        {
            if (Hardness != -1)
            {
                var slot = player.Inventory[player.SelectedSlot];
                world.SetBlock(destroyedBlock, new AirBlock());
                if (CanHarvest(slot.Item as ToolItem) && player.GameMode != GameMode.Creative)
                {
                    Slot[] drops;
                    bool spawnEntity = GetDrop(slot.Item as ToolItem, out drops);
                    if (spawnEntity)
                    {
                        foreach (var drop in drops)
                            world.OnSpawnEntity(new ItemEntity(destroyedBlock + new Vector3(0.5), drop));
                    }
                }
            }
        }

        /// <summary>
        /// Called when a block update occurs. Default handler will handle
        /// common activities such as destroying blocks that lose support -
        /// make sure you call it if you don't want to lose this functionality.
        /// </summary>
        public virtual void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (RequiresSupport)
            {
                // Ensure that it hasn't lost support
                var block = world.GetBlock(updatedBlock + SupportDirection);
                if (block is AirBlock)
                {
                    world.SetBlock(updatedBlock, new AirBlock());
                    Slot[] drops;
                    bool spawnEntity = GetDrop(null, out drops);
                    if (spawnEntity)
                    {
                        foreach (var drop in drops)
                            world.OnSpawnEntity(new ItemEntity(updatedBlock + new Vector3(0.5), drop));
                    }
                }
            }
        }

        public virtual bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot(this.Id, 1, this.Metadata) };
            return CanHarvest(tool);
        }

        public virtual int GetHarvestTime(Item item, World world, PlayerEntity entity, out int damage)
        {
            int time = GetHarvestTime(item, out damage);
            var tool = item as ToolItem;
            if (tool != null)
            {
                if (!tool.IsEfficient(this))
                {
                    if (entity.IsUnderwater(world) && !entity.IsOnGround(world))
                        time *= 25;
                    else if (entity.IsUnderwater(world) || !entity.IsOnGround(world))
                        time *= 5;
                }
            }
            return time;
        }

        /// <summary>
        /// Gets the amount of time (in milliseconds) it takes to harvest this
        /// block with the given tool.
        /// </summary>
        public virtual int GetHarvestTime(Item item, out int damage)
        {
            // time is in seconds until returned
            if (Hardness == -1)
            {
                damage = 0;
                return -1;
            }
            double time = Hardness * 1.5;
            var tool = item as ToolItem;
            damage = 0;
            // Adjust for tool in use
            if (tool != null)
            {
                if (!CanHarvest(tool))
                    time *= 3.33;
                if (tool.IsEfficient(this))
                {
                    switch (tool.ToolMaterial)
                    {
                        case ToolMaterial.Wood:
                            time /= 2;
                            break;
                        case ToolMaterial.Stone:
                            time /= 4;
                            break;
                        case ToolMaterial.Iron:
                            time /= 6;
                            break;
                        case ToolMaterial.Diamond:
                            time /= 8;
                            break;
                        case ToolMaterial.Gold:
                            time /= 12;
                            break;
                    }
                }
                // Do tool damage
                damage = 1;
                if (tool.ToolType == ToolType.Pick || tool.ToolType == ToolType.Axe || tool.ToolType == ToolType.Shovel)
                    damage = Hardness != 0 ? 1 : 0;
                else if (tool.ToolType == ToolType.Sword)
                {
                    damage = Hardness != 0 ? 2 : 0;
                    time /= 1.5;
                    if (this is CobwebBlock)
                        time /= 15;
                }
                else if (tool.ToolType == ToolType.Hoe)
                    damage = 0;
                else if (tool is ShearsItem)
                {
                    if (this is WoolBlock)
                        time /= 5;
                    if (this is LeavesBlock || this is CobwebBlock)
                        time /= 15;
                    if (this is CobwebBlock || this is LeavesBlock || this is TallGrassBlock ||
                        this is TripwireBlock || this is VineBlock)
                        damage = 1;
                    else
                        damage = 0;
                }
                else
                    damage = 0;
            }
            return (int)(time * 1000);
        }

        public virtual bool CanHarvest(ToolItem tool)
        {
            if (Hardness == -1)
                return false;
            return true;
        }

        #endregion

        internal static Block Create(short id, byte metadata)
        {
            var b = (Block)id;
            b.Metadata = metadata;
            return b;
        }
    }
}
