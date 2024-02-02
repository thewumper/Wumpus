using System;
using WumpusCore.Topology;

namespace WumpusCore.Controller
{
    public class Controller
    {
        public Random Random = new Random();

        private ControllerState state;

        private ITopology topology;

        public IRoom getRoom(ushort roomNumber)
        {
            return topology.GetRoom(roomNumber);
        }

        public ControllerState getState()
        {
            return state;
        }
    }

}