namespace Backup
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Class to log to a file.
    /// </summary>
    public class FileLog : ILog
    {
        private readonly string logFile;
        private readonly string errorFile;

        /// <summary>
        /// Initializes a new instance of a FileLog.
        /// </summary>
        /// <param name="logFile">The file to log messages to.</param>
        /// <param name="errorFile">The file to log errors to.</param>
        public FileLog(string logFile, string errorFile)
        {
            if (string.IsNullOrEmpty(logFile))
            {
                throw new ArgumentNullException("logFile");
            }

            if (string.IsNullOrEmpty(errorFile))
            {
                throw new ArgumentNullException("errorFile");
            }

            if (!File.Exists(logFile))
            {
                var fs = File.Create(logFile);
                fs.Dispose();
            }

            if (!File.Exists(errorFile))
            {
                var fs = File.Create(errorFile);
                fs.Dispose();
            }

            this.logFile = logFile;
            this.errorFile = errorFile;
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="format">The format for the message.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogMessage(string format, params object[] args)
        {
            string message = Environment.NewLine + string.Format(CultureInfo.CurrentCulture, format, args);
            File.AppendAllText(this.logFile, message);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="format">The format for the error.</param>
        /// <param name="args">The arguments to the format string.</param>
        public void LogError(string format, params object[] args)
        {
            string message = Environment.NewLine + string.Format(CultureInfo.CurrentCulture, format, args);
            File.AppendAllText(this.errorFile, message);
        }
    }
}
