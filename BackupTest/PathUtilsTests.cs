﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backup;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackupTest
{
    [TestClass]
    public class PathUtilsTests
    {
        [TestMethod]
        public void GetLeafDirectoryNull()
        {
            Action action = () => PathUtils.GetLeafDirectory(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void GetLeafDirectoryRoot()
        {
            PathUtils.GetLeafDirectory("c:\\").Should().Be("c:");
        }

        [TestMethod]
        public void GetLeafDirectoryPathWithFinalSlash()
        {
            PathUtils.GetLeafDirectory("c:\\some\\path\\").Should().Be("path");
        }

        [TestMethod]
        public void GetLeafDirectoryPathWithoutFinalSlash()
        {
            PathUtils.GetLeafDirectory("c:\\some\\path").Should().Be("path");
        }
    }
}
