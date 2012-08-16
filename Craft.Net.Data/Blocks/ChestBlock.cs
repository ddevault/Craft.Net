using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Chest Block (ID = 54)
    /// </summary>
    /// <remarks></remarks>
    public class ChestBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (54)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 54; }
        }

        /// <summary>
        /// Gets the opacity of a block.
        /// A Chest is a CubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }

        /// <summary>
        /// Called when this block is placed
        /// </summary>
        /// <param name="world">The world it was placed in</param>
        /// <param name="position">The position it was placed at</param>
        /// <param name="blockClicked">The location of the block left clicked upon placing</param>
        /// <param name="facing">The facing of the placement</param>
        /// <param name="placedBy">The entity who placed the block</param>
        /// <returns>False to override placement</returns>
        /// <remarks></remarks>
        public override bool BlockPlaced(World world, Vector3 position, Vector3 blockClicked, byte facing,
                                         Entity placedBy)
        {
            return true;
        }

        /// <summary>
        /// Called when this block is right clicked by a player.
        /// </summary>
        /// <param name="world">The world in which the event occured</param>
        /// <param name="position">The location of the block being clicked</param>
        /// <param name="clickedBy">The player who clicked the block</param>
        /// <returns>False to override the default action (block placement)</returns>
        /// <remarks></remarks>
        public override bool BlockRightClicked(World world, Vector3 position, PlayerEntity clickedBy)
        {
            return false;
        }
    }
}