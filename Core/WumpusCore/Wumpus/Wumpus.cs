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
        public int Position { get; private set; }
        /// <summary>
        /// Current state Wumpus is in
        /// </summary>
        /*
        0: sleep
        1: moving
        2: combat
        3: dead
         */
        public int State { get; private set; }
        public Wumpus()
        {
            Position = 0;
            State = 0;
        }
        /// <summary>
        /// Moves the wumpus randomly
        /// </summary>
        public void move(int maxMove)
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
