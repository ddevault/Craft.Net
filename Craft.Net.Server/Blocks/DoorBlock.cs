using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// The base class for IronDoorBlock and WoodenDoorBlock
    /// </summary>
    /// <remarks></remarks>
    public abstract class DoorBlock : Block
    {
        /// <summary>
        /// Just the overide for the original MetaData property.
        /// </summary>
        public override byte Metadata
        {
            get
            {
                return base.Metadata;
            }
            set
            {
                base.Metadata = value;
            }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// DoorBlock is a NonCubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DoorBlock()
        {
        }
    }
}
