namespace WumpusCore.Entity
{
    /// <summary>
    /// An interface to be implemented by all entities (Game elements that move). Required for Mover to function.
    /// All Entities should contain Movers.
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
        /// <summary>
        /// Gets the location of this entity.
        /// </summary>
        /// <returns>The location of the current room, in Topology room index.</returns>
        int GetLocation();
    }
}