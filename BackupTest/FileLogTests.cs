using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackUp;
using FluentAssertions;
using System.IO;

namespace BackupTest
{
    [TestClass]
    public class FileLogTests
    {
        private string log;
        private string error;

        [TestInitialize]
        public void TestInitialize()
        {
            this.log = GetFileName();
            this.error = GetFileName();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(this.log))
            {
                File.Delete(this.log);
            }

            if (File.Exists(this.error))
            {
                File.Delete(this.error);
            }
        }

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
            new FileLog(log, error);

            File.Exists(log).Should().BeTrue();
            File.Exists(error).Should().BeTrue();

            File.Delete(log);
            File.Delete(error);
        }

        [TestMethod]
        public void LogMessageAppendsToLogFile()
        {
            File.WriteAllText(log, "a message");
            var fileLog = new FileLog(log, error);
            fileLog.LogMessage("another message {0}", "here");

            File.ReadAllText(log).Should().Be("a message" + Environment.NewLine + "another message here");
        }

        [TestMethod]
        public void LogErrorAppendsToErrorFile()
        {
            File.WriteAllText(error, "a message");
            var fileLog = new FileLog(log, error);
            fileLog.LogError("another message {0}", "here");

            File.ReadAllText(error).Should().Be("a message" + Environment.NewLine + "another message here");
        }

        private static string GetFileName()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }
    }
}
