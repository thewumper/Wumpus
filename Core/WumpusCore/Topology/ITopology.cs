using System;

namespace WumpusCore.Topology
{
    /// <summary>
    /// A object that can represents the way rooms connect to each other
    /// </summary>
    public interface ITopology
    {
        /// <summary>
        /// Get a room from an ID
        /// </summary>
        /// <param name="id">the room ID. Zero-indexed</param>
        /// <returns>The room with the given ID</returns>
        IRoom GetRoom(ushort id);


        IRoom[] GetRooms();

        ushort RoomCount { get; }
        /// <summary>
        /// Finds the distance in room movements between two given room indices, ignoring walls, doors, and obstacles.
        /// </summary>
        /// <param name="roomIndexA">The first room, the start of the search</param>
        /// <param name="roomIndexB">The second room, the end point of the search</param>
        /// <param name="getConnections">Takes in an IRoom and returns an array of all rooms that should be navigable to. Use to define which paths are considered to route through.</param>
        /// <returns>The distance in movements between the two rooms</returns>
        int DistanceBetweenRooms(ushort roomIndexA, ushort roomIndexB, Func<IRoom, IRoom[]> getConnections);
    }
}