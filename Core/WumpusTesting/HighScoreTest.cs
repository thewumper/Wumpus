using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
          
            HighScore.StoredHighScore compScore = testScore.compactScore;
            Assert.AreEqual("player", compScore.playerName);
            Assert.AreEqual(5, compScore.numTurns);
            Assert.AreEqual(300, compScore.goldLeft);
            Assert.AreEqual(0, compScore.arrowsLeft);
            Assert.AreEqual(false, compScore.isWumpusDead);
            Assert.AreEqual(2, compScore.mapUsed);
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
            SaveFile testSaveFile = new SaveFile("File is made", true);
            string saveText = testSaveFile.ReadFile(false);
            Assert.IsNotNull(testSaveFile);
            Assert.AreEqual("File is made", saveText);
        }

        /// <summary>
        /// Test if score is put into the file
        /// </summary>
        [TestMethod]
        public void TestHighScoreSaveFile()
        {
            SaveFile file = new SaveFile("[:_]", false);
            HighScore saveScore = new HighScore(file.path,"player", 5, 1, 4, true, 5);
            saveScore.StoreScoreToFile(saveScore.compactScore);
            string path = saveScore.savePath;
            SaveFile usedFile = new SaveFile(false, path);
            string info = usedFile.ReadFile(false);
            Assert.AreEqual(saveScore.compactScore, saveScore.ConvertStringToStoredHighScore(info));
        }

        /// <summary>
        /// Test if the top ten scores are stored in file
        /// </summary>
        [TestMethod]
        public void TestTopTenScoresSave()
        {
            SaveFile testHeadFile = new SaveFile("pesacdo", false);
            string pathToUse = testHeadFile.path;
            HighScore saveScore = new HighScore(pathToUse, "playing", 8, 10, 4, true, 8);
            saveScore.StoreTopTenToFile();
        }

        /// <summary>
        /// Generate random scores and have
        /// the top ten high scores list
        /// update for each from the
        /// head file generated at the start
        /// </summary>
        [TestMethod]
        public void TestRandomTopTen()
        {
            SaveFile testHeadFile = new SaveFile("¯\\_(ツ)_/¯", false);
            string pathToUse = testHeadFile.path;
            HighScore lastScore = null;

            for (int i = 0; i < 10; i++)
            {
                HighScore randScore = RandomScore(i, pathToUse);
                randScore.StoreTopTenToFile();
                Console.WriteLine();
                if (i == 9)
                {
                    lastScore = randScore;
                }
            }
            if (lastScore != null)
            {
                List<HighScore.StoredHighScore> topTen = lastScore.GetTopTen();
                for (int i = 0; i < topTen.Count;i++)
                {
                    Console.WriteLine(topTen[i].ToString());
                }
                Assert.AreEqual(10, topTen.Count);
            }
        }

        /// <summary>
        /// Generate random variables to
        /// create a high score object
        /// </summary>
        /// <param name="index"> multiplier for seed for random constructor </param>
        /// <param name="path"> the directory to send to highscore object alternate constructor </param>
        /// <returns>  </returns>
        private HighScore RandomScore(int index, string path)
        {
            Random rand = new Random(index * (int)DateTime.Now.Ticks);
            bool wumpusDead = false;
            int checkDeath = rand.Next(0, 1);
            if (checkDeath == 1) { wumpusDead = true; }
            HighScore generatedScore = new HighScore(path, ("play" + index), rand.Next(1,20), rand.Next(0,10), rand.Next(0,5), wumpusDead, rand.Next(1,10));
            return generatedScore;
        }
    }
}
