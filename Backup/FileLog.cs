using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public class FileLog : ILog
    {
        private readonly string logFile;
        private readonly string errorFile;

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

        public void LogMessage(string format, params object[] args)
        {
            File.AppendAllText(this.logFile, Environment.NewLine);
            File.AppendAllText(this.logFile, string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public void LogError(string format, params object[] args)
        {
            File.AppendAllText(this.errorFile, Environment.NewLine);
            File.AppendAllText(this.errorFile, string.Format(CultureInfo.CurrentCulture, format, args));
        }
    }
}
