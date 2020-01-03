using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAnalyzer.Tests
{

    [TestClass]
    public class PackageHasherTests
    {
        [TestMethod]
        public void SimpleTest1()
        {
            PackageHasher ph = new PackageHasher();

            string cwd = Directory.GetCurrentDirectory();
            string root = $"{cwd}\\testcases\\simple2";

            List<string> paths = new List<string>();
            paths.Add($"{root}\\package1");

            var task = Task.Run(async () => await ph.HashFoldersAsync(paths, root));

            var result = task.Result;

            Assert.AreEqual(1, result.Keys.Count);
            Assert.IsTrue(result.ContainsKey("package1"));
        }

        [TestMethod]
        public void SimpleTest2()
        {
            PackageHasher ph = new PackageHasher();

            string cwd = Directory.GetCurrentDirectory();
            string root = $"{cwd}\\testcases\\simple2";

            List<string> paths = new List<string>();
            paths.Add($"{root}\\package1");
            paths.Add($"{root}\\package2");

            var task = Task.Run(async () => await ph.HashFoldersAsync(paths, root));

            var result = task.Result;

            Assert.AreEqual(2, result.Keys.Count);
            Assert.IsTrue(result.ContainsKey("package1"));
            Assert.IsTrue(result.ContainsKey("package2"));
        }
    }

}