using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public class Copier
    {
        private readonly IEnumerable<ILog> logs;

        public Copier(ILog log)
            : this(new List<ILog>() { log })
        {
        }

        public Copier(IEnumerable<ILog> logs)
        {
            if (logs == null)
            {
                throw new ArgumentNullException("logs");
            }

            this.logs = logs;
        }

        public bool TryCopy(string src, string dest)
        {
            this.CheckArguments(src, dest);
            Exception ex = null;
            try
            {
                FileUtils.Copy(src, dest, overwrite: true);
            }
            catch (Exception e)
            {
                ex = e;
            }

            if (ex == null)
            {
                this.logs.LogMessage("Copied '{0}' to '{1}'", src, dest);
            }
            else
            {
                this.logs.LogError("Failed to copy '{0}' to '{1}' due to {2}.", src, dest, ex);
            }

            return ex == null;
        }

        public bool CopyIfNewer(string src, string dest)
        {
            this.CheckArguments(src, dest);
            bool copied = false;

            if (!File.Exists(dest) || FileUtils.CompareLastWriteTime(src, dest) > 0)
            {
                copied = this.TryCopy(src, dest);
            }

            return copied;
        }

        private void CheckArguments(string src, string dest)
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentNullException("src");
            }

            if (string.IsNullOrEmpty(dest))
            {
                throw new ArgumentNullException("dest");
            }

            if (!File.Exists(src))
            {
                throw new FileNotFoundException(string.Format("Source file '{0}' does not exist", src));
            }
        }
    }
}
