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
    public class CopierTests : FileTestBase
    {
        [TestMethod]
        public void CtorThrowsWhenLogsNull()
        {
            Action action = () => new Copier(logs: null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void SourceFileDoesNotExist()
        {
            Action action = () =>
                {
                    Copier copier = new Copier(new MemoryLog());
                    copier.TryCopy(this.file1, this.file2);
                };
            action.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void NewFileCreated()
        {
            File.WriteAllText(this.file1, "some text");
            MemoryLog log = new MemoryLog();
            Copier copier = new Copier(log);

            copier.TryCopy(this.file1, this.file2).Should().BeTrue();
            File.ReadAllText(this.file2).Should().Be("some text");
            log.Log.ToString().Should().Contain(this.file1);
            log.Log.ToString().Should().Contain(this.file2);
        }

        [TestMethod]
        public void ExistingFileOverwritten()
        {
            File.WriteAllText(this.file1, "some text");
            File.WriteAllText(this.file2, "some other text");
            MemoryLog log = new MemoryLog();
            Copier copier = new Copier(log);

            copier.TryCopy(this.file1, this.file2).Should().BeTrue();
            File.ReadAllText(this.file2).Should().Be("some text");
            log.Log.ToString().Should().Contain(this.file1);
            log.Log.ToString().Should().Contain(this.file2);
        }

        [TestMethod]
        public void ExceptionsLogged()
        {
            File.WriteAllText(this.file1, "some text");
            MemoryLog log = new MemoryLog();
            Copier copier = new Copier(log);
            using (var fs = File.Open(this.file1, FileMode.Open)) // open file handle
            {
                copier.TryCopy(this.file1, this.file2).Should().BeFalse();
                log.Error.ToString().Should().Contain(this.file1);
                log.Error.ToString().Should().Contain(this.file2);
            }
        }

        [TestMethod]
        public void CopyIfNewerDestinationDoesNotExist()
        {
            File.WriteAllText(this.file1, "file");
            Copier copier = new Copier(new MemoryLog());

            copier.CopyIfNewer(this.file1, this.file2).Should().BeTrue();
            File.ReadAllText(this.file2).Should().Be("file");
        }

        [TestMethod]
        public void CopyIfNewerSourceDoesNotExist()
        {
            Action action = () =>
                {
                    Copier copier = new Copier(new MemoryLog());
                    copier.CopyIfNewer(this.file1, this.file2);
                };
            action.ShouldThrow<FileNotFoundException>();
        }

        [TestMethod]
        public void CopyIfNewerSourceNewer()
        {
            File.WriteAllText(this.file2, "file2");
            Thread.Sleep(5);
            File.WriteAllText(this.file1, "file1");
            Copier copier = new Copier(new MemoryLog());

            copier.CopyIfNewer(this.file1, this.file2).Should().BeTrue();
            File.ReadAllText(this.file2).Should().Be("file1");
        }

        [TestMethod]
        public void CopyIfNewerSourceOlder()
        {
            File.WriteAllText(this.file1, "file1");
            Thread.Sleep(5);
            File.WriteAllText(this.file2, "file2");
            Copier copier = new Copier(new MemoryLog());

            copier.CopyIfNewer(this.file1, this.file2).Should().BeFalse();
            File.ReadAllText(this.file2).Should().Be("file2");
        }

        [TestMethod]
        public void CopyIfNewerSameTime()
        {
            File.WriteAllText(this.file1, "file1");
            File.Copy(this.file1, this.file2);
            Copier copier = new Copier(new MemoryLog());

            copier.CopyIfNewer(this.file1, this.file2).Should().BeFalse();
            File.ReadAllText(this.file2).Should().Be("file1");
        }
    }
}
