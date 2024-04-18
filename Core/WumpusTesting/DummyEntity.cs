using WumpusCore.Entity;

namespace WumpusTesting
{
    public class DummyEntity: Entity
    {
        public DummyEntity(WumpusCore.Topology.Topology topology, WumpusCore.GameLocations.GameLocations parent, EntityType type) : 
            base(topology, parent, 14, type) { }

        public void MoveToRoomWrapper(ushort roomIndex)
        {
            this.MoveToRoom(roomIndex);
        }
        
        public void TeleportToRoomWrapper(ushort roomIndex)
        {
            this.TeleportToRoom(roomIndex);
        }
        
        public void MoveToRandomAdjacentWrapper()
        {
            this.MoveToRandomAdjacent();
        }
        
        public void TeleportToRandomWrapper()
        {
            this.TeleportToRandom();
        }
    }
}