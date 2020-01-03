using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace PackageAnalyzer.Tests
{
    [TestClass]
    public class PackageBuilderTests
    {
        [TestMethod]
        public void SimpleBuilderTest1()
        {
            PackageBuilder pb = new PackageBuilder();

            string cwd = Directory.GetCurrentDirectory();

            DirectoryInfo di = new DirectoryInfo($"{cwd}\\testcases\\simple1");

            List<string> buildResults = pb.Build("A", di.FullName);

            Assert.AreEqual(0, buildResults.Count);
        }
    }
}
