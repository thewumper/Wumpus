namespace WumpusCore.Topology
{
    public class Topology : ITopology
    {
        /// <summary>
        /// Array of rooms
        /// </summary>
        private Room[] rooms;
        
        /// <summary>
        /// Creates topology from filepath to map data
        /// </summary>
        /// <param name="filePath">path</param>
        public Topology(string filePath)
        {
            
        }
        /// <summary>
        /// Creates topology from a folder containing maps and the id of the map
        /// </summary>
        /// <param name="folder">Path to folder containing maps</param>
        /// <param name="mapId">ID of the map</param>
        public Topology(string folder, ushort mapId) : this($"{folder}/map{mapId}.map")
        {
            
        }
        /// <summary>
        /// Returns a room in the cave given and ID
        /// </summary>
        /// <param name="id">ID of the room</param>
        /// <returns></returns>
        public IRoom GetRoom(ushort id)
        {
            return null;
        }
        
        /// <summary>
        /// Internally used to keep track of rooms
        /// </summary>
        private class Room : IRoom
        {
            Room(Directions[] exitDirections, ushort id)
            {
                ExitDirections = exitDirections;
                Id = id;
            }
            
            /// <summary>
            /// Directions of exits in the room
            /// </summary>
            public Directions[] ExitDirections { get; }
            /// <summary>
            /// Connected rooms, in same order as ExitDirections
            /// </summary>
            public IRoom[] ExitRooms { get; }
            /// <summary>
            /// This rooms ID
            /// </summary>
            public ushort Id { get; }
        }
    }


}