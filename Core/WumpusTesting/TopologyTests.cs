
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Topology;

namespace WumpusTesting
{
    [TestClass]
    public class TopologyTests
    {
        public TopologyTests()
        {

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("test1.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,S");
                }
            }
        }
        
        [TestMethod]
        public void TestTopologyCanBeCreated()
        {
            ITopology topology = new Topology("test1.map");
        }
    }
}