using System;
using WumpusCore.Entity;
using WumpusCore.Player;
using WumpusCore.Controller;
using WumpusCore.GameLocations;
using System.Diagnostics.PerformanceData;

namespace WumpusCore.LuckyCat
{
    public class Cat: Entity.Entity
    {
        /// <summary>
        /// Is the cat tamed
        /// </summary>
        private bool tamed;

        /// <summary>
        /// The amount of coins the player has
        /// Used to check for taming cat
        /// </summary>
        public ushort coins;

        /// <summary>
        /// The radius that the cat should be heard from
        /// </summary>
        public const int AudibleMewingRadius = 2;

        public int Location;

        /// <summary>
        /// Attempts to Tame the Lucky Cat
        /// </summary>
        /// <returns>The state of cat tame, successful or not</returns>
        public bool Tame()
        {
            if (!tamed && coins >= 20)
            {
                coins = 0;
                // (Taming success message)
                return true;
            }
            else if (!tamed && coins < 20 )
            {
                coins = 0;
                // (Taming failure message)
                return false;
            }
            else if ( tamed )
            {
                // (Cat already tamed)
                return true;
            }
            else 
            { 
                return false; 
            }

        }

        /// <summary>
        /// Pets the cat
        /// </summary>
        public void Pet()
        {
            // (You pet the cat, yippee)
        }
        
        /// <summary>
        /// Plays the mewing audio file if player is nearby the cat
        /// Will depend on audio manager for sound
        /// </summary>
        /// <returns> Audio id </returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Mew()
        {
/*          
            if (DistanceToEntity() < AudibleMewingRadius) 
            {
                return 69;
            }
            
            else if (DistanceToEntity() == AudibleMewingRadius)
            {
                return 70;
            }

            else { return 0; }
*/ 
            throw new NotImplementedException();
        }
    }
}