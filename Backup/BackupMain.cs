namespace Backup
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.IO;
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
            string srcRoot = args[0];
            string destRoot = args[1];

            if (copyDirs == null || !copyDirs.Any())
            {
                throw new InvalidOperationException("There were no directories found to back up.");
            }

            string logDir = Path.Combine(Path.GetTempPath(), "backup");
            string logPathWithoutExtension = Path.Combine(logDir, "backup-" + DateTime.Now.ToString("yyyyMMdd"));
            string log = logPathWithoutExtension + ".log";
            string err = logPathWithoutExtension = ".err";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            Copier copier = new Copier(new List<ILog>() { new ConsoleLog(), new FileLog(log, err) });

            foreach (string dir in copyDirs)
            {
                string src = Path.Combine(srcRoot, dir);
                string dest = Path.Combine(destRoot, dir);
                copier.UpdateDirectory(src, dest);
            }
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
