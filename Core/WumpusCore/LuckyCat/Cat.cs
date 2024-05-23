using System;
using WumpusCore.Entity;
using WumpusCore.Player;
using WumpusCore.Controller;
using WumpusCore.GameLocations;
using System.Diagnostics.PerformanceData;
using WumpusCore.Topology;

namespace WumpusCore.LuckyCat
{
    public class Cat: Entity.Entity
    {
        /// <summary>
        /// Is the cat tamed
        /// </summary>
        private bool tamed;
        public bool GetTamed()
        {
            return this.tamed;
        }

        public void SetTamed(bool input)
        {
            this.tamed = input;
        }
        /// <summary>
        /// The amount of coins the player has
        /// Used to check for taming cat
        /// </summary>
        public ushort coins;

        /// <summary>
        /// The radius that the cat should be heard from
        /// </summary>
        public const int AudibleMewingRadius = 1;

        public Cat(ITopology topology, GameLocations.GameLocations parent, ushort location) : base(topology, parent, location, EntityType.Cat)
        {
        }

        /// <summary>
        /// Location of cat entity
        /// </summary>
        public int Location;

        public enum catSound
        {
            Silent,
            Mew,
            MewQuiet
        }

        public Cat(Topology.Topology topology, GameLocations.GameLocations parent, ushort location): base(topology, parent, location, EntityType.Cat)
        {
        }

        /// <summary>
        /// Attempts to Tame the Lucky Cat
        /// </summary>
        /// <returns>The state of cat tame, successful or not</returns>
        public void Tame(ushort coins, bool tamed)
        {
            if (!tamed && coins >= 20)
            {
                coins -= 20;
                Console.WriteLine("You tame'd the kitty cat");
                SetTamed(true);
            }
            else if (!tamed && coins < 20 )
            {
                Console.WriteLine("Failed to tame'd the kitty cat.");
                SetTamed(false);
            }
            else if ( tamed )
            {
                Console.WriteLine("Kitty cat already tame'd idoit");
                SetTamed(true);
            }
            else 
            {
                SetTamed(false); 
            }

        }

        /// <summary>
        /// Pets the cat
        /// </summary>
        /// <returns> Meow Sound Effect ID </returns>
        public catSound Pet()
        {
            if (GetTamed() && AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) == 0) 
            { 
            // (You pet the cat, yippee)
            return catSound.Mew;
            }
            
            else { return catSound.Silent; }

        }

        /// <summary>
        /// Plays the mewing audio file if player is nearby the cat
        /// Will depend on audio manager for sound
        /// </summary>
        /// <returns> Cat Noises </returns>
        public catSound Mew()
        {          

            if (AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) < AudibleMewingRadius)
            {
                return catSound.Mew;
            }
            
            else if (AccessibleDistanceToEntity(gameLocations.GetEntity(EntityType.Player)) == AudibleMewingRadius)
            {
                return catSound.MewQuiet;
            }

            else { return catSound.Silent; } 

        }
    }
}