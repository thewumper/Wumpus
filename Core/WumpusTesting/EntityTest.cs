using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Entity;
using WumpusCore.GameLocations;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace WumpusTesting
{
    [TestClass] 
    public class EntityTest
    {
        public Trivia makeTrivia()
        {
            string path = Path.GetFullPath("..\\..\\..\\..\\Unity\\Assets\\Trivia\\Questions.json");
            Trivia trivia = new Trivia(path);
            if (trivia.Count == 0)
            {
                throw new Exception("THERE ARE NO QUESTIONS HERE!");
            }
            return trivia;
        }
        
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
            Trivia trivia = makeTrivia();
            GameLocations game = new GameLocations(30,0,0,0,0,topology,new Random(), trivia);
            game.AddEntity(new DummyEntity(topology, game, EntityType.Player));
            game.AddEntity(new Entity(topology, game, 19, EntityType.Wumpus));
            return game;
        }
        
        public GameLocations SetupGameLocationsDirected()
        {
            Topology topology = new Topology("entitytest2.map");
            Trivia trivia = makeTrivia();
            GameLocations game = new GameLocations(30,0,0,0,0,topology,new Random(), trivia);
            game.AddEntity(new DummyEntity(topology, game, EntityType.Player));
            game.AddEntity(new Entity(topology, game, 19, EntityType.Wumpus));
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
            ushort[] expected = new ushort[] { 8, 9, 15, 20, 13, 7 };
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
            ushort[] expected = new ushort[] { 8, 9, 15, 20, 13, 7 };
            ushort[] actual = getEntity(game).GetAccessibleRooms();
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
            Assert.AreEqual(expected[4], actual[4]);
            Assert.AreEqual(expected[5], actual[5]);
            Assert.AreEqual(6, actual.Length);
            
            game = SetupGameLocationsDirected();
            expected = new ushort[] { 8, 9, 15, 20, 7 };
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
            getEntity(game).MoveToRoomWrapper(9);
            getEntity(game).MoveToRoomWrapper(16);
            Assert.AreEqual(16, getEntity(game).location);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { getEntity(game).MoveToRoomWrapper(0); });
        }
        
        [TestMethod]
        public void TestTeleportToRoom()
        {
            GameLocations game = SetupGameLocations();
            getEntity(game).TeleportToRoomWrapper(0);
            Assert.AreEqual(0, getEntity(game).location);
            getEntity(game).TeleportToRoomWrapper(4);
            Assert.AreEqual(4, getEntity(game).location);
            getEntity(game).TeleportToRoomWrapper(14);
            Assert.AreEqual(14, getEntity(game).location);
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
            ((DummyEntity)game.GetEntity(EntityType.Player)).TeleportToRoomWrapper(19);
            Assert.IsTrue(getEntity(game).CheckIfEntitySharingRoom(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestCheckIfEntityAdjacent()
        {
            GameLocations game = SetupGameLocations();
            Assert.IsFalse(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).MoveToRoomWrapper(13);
            Assert.IsTrue(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).MoveToRoomWrapper(19);
            Assert.IsFalse(getEntity(game).CheckIfEntityAdjacent(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestAccessibleDistanceToEntity()
        {
            GameLocations game = SetupGameLocationsDirected();
            Assert.AreEqual(3, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(28);
            Assert.AreEqual(3, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(20);
            Assert.AreEqual(2, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(19);
            Assert.AreEqual(0, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(25);
            Assert.AreEqual(1, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(0);
            Assert.AreEqual(2, getEntity(game).AccessibleDistanceToEntity(game.GetEntity(EntityType.Wumpus)));
        }
        
        [TestMethod]
        public void TestDistanceToEntity()
        {
            GameLocations game = SetupGameLocationsDirected();
            Assert.AreEqual(2, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(28);
            Assert.AreEqual(3, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(20);
            Assert.AreEqual(1, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
            getEntity(game).TeleportToRoomWrapper(19);
            Assert.AreEqual(0, getEntity(game).DistanceToEntity(game.GetEntity(EntityType.Wumpus)));
        }
    }
}