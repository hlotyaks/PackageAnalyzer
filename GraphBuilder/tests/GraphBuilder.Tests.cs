using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer;
using System.IO;

namespace PackageAnalyzer.Graph.Tests
{
    [TestClass]
    public class GraphBuilderTests
    {


        [TestMethod]
        public void SimpleTest1()
        {
            GraphBuilder gb = new GraphBuilder();

            string cwd = Directory.GetCurrentDirectory();

            DirectoryInfo di = new DirectoryInfo($"{cwd}\\testcases\\simple1");

            FileInfo[] fi = di.GetFiles();

            gb.Insert(fi[0]);

            Assert.AreEqual(1, gb.Graph.NodeCount);

            Assert.AreEqual(0, gb.Graph.Edges("A").Count);

        }

                [TestMethod]
        public void SimpleTest2()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simple2");

            Assert.AreEqual(2, gb.Graph.NodeCount);

            Assert.AreEqual(1, gb.Graph.Edges("A").Count);

            Assert.AreEqual(0, gb.Graph.Edges("B").Count);

        }

        [TestMethod]
        [ExpectedException(typeof(GraphBuilderException))]
        public void FailTest1()
        {
            // tests the top level package field is malformed
            GraphBuilder gb = GraphTestUtilities.PopulateGB("failure1");        
        }

        #region Utilities


        #endregion
    }
}
