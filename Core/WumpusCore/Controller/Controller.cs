using WumpusCore.Topology;

namespace WumpusCore.Controller
{
    public class Controller
    {
        private ControllerState state;

        private ITopology topology;

        public IRoom getRoom(ushort roomNumber)
        {
            return getRoom(roomNumber);
        }

        public ControllerState getState()
        {
            return state;
        }
    }

}