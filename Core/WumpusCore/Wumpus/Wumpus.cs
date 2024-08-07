﻿using System;
using System.Runtime.InteropServices;
using WumpusCore.Entity;
using WumpusCore.Topology;

namespace WumpusCore.Wumpus
{
    public class Wumpus: Entity.Entity
    {
        /// <summary>
        /// Current state of Wumpus
        /// </summary>
        public enum State
        {
            Sleeping,
            FleeingArrow,
            FleeingCombat,
            Combat,
            Dead
        }
        
        private State WumpState;
        
        /// <summary>
        /// Constructs the Wump.
        /// </summary>
        public Wumpus(ITopology topology, GameLocations.GameLocations parent, ushort location) : base(topology, parent, location, EntityType.Wumpus)
        {
            WumpState = State.Sleeping;
        }
        
        /// <summary>
        /// Moves the Wump into a connected room randomly a number of times depending on the state.
        /// </summary>
        /// <param name="Random">Represents a pseudo-random number generator, which is a device that produces a sequence of numbers that meet certain statistical requirements for randomness</param>
        public void move(Random Random)
        {
            int maxMoves;
            if ((int)WumpState == 1)
            {
                maxMoves = 1;
            }
            else if ((int)WumpState == 2)
            {
                maxMoves = 4;
            }
            else
            {
                maxMoves = 0;
            }
                for (int i = 0; i<maxMoves; i++){
                if ((maxMoves==4&&i<=1)||Random.Next(0,2)<=1) {
                    MoveToRandomAdjacent();
                }
            }
            WumpState = State.Sleeping;
        }
        
        /// <summary>
        /// Sets the state of the Wump
        /// </summary>
        /// <param name="newState">The new state of the Wump.</param>
        public void setState(State newState)
        {
            this.WumpState = (State)newState;
        }
        /// <summary>
        /// Returns a Wump state
        /// </summary>
        /// <returns>Wump State</returns>
        public State getWumpState()
        {
            return WumpState;
        }
    }
}