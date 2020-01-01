using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageAnalyzer.Graph.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void SimpleCycleTest1()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle1");

            Assert.IsTrue(gb.Graph.IsCyclic);
        }

                [TestMethod]
        public void SimpleCycleTest2()
        {
            GraphBuilder gb = GraphTestUtilities.PopulateGB("simplecycle2");

            Assert.IsFalse(gb.Graph.IsCyclic);
        }
    }
}
