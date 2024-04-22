using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.GameLocations;
using WumpusCore.Player;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace WumpusTesting
{
    [TestClass]
    public class PlayerTesting
    {
        public PlayerTesting()
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
        
        private Player makePlayer()
        {
            Topology topology = new Topology("test1.map");
            GameLocations gameLocations = new GameLocations(30,0,0,0,0,topology,new Random());
            return new Player(topology, gameLocations, 15);
        }
        
        [TestMethod]
        public void TestCoins()
        {
            Player player = makePlayer();
            Assert.AreEqual(0, player.Coins);
            player.AddCoins(5);
            Assert.AreEqual(5, player.Coins);
            player.MoveInDirection(Directions.North);
            Assert.AreEqual(6, player.Coins);
            player.MoveInDirection(Directions.SouthEast);
            Assert.AreEqual(7, player.Coins);
            player.MoveInDirection(Directions.SouthWest);
            Assert.AreEqual(8, player.Coins);
            player.MoveInDirection(Directions.North);
            Assert.AreEqual(8, player.Coins);
            player.MoveInDirection(Directions.SouthEast);
            Assert.AreEqual(8, player.Coins);
        }

        [TestMethod]
        public void TestArrows()
        {
            Player player = makePlayer();
            player.AddCoins(5);
            Assert.AreEqual(3, player.Arrows);
            player.EarnArrows(GameResult.Win);
            Assert.AreEqual(5, player.Arrows);
            player.EarnArrows(GameResult.Loss);
            Assert.AreEqual(4, player.Coins);
        }
    }
}