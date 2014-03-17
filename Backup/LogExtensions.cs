namespace Backup
{
    using System.Collections.Generic;

    /// <summary>
    /// Extensions to loggers
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Logs a message to a set of loggers.
        /// </summary>
        /// <param name="logs">The logs to log to.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        public static void LogMessage(this IEnumerable<ILog> logs, string format, params object[] args)
        {
            foreach (ILog log in logs)
            {
                log.LogMessage(format, args);
            }
        }

        /// <summary>
        /// Logs an error to a set of loggers.
        /// </summary>
        /// <param name="logs">The logs to log to.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        public static void LogError(this IEnumerable<ILog> logs, string format, params object[] args)
        {
            foreach (ILog log in logs)
            {
                log.LogError(format, args);
            }
        }
    }
}
