using System.Collections.Generic;

namespace WumpusCore.Topology
{
    /// <summary>
    /// Represents a room on a map
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// Get all of the available exit directions from this room
        /// </summary>
        Directions[] ExitDirections { get; }
        /// <summary>
        /// Get an exit room from a direction
        /// </summary>
        Dictionary<Directions, IRoom> ExitRooms { get; }
        /// <summary>
        /// Get the room id (1-30)
        /// </summary>
        ushort Id { get; }
    }
}