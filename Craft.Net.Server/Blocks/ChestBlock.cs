using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server.Blocks
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
        /// Variable to tell the server what square are adjacent to the chest to allow them to combine
        /// </summary>
        static Vector3[] adjacentLocations = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 0, -1),
        };

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
        public override bool BlockPlaced(World world, Vector3 position, Vector3 blockClicked, byte facing, Entity placedBy)
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

        /// <summary>
        /// Gets the opacity of a block.
        /// A Chest is a CubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }
    }
}
