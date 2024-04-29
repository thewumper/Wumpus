
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
                for (int i = 0; i <= 29; i++)
                {
                    if (i == 8)
                    {
                        // Omit door for 9 -> 10
                        outputFile.WriteLine("N,NE,S,SW,NW");
                        continue;
                    }
                    if (i == 9)
                    {
                        // Omit door for 10 -> 9
                        outputFile.WriteLine("N,NE,SE,S,SW");
                        continue;
                    }
                    if (i == 4)
                    {
                        // Omit door for 5 -> 30
                        outputFile.WriteLine("N,SE,S,SW,NW");
                        continue;
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
            IRoom room = topology.GetRoom(0);
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
            for (ushort i = 0; i <= 29; i++)
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
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.North], topology.GetRoom(24));
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.NorthWest], topology.GetRoom(29));
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.SouthWest], topology.GetRoom(5));
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.South], topology.GetRoom(6));
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.SouthEast], topology.GetRoom(1));
            Assert.AreEqual(topology.GetRoom(0).ExitRooms[Directions.NorthEast], topology.GetRoom(25));
            // 6
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.North], topology.GetRoom(29));
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.NorthWest], topology.GetRoom(4));
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.SouthWest], topology.GetRoom(10));
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.South], topology.GetRoom(11));
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.SouthEast], topology.GetRoom(6));
            Assert.AreEqual(topology.GetRoom(5).ExitRooms[Directions.NorthEast], topology.GetRoom(0));
            // 25
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.North], topology.GetRoom(18));
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.NorthWest], topology.GetRoom(23));
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.SouthWest], topology.GetRoom(29));
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.South], topology.GetRoom(0));
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.SouthEast], topology.GetRoom(25));
            Assert.AreEqual(topology.GetRoom(24).ExitRooms[Directions.NorthEast], topology.GetRoom(19));
            // 30
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.North], topology.GetRoom(23));
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.NorthWest], topology.GetRoom(28));
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.SouthWest], topology.GetRoom(4));
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.South], topology.GetRoom(5));
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.SouthEast], topology.GetRoom(0));
            Assert.AreEqual(topology.GetRoom(29).ExitRooms[Directions.NorthEast], topology.GetRoom(24));
            // 8
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.North], topology.GetRoom(1));
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.NorthWest], topology.GetRoom(6));
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.SouthWest], topology.GetRoom(12));
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.South], topology.GetRoom(13));
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.SouthEast], topology.GetRoom(14));
            Assert.AreEqual(topology.GetRoom(7).ExitRooms[Directions.NorthEast], topology.GetRoom(8));
            // 21
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.North], topology.GetRoom(14));
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.NorthWest], topology.GetRoom(13));
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.SouthWest], topology.GetRoom(19));
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.South], topology.GetRoom(26));
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.SouthEast], topology.GetRoom(21));
            Assert.AreEqual(topology.GetRoom(20).ExitRooms[Directions.NorthEast], topology.GetRoom(15));
            // 10
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.North], topology.GetRoom(3));
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.NorthWest], topology.GetRoom(8));
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.SouthWest], topology.GetRoom(14));
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.South], topology.GetRoom(15));
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.SouthEast], topology.GetRoom(16));
            Assert.AreEqual(topology.GetRoom(9).ExitRooms[Directions.NorthEast], topology.GetRoom(10));
            // 23
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.North], topology.GetRoom(16));
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.NorthWest], topology.GetRoom(15));
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.SouthWest], topology.GetRoom(21));
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.South], topology.GetRoom(28));
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.SouthEast], topology.GetRoom(23));
            Assert.AreEqual(topology.GetRoom(22).ExitRooms[Directions.NorthEast], topology.GetRoom(17));
        }

        [TestMethod]
        public void TestDijkstra()
        {
            ITopology topology = new Topology("test1.map");

            Assert.AreEqual(1, topology.DistanceBetweenRooms(0, 1, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(1, topology.DistanceBetweenRooms(0, 1, room => room.ExitRooms.Values.ToArray()));
            Assert.AreEqual(3, topology.DistanceBetweenRooms(0, 14, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(3, topology.DistanceBetweenRooms(18, 9, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(23, 0, room => room.AdjacentRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(25, 6, room => room.AdjacentRooms.Values.ToArray()));
            
            // Test doors
            topology = new Topology("test2.map");
            Assert.AreEqual(2, topology.DistanceBetweenRooms(8, 9, room => room.ExitRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(9, 8, room => room.ExitRooms.Values.ToArray()));
            
            // Test one-way doors
            Assert.AreEqual(3, topology.DistanceBetweenRooms(4, 24, room => room.ExitRooms.Values.ToArray()));
            Assert.AreEqual(2, topology.DistanceBetweenRooms(24, 4, room => room.ExitRooms.Values.ToArray()));
        }

        [TestMethod]
        public void TestHexagon()
        {
            // Base hexagon
            Hexagon hex = new Hexagon(0, 0);
            
            // Test each direction for south hexagons
            Assert.AreEqual(new Hexagon(-1, 0), hex.GetFromDirection(Directions.North));
            Assert.AreEqual(new Hexagon(0, 1), hex.GetFromDirection(Directions.NorthEast));
            Assert.AreEqual(new Hexagon(1, 1), hex.GetFromDirection(Directions.SouthEast));
            Assert.AreEqual(new Hexagon(1, 0), hex.GetFromDirection(Directions.South));
            Assert.AreEqual(new Hexagon(1, -1), hex.GetFromDirection(Directions.SouthWest));
            Assert.AreEqual(new Hexagon(0, -1), hex.GetFromDirection(Directions.NorthWest));
            
            // Test each direction for north hexagons
            Hexagon hexHigh = new Hexagon(0, 1);
            Assert.AreEqual(new Hexagon(-1, 1), hexHigh.GetFromDirection(Directions.North));
            Assert.AreEqual(new Hexagon(-1, 2), hexHigh.GetFromDirection(Directions.NorthEast));
            Assert.AreEqual(new Hexagon(0, 2), hexHigh.GetFromDirection(Directions.SouthEast));
            Assert.AreEqual(new Hexagon(1, 1), hexHigh.GetFromDirection(Directions.South));
            Assert.AreEqual(new Hexagon(0, 0), hexHigh.GetFromDirection(Directions.SouthWest));
            Assert.AreEqual(new Hexagon(-1, 0), hexHigh.GetFromDirection(Directions.NorthWest));
            
            // Traveling hexagon that should return on its original position
            Hexagon hex2 = hex.GetFromDirection(Directions.South);
            Hexagon hex3 = hex2.GetFromDirection(Directions.NorthEast);
            Hexagon hex4 = hex3.GetFromDirection(Directions.North);
            Hexagon hex5 = hex4.GetFromDirection(Directions.NorthWest);
            Hexagon hex6 = hex5.GetFromDirection(Directions.SouthWest);
            Hexagon hex7 = hex6.GetFromDirection(Directions.South);
            Hexagon hex8 = hex7.GetFromDirection(Directions.NorthEast);
            Assert.AreEqual(hex, hex8);
            
            Hexagon hex9 = hex.GetFromDirection(Directions.NorthEast).GetFromDirection(Directions.SouthEast)
                .GetFromDirection(Directions.South).GetFromDirection(Directions.NorthWest);
            Assert.AreEqual(new Hexagon(1, 1), hex3);
        }
    }
}