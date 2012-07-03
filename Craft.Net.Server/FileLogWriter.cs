using System;
using System.IO;

namespace Craft.Net.Server
{
    public class FileLogWriter : ILogProvider
    {
        public LogImportance MinimumImportance;
        private StreamWriter LogWriter;

        public FileLogWriter(string File, LogImportance MinimumImportance)
        {
            this.MinimumImportance = MinimumImportance;
            LogWriter = new StreamWriter(File, true);
        }

        public void Log(string text, LogImportance Level)
        {
            if (Level >= MinimumImportance)
                LogWriter.WriteLine(text);
            LogWriter.Flush();
        }
    }
}

