using System;
using System.Collections.Generic;
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
                    throw new NullReferenceException(
                        "You have to initialize a controller before you can grab a global controller");
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
        /// <param name="triviaFile">The path to the file you want to load trivia from. See Triva/Questions.json for format</param>
        /// <param name="topologyDirectory">The directory to load map files from</param>
        /// <param name="mapId">The mapid to load from the topologyDirectory. Format is map{n}.wmp where n is the mapId</param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId)
        {
            controllerReference = this;
            trivia = new Trivia.Trivia(triviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
            gameLocations = new GameLocations.GameLocations(topology.RoomCount);
        }


        /// <summary>
        /// Returns the room at the room number from topology (0 indexed)
        /// </summary>
        /// <param name="roomNumber">The 0 indexed room number</param>
        /// <returns>The room at the room number from topology</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IRoom GetRoom(ushort roomNumber)
        {
            if (roomNumber < 0)
            {
                throw new IndexOutOfRangeException("Room number is 0 indexed, not -1.");
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

            nextRoom = topology.GetRoom (player.location).ExitRooms[direction];


            player.location = topology.GetRoom(player.location).ExitRooms[direction].Id;
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
            return gameLocations.GetPlayer().location;
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
            return gameLocations.GetPlayer().Coins;
        }

        public int GetArrowCount()
        {
            return gameLocations.GetPlayer().Arrows;
        }


        /// <summary>
        /// Gives the hazards that are in the room the player is currently in.
        /// </summary>
        /// <returns>A `HazardType` enum</returns>
        public List<HazardType> getRoomHazards()
        {
            var hazards = new List<HazardType>();
            // if (gameLocations.GetEntity(EntityType.Wumpus).location == GetPlayerLocation())
            // {
                hazards.Add(HazardType.Wumpus);
            // }

            return hazards;
        }


        public Trivia.AskableQuestion GetTriviaQuestion()
        {
            return trivia.GetQuestion();
        }

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

        /// <summary>
        /// Called when the player successfully earns a secret. Generate a new secret and give it to the player.
        /// </summary>
        public void GenerateSecret()
        {
            throw new NotImplementedException();
        }
    }
}