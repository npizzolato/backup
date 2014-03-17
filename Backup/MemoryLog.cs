namespace Backup
{
    using System.IO;

    /// <summary>
    /// A class to log messages to memory.
    /// </summary>
    public class MemoryLog : ILog
    {
        /// <summary>
        /// Initializes a new instance of the MemoryLog.
        /// </summary>
        public MemoryLog()
        {
            this.Log = new StringWriter();
            this.Error = new StringWriter();
        }

        /// <summary>
        /// Gets the messages logged.
        /// </summary>
        public StringWriter Log { get; private set; }

        /// <summary>
        /// Gets the errors logged.
        /// </summary>
        public StringWriter Error { get; private set; }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogMessage(string format, params object[] args)
        {
            this.Log.WriteLine(format, args);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="format">The format for the error.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogError(string format, params object[] args)
        {
            this.Error.WriteLine(format, args);
        }
    }
}
