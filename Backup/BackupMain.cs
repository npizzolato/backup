namespace Backup
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Executable to copy new files from a source to a destination
    /// </summary>
    public static class BackupMain
    {
        private const string DirsKey = "dirs";

        /// <summary>
        /// The main entry of the program
        /// </summary>
        /// <param name="args">The arguments</param>
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                PrintUsage();
                return;
            }

            IEnumerable<string> copyDirs = GetDirectoriesToCopy(args);

            if (copyDirs == null || !copyDirs.Any())
            {
                throw new InvalidOperationException("There were no directories found to back up.");
            }

            Backup backup = new Backup(args[0], args[1], copyDirs);

            backup.CopyAllFiles();
        }

        /// <summary>
        /// Prints the usage for the program
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine(
                "\tBackup - a tool to backup files from one location to another{0}{0}" +
                "\tBackup.exe source-directory destination-directory [-a] [list of directories]{0}{0}" +
                "\t-a: All directories{0}" +
                "\t\tDirectories from config file{0}" +
                "\tlist of directories: Space-separated list of directories to copy over",
                Environment.NewLine);
        }

        /// <summary>
        /// Gets the directories to copy from the arguments
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns>A list of directories to copy</returns>
        private static IEnumerable<string> GetDirectoriesToCopy(string[] args)
        {
            List<string> copyDirs = new List<string>();

            if (args[2].Equals("-a", StringComparison.OrdinalIgnoreCase))
            {
                copyDirs = ConfigurationManager.AppSettings[DirsKey].Split(';', ',', ':').ToList();
            }
            else
            {
                for (int i = 2; i < args.Length; i++)
                {
                    copyDirs.Add(args[i]);
                }
            }

            return copyDirs;
        }
    }
}
