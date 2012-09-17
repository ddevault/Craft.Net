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

        private static Dictionary<ushort, Block> OverridenBlocks { get; set; }

        /// <summary>
        /// Gets or sets this block's metadata value as a
        /// 16-bit unsigned integer.
        /// </summary>
        public override ushort Data
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

        public virtual ToolType HarvestingTools
        {
            get { return ToolType.All; }
        }

        public virtual ToolType EffecientTools
        {
            get { return ToolType.None; }
        }

        /// <summary>
        /// The transparency of this block.
        /// </summary>
        public virtual Transparency Transparency
        {
            get { return Transparency.Opaque; }
        }

        public virtual bool IsSolid
        {
            get { return true; }
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

        public virtual void OnBlockMined(World world, Vector3 destroyedBlock)
        {
            world.OnSpawnEntity(new ItemEntity(destroyedBlock + new Vector3(0.5, 0.5, 0.5), 
                new Slot(Id, 1, Metadata)));
        }

        /// <summary>
        /// Called when a block update occurs. Default handler will handle
        /// common activities such as destroying blocks that lose support,
        /// make sure you call it if you don't want to lose this functionality.
        /// </summary>
        public virtual void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            // TODO: Stuff described in XML comments
        }

        public virtual int GetHarvestTime(ToolItem tool, World world, PlayerEntity entity, out int damage)
        {
            int time = GetHarvestTime(tool, out damage);
            if ((EffecientTools & tool.ToolType) != EffecientTools)
            {
                if (entity.IsUnderwater(world) && !entity.IsOnGround(world))
                    time *= 25;
                else if (entity.IsUnderwater(world) || !entity.IsOnGround(world))
                    time *= 5;
            }
            return time;
        }

        // Used to simplify calculating harvest time and such
        private class HandTool : ToolItem
        {
            public override ToolType ToolType
            {
                get { return ToolType.None; }
            }
            public override ushort Id
            {
                get { return 0; }
            }
        }

        // TODO: This is done horribly
        private const ToolType InstantBreakNoDamage =
            ToolType.Axe | ToolType.Pick | ToolType.Shovel | ToolType.Sword;

        private const ToolType AllMaterials =
            ToolType.Wood | ToolType.Stone | ToolType.Iron | ToolType.Gold | ToolType.Diamond;

        /// <summary>
        /// Gets the amount of time (in milliseconds) it takes to harvest this
        /// block with the given tool.
        /// </summary>
        public virtual int GetHarvestTime(Item item, out int damage)
        {
            // time is in seconds until returned
            double time = Hardness * 1.5;
            var tool = item as ToolItem;
            if (tool == null)
                tool = new HandTool();
            if (!CanHarvest(tool))
                time *= 3.33;
            if (EffecientTools != ToolType.None &&
                (EffecientTools & ~tool.ToolType) == ToolType.None)
            {
                if ((tool.ToolType & ToolType.Wood) == ToolType.Wood)
                    time /= 2;
                else if ((tool.ToolType & ToolType.Stone) == ToolType.Stone)
                    time /= 4;
                else if ((tool.ToolType & ToolType.Iron) == ToolType.Iron)
                    time /= 6;
                else if ((tool.ToolType & ToolType.Diamond) == ToolType.Diamond)
                    time /= 8;
                else if ((tool.ToolType & ToolType.Gold) == ToolType.Gold)
                    time /= 12;
                // TODO: Enchantments
            }

            // Calculate damage
            damage = 1;
            if ((tool.ToolType & InstantBreakNoDamage) == tool.ToolType && Hardness == 0)
                damage = 0;
            else if ((tool.ToolType & ToolType.Sword) == ToolType.Sword)
                damage = 2;
            if ((tool.ToolType & ToolType.Hoe) == ToolType.Hoe)
                damage = 0;
            if (tool is ShearsItem && !ShearsItem.DamagingBlocks.Contains(GetType()))
                damage = 0;

            return (int)(time * 1000);
        }

        public virtual bool CanHarvest(ToolItem tool)
        {
            if (Hardness == -1)
                return false;
            return (HarvestingTools & tool.ToolType) == tool.ToolType;
        }

        #endregion
    }
}
