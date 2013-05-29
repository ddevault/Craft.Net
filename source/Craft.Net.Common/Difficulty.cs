namespace Craft.Net.Common
{
    /// <summary>
    /// Specifies how difficulty of the server.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// No hostile mobs will spawn and hunger will not drain.
        /// </summary>
        Peaceful = 0,
        /// <summary>
        /// Hostile mobs will spawn and will not be difficult to overcome.
        /// Hunger will not drain.
        /// </summary>
        Easy = 1,
        /// <summary>
        /// Hostile mobs will spawn.
        /// Hunger will drain, but will not cause death when empty.
        /// </summary>
        Normal = 2,
        /// <summary>
        /// Hostile mobs will spawn and be more damaging and difficult.
        /// Hunger will drain and death by starvation is possible.
        /// </summary>
        Hard = 3
    }
}