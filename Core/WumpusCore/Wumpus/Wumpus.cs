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
        /// state of wumups
        /// </summary>
        public enum State
        {
            Sleeping,
            Wake,
            Wandering,
            Combat,
            FleeingArrow,
            FleeingCombat,
            Dead
        }
        public Wumpus()
        {
            Position = 0;
            State currentState = State.Sleeping;
        }
        /// <summary>
        /// Moves the wumpus randomly
        /// </summary>
        public void move(Random rand)
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