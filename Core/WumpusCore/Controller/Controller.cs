using System;
using WumpusCore.Topology;
using WumpusCore.GameLocations;

namespace WumpusCore.Controller
{
    public class Controller
    {
        public static Random Random = new Random();

        private ControllerState state;

        private Player.Player player;
        private ITopology topology;
        private MinigameController.MinigameController minigameController;
        private GameLocations.GameLocations gameLocations;

        public Controller()
        {
            player = new Player.Player();
            // TODO! This won't work
            topology = new Topology.Topology(null, 0);

        }

        public IRoom GetRoom(ushort roomNumber)
        {
            return topology.GetRoom(roomNumber);
        }

        public GameLocations.GameLocations.RoomTypes getRoomType(ushort roomNumber)
        {
            return gameLocations.GetRoomAt(roomNumber);
        }

        public ControllerState GetState()
        {
            return state;
        }

        public string GetPlayerSpritePath()
        {
            throw new NotImplementedException();
        }

        public int GetCoins()
        {
            throw new NotImplementedException();
        }

        public int GetArrowCount()
        {
            throw new NotImplementedException();
        }

        public bool SubmitTriviaAnswer(int guess)
        {
            return minigameController.SubmitTriviaAnswer(guess);
        }

        public string GetTriviaQuestion()
        {
            return minigameController.GetTriviaQuestion();
        }
    }

}