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
			
			cat.SetTamed(true);
			cat.Tame(coins, cat.GetTamed());
			Assert.AreEqual(coins2, coins);
			Console.WriteLine(coins + ", " + coins2);

			cat.SetTamed(false);
			cat.Tame(coins, cat.GetTamed());
			if (coins < 20)
			{
				Assert.AreEqual(coins2, coins);
				Assert.AreEqual(false, cat.GetTamed());
                Console.WriteLine(coins + ", " + coins2);
            }
			else if (coins >= 20)
			{
				Assert.AreEqual((coins2 - 20), coins);
				Assert.AreEqual(true, cat.GetTamed());
                Console.WriteLine(coins + ", " + coins2);
            }
		}

		[TestMethod]
		public void PetTest()
		{
			cat.Tame(10, cat.GetTamed());
			Assert.AreEqual(false, cat.GetTamed());
			cat.Pet();

			cat.Tame(20, cat.GetTamed());
			Assert.AreEqual(true, cat.GetTamed());
			cat.Pet();
		}
	}
}