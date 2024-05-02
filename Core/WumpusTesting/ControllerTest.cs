using System;
using System.Collections.Generic;
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
            using (StreamWriter outputFile = new StreamWriter("./map0.wmp"))
            {

                outputFile.WriteLine("N");
                for (int i = 0; i < 29; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }

            using (StreamWriter outputFile = new StreamWriter("./questions.json"))
            {
                outputFile.WriteLine("[{\"question\": \"Which is right\", choices : [\"correct\",\"wrong\",\"wrong\",\"wrong\"],\"answer\": 0}]");
            }

            // This will just create it at global controller which is what we want. Resharper doesn't like this, but it's fine
            // ReSharper disable once ObjectCreationAsStatement
            new Controller("./questions.json","./",0);
        }

        [TestMethod]
        public void TestGlobalController()
        {
            Assert.AreEqual(Controller.GlobalController, Controller.GlobalController);
        }
    }
}