using System;
using System.IO;
namespace WumpusCore.Player
{
    public class Player
    {
        /// <summary>
        /// The room the player is currently in.
        /// </summary>
        public int position { get; private set; }

        /// <summary>
        /// The path of the player's sprite.
        /// </summary>
        public string spritePath { get; private set; }

        /// <summary>
        /// The amount of coins the player currently has.
        /// </summary>
        public uint coins { get; set; }
        /// <summary>
        /// The amount of arrows the player currently has.
        /// </summary>
        public uint arrows { get; set; }

        public Player()
        {
            coins = 0;
            arrows = 0;

            spritePath = "";

            if (!File.Exists(spritePath))
            {
                throw new FileNotFoundException();
            }
        }
    }
}