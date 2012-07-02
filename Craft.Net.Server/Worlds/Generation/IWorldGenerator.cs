using System;

namespace Craft.Net.Server.Worlds.Generation
{
    public interface IWorldGenerator
    {
        string LevelType { get; }
        /// <summary>
        /// Generates one chunk at the given position.
        /// </summary>
        Chunk CreateChunk(Vector3 Position);
    }
}

