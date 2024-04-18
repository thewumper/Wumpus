using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.GameLocations;
using WumpusCore.Topology;

namespace WumpusTesting
{
    [TestClass]
    public class GameLocationsTest
    {
        public GameLocationsTest()
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
        public void TestGameLocationsInitalization()
        {
            GameLocations gl = new GameLocations(30,5,5,5,5,new Topology("test1.map"),new Random());
        }
    }
}