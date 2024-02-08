using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WumpusCore.Wumpus
{
    internal class Wumpus
    {
        /// <summary>
        /// Current Room Wumpus is in
        /// </summary>
        public ushort Position { get; private set; }
        /// <summary>
        /// rather the wumpus was killed
        /// </summary>
        public bool isDead { get; private set; }
        public Wumpus()
        {
            Position = 0;
            isDead = false;
        }
        /// <summary>
        /// Moves the wumpus randomly
        /// </summary>
        public void move(int maxMove, Random rand)
        { 
            throw new NotImplementedException();
        }
        /// <summary>
        /// mods state and starts a battle changing state stuff
        /// </summary>
        public void startBattle()
        {
            throw new NotImplementedException();
        }
    }
}