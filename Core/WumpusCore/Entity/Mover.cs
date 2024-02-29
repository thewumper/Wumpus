using System;

namespace WumpusCore.Entity
{
    public class Mover
    {
        // Location index in Topology
        private int location;

        /// <summary>
        /// Gets all room indices adjacent to the room the Mover is currently in
        /// </summary>
        /// <returns>A list of integer indices of the adjacent rooms</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int[] GetAdjacentRooms()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the given room is adjacent to this Mover
        /// </summary>
        /// <param name="roomIndex">The room that might be adjacent</param>
        /// <returns>True if the given room is adjacent, False otherwise</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckIfAdjacent(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to move to the given room, fails if not adjacent
        /// </summary>
        /// <param name="roomIndex">The room to move to</param>
        /// <exception cref="NotImplementedException"></exception>
        public void MoveToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Forces a move to the given room.
        /// </summary>
        /// <param name="roomIndex">The room index to move to</param>
        /// <exception cref="NotImplementedException"></exception>
        public void TeleportToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the index of the room the Mover is in
        /// </summary>
        /// <returns>The index of the room the Mover is in</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetLocaton()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves once in a random direction.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void MoveToRandomAdjacent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Teleports to a random room on the map.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void TeleportToRandom()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns True if the given Entity is in the same room as this Mover, False otherwise
        /// </summary>
        /// <param name="e">The entity that might be in the same room</param>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckIfEntitySharingRoom(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the given Entity is adjacent to this Mover 
        /// </summary>
        /// <param name="e">The entity that might be adjacent to this Mover</param>
        /// <returns>True if the given Entity is in a room adjacent to the current room, False if otherwise, even if they are in the same room.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CheckIfEntityAdjacent(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds how many moves it would take for this Mover to move to the given roomIndex
        /// </summary>
        /// <param name="roomIndex">The room to navigate to</param>
        /// <returns>Distance (in accessible rooms) to the given room</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AccessibleDistanceToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds how many moves it would take for this Mover to move to the given Entity
        /// </summary>
        /// <param name="e">The Entity to navigate to</param>
        /// <returns>Distance (in accessible rooms) to the given Entity</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AccessibleDistanceToEntity(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the distance in rooms to the given roomIndex, ignoring walls, doors, and obstacles.
        /// </summary>
        /// <param name="roomIndex">The room to find the distance to</param>
        /// <returns>The distance to the given room</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int DistanceToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Finds the distance in rooms to the given Entity, ignoring walls, doors, and obstacles.
        /// </summary>
        /// <param name="e">The Entity to find the distance to</param>
        /// <returns>The distance to the given Entity</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int DistanceToEntity(Entity e)
        {
            throw new NotImplementedException();
        }
    }
}