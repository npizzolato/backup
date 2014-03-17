namespace Backup
{
    using System;
    using System.IO;

    /// <summary>
    /// Utilities for dealing with files.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Compares the last write times of two files.
        /// </summary>
        /// <param name="file1">The first file to compare.</param>
        /// <param name="file2">The second file to compare.</param>
        /// <returns>
        /// 0: The files' last write time are identical.
        /// positive: The first file's last write time is more recent.
        /// negative: The second file's last write time is more recent.
        /// </returns>
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

        /// <summary>
        /// Copies a file to a destination, creating any directories that do not exist.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The destination.</param>
        /// <param name="overwrite">True to overwrite an existing file.</param>
        public static void Copy(string src, string dest, bool overwrite)
        {
            string dir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.Copy(src, dest, overwrite);
        }

        /// <summary>
        /// Gets a random file name.
        /// </summary>
        /// <returns>A random filename.</returns>
        public static string GetRandomFileName()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }
    }
}
