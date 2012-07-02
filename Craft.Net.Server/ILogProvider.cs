using System;

namespace Craft.Net.Server
{
    public enum LogLevel
    {
        /// <summary>
        /// This text should be output to the console and the log file.
        /// </summary>
        Console,
        /// <summary>
        /// This text should be output to the log file.
        /// </summary>
        LogFile,
    }

    public interface ILogProvider
    {
        void Log(string text, LogLevel Level);
    }
}

