using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        public int Arrows { get; private set; }
        
        /// <summary>
        /// Number of moves made so far
        /// </summary>
        public int TurnsTaken { get; private set; }

        /// <summary>
        /// Player's health
        /// </summary>
        public int Health;

        /// <summary>
        /// Limit of player's health
        /// </summary>
        public int HealthMax;
        
        /// <summary>
        /// Stores everything to do with the player.
        /// </summary>
        public Player(ITopology topology, WumpusCore.GameLocations.GameLocations parent, ushort location)
            : base(topology, parent, location, EntityType.Player)
        {   
            Coins = 0;
            Arrows = 3;
            Health = 100;
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
        public RoomType GetRoomType()
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
                gameLocations.hallwayCoins[location][direction] = false;
                gameLocations.hallwayCoins[thisRoom.ExitRooms[direction].Id][direction.GetInverse()] = false;
            }
            TurnsTaken++;
        }

        /// <summary>
        /// Returns whether there is a coin in the hallway
        /// </summary>
        /// <returns>Whether there is a coin in the hallway</returns>
        public bool CoinRemainingInHallway(Directions direction)
        {
            return gameLocations.hallwayCoins[location][direction];
        }

        /// <summary>
        /// Whether trivia can be played in a room to earn items or knowledge
        /// </summary>
        public bool TriviaAvailable
        {
            get
            {
                return gameLocations.GetTriviaAvailable(location);
            }
        }

        /// <summary>
        /// Returns the trivia hint in the given hallway from Player's current position
        /// </summary>
        /// <param name="direction">The direction of the hallway from the current position</param>
        /// <returns>The question and answer in the hallway</returns>
        public AnsweredQuestion GetHallwayHint(Directions direction)
        {
            return gameLocations.hallwayTrivia[location][direction];
        }

        /// <summary>
        /// Earn arrows by answering trivia questions (3, 2)
        /// Run after trivia is complete
        /// </summary>
        /// <param name="triviaOutcome">The outcome of the preceding trivia game</param>
        public void EarnArrows(GameResult triviaOutcome)
        {
            if (triviaOutcome == GameResult.Win)
            {
                Arrows += 2;
                gameLocations.SetTriviaRemaining(location, false);
            }
            else if (triviaOutcome == GameResult.Loss)
            {
                LoseCoins(1);
                gameLocations.SetRoom(location, RoomType.Acrobat);
                gameLocations.SetTriviaRemaining(location, false);
            }
            else
            {
                throw new ArgumentException("Game still in progress!");
            }
        }

        /// <summary>
        /// Earn knowledge by answering trivia questions (3, 2)
        /// Run after trivia is complete
        /// </summary>
        /// <param name="triviaOutcome">The outcome of the preceding trivia game</param>
        public void EarnSecret(GameResult triviaOutcome)
        {
            if (triviaOutcome == GameResult.Win)
            {
                Controller.Controller.GlobalController.GenerateSecret();
            } 
            else if (triviaOutcome == GameResult.Loss)
            {
                LoseCoins(1);
                gameLocations.SetRoom(location, RoomType.Acrobat);
                gameLocations.SetTriviaRemaining(location, false);
            }
            else
            {
                throw new ArgumentException("Game still in progress!");
            }
        }
    }
}