namespace BackUp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class to backup files to a location
    /// </summary>
    public sealed class Backup
    {
        private List<string> expandedSourceDirs;

        private List<string> expandedDestinationDirs;

        private StreamWriter writer;

        /// <summary>
        /// Initalizes a new instance of the <see cref="Backup"/> class
        /// </summary>
        /// <param name="sourceDir">The source directory</param>
        /// <param name="destinationDir">The destination directory</param>
        /// <param name="l1Dirs">The level 1 directories in the source to copy to the destination</param>
        public Backup(string sourceDir, string destinationDir, IEnumerable<string> l1Dirs)
        {
            if (string.IsNullOrWhiteSpace(sourceDir))
            {
                throw new ArgumentNullException("sourceDir");
            }

            if (string.IsNullOrWhiteSpace(destinationDir))
            {
                throw new ArgumentNullException("destinationDir");
            }

            if (l1Dirs == null)
            {
                throw new ArgumentNullException("l1Dirs");
            }

            this.SourceDir = sourceDir;
            this.DestinationDir = destinationDir;
            this.L1Dirs = l1Dirs;

            expandedSourceDirs = new List<string>();
            expandedDestinationDirs = new List<string>();

            foreach (string dir in l1Dirs)
            {
                expandedSourceDirs.Add(Path.Combine(this.SourceDir, dir));
                expandedDestinationDirs.Add(Path.Combine(this.DestinationDir, dir));
            }
        }

        /// <summary>
        /// Gets the list of level 1 directories
        /// </summary>
        public IEnumerable<string> L1Dirs { get; private set; }

        /// <summary>
        /// Gets the source directory
        /// </summary>
        public string SourceDir { get; private set; }

        /// <summary>
        /// Gets the destination directory
        /// </summary>
        public string DestinationDir { get; private set; }

        /// <summary>
        /// Copies all files in the level 1 directories in the source to the destination
        /// </summary>
        public void CopyAllFiles()
        {
            string path = Path.Combine(Path.GetTempPath(), "backup-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            using (this.writer = new StreamWriter(path))
            {
                List<string> sourceDirs = Directory.EnumerateDirectories(this.SourceDir).ToList();

                foreach (string dir in sourceDirs.Where(e => this.expandedSourceDirs.Exists(f => f.Equals(e, StringComparison.OrdinalIgnoreCase))))
                {
                    Console.WriteLine("{0}Copying {1}", Environment.NewLine, dir);
                    this.writer.WriteLine("***********************************");
                    this.writer.WriteLine("Copying {0}", dir);

                    string destination = dir.Replace(this.SourceDir, this.DestinationDir);

                    this.CopyDir(dir, destination);
                }
            }
        }

        /// <summary>
        /// Copies a directory to a destination
        /// </summary>
        /// <param name="sourceDirectory">The source directory</param>
        /// <param name="destinationDirectory">The destination directory</param>
        private void CopyDir(string sourceDirectory, string destinationDirectory)
        {
            this.writer.WriteLine("Copying {0} to {1}", sourceDirectory, destinationDirectory);

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            try
            {

                foreach (string sourceDir in Directory.EnumerateDirectories(sourceDirectory))
                {
                    string dest = sourceDir.Replace(this.SourceDir, this.DestinationDir);

                    CopyDir(sourceDir, dest);
                }

                foreach (string sourceFile in Directory.EnumerateFiles(sourceDirectory))
                {
                    string destFile = sourceFile.Replace(sourceDirectory, destinationDirectory);

                    CopyFile(sourceFile, destFile);
                }
            }
            catch (UnauthorizedAccessException e) 
            {
                Console.WriteLine("Encountered unauthorized access exception: {0}", e.ToString());
                this.writer.WriteLine("Encountered unauthorized access exception {0}", e.ToString());
            }
        }

        /// <summary>
        /// Copies a file from a source to a destination
        /// </summary>
        /// <param name="sourceFile">The file to copy</param>
        /// <param name="destinationFile">The file to copy to</param>
        /// <returns><c>True</c> if the copy was successful</returns>
        private bool CopyFile(string sourceFile, string destinationFile)
        {
            bool copySuccessful = false;

            if (!File.Exists(destinationFile))
            {
                Console.WriteLine("Copying {0}", sourceFile);
                this.writer.WriteLine("Copying file {0}", sourceFile);

                copySuccessful = this.TryCopy(sourceFile, destinationFile);
            }
            else
            {
                DateTime sourceModTime = File.GetLastWriteTime(sourceFile);
                DateTime destinationModTime = File.GetLastWriteTime(destinationFile);

                if (DateTime.Compare(sourceModTime, destinationModTime) > 0)
                {
                    Console.WriteLine("Copying {0}", sourceFile);
                    this.writer.WriteLine("Copying file {0}", sourceFile);

                    copySuccessful = this.TryCopy(sourceFile, destinationFile);
                }
            }

            return copySuccessful;
        }

        /// <summary>
        /// Tries to copy a file from a source location to a destination
        /// </summary>
        /// <param name="sourceFile">The source location</param>
        /// <param name="destinationFile">The destination</param>
        /// <returns><c>True</c> if the copy was successful</returns>
        private bool TryCopy(string sourceFile, string destinationFile)
        {
            try
            {
                File.Copy(sourceFile, destinationFile, true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to copy {0} because: {1}", sourceFile, e.ToString());
                this.writer.WriteLine("Failed to copy {0} because: {1}", sourceFile, e.ToString());
                return false;
            }
        }
    }
}
