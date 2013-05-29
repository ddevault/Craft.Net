namespace Craft.Net.Common
{
    /// <summary>
    /// Specifies the game mode and describes default
    /// player abilities.
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// Players fight against the enviornment, mobs, and players
        /// with limited resources.
        /// </summary>
        Survival = 0,
        /// <summary>
        /// Players are given unlimited resources, flying, and
        /// invulnerability.
        /// </summary>
        Creative = 1,
        /// <summary>
        /// Similar to survival, with the exception that players may
        /// not place or remove blocks.
        /// </summary>
        Adventure = 2
    }
}