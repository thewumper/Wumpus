using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WumpusCore.HighScoreNS;

namespace WumpusTesting
{
    [TestClass]
    public class HighScoreTest
    {
        [TestMethod]
        public void TestHighScoreConstructor()
        {
            HighScore testScore = new HighScore("pescado", 5, 300, 0, false, 2);
            Assert.IsNotNull(testScore);

        }

        [TestMethod]
        public void TestStoredHighScoreStruct()
        {
            HighScore.StoredHighScore testStruct = new HighScore.StoredHighScore(47, "player", 15, 5, 4, true, 1);
            Assert.IsNotNull(testStruct);
        }

        [TestMethod]
        public void ScoreCalculationTest()
        {
            HighScore scoring = new HighScore("crab", 27, 4, 3, true, 4);
            int score = scoring.compactScore.score;
            Assert.AreEqual(142, score);
        }
        
    }
}
