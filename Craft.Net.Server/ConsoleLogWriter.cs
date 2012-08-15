using System;

namespace Craft.Net.Server
{
    public class ConsoleLogWriter : ILogProvider
    {
        public LogImportance MinimumImportance;

        public ConsoleLogWriter(LogImportance minimumImportance)
        {
            this.MinimumImportance = minimumImportance;
        }

        #region ILogProvider Members

        public void Log(string text, LogImportance level)
        {
            if (level >= MinimumImportance)
                Console.WriteLine(text);
        }

        #endregion
    }
}