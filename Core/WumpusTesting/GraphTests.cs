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
            
            Graph graph = new Graph(new List<IRoom>(nodes), new Random());
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[0]));
            Assert.IsFalse(graph.IsNodeRemovalValid(nodes[1]));
            Assert.IsTrue(graph.IsNodeRemovalValid(nodes[2]));
        }
        [TestMethod]
        public void TestScenarioWithAllValidRemovals()
        {
            List<TestNode> nodes = new List<TestNode>
            {
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
            };
            nodes[0].Connect(nodes[1], Directions.North).Connect(nodes[2], Directions.North);
            nodes[3].Connect(nodes[4], Directions.North).Connect(nodes[5], Directions.North);
            nodes[6].Connect(nodes[7], Directions.North).Connect(nodes[8], Directions.North);
            nodes[0].Connect(nodes[3], Directions.NorthEast).Connect(nodes[6], Directions.NorthEast);
            nodes[1].Connect(nodes[4], Directions.NorthEast).Connect(nodes[7], Directions.NorthEast);
            nodes[2].Connect(nodes[5], Directions.NorthEast).Connect(nodes[8], Directions.NorthEast);

            
            Graph graph = new Graph(new List<IRoom>(nodes), new Random());
            foreach (TestNode node in nodes)
            {
                Assert.IsTrue(graph.IsNodeRemovalValid(node));
            }
        }
        
        [TestMethod]
        public void TestScenarioWithTwoRemovals()
        {
            List<TestNode> nodes = new List<TestNode>
            {
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
            };
            nodes[0].Connect(nodes[1], Directions.North).Connect(nodes[2], Directions.North);
            nodes[3].Connect(nodes[4], Directions.North).Connect(nodes[5], Directions.North);
            nodes[6].Connect(nodes[7], Directions.North).Connect(nodes[8], Directions.North);
            nodes[0].Connect(nodes[3], Directions.NorthEast).Connect(nodes[6], Directions.NorthEast);
            nodes[1].Connect(nodes[4], Directions.NorthEast).Connect(nodes[7], Directions.NorthEast);
            nodes[2].Connect(nodes[5], Directions.NorthEast).Connect(nodes[8], Directions.NorthEast);
            
            Graph graph = new Graph(new List<IRoom>(nodes), new Random());
            Assert.IsTrue(graph.IsNodeRemovalValid(new HashSet<IRoom>()
            {
                nodes[0],
                nodes[8]
            }));
            Assert.IsFalse(graph.IsNodeRemovalValid(new HashSet<IRoom>()
            {
                nodes[1],
                nodes[3]
            }));
        }
        [TestMethod]
        public void TestFindSolution()
        {
            List<TestNode> nodes = new List<TestNode>
            {
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
                new TestNode(),
            };
            nodes[0].Connect(nodes[1], Directions.North).Connect(nodes[2], Directions.North);
            nodes[3].Connect(nodes[4], Directions.North).Connect(nodes[5], Directions.North);
            nodes[6].Connect(nodes[7], Directions.North).Connect(nodes[8], Directions.North);
            nodes[0].Connect(nodes[3], Directions.NorthEast).Connect(nodes[6], Directions.NorthEast);
            nodes[1].Connect(nodes[4], Directions.NorthEast).Connect(nodes[7], Directions.NorthEast);
            nodes[2].Connect(nodes[5], Directions.NorthEast).Connect(nodes[8], Directions.NorthEast);
            
            Graph graph = new Graph(new List<IRoom>(nodes), new Random());
            graph.GetRandomPossibleSolutions(1);
            graph.GetRandomPossibleSolutions(2);
            graph.GetRandomPossibleSolutions(3);
            graph.GetRandomPossibleSolutions(4);
            graph.GetRandomPossibleSolutions(5);
            graph.GetRandomPossibleSolutions(6);
            graph.GetRandomPossibleSolutions(7);
            graph.GetRandomPossibleSolutions(8);
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
        public Dictionary<Directions, IRoom> AdjacentRooms { get; }

        public ushort Id => 0;
    }
    
    
}