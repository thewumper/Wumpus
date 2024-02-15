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
        public void HighScoreCalculations()
        {
            HighScore funny = new HighScore("Crab", 5, 10, 4, true, 1);
            int score = funny.getScore();
            Assert.AreEqual(175, score);
        }

        [TestMethod]
        public void translatingWumpusDeadBool()
        {
            HighScore notFunny = new HighScore("DefaultPlayerName", 1, 0, 15, false, 1);
            bool 
        }
    }
}
