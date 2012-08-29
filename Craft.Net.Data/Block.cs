using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;

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
            get
            {
                return Metadata;
            }
            set
            {
                Metadata = (byte)(value & 0xF);
            }
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

        public override void OnItemUsed(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entity usedBy)
        {
            var clicked = world.GetBlock(clickedBlock);
            if (clicked.OnBlockRightClicked(clickedBlock, clickedSide, cursorPosition, world, usedBy))
            {
                if (this.OnBlockPlaced(clickedBlock, clickedSide, cursorPosition, world, usedBy))
                {
                    world.SetBlock(clickedBlock + clickedSide, this);
                }
            }
        }

        #endregion

        #region Virtual Members

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

        /// <summary>
        /// When the block is placed, this will be called. Return
        /// false to cancel block placement. The default behavoir
        /// is to simply place the block.
        /// </summary>
        public virtual bool OnBlockPlaced(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entity usedBy)
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

        #endregion
    }
}
