using System.Collections.Generic;
using WumpusCore.Topology;

namespace WumpusCore.Controller
{
    /// <summary>
    /// A struct representing a directional hint which is a direction and the hazards that are in that direction
    /// </summary>
    public struct DirectionalHint
    {
        /// <summary>
        /// The direction that the hint(s) are in
        /// </summary>
        public Directions Direction;

        /// <summary>
        /// The list of hazards that the player is hinted about
        /// </summary>
        public List<RoomAnomaly> Hazards;

        /// <summary>
        /// Constructs a directionalhint
        /// </summary>
        /// <param name="hazards">The list of hazards that are in that direction</param>
        /// <param name="direction">The direction that those hazards are in from the player's room</param>
        public DirectionalHint(List<RoomAnomaly> hazards, Directions direction)
        {
            Hazards = hazards;
            Direction = direction;
        }
    }
}
