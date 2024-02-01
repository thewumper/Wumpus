namespace WumpusCore.Topology
{
    public class Topology : ITopology
    {
        private Topology(string filePath)
        {
            
        }

        private Topology(string folder, ushort mapId) : this($"{folder}/map{mapId}.map")
        {
            
        }
        
        class Room : IRoom
        {
            Room(Directions[] exitDirections, ushort id)
            {
                ExitDirections = exitDirections;
                Id = id;
            }
            
            
            public Directions[] ExitDirections { get; }
            public IRoom[] ExitRooms { get; }
            public ushort Id { get; }
        }
        
        public IRoom GetRoom(ushort id)
        {
            return null;
        }

        public ITopology Create(string folder, ushort mapId)
        {
            return new Topology(folder, mapId);
        }

        public ITopology Create(string filepath)
        {
            return new Topology(filepath);
        }
    }


}