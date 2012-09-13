namespace Craft.Net.Data.Generation
{
    public interface IWorldGenerator
    {
        string LevelType { get; }
        string GeneratorName { get; }
        long Seed { get; set; }
        Vector3 SpawnPoint { get; }

        /// <summary>
        /// Generates one chunk at the given position.
        /// </summary>
        Chunk GenerateChunk(Vector3 position, Region parentRegion);

        Chunk GenerateChunk(Vector3 position);

        /// <summary>
        /// Called after the world generator is created and
        /// all values are set.
        /// </summary>
        void Initialize(Level level);
    }
}