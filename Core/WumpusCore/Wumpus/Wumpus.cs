using System;
using WumpusCore.Entity;

namespace WumpusCore.Wumpus
{
    public class Wumpus: Entity.Entity
    {
        /// <summary>
        /// Current Room Wumpus is in.  
        /// Better description coming soon
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// state of wumpus.  
        /// Better description coming soon
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
        /// <summary>
        /// Constructs the Wumpus.  
        /// Better description coming soon
        /// </summary>
        public Wumpus(Topology.Topology topology, GameLocations.GameLocations gameLocations): base(topology, gameLocations, 0, EntityType.Wumpus)
        {
            Position = 0;
            State currentState = State.Sleeping;
        }
        /// <summary>
        /// Moves the wumpus into a connected room randomly a number of times depending on the state.  
        /// Better description coming soon
        /// </summary>
        public void move(Random rand)
        { 
            throw new NotImplementedException();
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