using System;
using WumpusCore.Entity;
using WumpusCore.Player;
using WumpusCore.Controller;
using WumpusCore.GameLocations;
using System.Diagnostics.PerformanceData;
using WumpusCore.Topology;

namespace WumpusCore.LuckyCat
{
    public class Cat : Entity.Entity
    {
        /// <summary>
        /// Is the cat tamed
        /// </summary>
        public bool tamed { get; private set; }

        /// <summary>
        /// The radius that the cat should be heard from
        /// </summary>
        public const int AudibleMewingRadius = 1;

        public Cat(ITopology topology, GameLocations.GameLocations parent, ushort location) : base(topology, parent, location, EntityType.Cat)
        {
        }

        /// <summary>
        /// Attempts to Tame the Lucky Cat
        /// </summary>
        /// <returns>The state of cat tame, successful or not</returns>
        public bool Tame(int coinInput)
        {
            if (coinInput>gameLocations.GetPlayer().Coins)
            {
                throw new InvalidOperationException("You can't tame a cat with more coins than you have");
            }

            if (tamed)
            {
                throw new InvalidOperationException("You have already tamed the cat");
            }

            // The cat cannot be guaranteed to be tamed, but instead it uses
            // a modified sigmoid function to determine your chance of taming
            // the cat as a function of the coins you put in

            // The function is 1/(1+e^{-x/2+3})*100 (%)
            // there isn't anything specific about that function
            // other than it looks decent in Desmos
            // 0 coins gives a ~4.7% chance to tame
            // 10 coins gives a ~90% change to tame
            // 18+ coins gives a 99% change to tame
            // You just won't ever have a 100% chance of taming
            gameLocations.GetPlayer().LoseCoins((uint) coinInput);

            int threshold = Controller.Controller.Random.Next(0, 100);
            int value = (int)(1 / (1 + Math.Pow(Math.E, -coinInput / 2.0 + 3)) * 100);

            if (value >= threshold)
            {
                tamed = true;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Pets the cat
        /// </summary>
        /// <returns> Meow Sound Effect ID </returns>
        public int Pet()
        {
            if (tamed && AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) == 0) 
            { 
            // (You pet the cat, yippee)
            return 70;
            }
            
            else { return 0; }

        }

        /// <summary>
        /// Plays the mewing audio file if player is nearby the cat
        /// Will depend on audio manager for sound
        /// </summary>
        public int Mew()
        {          

            if (AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) < AudibleMewingRadius)
            {
                return 69;
            }
            
            else if (AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) == AudibleMewingRadius)
            {
                return 70;
            }

            else { return 0; } 

        }
    }
}