using System;

namespace Craft.Net.Server
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
	/// The delegate that is used for logging.
	/// </summary>
	/// <param name="message">The message that should be logged.</param>
	/// <param name="importance">How important is it to log the message.</param>
	public delegate void LogDelegate(string message, LogImportance importance);

	public class LogManager
	{
		public event LogDelegate LogEvent;

		public void Log(string message, LogImportance importance)
		{
			if (LogEvent != null)
				LogEvent(message, importance);
		}
	}
}
