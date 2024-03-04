using System;
using WumpusCore.Topology;
using WumpusCore.GameLocations;
using WumpusCore.Trivia;

namespace WumpusCore.Controller
{
    public class Controller
    {
        public static Random Random = new Random();

        private ControllerState state;

        private Player.Player player = new Player.Player();
        private ITopology topology = new Topology.Topology(null, 0);
        private GameLocations.GameLocations gameLocations;
        // TODO! This likely won't construct properly
        private Trivia.Trivia trivia = new Trivia.Trivia("../Trivia/");


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
            return trivia.SubmitAnswer(guess);
        }

        public Trivia.AskableQuestion GetTriviaQuestion()
        {
            return trivia.GetQuestion();
        }
    }

}