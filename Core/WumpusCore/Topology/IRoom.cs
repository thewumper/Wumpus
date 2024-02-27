using System.Collections.Generic;

namespace WumpusCore.Topology
{
    public interface IRoom
    {
        Directions[] ExitDirections { get; }
        Dictionary<Directions, IRoom> ExitRooms { get; }
        ushort Id { get; }
    }
}