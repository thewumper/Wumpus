using System;
using WumpusCore.Entity;

namespace WumpusCore.Wumpus
{
        /// <summary>
        /// state of wumpus.  
        /// Better description coming soon
        /// </summary>
        public enum State
        { 
            Sleeping,
            FleeingArrow,
            FleeingCombat,
            Combat,
            Dead
        }
    public class Wumpus: Entity.Entity
    {
        private State WumpusState;
        /// <summary>
        /// Constructs the Wumpus.  
        /// </summary>
        public Wumpus(Topology.Topology topology, GameLocations.GameLocations parent, ushort location, EntityType entityType) : base(topology, parent, location, entityType)
        {
            WumpusState = State.Sleeping;
        }
        /// <summary>
        /// Moves the wumpus into a connected room randomly a number of times depending on the state.
        /// </summary>
        /// <param name="Random">Represents a pseudo-random number generator, which is a device that produces a sequence of numbers that meet certain statistical requirements for randomness</param>
        public void move(Random Random)
        {
            int maxMoves;
            if ((int)WumpusState == 1)
            {
                maxMoves = 1;
            }
            else if ((int)WumpusState == 2)
            {
                maxMoves = 4;
            }
            else
            {
                maxMoves = 0;
            }
                for (int i = 0; i<maxMoves; i++){
                if ((maxMoves==4&&i <= 1)||Random.Next(0,2)==1) {
                    MoveToRandomAdjacent();
                }
            }
            WumpusState = State.Sleeping;
        }
        /// <summary>
        /// Changes state and starts minigame, then changes state again based on result of battle.  
        /// Better description coming soon
        /// </summary>
        public void startBattle()
        {
            throw new NotImplementedException();
        }   
    }
}