namespace BackupTest
{
    using System.IO;
    using Backup;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
