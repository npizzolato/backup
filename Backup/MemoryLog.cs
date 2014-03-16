using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp
{
    public class MemoryLog : ILog
    {
        public MemoryLog()
        {
            this.Log = new StringWriter();
            this.Error = new StringWriter();
        }

        public StringWriter Error { get; private set; }

        public StringWriter Log { get; private set; }

        public void LogMessage(string format, params object[] args)
        {
            this.Log.WriteLine(format, args);
        }

        public void LogError(string format, params object[] args)
        {
            this.Error.WriteLine(format, args);
        }
    }
}
