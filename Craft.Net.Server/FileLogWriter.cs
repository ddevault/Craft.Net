using System.IO;

namespace Craft.Net.Server
{
    public class FileLogWriter : ILogProvider
    {
        private readonly StreamWriter LogWriter;
        public LogImportance MinimumImportance;

        public FileLogWriter(string File, LogImportance MinimumImportance)
        {
            this.MinimumImportance = MinimumImportance;
            LogWriter = new StreamWriter(File, true);
        }

        #region ILogProvider Members

        public void Log(string text, LogImportance Level)
        {
            lock (LogWriter)
            {
                if (Level >= MinimumImportance)
                    LogWriter.WriteLine(text);
                LogWriter.Flush();
            }
        }

        #endregion
    }
}