using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Backup;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackupTest
{
    [TestClass]
    public class FileUtilsTest : FileTestBase
    {
        [TestMethod]
        public void WasFileCreatedFirstThrowsWhenFirstFileNull()
        {
            Action action = () => FileUtils.CompareLastWriteTime(null, this.file2);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void WasFileCreatedFirstThrowsWhenSecondFileNull()
        {
            Action action = () => FileUtils.CompareLastWriteTime(this.file1, null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void FirstFileDoesNotExist()
        {
            Action action = () => FileUtils.CompareLastWriteTime(FileUtils.GetRandomFileName(), this.file2);
            action.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void SecondFileDoesNotExist()
        {
            Action action = () => FileUtils.CompareLastWriteTime(this.file1, FileUtils.GetRandomFileName());
            action.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void FirstFileCreatedFirst()
        {
            CreateFile(this.file1);
            Thread.Sleep(5);
            CreateFile(this.file2);

            FileUtils.CompareLastWriteTime(this.file1, this.file2).Should().BeNegative();
        }

        [TestMethod]
        public void SecondFileCreatedFirst()
        {
            CreateFile(this.file2);
            Thread.Sleep(5);
            CreateFile(this.file1);

            FileUtils.CompareLastWriteTime(this.file1, this.file2).Should().BePositive();
        }

        [TestMethod]
        public void FilesCreatedAtTheSameTime()
        {
            CreateFile(this.file1);
            FileUtils.CompareLastWriteTime(this.file1, this.file1).Should().Be(0);
        }

        [TestMethod]
        public void CopyDirectoryDoesNotExist()
        {
            CreateFile(this.file1);
            this.file2 = Path.Combine(Path.GetTempPath(), "somedir", "anotherdir", Guid.NewGuid().ToString());
            FileUtils.Copy(this.file1, this.file2, true);
            File.Exists(this.file2).Should().BeTrue();

            File.Delete(this.file2);
            Directory.Delete(Path.Combine(Path.GetTempPath(), "somedir", "anotherdir"));
            Directory.Delete(Path.Combine(Path.GetTempPath(), "somedir"));
        }

        private static void CreateFile(string file)
        {
            var fs = File.Create(file);
            fs.Dispose();
        }
    }
}
