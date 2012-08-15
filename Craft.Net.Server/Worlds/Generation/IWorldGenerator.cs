namespace Craft.Net.Server.Worlds.Generation
{
    public interface IWorldGenerator
    {
        string LevelType { get; }
        long Seed { get; set; }
        Vector3 SpawnPoint { get; }

        /// <summary>
        /// Generates one chunk at the given position.
        /// </summary>
        Chunk GenerateChunk(Vector3 position, Region parentRegion);

        Chunk GenerateChunk(Vector3 position);
    }
}