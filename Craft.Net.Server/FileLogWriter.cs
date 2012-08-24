using System.IO;

namespace Craft.Net.Server
{
    /// <summary>
    /// Logs server messages to a file.
    /// </summary>
    public class FileLogWriter
    {
        private readonly StreamWriter logWriter;
        /// <summary>
        /// The minimum message importance level required to log a message.
        /// </summary>
	public LogImportance MinimumImportance
	{
		get;
		set;
	}

        /// <summary>
        /// Creates a new logger to output messages at or above the specified
        /// importance to the given file.
        /// </summary>
        public FileLogWriter(string file, LogImportance minimumImportance)
        {
            this.MinimumImportance = minimumImportance;
            logWriter = new StreamWriter(file, true);
        }

        #region ILogProvider Members

        /// <summary>
        /// Logs the given text at the given importance.
        /// </summary>
        public void Log(string text, LogImportance level)
        {
            lock (logWriter)
            {
                if (level >= MinimumImportance)
                    logWriter.WriteLine(text);
                logWriter.Flush();
            }
        }

        #endregion
    }
}