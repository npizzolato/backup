using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUp
{
    public class BackupMain
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("args must be 2: sourceDirectory destinationDirectory");
            }

            List<string> dirs = new List<string>() {"books", "comics", "documents", "games", "isos", "music", "pictures", "videos"};

            Backup backup = new Backup(args[0], args[1], dirs);

            backup.CopyAllFiles();
        }
    }
}
