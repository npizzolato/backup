namespace BackupTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BackUp;

    [TestClass]
    public class BackupTest
    {
        private readonly string temp = Environment.GetEnvironmentVariable("TEMP");
        private const string testDir = "TestDir";
        private const string onlySourceFile = "SourceFile";
        private const string modifiedFile = "ModSourceFile";
        private const string laterDestFile = "DestFile";

        [TestMethod]
        public void ConstructorThrowsWhenSourceDirsIsNull()
        {
            string sourceDir = "";
            string destinationDir = Path.Combine(temp, "destination");
            List<string> dirs = new List<string>() {"dir1"};

            try
            {
                Backup backup = new Backup(sourceDir, destinationDir, dirs);
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.ToString(), "sourceDir");
                return;
            }

            Assert.Fail("No ArgumentNullException");
        }

        [TestMethod]
        public void ConstructorThrowsWhenDestDirsIsNull()
        {
            string sourceDir = Path.Combine(temp, "source");
            string destinationDir = null;
            List<string> dirs = new List<string>() { "dir1" };

            try
            {
                Backup backup = new Backup(sourceDir, destinationDir, dirs);
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.ToString(), "destinationDir");
                return;
            }

            Assert.Fail("No ArgumentNullException");
        }

        [TestMethod]
        public void ConstructorThrowsWhenL1DirsIsNull()
        {
            string sourceDir = Path.Combine(temp, "source");
            string destinationDir = Path.Combine(temp, "destination");
            List<string> dirs = null;

            try
            {
                Backup backup = new Backup(sourceDir, destinationDir, dirs);
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.ToString(), "l1Dirs");
                return;
            }

            Assert.Fail("No ArgumentNullException");
        }

        [TestMethod]
        public void CopyAllFilesCopiesCorrectFiles()
        {
            CreateTempFiles();

            string sourceDir = Path.Combine(temp, "source");
            string destinationDir = Path.Combine(temp, "destination");
            List<string> l1Dirs = new List<string>() { testDir };

            Backup backup = new Backup(sourceDir, destinationDir, l1Dirs);

            try
            {
                backup.CopyAllFiles();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.AreEqual(true, File.Exists(Path.Combine(destinationDir, testDir, onlySourceFile)), "SourceFile not copied to Destination");
            Assert.AreEqual(true, File.Exists(Path.Combine(destinationDir, testDir, modifiedFile)),  "Modified file not copied to Destination");

            DateTime correctTime = new DateTime(2013, 1, 2);

            Assert.AreEqual(0, File.GetLastWriteTime(Path.Combine(destinationDir, testDir, modifiedFile)).CompareTo(correctTime),
                "Modified file does not have new last write time");
            Assert.AreEqual(0, File.GetLastWriteTime(Path.Combine(destinationDir, testDir, laterDestFile)).CompareTo(correctTime),
                "Later file did not keep same last write time");
        }

        private void CreateTempFiles()
        {
            string sourceDir = Path.Combine(temp, "source");
            string destinationDir = Path.Combine(temp, "destination");
            List<string> l1Dirs = new List<string>() { testDir };

            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(destinationDir);

            string sourceTestDir = Path.Combine(sourceDir, testDir);
            string destinationTestDir = Path.Combine(destinationDir, testDir);

            Directory.CreateDirectory(sourceTestDir);
            Directory.CreateDirectory(destinationTestDir);

            File.Create(Path.Combine(sourceTestDir, onlySourceFile)).Close();

            File.Create(Path.Combine(destinationTestDir, modifiedFile)).Close();
            File.Create(Path.Combine(sourceTestDir, modifiedFile)).Close();
            File.SetLastWriteTime(Path.Combine(destinationTestDir, modifiedFile), new DateTime(2013, 1, 1));
            File.SetLastWriteTime(Path.Combine(sourceTestDir, modifiedFile), new DateTime(2013, 1, 2));

            File.Create(Path.Combine(sourceTestDir, laterDestFile)).Close();
            File.Create(Path.Combine(destinationTestDir, laterDestFile)).Close();
            File.SetLastWriteTime(Path.Combine(destinationTestDir, laterDestFile), new DateTime(2013, 1, 2));
            File.SetLastWriteTime(Path.Combine(sourceTestDir, laterDestFile), new DateTime(2013, 1, 1));
        }
    }
}
