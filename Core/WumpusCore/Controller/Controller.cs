using System;
using System.IO;
using System.Linq;
using WumpusCore.Entity;
using WumpusCore.Topology;
using static WumpusCore.Controller.ControllerState;
using WumpusCore.GameLocations;
using WumpusCore.Trivia;


namespace WumpusCore.Controller
{
    public class Controller
    {
        private static Controller controllerReference;
        private IRoom nextRoom;

        public static Controller GlobalController
        {
            get
            {
                if (controllerReference == null)
                {
                    throw new NullReferenceException("Cannot refrence a controller that hasn't been instantiated yet");
                }

                return controllerReference;
            }
        }

        public static Random Random = new Random();

        private ControllerState state = StartScreen;
        private ITopology topology;


        private GameLocations.GameLocations gameLocations;
        private Trivia.Trivia trivia;

        /// <summary>
        /// Instantiates a controller and setup the required stuff for global controller
        /// </summary>
        /// <param name="trviaFile">The path to the file you want to load trivia from. See Triva/Questions.json for format</param>
        /// <param name="topologyDirectory">The directory to load map files from</param>
        /// <param name="mapId">The mapid to load from the topologyDirectory. Format is map{n}.wmp where n is the mapId</param>
        public Controller(string trviaFile, string topologyDirectory, ushort mapId)
        {
            controllerReference = this;
            trivia = new Trivia.Trivia(trviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
            gameLocations = new GameLocations.GameLocations(topology.RoomCount);
        }


        /// <summary>
        /// Returns the room at the room number from topology (1 indexed)
        /// </summary>
        /// <param name="roomNumber">The 1 indexed room number</param>
        /// <returns>The room at the room number from topology</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IRoom GetRoom(ushort roomNumber)
        {
            if (roomNumber <= 0)
            {
                throw new IndexOutOfRangeException("Room number is 1 indexed, not 0.");
            }

            return topology.GetRoom(roomNumber);
        }

        public GameLocations.GameLocations.RoomType GetRoomType(ushort roomNumber)
        {
            return gameLocations.GetRoomAt(roomNumber);
        }

        public void MoveInADirection(Directions direction)
        {
            state = InBetweenRooms;

            Entity.Entity player = gameLocations.GetEntity(EntityType.Player);

            nextRoom = topology.GetRoom ((ushort) player.location).ExitRooms[direction];


            player.location = topology.GetRoom((ushort) player.location).ExitRooms[direction].Id;
        }

        /// <summary>
        /// Moves a player from the hallway they are in  to the room they previously targets
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int MoveFromHallway()
        {
            ValidateState(new [] {InBetweenRooms});
            if (nextRoom == null)
            {
                throw new InvalidOperationException("You need to start in a room and move from it before calling move from hallway");
            }

            Entity.Entity player = gameLocations.GetEntity(EntityType.Player);

            player.location = nextRoom.Id;
            state=InRoom;

            return player.location;
        }

        /// <summary>
        /// Get the location of the player from topology.
        /// </summary>
        /// <returns>The location of the player</returns>
        public int GetPlayerLocation()
        {
            return gameLocations.GetEntity(EntityType.Player).location;
        }

        public ControllerState GetState()
        {
            return state;
        }

        /// <summary>
        /// Returns a question that hasn't been asked yet
        /// </summary>
        /// <returns>A question that hasn't been used by trivia yet</returns>
        public AnsweredQuestion GetUnaskedQuestion()
        {
            return trivia.PeekRandomQuestion();
        }

        public int GetCoins()
        {
            throw new NotImplementedException("This can't exist rn ngl");
        }

        public int GetArrowCount()
        {
            throw new NotImplementedException("This can't exist rn ngl");
        }

        // public bool SubmitTriviaAnswer(int guess)
        // {
        //     return trivia.SubmitAnswer(guess);
        // }

        // public Trivia.AskableQuestion GetTriviaQuestion()
        // {
        //     return trivia.GetQuestion();
        // }

        public void StartGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not
            // being prepared to handle that controller state
            ValidateState(new[] { StartScreen, InRoom });
            this.state = InRoom;
        }

        public void EndGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not
            // being prepared to handle that controller state
            ValidateState(new[] { StartScreen, InRoom });
            this.state = StartScreen;
        }

        /// <summary>
        /// Meant to be used as validation for methods to prevent UI from getting any funny ideas. Throws an invalid operations exception if the current state is not in the valid states
        /// </summary>
        /// <param name="validStates">The list of states that you are allowed to be in to use the method</param>
        /// <exception cref="InvalidOperationException">Thrown if you are not in the valid states to call the function</exception>
        private void ValidateState(ControllerState[] validStates)
        {
            if (!validStates.Contains(state))
            {
                throw new InvalidOperationException(
                    $"You cannot go to that state from {state}. The only valid options are {validStates}");
            }
        }
    }
}