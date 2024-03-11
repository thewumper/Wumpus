using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Security;
using WumpusCore.HighScoreNS;

namespace WumpusTesting
{
    [TestClass]
    public class HighScoreTest
    {
        /// <summary>
        /// Test if constructor makes the object without error
        /// </summary>
        [TestMethod]
        public void TestHighScoreConstructor()
        {
            HighScore testScore = new HighScore("player", 5, 300, 0, false, 2);
            Assert.IsNotNull(testScore);

        }

        /// <summary>
        /// Test if Struct constructs
        /// </summary>
        [TestMethod]
        public void TestStoredHighScoreStruct()
        {
            HighScore.StoredHighScore testStruct = new HighScore.StoredHighScore(47, "player", 15, 5, 4, true, 1);
            Assert.IsNotNull(testStruct);
        }

        /// <summary>
        /// Test if score is calculating correctly
        /// </summary>
        [TestMethod]
        public void ScoreCalculationTest()
        {
            HighScore scoring = new HighScore("player", 27, 4, 3, true, 4);
            int score = scoring.compactScore.score;
            Assert.AreEqual(142, score);
        }
    }

    [TestClass]
    public class SaveFileTests
    {
        /// <summary>
        /// Test if generation for save file works
        /// </summary>
        [TestMethod]
        public void TestSaveFileGeneration()
        {
            SaveFile testSaveFile = new SaveFile("File is made");
            Assert.IsNotNull(testSaveFile);
        }

        /// <summary>
        /// Test if score is put into the file
        /// </summary>
        [TestMethod]
        public void TestHighScoreSaveFile()
        {
            HighScore saveScore = new HighScore("player", 5, 1, 4, true, 5);
            saveScore.storeScoreToFile(saveScore.compactScore);
        }

        /// <summary>
        /// Test if the top ten scores are stored in file
        /// </summary>
        [TestMethod]
        public void TestTopTenScoresSave()
        {
            HighScore saveScore = new HighScore("playing", 8, 10, 4, true, 8);
            saveScore.storeTopTenToFile();
        }

        [TestMethod]
        public void TestRandomTopTen()
        {
            // do a thing here
        }

        private HighScore RandomScore(int index)
        {
            Random rand = new Random();
            bool wumpusDead = false;
            int checkDeath = rand.Next(0, 1);
            if (checkDeath == 1) { wumpusDead = true; }
            HighScore generatedScore = new HighScore(("play" + index), rand.Next(1,20), rand.Next(0,10), rand.Next(0,5), wumpusDead, rand.Next(1,10));
            return generatedScore;
        }
    }
}
