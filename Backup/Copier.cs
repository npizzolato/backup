namespace Backup
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class to copy files
    /// </summary>
    public class Copier
    {
        private readonly IEnumerable<ILog> logs;

        /// <summary>
        /// Initializes a new instance of a Copier.
        /// </summary>
        /// <param name="log">The log to use.</param>
        public Copier(ILog log)
            : this(new List<ILog>() { log })
        {
        }

        /// <summary>
        /// Initializes a new instance of a Copier.
        /// </summary>
        /// <param name="logs">The logs to use.</param>
        public Copier(IEnumerable<ILog> logs)
        {
            if (logs == null)
            {
                throw new ArgumentNullException("logs");
            }

            this.logs = logs;
        }

        /// <summary>
        /// Tries to copy a file to a destination.
        /// </summary>
        /// <param name="src">The source file.</param>
        /// <param name="dest">The destination file.</param>
        /// <returns>True if the file was copied.</returns>
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

        /// <summary>
        /// Tries to copy a file if it is newer than the destination.
        /// </summary>
        /// <param name="src">The source file.</param>
        /// <param name="dest">The destination file.</param>
        /// <returns>True if the file was copied.</returns>
        public bool TryCopyIfNewer(string src, string dest)
        {
            this.CheckArguments(src, dest);
            bool copied = false;

            if (!File.Exists(dest) || FileUtils.CompareLastWriteTime(src, dest) > 0)
            {
                copied = this.TryCopy(src, dest);
            }

            return copied;
        }

        /// <summary>
        /// Updates all files in the directory and subdirectory that are newer than the destination.
        /// </summary>
        /// <param name="src">The source directory.</param>
        /// <param name="dest">The destination directory.</param>
        public void UpdateDirectory(string src, string dest)
        {
            if (string.IsNullOrEmpty(src))
            {
                throw new ArgumentNullException("src");
            }

            if (string.IsNullOrEmpty(dest))
            {
                throw new ArgumentNullException("dest");
            }

            if (!Directory.Exists(src))
            {
                throw new DirectoryNotFoundException(string.Format("Directory '{0}' does not exist.", src));
            }

            IEnumerable<string> sourceFiles = Directory.EnumerateFiles(src);
            IEnumerable<string> sourceDirs = Directory.EnumerateDirectories(src);

            foreach (string sourceFile in sourceFiles)
            {
                string filename = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(dest, filename);
                this.TryCopyIfNewer(sourceFile, destFile);
            }

            foreach (string sourceDir in sourceDirs)
            {
                string dirname = PathUtils.GetLeafDirectory(sourceDir);
                string destDir = Path.Combine(dest, dirname);
                this.UpdateDirectory(sourceDir, destDir);
            }
        }

        /// <summary>
        /// Checks whether the arguments to a function are correct.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The destination.</param>
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
