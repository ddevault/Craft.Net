using System;

namespace Craft.Net.Server
{
    public class ConsoleLogWriter : ILogProvider
    {
        public ConsoleLogWriter()
        {
        }

        public void Log(string text, LogLevel Level)
        {
            if (Level == LogLevel.Console)
                Console.WriteLine(text);
        }
    }
}

