using System;
using System.Collections.Generic;
using System.IO;
using WumpusCore.Controller;
using WumpusCore.Entity;
using WumpusCore.Topology;
using WumpusCore.GameLocations;
using WumpusCore.Trivia;

namespace WumpusCore.Player
{
    public class Player: Entity.Entity
    {
        /// <summary>
        /// The amount of coins the player currently has.
        /// </summary>
        public int Coins { get; private set; }
        
        /// <summary>
        /// The amount of arrows the player currently has.
        /// </summary>
        public uint Arrows { get; private set; }
        
        /// <summary>
        /// Number of moves made so far
        /// </summary>
        public int TurnsTaken { get; private set; }

        // Whether there is a coin in the hallway out from a given room
        private Dictionary<Directions, bool>[] hallwayCoins;
        
        
        
        /// <summary>
        /// Stores everything to do with the player.
        /// </summary>
        public Player(WumpusCore.Topology.Topology topology, WumpusCore.GameLocations.GameLocations parent, ushort location) 
            : base(topology, parent, location, EntityType.Player)
        {   
            Coins = 0;
            Arrows = 3;

            hallwayCoins = new Dictionary<Directions, bool>[30];
            for (int i = 0; i < topology.RoomCount; i++)
            {
                hallwayCoins[i] = new Dictionary<Directions, bool>();
                for (int j = 0; j < 6; j++)
                {
                    hallwayCoins[i][(Directions)j] = true;
                }
            }
        }
        
        /// <summary>
        /// Gain some coins
        /// </summary>
        /// <param name="coinsGained">Number of coins gained</param>
        public void GainCoins(uint coinsGained)
        {
            Coins += (int)coinsGained;
        }

        /// <summary>
        /// Lose some coins
        /// </summary>
        /// <param name="coinsLost">Number of coins lost</param>
        public void LoseCoins(uint coinsLost)
        {
            Coins -= (int)coinsLost;
        }

        /// <summary>
        /// Gets the type of the room the player is currently in
        /// </summary>
        /// <returns>The type of the room the player is currently in</returns>
        public GameLocations.GameLocations.RoomType GetRoomType()
        {
            return gameLocations.GetRoomAt((ushort)(location - 1));
        }

        /// <summary>
        /// Adds coins
        /// </summary>
        /// <param name="coins">Number of coins to give player</param>
        public void AddCoins(ushort coins)
        {
            if ((int)Coins + coins > ushort.MaxValue)
            {
                Coins = ushort.MaxValue;
                return;
            }
            Coins += coins;
        }
        
        /// <summary>
        /// Moves the player in a certain direction relative to their current position.
        /// </summary>
        /// <param name="directions">The <see cref="Directions"/> direction to move to.</param>
        public void MoveInDirection(Directions direction)
        {
            MoveToRoom(GetRoomInDirection(direction));
            if (CoinRemainingInHallway(direction))
            {
                GainCoins(1);
                hallwayCoins[location][direction] = false;
                hallwayCoins[thisRoom.ExitRooms[direction].Id][direction.GetInverse()] = false;
            }
            TurnsTaken++;
        }

        /// <summary>
        /// Returns whether there is a coin in the hallway
        /// </summary>
        /// <returns>Whether there is a coin in the hallway</returns>
        public bool CoinRemainingInHallway(Directions direction)
        {
            return hallwayCoins[location][direction];
        }
    }
}