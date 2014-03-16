using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp
{
    public static class FileUtils
    {
        public static int CompareLastWriteTime(string file1, string file2)
        {
            if (string.IsNullOrEmpty(file1))
            {
                throw new ArgumentNullException("file1");
            }

            if (string.IsNullOrEmpty(file2))
            {
                throw new ArgumentNullException("file2");
            }

            if (!File.Exists(file1))
            {
                throw new FileNotFoundException(string.Format("{0} does not exist.", file1));
            }

            if (!File.Exists(file2))
            {
                throw new FileNotFoundException(string.Format("{0} does not exist.", file2));
            }

            return DateTime.Compare(File.GetLastWriteTimeUtc(file1), File.GetLastWriteTimeUtc(file2));
        }

        public static string GetRandomFileName()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }
    }
}
