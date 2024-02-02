using System;
using WumpusCore.Topology;

namespace WumpusCore.Controller
{
    public class Controller
    {
        public Random Random = new Random();

        private ControllerState state;

        private Player.Player player;
        private ITopology topology;

        public Controller()
        {
            player = new Player.Player();
            // TODO! This won't work
            topology = new Topology.Topology(null, 0);

        }

        public IRoom getRoom(ushort roomNumber)
        {
            return topology.GetRoom(roomNumber);
        }

        public ControllerState getState()
        {
            return state;
        }

        public string GetPlayerSpritePath()
        {
            throw new NotImplementedException();
        }

        public int getCoins()
        {
            throw new NotImplementedException();
        }

        public int getArrowCount()
        {
            throw new NotImplementedException();
        }
    }

}