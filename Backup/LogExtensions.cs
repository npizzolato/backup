using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public static class LogExtensions
    {
        public static void LogMessage(this IEnumerable<ILog> logs, string format, params object[] args)
        {
            foreach (ILog log in logs)
            {
                log.LogMessage(format, args);
            }
        }

        public static void LogError(this IEnumerable<ILog> logs, string format, params object[] args)
        {
            foreach (ILog log in logs)
            {
                log.LogError(format, args);
            }
        }
    }
}
