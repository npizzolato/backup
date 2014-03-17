namespace Backup
{
    /// <summary>
    /// Interface for a logger
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        void LogMessage(string format, params object[] args);

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="format">The format for the error.</param>
        /// <param name="args">The arguments to the format string.</param>
        void LogError(string format, params object[] args);
    }
}
