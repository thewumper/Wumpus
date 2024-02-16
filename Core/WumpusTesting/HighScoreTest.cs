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
        public void TestStoredHighScoreStruct()
        {
            HighScore testScore = new HighScore("player", 15, 5, 4, true, 1);
        }
    }
}
