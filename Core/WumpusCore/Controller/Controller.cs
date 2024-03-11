using System;
using System.IO;
using WumpusCore.Topology;
using static WumpusCore.Controller.ControllerState;

namespace WumpusCore.Controller
{
    public class Controller
    {
        internal static Controller controllerReference;
        public static Controller GlobalController
        {
            get
            {
                if (controllerReference==null)
                {
                    controllerReference = new Controller();
                }

                return controllerReference;
            }
        }
        public static Random Random = new Random();

        private ControllerState state = StartScreen;

        private Player.Player player = new Player.Player();
        private ITopology topology;
        private MinigameController.MinigameController minigameController;
        private GameLocations.GameLocations gameLocations;

        // TODO! This won't work

        private Controller()
        {
            // This is stupid
            using (StreamWriter outputFile = new StreamWriter("map0.wmp"))
            {
                Directions[] directions = new Directions[]
                {
                    Directions.North, Directions.NorthEast, Directions.SouthEast,
                    Directions.South, Directions.SouthWest, Directions.NorthWest
                };
                for (int i = 0; i < 30; i++)
                {
                    string line = "";
                    for (int j = 0; j < 3; j++)
                    {
                        int num = Random.Next(0, 5);
                        line += DirectionHelper.GetShortNameFromDirection(directions[num]);
                        if (!(j == 2))
                        {
                            line += ",";
                        }
                    }

                    outputFile.WriteLine(line);
                }
            }

            topology = new Topology.Topology("./", 0);
        }

        /// <summary>
        /// Returns the room at the room number from topology (1 indexed)
        /// </summary>
        /// <param name="roomNumber">The 1 indexed room number</param>
        /// <returns>The room at the room number from topology</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IRoom GetRoom(ushort roomNumber)
        {
            if (roomNumber<=0)
            {
                throw new IndexOutOfRangeException("Room number is 1 indexed, not 0.");
            }
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

        public void StartGame()
        {
            this.state = InRoom;
        }
    }
}