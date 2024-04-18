using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WumpusCore.Entity;
using WumpusCore.LuckyCat;
using WumpusCore.Topology;
using WumpusCore.GameLocations;
using System.Runtime.Remoting.Messaging;
using System.IO;
using WumpusCore.Player;

namespace WumpusTesting
{
	[TestClass]
	public class LuckyCatTests
	{
		private Random rand;
		private Cat cat;
		private Player player;
		public LuckyCatTests()
		{
			rand = new Random();
			GameLocations gameLocations = new GameLocations(30);
			 // Write the string array to a new file named "WriteLines.txt".
			using (StreamWriter outputFile = new StreamWriter("luckyCat.map")) 
            {
                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }
            cat = new Cat(new WumpusCore.Topology.Topology("luckyCat.map"), gameLocations, 0);
		}
		
		[TestMethod]
		// Testing of taming method
		public void LuckyCatRobbery()
		{
			ushort coins = (ushort)(rand.Next(0, 40) + 1);
			ushort coins2 = coins;
			Console.WriteLine("Start Coins: " + coins + ", Checking with: " + coins2);
			
			bool tamed = true;
			cat.Tame(coins);
			Assert.AreEqual(coins2, coins);
			Console.WriteLine(coins + ", " + coins2);
			
			tamed = false;
			if (coins < 20)
			{
				Assert.AreEqual(coins2, coins);
				Assert.AreEqual(false, tamed);
                Console.WriteLine(coins + ", " + coins2);
            }
			else if (coins >= 20)
			{
				Assert.AreEqual((coins2 - 20), coins);
				Assert.AreEqual(true, tamed);
                Console.WriteLine(coins + ", " + coins2);
            }
		}
	}
}