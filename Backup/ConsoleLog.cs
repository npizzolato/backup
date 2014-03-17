namespace Backup
{
    using System;

    /// <summary>
    /// A log which logs to the console.
    /// </summary>
    class ConsoleLog : ILog
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogMessage(string format, params object[] args)
        {
            Console.Out.WriteLine(format, args);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="format">The format for the error.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogError(string format, params object[] args)
        {
            Console.Error.WriteLine(format, args);
        }
    }
}
