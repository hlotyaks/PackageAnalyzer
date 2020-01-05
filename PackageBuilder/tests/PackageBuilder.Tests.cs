using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageAnalyzer.Tests
{
    [TestClass]
    public class PackageBuilderTests
    {
        string packagedescriptions = @"\pkgs\descriptions";   
        string packagesource = @"\src";    

        private void CleanOutput(string testroot)
        {
            foreach (string f in Directory.GetFiles($"{testroot}\\output"))
            {
                File.Delete(f);
            }
        }

        private void CleanCache(string testroot, List<string> excludeList = null)
        {
            foreach (string f in Directory.GetFiles($"{testroot}\\packages\\cache"))
            {
                if (excludeList == null)
                {
                    File.Delete(f);
                }
                else
                {
                    if(!excludeList.Any(e => f.Contains(e)))
                    {
                        File.Delete(f);
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleBuilderTest1()
        {
            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\simple1";

            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            CleanCache(testroot);

            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsTrue(buildResults["A"]);
            }

            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsFalse(buildResults["A"]);
            }
        }

        [TestMethod]
        public void SimpleBuilderTest2()
        {
            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\simple2";
 
            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            CleanCache(testroot);
            
            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsTrue(buildResults["A"]);
                Assert.IsTrue(buildResults["B"]);
            }
        }

        [TestMethod]
        public void SimpleBuilderTest3()
        {
            //
            //  A, B, and C should build
            //

            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\simple3";
 
            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            CleanCache(testroot);
            
            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsTrue(buildResults["A"]);
                Assert.IsTrue(buildResults["B"]);
                Assert.IsTrue(buildResults["C"]);
            }
        }

        [TestMethod]
        public void SimpleBuilderTest4()
        {
            //
            // cache contains a pre built A. A does not build.  B and C do.
            //

            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\simple4";
 
            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            List<string> excludeList = new List<string>();
            excludeList.Add(packageSourceHash["A"]);
            CleanCache(testroot, excludeList);
            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsFalse(buildResults["A"]);
                Assert.IsTrue(buildResults["B"]);
                Assert.IsTrue(buildResults["C"]);
            }
        }

        [TestMethod]
        public void SimpleBuilderTest5()
        {
            //
            // cache contains a pre built A and B. A and B do not build.  C will build.
            //

            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\simple5";
 
            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            List<string> excludeList = new List<string>();
            excludeList.Add(packageSourceHash["A"]);
            excludeList.Add(packageSourceHash["B"]);
            CleanCache(testroot, excludeList);
            
            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsFalse(buildResults["A"]);
                Assert.IsFalse(buildResults["B"]);
                Assert.IsTrue(buildResults["C"]);
            }
        }

        [TestMethod]
        public void IntermediateBuilderTest1()
        {
            //
            // cache contains a pre built A and B. A and B do not build.  C will build.
            //

            string cwd = Directory.GetCurrentDirectory();
            string testroot = $"{cwd}\\testcases\\intermediate1";
 
            GraphBuilder gb = new GraphBuilder($"{testroot}{packagedescriptions}");

            DirectoryInfo di = new DirectoryInfo($"{testroot}{packagesource}");
            List<string> paths = di.GetDirectories().Select(d => d.FullName).ToList();
            var task = Task.Run(async () => await PackageHasher.HashFoldersAsync(paths, testroot));

            // package source to package source hash mapping
            Dictionary<string, string> packageSourceHash = task.Result;

            List<string> excludeList = new List<string>();
            //excludeList.Add(packageSourceHash["A"]);
            //excludeList.Add(packageSourceHash["B"]);
            CleanCache(testroot, excludeList);
            
            {
                CleanOutput(testroot);
                PackageBuilder pb = new PackageBuilder(testroot);
                Dictionary<string, bool> buildResults = pb.Build("A", $"{testroot}{packagesource}", packageSourceHash, gb.Graph);
                Assert.IsTrue(buildResults["A"]);
                Assert.IsTrue(buildResults["B"]);
                Assert.IsTrue(buildResults["C"]);
                Assert.IsTrue(buildResults["D"]);
                Assert.IsTrue(buildResults["E"]);                
            }
        }
    }
}
