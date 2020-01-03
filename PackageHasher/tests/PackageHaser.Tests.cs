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
        public void SimpleHashTest1()
        {
            PackageHasher ph = new PackageHasher();

            string cwd = Directory.GetCurrentDirectory();
            string root = $"{cwd}\\testcases\\simple1";

            List<string> paths = new List<string>();
            paths.Add($"{root}\\package1");

            var task = Task.Run(async () => await ph.HashFoldersAsync(paths, root));

            var result = task.Result;

            Assert.AreEqual(1, result.Keys.Count);
            Assert.IsTrue(result.ContainsKey("package1"));
        }

        [TestMethod]
        public void SimpleHashTest2()
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

        [TestMethod]
        public void CompareHashTest1()
        {
            // compare hash of same folder and contents from 2 different roots.  Should have the smae hash value.
            PackageHasher ph = new PackageHasher();

            string cwd = Directory.GetCurrentDirectory();
            string root1 = $"{cwd}\\testcases\\simple1";
            string root2 = $"{cwd}\\testcases\\simple2";

            List<string> paths1 = new List<string>();
            paths1.Add($"{root1}\\package1");

            List<string> paths2 = new List<string>();
            paths2.Add($"{root2}\\package1");

            var task1 = Task.Run(async () => await ph.HashFoldersAsync(paths1, root1));
            var result1 = task1.Result;
            
            var task2 = Task.Run(async () => await ph.HashFoldersAsync(paths2, root2));
            var result2 = task2.Result;     

            Assert.AreEqual(result1["package1"], result2["package1"]);      
        }
    }

}