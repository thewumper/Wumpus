using System;

// Everything that changes state should be protected
namespace WumpusCore.Entity
{
    /// <summary>
    /// Basic class for all entities to extend from. Contains all necessary movement options.
    /// </summary>
    public class Entity
    {
        // Entity's type
        public readonly EntityType Type;
        
        // Link to the GameLocations that spawned it
        private GameLocations.GameLocations gameLocations;
        
        // Location index in Topology
        public int location { get; protected set; }

        /// <summary>
        /// Create an Entity. Generic constructor that should never be run on its own.
        /// </summary>
        /// <param name="parent">The GameLocations object that spawned this Entity</param>
        /// <param name="location">Starting topology room id</param>
        /// <param name="entityType">Type of this Entity</param>
        public Entity(GameLocations.GameLocations parent, ushort location, EntityType entityType)
        {
            this.Type = entityType;
            this.location = location;
            this.gameLocations = parent;
        }

        /// <summary>
        /// Gets all room indices adjacent to the room the Entity is currently in
        /// </summary>
        /// <returns>A list of integer indices of the adjacent rooms</returns>
        public int[] GetAdjacentRooms()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the given room is adjacent to this Entity
        /// </summary>
        /// <param name="roomIndex">The room that might be adjacent</param>
        /// <returns>True if the given room is adjacent, False otherwise</returns>
        public bool CheckIfAdjacent(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to move to the given room, fails if not adjacent
        /// </summary>
        /// <param name="roomIndex">The room to move to</param>
        protected void MoveToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Forces a move to the given room.
        /// </summary>
        /// <param name="roomIndex">The room index to move to</param>
        protected void TeleportToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves once in a random direction.
        /// </summary>
        protected void MoveToRandomAdjacent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Teleports to a random room on the map.
        /// </summary>
        protected void TeleportToRandom()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns True if the given Entity is in the same room as this Entity, False otherwise
        /// </summary>
        /// <param name="e">The entity that might be in the same room</param>
        public bool CheckIfEntitySharingRoom(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the given Entity is in a room that is adjacent to the room this Entity is in. (distance == 1)
        /// </summary>
        /// <param name="e">The entity that might be adjacent to this Entity</param>
        /// <returns>True if the given Entity is in a room adjacent to the current room, False if otherwise, even if they are in the same room.</returns>
        public bool CheckIfEntityAdjacent(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds how many moves it would take for this Entity to move to the given roomIndex
        /// </summary>
        /// <param name="roomIndex">The room to navigate to</param>
        /// <returns>Distance (in accessible rooms) to the given room</returns>
        public int AccessibleDistanceToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds how many moves it would take for this Entity to move to the given Entity
        /// </summary>
        /// <param name="e">The Entity to navigate to</param>
        /// <returns>Distance (in accessible rooms) to the given Entity</returns>
        public int AccessibleDistanceToEntity(Entity e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the distance in rooms to the given roomIndex, ignoring walls, doors, and obstacles.
        /// </summary>
        /// <param name="roomIndex">The room to find the distance to</param>
        /// <returns>The distance to the given room</returns>
        public int DistanceToRoom(int roomIndex)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Finds the distance in rooms to the given Entity, ignoring walls, doors, and obstacles.
        /// </summary>
        /// <param name="e">The Entity to find the distance to</param>
        /// <returns>The distance to the given Entity</returns>
        public int DistanceToEntity(Entity e)
        {
            throw new NotImplementedException();
        }
    }
}