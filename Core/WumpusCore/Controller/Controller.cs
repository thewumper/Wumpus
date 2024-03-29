using System;
using System.IO;
using System.Linq;
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
        private IRoom beforeRoom;

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
        private Player.Player player = new Player.Player();
        private ITopology topology;


        private GameLocations.GameLocations gameLocations;
        private Trivia.Trivia trivia;

        public Controller(string trviaFile, string topologyDirectory, ushort mapId)
        {
            controllerReference = this;
            trivia = new Trivia.Trivia(trviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
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
            beforeRoom = topology.GetRoom(player.Position);
            nextRoom = topology.GetRoom (player.Position).ExitRooms[direction];


            player.MoveInDirection(topology, direction);
        }

        public ushort MoveFromHallway(HallwayDir hallwayDir)
        {
            if (hallwayDir == HallwayDir.Forward)
            {
                player.MoveTo(nextRoom.Id);
            }
            // If the player is going back to the previous room then you don't have to change anything
            state=InRoom;

            return player.Position;
        }

        /// <summary>
        /// Get the location of the player from topology.
        /// </summary>
        /// <returns>The location of the player</returns>
        public ushort GetPlayerLocation()
        {
            return player.Position;
        }

        public ControllerState GetState()
        {
            return state;
        }

        public int GetCoins()
        {
            return player.coins;
        }

        public int GetArrowCount()
        {
            return player.arrows;
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