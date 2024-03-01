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
        /// <param name="id">the room ID</param>
        /// <returns>The room with the given ID</returns>
        IRoom GetRoom(ushort id);

    }
}