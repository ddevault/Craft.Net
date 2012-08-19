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
        #region Properties and Fields

        private static Dictionary<ushort, Block> OverridenBlocks;

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
        public byte Metadata;

        /// <summary>
        /// The light on this block cast by other blocks.
        /// </summary>
        public byte BlockLight;

        /// <summary>
        /// The light on this block cast by the sky.
        /// </summary>
        public byte SkyLight;

        #endregion

        #region Overriden Members

        public override void OnItemUsed(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entity usedBy)
        {
            
        }

        #endregion

        #region Virtual Members

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
        public virtual bool OnBlockRightClicked()
        {
            return true;
        }

        #endregion
    }
}
