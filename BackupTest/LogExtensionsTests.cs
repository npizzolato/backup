namespace BackupTest
{
    using System.Collections.Generic;
    using Backup;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LogExtensionsTests
    {
        [TestMethod]
        public void LogMessagesLogsToAllLogs()
        {
            const string message = "a message";
            MemoryLog log1 = new MemoryLog();
            MemoryLog log2 = new MemoryLog();
            List<ILog> logs = new List<ILog>() { log1, log2 };
            logs.LogMessage(message);

            log1.Log.ToString().Should().Contain(message);
            log2.Log.ToString().Should().Contain(message);
        }

        [TestMethod]
        public void LogErrorLogsToAllLogs()
        {
            const string error = "an error";
            MemoryLog log1 = new MemoryLog();
            MemoryLog log2 = new MemoryLog();
            List<ILog> logs = new List<ILog>() { log1, log2 };
            logs.LogError(error);

            log1.Error.ToString().Should().Contain(error);
            log2.Error.ToString().Should().Contain(error);
        }
    }
}
