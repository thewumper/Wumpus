using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Topology;

namespace WumpusTesting
{       
    [TestClass] 
    public class GraphTests
    {
        
        public GraphTests()
        {
            
        }
        

        [TestMethod]
        public void TestSimpleConfig()
        {
            List<TestNode> nodes = new List<TestNode>
            {
                new TestNode(),
                new TestNode(),
                new TestNode()
            };
            nodes[0].Connect(nodes[1], Directions.North).Connect(nodes[2], Directions.North);
            
            Graph graph = new Graph(new List<IRoom>(nodes));
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[0]));
            Assert.IsFalse(graph.IsNodeRemovalValid(nodes[1]));
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[2]));
        }
        
    }

    public class TestNode : IRoom 
    {
        public TestNode()
        {
            ExitRooms = new Dictionary<Directions, IRoom>();
        }
        public TestNode Connect(TestNode other,Directions direction)
        {
            ExitRooms.Add(direction,other);
            other.ExitRooms.Add(direction.GetInverse(), this);
            return other;
        }
        
        public Directions[] ExitDirections => ExitRooms.Keys.ToArray();

        public Dictionary<Directions, IRoom> ExitRooms { get; private set; }

        public ushort Id => 0;
    }
    
    
}