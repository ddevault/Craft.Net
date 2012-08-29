using System;
using System.Collections.Generic;
using System.IO;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a 32x32 area of <see cref="Chunk"/> objects.
    /// Not all of these chunks are represented at any given time, and
    /// will be loaded from disk or generated when the need arises.
    /// </summary>
    public class Region
    {
        // In chunks
        public const int Width = 32, Depth = 32;

        /// <summary>
        /// The currently loaded chunk list.
        /// </summary>
        public Dictionary<Vector3, Chunk> Chunks { get; set; }
        /// <summary>
        /// The location of this region in the overworld.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// The <see cref="IWorldGenerator"/> used to generate this world.
        /// </summary>
        public IWorldGenerator WorldGenerator { get; set; }

        private Stream regionFile { get; set; }

        /// <summary>
        /// Creates a new Region for server-side use at the given position using
        /// the provided terrain generator.
        /// </summary>
        public Region(Vector3 position, IWorldGenerator worldGenerator)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            this.WorldGenerator = worldGenerator;
        }

        /// <summary>
        /// Creates a new Region for client-side use at the given position.
        /// </summary>
        public Region(Vector3 position)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            WorldGenerator = null;
        }

        /// <summary>
        /// Retrieves the requested chunk from the region, or
        /// generates it if a world generator is provided.
        /// </summary>
        /// <param name="position">The position of the requested local chunk coordinates.</param>
        public Chunk GetChunk(Vector3 position)
        {
            lock (Chunks)
            {
                if (!Chunks.ContainsKey(position))
                {
                    if (WorldGenerator == null)
                        throw new ArgumentException("The requested chunk is not loaded.", "position");
                    Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));
                }
                return Chunks[position];
            }
        }

        /// <summary>
        /// Sets the chunk at the specified local position to the given value.
        /// </summary>
        public void SetChunk(Vector3 position, Chunk chunk)
        {
            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, chunk);
            Chunks[position] = chunk;
        }

        /// <summary>
        /// Gets the block at the given local position.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            position = position.Floor();
            Vector3 relativePosition = position;
            position.X = (int)(position.X) / Chunk.Width;
            position.Y = 0;
            position.Z = (int)(position.Z) / Chunk.Depth;

            relativePosition.X = (int)(relativePosition.X) % Chunk.Width;
            relativePosition.Y = 0;
            relativePosition.Z = (int)(relativePosition.Z) % Chunk.Depth;

            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

            return Chunks[position].GetBlock(relativePosition);
        }

        /// <summary>
        /// Sets the block at the given local position.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            position = position.Floor();
            Vector3 relativePosition = position;
            position.X = (int)(position.X)/Chunk.Width;
            position.Y = 0;
            position.Z = (int)(position.Z)/Chunk.Depth;

            relativePosition.X = (int)(relativePosition.X)%Chunk.Width;
            relativePosition.Z = (int)(relativePosition.Z)%Chunk.Depth;

            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

            Chunks[position].SetBlock(relativePosition, value);
        }
    }
}