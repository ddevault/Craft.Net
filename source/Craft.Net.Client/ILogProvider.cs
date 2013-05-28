namespace Craft.Net.Client
{
    /// <summary>
    /// Specifies the importance of a given message with regard to logging.
    /// </summary>
    public enum LogImportance
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    /// <summary>
    /// Describes a mechanism for logging server information.
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Logs the given text at the given importance.
        /// </summary>
        void Log(string text, LogImportance level);
    }
}