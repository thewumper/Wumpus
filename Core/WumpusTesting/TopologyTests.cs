
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Topology;

namespace WumpusTesting
{
    [TestClass]
    public class TopologyTests
    {
        public TopologyTests()
        {

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("test1.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }
        }
        
        [TestMethod]
        public void TestTopologyCanBeCreated()
        {
            ITopology topology = new Topology("test1.map");
        }
        [TestMethod]
        public void TestLoopingNoErrs()
        {
            ITopology topology = new Topology("test1.map");
            IRoom room = topology.GetRoom(1);
            foreach (var directions in room.ExitDirections)
            {
                for (int i = 0; i < 100; i++)
                {
                    room = room.ExitRooms[directions]; // Just keep going until something bad happens
                }
            }

            Assert.AreEqual("If we are here it should pass", "If we are here it should pass");

        }
        [TestMethod]
        public void TestEdges()
        {
            ITopology topology = new Topology("test1.map");
            Assert.AreEqual(30,topology.GetRoom(1).ExitRooms[Directions.NorthWest].Id);

        }
    }
}