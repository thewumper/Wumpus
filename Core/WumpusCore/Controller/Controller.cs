using System;
using System.Collections.Generic;
using System.Linq;
using WumpusCore.Entity;
using WumpusCore.GameLocations;
using WumpusCore.Topology;
using static WumpusCore.Controller.ControllerState;
using WumpusCore.LuckyCat;
using WumpusCore.Trivia;


namespace WumpusCore.Controller
{
    /// <summary>
    /// The overall controller for the Wumpus game. This shouuld be your main interaction point for any UI implementation
    /// </summary>
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
        /// Instantiates a controller and setup the required stuff for global controller.
        /// </summary>
        /// <param name="triviaFile">The path to the file you want to load trivia from. See Triva/Questions.json for format.</param>
        /// <param name="topologyDirectory">The directory to load map files from.</param>
        /// <param name="mapId">The mapid to load from the topologyDirectory. Format is map{n}.wmp where n is the mapId.</param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId)
        {
            controllerReference = this;
            trivia = new Trivia.Trivia(triviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
            gameLocations = new GameLocations.GameLocations(topology.RoomCount,2,1,1,2,topology,Controller.Random);

            gameLocations.AddEntity(new Player.Player(topology, gameLocations, 0));
            gameLocations.AddEntity(new Cat(topology, gameLocations, 1));
            gameLocations.AddEntity(new Wumpus.Wumpus(topology, gameLocations));
        }


        /// <summary>
        /// Returns the room at the room number from topology (0 indexed)
        /// </summary>
        /// <param name="roomNumber">The 0 indexed room number</param>
        /// <returns>The room at the room number from topology</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IRoom GetRoom(ushort roomNumber)
        {
            return topology.GetRoom(roomNumber);
        }
        
        /// <summary>
        /// Returns the RoomType of the given room.
        /// </summary>
        /// <param name="roomNumber">The room's position.</param>
        /// <returns>The RoomType of the given room.</returns>
        public RoomType GetRoomType(ushort roomNumber)
        {
            return gameLocations.GetRoomAt(roomNumber);
        }

        /// <summary>
        /// Gets the room type for the current room
        /// </summary>
        /// <returns>A RoomType enum with the current room type</returns>
        public RoomType GetCurrentRoomType()
        {
            return GetRoomType((ushort)GetPlayerLocation());
        }
        
        /// <summary>
        /// Moves the player in a given direction.
        /// </summary>
        /// <param name="direction">The direction to move the player in.</param>
        public void MoveInADirection(Directions direction)
        {
            state = InBetweenRooms;

            Entity.Entity player = gameLocations.GetEntity(EntityType.Player);

            nextRoom = topology.GetRoom (player.location).ExitRooms[direction];


            player.location = topology.GetRoom(player.location).ExitRooms[direction].Id;
        }

        /// <summary>
        /// Moves a player from the hallway they are in  to the room they previously targets.
        /// </summary>
        /// <returns>The new position of the player after moving.</returns>
        /// <exception cref="InvalidOperationException">You need to move from a room.</exception>
        public int MoveFromHallway()
        {
            ValidateState(new [] {InBetweenRooms});
            if (nextRoom == null)
            {
                throw new InvalidOperationException("You need to start in a room and move from it before calling move from hallway");
            }

            Entity.Entity player = gameLocations.GetEntity(EntityType.Player);

            player.location = nextRoom.Id;

            RoomType nextroomType =  gameLocations.GetRoomAt(nextRoom.Id);
            if (nextroomType == RoomType.Flats)
            {
                state=InRoom;
            }
            else if (nextroomType == RoomType.Acrobat)
            {
                state = ControllerState.Acrobat;
            }
            else if (nextroomType == RoomType.Bats)
            {

            }


            return player.location;
        }

        /// <summary>
        /// Get the location of the player from topology.
        /// </summary>
        /// <returns>The location of the player.</returns>
        public int GetPlayerLocation()
        {
            return gameLocations.GetPlayer().location;
        }
        
        /// <summary>
        /// Returns the current state of the controller.
        /// </summary>
        /// <returns>The current <see cref="ControllerState"/></returns>
        public ControllerState GetState()
        {
            return state;
        }

        /// <summary>
        /// Returns a question that hasn't been asked yet.
        /// </summary>
        /// <returns>A question that hasn't been used by trivia yet.</returns>
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
        /// <returns>All hazards that are in the room the player is currently in.</returns>
        public List<HazardType> getRoomHazards()
        {
            var hazards = new List<HazardType>();
            if (gameLocations.GetEntity(EntityType.Wumpus).location == GetPlayerLocation())
            {
                hazards.Add(HazardType.Wumpus);
            }
            HazardType? hazard = gameLocations.GetRoomAt((ushort)GetPlayerLocation()).ToHazard();
            if (hazard != null)
            {
                hazards.Add((HazardType)hazard);
            }
            return hazards;
        }
        
        /// <summary>
        /// Returns the hazards currently around the player.
        /// </summary>
        /// <returns>List containing the hazards that are around the player</returns>
        public List<HazardType> GetHazardHints()
        {
            private struct DirectionalHint
            {
                public Directions Direction;
                public List<HazardType> Hazards;
                public DirectionalHint(List<HazardType> hazards, Directions direction)
                {
                    Hazards = hazards;
                    Direction = direction;
                }
            }

            List<RoomType> rooms = gameLocations.GetAdjacentRoomTypes(GetPlayerLocation()).Values.ToList();
            
            List<string> hints = new List<string>();
            foreach (RoomType roomType in rooms)
            {
                HazardType? hazardType = roomType.ToHazard();
                if (hazardType != null)
                {
                    hints.Add(((HazardType)hazardType).GetHint());
                }
            }

            return hints;
        }
        

        /// <summary>
        /// This is a debug method.
        /// </summary>
        /// <returns>The room number the wumpus is in.</returns>
        public int GetWumpusLocation()
        {
            return gameLocations.GetWumpus().location;
        }


        public Trivia.AskableQuestion GetTriviaQuestion()
        {
            return trivia.GetQuestion();
        }

        public void StartGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not.
            // being prepared to handle that controller state.
            ValidateState(new[] { StartScreen, InRoom });
            this.state = InRoom;
        }

        public void EndGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not.
            // being prepared to handle that controller state.
            ValidateState(new[] { StartScreen, InRoom });
            this.state = StartScreen;
        }
        
        

        /// <summary>
        /// Meant to be used as validation for methods to prevent UI from getting any funny ideas. Throws an invalid operations exception if the current state is not in the valid states.
        /// </summary>
        /// <param name="validStates">The list of states that you are allowed to be in to use the method.</param>
        /// <exception cref="InvalidOperationException">Thrown if you are not in the valid states to call the function.</exception>
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