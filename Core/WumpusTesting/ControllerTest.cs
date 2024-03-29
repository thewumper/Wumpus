using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Controller;
using WumpusCore.Topology;

namespace WumpusTesting
{
    [TestClass]
    public class ControllerTest
    {
        public ControllerTest()
        {
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("map0.wmp"))
            {
                outputFile.WriteLine("N,NE,SE,S,SW,NW");
                outputFile.WriteLine("N");
            }

            using (StreamWriter outputFile = new StreamWriter("./questions.json"))
            {
                outputFile.WriteLine("[{\"question\": \"Which is right\", choices : [\"correct\",\"wrong\",\"wrong\",\"wrong\"],\"answer\": 0}]");
            }



        }

        [TestMethod]
        public void TestGettingARoom()
        {
            Assert.AreEqual(
                new Controller("./questions.json","./",0).GetRoom(1).ExitDirections,
                new []
                {
                    Directions.North ,
                    Directions.NorthEast,
                    Directions.SouthEast,
                    Directions.South,
                    Directions.SouthEast,
                    Directions.NorthWest,
                }
            );

            Assert.AreEqual(
                new Controller("./questions.json","./",0).GetRoom(2).ExitDirections,
                new [] { Directions.North }
            );
        }
    }
}