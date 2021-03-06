using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageAnalyzer;
using System.IO;

namespace PackageAnalyzer.Tests
{
    [TestClass]
    public class GraphBuilderTests
    {


        [TestMethod]
        public void SimpleGraphBuilderTest1()
        {
            GraphBuilder gb = new GraphBuilder();

            string cwd = Directory.GetCurrentDirectory();

            DirectoryInfo di = new DirectoryInfo($"{cwd}\\testcases\\simple1");

            FileInfo[] fi = di.GetFiles();

            gb.Insert(fi[0]);

            Assert.AreEqual(1, gb.Graph.NodeCount);

            Assert.AreEqual(0, gb.Graph.Paths("A").Count);

            Assert.AreEqual(0, gb.Graph.Children("A").Count);

        }

                [TestMethod]
        public void SimpleGraphBuilderTest2()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simple2");

            Assert.AreEqual(2, gb.Graph.NodeCount);

            Assert.AreEqual(1, gb.Graph.Paths("A").Count);

            Assert.AreEqual(0, gb.Graph.Paths("B").Count);

            Assert.AreEqual(1, gb.Graph.Children("B").Count);
        }

        [TestMethod]
        public void SimpleGraphBuilderTest3()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simple3");

            Assert.AreEqual(3, gb.Graph.NodeCount);

            Assert.AreEqual(2, gb.Graph.Paths("A").Count);

            Assert.AreEqual(0, gb.Graph.Paths("B").Count);

            Assert.AreEqual(0, gb.Graph.Paths("C").Count);

            Assert.IsTrue(gb.Graph.Paths("A").Contains("B"));

            Assert.IsTrue(gb.Graph.Paths("A").Contains("C"));

            Assert.AreEqual(1, gb.Graph.Children("B").Count);

            Assert.AreEqual(1, gb.Graph.Children("C").Count);
        }


        [TestMethod]
        [ExpectedException(typeof(Exceptions.GraphBuilderException))]
        public void FailGraphBuilderTest1()
        {
            // tests the top level package field is malformed
            GraphBuilder gb = GraphTestUtilities.PopulateGB("failure1");        
        }

        #region Utilities


        #endregion
    }
}
