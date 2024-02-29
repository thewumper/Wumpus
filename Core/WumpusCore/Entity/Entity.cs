namespace WumpusCore.Entity
{
    /// <summary>
    /// An interface to be implemented by all entities (Game elements that move). Required for Mover to function.
    /// All Entities should contain Movers.
    /// </summary>
    public interface Entity
    {
        /// <summary>
        /// Gets the location of this entity.
        /// </summary>
        /// <returns>The location of the current room, in Topology room index.</returns>
        int GetLocation();
    }
}