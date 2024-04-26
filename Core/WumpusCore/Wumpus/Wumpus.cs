using System;
using WumpusCore.Entity;
using WumpusCore.Topology;

namespace WumpusCore.Wumpus
{
    public class Wumpus: Entity.Entity
    {
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
        public Wumpus(ITopology topology, GameLocations.GameLocations gameLocations): base(topology, gameLocations, 0, EntityType.Wumpus)
        {
            location = gameLocations.GetEmptyRoom();

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