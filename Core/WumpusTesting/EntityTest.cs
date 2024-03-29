using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Entity;
using WumpusCore.GameLocations;
using WumpusCore.Topology;

namespace WumpusTesting
{
    [TestClass] 
    public class EntityTest
    {
        public EntityTest()
        {
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("entitytest1.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    // All doors open
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }
            
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("entitytest2.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    // No traveling south-west
                    outputFile.WriteLine("N,NE,SE,S,NW");
                }
            }
        }
        
        public GameLocations SetupGameLocations()
        {
            Topology topology = new Topology("entitytest1.map");
            GameLocations game = new GameLocations(30);
            game.AddEntity(new DummyEntity(topology, game, EntityType.Player));
            game.AddEntity(new Entity(topology, game, 20, EntityType.Wumpus));
            return game;
        }
        
        public GameLocations SetupGameLocationsDirected()
        {
            Topology topology = new Topology("entitytest2.map");
            GameLocations game = new GameLocations(30);
            game.AddEntity(new DummyEntity(topology, game, EntityType.Player));
            game.AddEntity(new Entity(topology, game, 20, EntityType.Wumpus));
            return game;
        }

        private DummyEntity getEntity(GameLocations game)
        {
            return (DummyEntity)game.GetEntity(EntityType.Player);
        }
        
        [TestMethod]
        public void TestGetAdjacentRooms()
        {
            GameLocations game = SetupGameLocations();
            // Clockwise from top
            ushort[] expected = new ushort[] { 9, 10, 16, 21, 14, 8 };
            ushort[] actual = getEntity(game).GetAdjacentRooms();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
            Assert.AreEqual(expected[4], actual[4]);
            Assert.AreEqual(expected[5], actual[5]);
            Assert.AreEqual(6, actual.Length);
        }
        
        [TestMethod]
        public void TestGetAccessibleRooms()
        {
            GameLocations game = SetupGameLocations();
            // Clockwise from top
            ushort[] expected = new ushort[] { 9, 10, 16, 21, 14, 8 };
            ushort[] actual = getEntity(game).GetAccessibleRooms();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
            Assert.AreEqual(expected[4], actual[4]);
            Assert.AreEqual(expected[5], actual[5]);
            Assert.AreEqual(6, actual.Length);
            
            game = SetupGameLocationsDirected();
            expected = new ushort[] { 9, 10, 16, 21, 8 };
            actual = getEntity(game).GetAccessibleRooms();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
            Assert.AreEqual(expected[4], actual[4]);
            Assert.AreEqual(5, actual.Length);
        }
        
        [TestMethod]
        public void TestMoveToRoom()
        {
            GameLocations game = SetupGameLocations();
            getEntity(game).MoveToRoomWrapper(10);
            getEntity(game).MoveToRoomWrapper(17);
            Assert.AreEqual(17, getEntity(game).location);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { getEntity(game).MoveToRoomWrapper(1); });
        }
        
        [TestMethod]
        public void TestTeleportToRoom()
        {
            GameLocations game = SetupGameLocations();
            getEntity(game).TeleportToRoomWrapper(1);
            Assert.AreEqual(1, getEntity(game).location);
            getEntity(game).TeleportToRoomWrapper(5);
            Assert.AreEqual(5, getEntity(game).location);
            getEntity(game).TeleportToRoomWrapper(15);
            Assert.AreEqual(15, getEntity(game).location);
        }
        
        [TestMethod]
        public void TestTeleportToRandom()
        {
            GameLocations game = SetupGameLocations();
            ((DummyEntity)game.GetEntity(EntityType.Player)).TeleportToRandomWrapper();
            // Statistically impossible to fail
            for (int i = 0; i < 500; i++)
            {
                int locA = game.GetEntity(EntityType.Player).location;
                getEntity(game).TeleportToRandomWrapper();
                int locB = game.GetEntity(EntityType.Player).location;
                getEntity(game).TeleportToRandomWrapper();
                int locC = game.GetEntity(EntityType.Player).location;
                getEntity(game).TeleportToRandomWrapper();
                int locD = game.GetEntity(EntityType.Player).location;
                if (locA != locB && locB != locC && locC != locD)
                {
                    return;
                }
            }
            Assert.Fail();
        }
        
        [TestMethod]
        public void TestCheckIfEntitySharingRoom()
        {
            GameLocations game = SetupGameLocations();
            Assert.IsFalse(getEntity(game).CheckIfEntitySharingRoom(game.GetEntity(EntityType.Wumpus)));
            ((DummyEntity)game.GetEntity(EntityType.Player)).TeleportToRoomWrapper(20);
            Assert.IsTrue(getEntity(game).CheckIfEntitySharingRoom(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestCheckIfEntityAdjacent()
        {
            GameLocations game = SetupGameLocations();
            Assert.IsFalse(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).MoveToRoomWrapper(14);
            Assert.IsTrue(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).MoveToRoomWrapper(20);
            Assert.IsFalse(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestAccessibleDistanceToEntity()
        {
            GameLocations game = SetupGameLocationsDirected();
            Assert.AreEqual(3, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(29);
            Assert.AreEqual(3, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(21);
            Assert.AreEqual(2, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(20);
            Assert.AreEqual(0, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(26);
            Assert.AreEqual(1, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(1);
            Assert.AreEqual(2, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestDistanceToEntity()
        {
            GameLocations game = SetupGameLocationsDirected();
            Assert.AreEqual(2, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(29);
            Assert.AreEqual(3, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(21);
            Assert.AreEqual(1, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(20);
            Assert.AreEqual(0, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
        }
    }
}