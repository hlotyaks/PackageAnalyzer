using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageAnalyzer.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void SimpleGraphCycleTest1()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle1");

            Assert.IsTrue(gb.Graph.IsCyclic);
        }

        [TestMethod]
        public void SimpleGraphCycleTest2()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle2");

            Assert.IsFalse(gb.Graph.IsCyclic);
            Assert.AreEqual(1, gb.Graph.Paths("A").Count);
            Assert.IsTrue(gb.Graph.Paths("A").Contains("B"));

        }

        
        [TestMethod]
        public void SimpleGraphCycleTest3()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle3");

            Assert.IsTrue(gb.Graph.IsCyclic);
            Assert.AreEqual(1, gb.Graph.Paths("B").Count);
            Assert.IsTrue(gb.Graph.Paths("B").Contains("C"));
        }

        [TestMethod]
        public void SimpleGraphCycleTest4()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle4");

            Assert.IsTrue(gb.Graph.IsCyclic);
        }

        [TestMethod]
        public void IntermediateGraphCycleTest1()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("intermediatecycle1");

            Assert.IsFalse(gb.Graph.IsCyclic);

            Assert.AreEqual(2, gb.Graph.Children("A").Count);
            Assert.IsTrue(gb.Graph.Children("A").Contains("B"));
            Assert.IsTrue(gb.Graph.Children("A").Contains("C"));

            Assert.AreEqual(0, gb.Graph.Children("B").Count);

            Assert.AreEqual(2, gb.Graph.Children("C").Count);
            Assert.IsTrue(gb.Graph.Children("C").Contains("D"));
            Assert.IsTrue(gb.Graph.Children("C").Contains("E"));

            Assert.AreEqual(1, gb.Graph.Children("D").Count);
            Assert.IsTrue(gb.Graph.Children("D").Contains("E"));

            Assert.AreEqual(0, gb.Graph.Children("E").Count);                        
        }
    }
}
