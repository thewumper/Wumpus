using System;
using System.IO;
namespace WumpusCore.Player
{
    public class Player
    {
        /// <summary>
        /// The room the player is currently in.
        /// </summary>
        private ushort position;
        /// <summary>
        /// The room the player is currently in.
        /// </summary>
        public ushort Position
        {
            get { return position; }
        }

        /// <summary>
        /// The amount of coins the player currently has.
        /// </summary>
        public ushort coins;
        
        /// <summary>
        /// The amount of arrows the player currently has.
        /// </summary>
        public ushort arrows;
        
        /// <summary>
        /// Stores everything to do with the player.
        /// </summary>
        /// <exception cref="FileNotFoundException">When there is no sprite stored at <see cref="spritePath"/>.</exception>
        public Player()
        {   
            coins = 0;
            arrows = 0;
        }
        
        /// <summary>
        /// Moves the Player to the room at <c>target</c>.
        /// </summary>
        /// <param name="target">The position to move the player to.</param>
        public void MoveTo(ushort target)
        {
            position = target;
        }
    }
}