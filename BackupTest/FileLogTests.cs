namespace BackupTest
{
    using System;
    using System.IO;
    using Backup;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileLogTests : FileTestBase
    {
        [TestMethod]
        public void CtorThrowsWhenLogFileNull()
        {
            Action action = () => new FileLog(null, "file.err");
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void CtorThrowsWhenErrorFileNull()
        {
            Action action = () => new FileLog("file.log", null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void CtorCreatesFiles()
        {
            new FileLog(file1, file2);

            File.Exists(file1).Should().BeTrue();
            File.Exists(file2).Should().BeTrue();

            File.Delete(file1);
            File.Delete(file2);
        }

        [TestMethod]
        public void LogMessageAppendsToLogFile()
        {
            File.WriteAllText(file1, "a message");
            var fileLog = new FileLog(file1, file2);
            fileLog.LogMessage("another message {0}", "here");

            File.ReadAllText(file1).Should().Be("a message" + Environment.NewLine + "another message here");
        }

        [TestMethod]
        public void LogErrorAppendsToErrorFile()
        {
            File.WriteAllText(file2, "a message");
            var fileLog = new FileLog(file1, file2);
            fileLog.LogError("another message {0}", "here");

            File.ReadAllText(file2).Should().Be("a message" + Environment.NewLine + "another message here");
        }
    }
}
