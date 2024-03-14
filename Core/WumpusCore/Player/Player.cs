using System;
using System.IO;
using WumpusCore.Topology;

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

        /// <summary>
        /// Moves the player in a certain direction relative to their current position.
        /// </summary>
        /// <param name="topology">The <see cref="ITopology"/> topology opject used to find the <see cref="IRoom"/> room to move to.</param>
        /// <param name="directions">The <see cref="Directions"/> direction to move to.</param>
        public void MoveInDirection(ITopology topology, Directions directions)
        {
            Console.WriteLine("Made it #2");
            MoveTo(topology.GetRoom(position).ExitRooms[directions].Id);
        }
    }
}