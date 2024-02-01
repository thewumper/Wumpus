namespace WumpusCore.Topology
{
    public interface ITopology
    {
        IRoom GetRoom(ushort id);
        ITopology Create(string folder, ushort mapId);
        ITopology Create(string filepath);
    }
}