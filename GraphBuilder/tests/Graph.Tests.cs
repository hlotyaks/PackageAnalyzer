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
        }

        
        [TestMethod]
        public void SimplGraphCycleTest3()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle3");

            Assert.IsTrue(gb.Graph.IsCyclic);
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
        }
    }
}
