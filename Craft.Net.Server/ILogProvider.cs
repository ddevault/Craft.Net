namespace Craft.Net.Server
{
    public enum LogImportance
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public interface ILogProvider
    {
        void Log(string text, LogImportance Level);
    }
}