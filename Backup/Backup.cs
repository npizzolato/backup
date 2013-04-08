using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp
{
    public class Backup
    {
        private List<string> expandedSourceDirs;

        private List<string> expandedDestinationDirs;

        public Backup(string sourceDir, string destinationDir, List<string> l1Dirs)
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

        public List<string> L1Dirs { get; private set; }

        public string SourceDir { get; set; }

        public string DestinationDir { get; set; }

        public bool CopyAllFiles()
        {
            bool copySuccessful = false;

            List<string> sourceDirs = Directory.EnumerateDirectories(this.SourceDir).ToList();

            foreach (string dir in sourceDirs.Where(e => this.expandedSourceDirs.Exists(f => f.Equals(e, StringComparison.OrdinalIgnoreCase))))
            {
                Console.WriteLine("{0}Copying {1}", Environment.NewLine, dir);

                string destination = dir.Replace(this.SourceDir, this.DestinationDir);

                this.CopyDir(dir, destination);
            }

            return copySuccessful;
        }

        private void CopyDir(string sourceDirectory, string destinationDirectory)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

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

        private void CopyFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(destinationFile))
            {
                Console.WriteLine("Copying {0}", sourceFile);

                this.TryCopy(sourceFile, destinationFile);
            }
            else
            {
                DateTime sourceModTime = File.GetLastWriteTime(sourceFile);
                DateTime destinationModTime = File.GetLastWriteTime(destinationFile);

                if (DateTime.Compare(sourceModTime, destinationModTime) > 0)
                {
                    Console.WriteLine("Copying {0}", sourceFile);

                    this.TryCopy(sourceFile, destinationFile);
                }
            }
        }

        private void TryCopy(string sourceFile, string destinationFile)
        {
            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to copy {0} because: {1}", sourceFile, e.ToString());
            }
        }
    }
}
