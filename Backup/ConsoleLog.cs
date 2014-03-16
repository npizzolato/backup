using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    class ConsoleLog : ILog
    {
        public void LogMessage(string format, params object[] args)
        {
            Console.Out.WriteLine(format, args);
        }

        public void LogError(string format, params object[] args)
        {
            Console.Error.WriteLine(format, args);
        }
    }
}
