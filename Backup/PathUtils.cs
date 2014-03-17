using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    public static class PathUtils
    {
        public static string GetLeafDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            char sep = Path.DirectorySeparatorChar;
            string[] split = path.TrimEnd(sep).Split(sep);

            return split[split.Length - 1];
        }
    }
}
