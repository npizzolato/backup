using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp
{
    interface ILog
    {
        void LogMessage(string format, params object[] args);

        void LogError(string format, params object[] args);
    }
}
