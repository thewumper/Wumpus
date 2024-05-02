using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.GameLocations;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace WumpusTesting
{
    [TestClass]
    public class GameLocationsTest
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
            Trivia trivia = makeTrivia();
            GameLocations gl = new GameLocations(30,5,5,5,5,new Topology("test1.map"),new Random(), trivia);
            gl.GetRoomAt(0);
        }
    }
}