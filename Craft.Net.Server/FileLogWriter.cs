using System.IO;

namespace Craft.Net.Server
{
    public class FileLogWriter : ILogProvider
    {
        private readonly StreamWriter logWriter;
        public LogImportance MinimumImportance;

        public FileLogWriter(string file, LogImportance minimumImportance)
        {
            this.MinimumImportance = minimumImportance;
            logWriter = new StreamWriter(file, true);
        }

        #region ILogProvider Members

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