using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Topology;

namespace WumpusTesting
{       
    [TestClass] 
    public class GraphTests
    {
        GraphTests()
        {
            
        }

        [TestMethod]
        public void TestSimpleConfig()
        {
            List<IRoom> nodes = new List<IRoom>
            {
                new TestNode(),
                new TestNode(),
                new TestNode()
            };
            ((TestNode)nodes[0]).Connect(nodes[1], Directions.North).Connect(nodes[2],Directions.North);
            Graph graph = new Graph(nodes);
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[0]));
            Assert.IsFalse(graph.IsNodeRemovalValid(nodes[1]));
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[2]));
        }
        
    }
    public class TestNode : IRoom 
    {


        public TestNode Connect(IRoom other,Directions direction)
        {
            this.ExitRooms[direction] = other;
            other.ExitRooms[direction.GetInverse()] = this;
            return (TestNode)other;
        }
        
        public Directions[] ExitDirections => ExitRooms.Keys.ToArray();

        public Dictionary<Directions, IRoom> ExitRooms { get; set; }
        
        public ushort Id => 0;
    }
    
}