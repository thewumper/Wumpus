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
        /// Get an exit room from a direction. Only rooms that can be reached through a door
        /// </summary>
        Dictionary<Directions, IRoom> ExitRooms { get; }
        /// <summary>
        /// Get an adjacent room from a direction. Includes rooms that are bordered by walls.
        /// </summary>
        Dictionary<Directions, IRoom> AdjacentRooms { get; }
        /// <summary>
        /// Get the room id (1-30)
        /// </summary>
        ushort Id { get; }
    }
}