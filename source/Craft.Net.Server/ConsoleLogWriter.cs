using System;

namespace Craft.Net.Server
{
    /// <summary>
    /// Logs output to the command window.
    /// </summary>
    public class ConsoleLogWriter : ILogProvider
    {
        /// <summary>
        /// The minimum importance required to log a message.
        /// </summary>
        public LogImportance MinimumImportance;

        /// <summary>
        /// Creates a new logger with the provided minimum importance.
        /// </summary>
        public ConsoleLogWriter(LogImportance minimumImportance)
        {
            this.MinimumImportance = minimumImportance;
        }

        #region ILogProvider Members

        /// <summary>
        /// Logs the given text at the given importance.
        /// </summary>
        public void Log(string text, LogImportance level)
        {
            if (level >= MinimumImportance)
                Console.WriteLine(text);
        }

        #endregion
    }
}