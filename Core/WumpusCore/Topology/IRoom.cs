namespace WumpusCore.Topology
{
    public interface IRoom
    {
        Directions[] ExitDirections { get; }
        IRoom[] ExitRooms { get; }
        ushort Id { get; }
    }
}