namespace Backup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Utilities for dealing with paths.
    /// </summary>
    public static class PathUtils
    {
        /// <summary>
        /// Gets the leaf directory in a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLeafDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            
            char sep = Path.DirectorySeparatorChar;
            string[] split = path.TrimEnd(sep).Split(sep);

            return split[split.Length - 1];
        }
    }
}
