using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackupTest
{
    [TestClass]
    public abstract class FileTestBase
    {
        protected string file1;
        protected string file2;

        protected virtual void InitializeTest()
        { }

        protected virtual void CleanupTest()
        { }

        [TestInitialize]
        public void _TestInitialize()
        {
            this.file1 = FileUtils.GetRandomFileName();
            this.file2 = FileUtils.GetRandomFileName();
        }

        [TestCleanup]
        public void _TestCleanup()
        {
            if (File.Exists(this.file1))
            {
                File.Delete(this.file1);
            }

            if (File.Exists(this.file2))
            {
                File.Delete(this.file2);
            }
        }
    }
}
