using System.Collections.Generic;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;

namespace Craft.Net.Data
{
    public class Region
    {
        // In chunks
        public const int Width = 32, Depth = 32;

        public Dictionary<Vector3, Chunk> Chunks;
        public Vector3 Position;
        public IWorldGenerator WorldGenerator;

        public Region(Vector3 position, IWorldGenerator worldGenerator)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            this.WorldGenerator = worldGenerator;
        }

        //Create new region without IWorldGenerator
        public Region(Vector3 position)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            WorldGenerator = null;
        }

        public Chunk GetChunk(Vector3 position)
        {
            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));
            return Chunks[position];
        }

        internal void SetChunk(Vector3 position, Chunk chunk)
        {
            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, chunk);
            Chunks[position] = chunk;
        }

        public Block GetBlock(Vector3 position)
        {
            position = position.Floor();
            Vector3 relativePosition = position;
            position.X = (int)(position.X)/Chunk.Width;
            position.Y = 0;
            position.Z = (int)(position.Z)/Chunk.Depth;

            relativePosition.X = (int)(relativePosition.X)%Chunk.Width;
            relativePosition.Y = 0;
            relativePosition.Z = (int)(relativePosition.Z)%Chunk.Depth;

            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

            return Chunks[position].GetBlock(relativePosition);
        }

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