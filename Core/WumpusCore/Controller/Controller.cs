using System;
using System.IO;
using WumpusCore.Topology;
using WumpusCore.GameLocations;

namespace WumpusCore.Controller
{
    public class Controller
    {
        public static Random Random = new Random();

        private ControllerState state = ControllerState.StartScreen;

        private Player.Player player = new Player.Player();
        private ITopology topology;
        private MinigameController.MinigameController minigameController;
        private GameLocations.GameLocations gameLocations;

        // TODO! This won't work

        public Controller()
        {
            // This is stupd
            using (StreamWriter outputFile = new StreamWriter("map0.wmp"))
            {
                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }
            topology = new Topology.Topology("./", 0);
        }

        public IRoom GetRoom(ushort roomNumber)
        {
            return topology.GetRoom(roomNumber);
        }

        public GameLocations.GameLocations.RoomType getRoomType(ushort roomNumber)
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