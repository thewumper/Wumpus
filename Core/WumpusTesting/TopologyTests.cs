
using System;
using System.IO;
using System.Linq;
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
            
            using (StreamWriter outputFile = new StreamWriter("test2.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    if (i == 9)
                    {
                        // Omit door only for 9 -> 10. Door stays for 10 -> 9
                        outputFile.WriteLine("N,NE,S,SW,NW");
                    }
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
        public void TestRoomsGoBackToThemsleves()
        {
            ITopology topology = new Topology("test1.map");
            for (ushort i = 1; i <= 30; i++)
            {
                IRoom room = topology.GetRoom(i);
                foreach (Directions dir in room.ExitDirections)
                {
                    Directions inverse = dir.GetInverse();
                    Assert.AreEqual(room.ExitRooms[dir].ExitRooms[inverse],room);
                }
            }
        }

        [TestMethod]
        public void TestRoomsMoveProperly()
        {
            ITopology topology = new Topology("test1.map");
            // 1
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.North], topology.GetRoom(25));
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.NorthWest], topology.GetRoom(30));
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.SouthWest], topology.GetRoom(6));
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.South], topology.GetRoom(7));
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.SouthEast], topology.GetRoom(2));
            Assert.AreEqual(topology.GetRoom(1).ExitRooms[Directions.NorthEast], topology.GetRoom(26));
            // 6
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.North], topology.GetRoom(30));
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.NorthWest], topology.GetRoom(5));
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.SouthWest], topology.GetRoom(11));
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.South], topology.GetRoom(12));
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.SouthEast], topology.GetRoom(7));
            Assert.AreEqual(topology.GetRoom(6).ExitRooms[Directions.NorthEast], topology.GetRoom(1));
            // 25
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.North], topology.GetRoom(19));
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.NorthWest], topology.GetRoom(24));
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.SouthWest], topology.GetRoom(30));
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.South], topology.GetRoom(1));
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.SouthEast], topology.GetRoom(26));
            Assert.AreEqual(topology.GetRoom(25).ExitRooms[Directions.NorthEast], topology.GetRoom(20));
            // 30
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.North], topology.GetRoom(24));
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.NorthWest], topology.GetRoom(29));
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.SouthWest], topology.GetRoom(5));
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.South], topology.GetRoom(6));
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.SouthEast], topology.GetRoom(1));
            Assert.AreEqual(topology.GetRoom(30).ExitRooms[Directions.NorthEast], topology.GetRoom(25));
            // 8
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.North], topology.GetRoom(2));
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.NorthWest], topology.GetRoom(7));
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.SouthWest], topology.GetRoom(13));
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.South], topology.GetRoom(14));
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.SouthEast], topology.GetRoom(15));
            Assert.AreEqual(topology.GetRoom(8).ExitRooms[Directions.NorthEast], topology.GetRoom(9));
            // 21
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.North], topology.GetRoom(15));
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.NorthWest], topology.GetRoom(14));
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.SouthWest], topology.GetRoom(20));
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.South], topology.GetRoom(27));
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.SouthEast], topology.GetRoom(22));
            Assert.AreEqual(topology.GetRoom(21).ExitRooms[Directions.NorthEast], topology.GetRoom(16));
            // 10
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.North], topology.GetRoom(4));
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.NorthWest], topology.GetRoom(9));
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.SouthWest], topology.GetRoom(15));
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.South], topology.GetRoom(16));
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.SouthEast], topology.GetRoom(17));
            Assert.AreEqual(topology.GetRoom(10).ExitRooms[Directions.NorthEast], topology.GetRoom(11));
            // 23
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.North], topology.GetRoom(17));
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.NorthWest], topology.GetRoom(16));
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.SouthWest], topology.GetRoom(22));
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.South], topology.GetRoom(29));
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.SouthEast], topology.GetRoom(24));
            Assert.AreEqual(topology.GetRoom(23).ExitRooms[Directions.NorthEast], topology.GetRoom(18));
        }

        [TestMethod]
        public void TestDijkstra()
        {
            ITopology topology = new Topology("test1.map");

            Assert.AreEqual(1, topology.DistanceBetweenRooms(1, 2, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(1, topology.DistanceBetweenRooms(1, 2, room => room.ExitRooms.Values.ToArray()));
            Assert.AreEqual(3, topology.DistanceBetweenRooms(1, 15, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(3, topology.DistanceBetweenRooms(19, 10, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(24, 1, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(26, 7, room => room.AdjacentRooms.Values.ToArray()));
            
            // Test one-way doors
            topology = new Topology("test2.map");
            Assert.AreEqual(2, topology.DistanceBetweenRooms(9, 10, room => room.ExitRooms.Values.ToArray()));
            Assert.AreEqual(1, topology.DistanceBetweenRooms(10, 9, room => room.ExitRooms.Values.ToArray()));
        }
    }
}