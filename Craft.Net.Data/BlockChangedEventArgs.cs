using System;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data
{
   /// <summary>
   /// Used to describe a change in a <see cref="Craft.Net.Data.World"/>.
   /// </summary>
   public class BlockChangedEventArgs : EventArgs
   {
      /// <summary>
      /// The position of the changed block.
      /// </summary>
      public Vector3 Position { get; set; }
      /// <summary>
      /// The new block.
      /// </summary>
      public Block Value { get; set; }
      /// <summary>
      /// The world it changed in.
      /// </summary>
      public World World { get; set; }

      /// <summary>
      /// Creates a new instance of <see cref="Craft.Net.Data.BlockChangedEventArgs"/>.
      /// </summary>
      /// <param name="world">The world the changed occurs in.</param>
      /// <param name="position">The position of the change.</param>
      /// <param name="value">The new block.</param>
      public BlockChangedEventArgs(World world, Vector3 position, Block value)
      {
         this.World = world;
         this.Position = position;
         this.Value = value;
      }
   }
}