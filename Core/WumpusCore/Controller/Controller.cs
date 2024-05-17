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
    /// The overall controller for the Wumpus game. This should be your main interaction point for any UI implementation
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
      
        // TODO: Add documentation, and/or move all of this into constructor. Code is illegible
        public static Random Random = new Random();
        
        private ControllerState state = StartScreen;
      
        private ITopology topology;
        public bool debug = false;
        private List<RoomAnomalies> currentRoomHandledAmomalies = new List<RoomAnomalies>();


        private GameLocations.GameLocations gameLocations;

        private Trivia.Trivia trivia;


        /// <summary>
        /// Instantiates a controller and setup the required stuff for global controller.
        /// </summary>
        /// <param name="triviaFile">The path to the file you want to load trivia from. See Triva/Questions.json for format.</param>
        /// <param name="topologyDirectory">The directory to load map files from.</param>
        /// <param name="mapId">The mapid to load from the topologyDirectory. Format is map{n}.wmp where n is the mapId.</param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId):this(triviaFile,topologyDirectory,mapId,2,1,1,2)
        {
        }

        /// <summary>
        /// Construct a controller with parameters for gamelocations data
        /// </summary>
        /// <param name="triviaFile"></param>
        /// <param name="topologyDirectory"></param>
        /// <param name="mapId"></param>
        /// <param name="numVats"></param>
        /// <param name="numBats"></param>
        /// <param name="numRats"></param>
        /// <param name="numAcrobats"></param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId, int numVats, int numBats, int numRats,
            int numAcrobats)
        {
            controllerReference = this;
            trivia = new Trivia.Trivia(triviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
            gameLocations = new GameLocations.GameLocations(topology.RoomCount,numVats,numBats,numRats,numAcrobats,topology,Controller.Random,trivia);

            gameLocations.AddEntity(new Cat(topology, gameLocations, gameLocations.GetEmptyRoom()));
            gameLocations.AddEntity(new Wumpus.Wumpus(topology, gameLocations));
            gameLocations.AddEntity(new Player.Player(topology, gameLocations, gameLocations.GetEmptyRoom()));

        }

        /// <summary>
        /// Gets the room type for the current room
        /// </summary>
        /// <returns>A RoomType enum with the current room type</returns>
        public RoomType GetCurrentRoomType()
        {
            // You can only get the room type if you're in the room
            ValidateState(new [] {InRoom});

            return gameLocations.GetRoomAt(gameLocations.GetPlayer().location);
        }
        
        /// <summary>
        /// Gets the IRoom the current room
        /// </summary>
        /// <returns>An IRoom for the current room</returns>
        public IRoom GetCurrentRoom()
        {
            // You can only get the room type if you're in the room
            ValidateState(new [] {InRoom});

            return topology.GetRoom(gameLocations.GetPlayer().location);
        }


        /// <summary>
        /// Moves the player in a given direction.
        /// </summary>
        /// <param name="direction">The direction to move the player in.</param>
        public void MoveInADirection(Directions direction)
        {
            ValidateState(new [] {InRoom});
            state = InBetweenRooms;
            currentRoomHandledAmomalies.Clear();

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

            SetCorrectStateForRoom(nextRoom.Id);

            return player.location;
        }

        private void SetCorrectStateForRoom(int roomId)
        {
            List<RoomAnomalies> anomaliesInRoom =  GetAnomaliesInRoom(roomId);


            // This if else block will determine the ordering that you deal with anomalies
            // This includes if you get sent away by the bats before fighting the wumpus
            // Or in which order the cat gets handled
            if (anomaliesInRoom.Contains(RoomAnomalies.Wumpus) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Wumpus))
            {
                state = WumpusFight;
            } else
            if (anomaliesInRoom.Contains(RoomAnomalies.Acrobat) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Acrobat))
            {
                state = Acrobat;
            } else
            if (anomaliesInRoom.Contains(RoomAnomalies.Vat) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Vat))
            {
                state = VatRoom;
            } else
            if (anomaliesInRoom.Contains(RoomAnomalies.Rat) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Rat))
            {
                state = Rats;
            } else
            if (anomaliesInRoom.Contains(RoomAnomalies.Cat) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Acrobat))
            {
                state = CatDialouge;
            } else
            if (anomaliesInRoom.Contains(RoomAnomalies.Bats) && !currentRoomHandledAmomalies.Contains(RoomAnomalies.Bats))
            {
                state = BatTransition;
            } else
            if ((anomaliesInRoom.Count == currentRoomHandledAmomalies.Count) || anomaliesInRoom.Count == 0)
            {
                state = InRoom;
            } else
            {
                throw new Exception("Somehow something in the room you're going to isn't handled here.");
            }

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


        public struct DirectionalHint
        {
            public Directions Direction;
            public List<RoomAnomalies> Hazards;
            public DirectionalHint(List<RoomAnomalies> hazards, Directions direction)
            {
                Hazards = hazards;
                Direction = direction;
            }
        }

        /// <summary>
        /// Returns the hazards currently around the player.
        /// </summary>
        /// <returns>List containing the hazards that are around the player</returns>
        public List<DirectionalHint> GetHazardHints()
        {

            Dictionary<Directions, IRoom> rooms = topology.GetRoom((ushort)GetPlayerLocation()).AdjacentRooms;
        
            List<DirectionalHint> hints = new List<DirectionalHint>();

            // Loop over all the keys
            foreach (Directions directions in rooms.Keys)
            {
                hints.Add(new DirectionalHint(
                    GetAnomaliesInRoom(rooms[directions].Id),directions));
            }

            return hints;
        }


        public List<RoomAnomalies> GetAnomaliesInRoom(int roomnum)
        {
            List<RoomAnomalies> anomaliesList = new List<RoomAnomalies>();

            if (gameLocations.GetCat().location == roomnum)
            {
                anomaliesList.Add(RoomAnomalies.Cat);
            }

            if (gameLocations.GetWumpus().location == roomnum)
            {
                anomaliesList.Add(RoomAnomalies.Wumpus);
            }

            RoomType room = gameLocations.GetRoomAt((ushort) roomnum);

            switch (room)
            {
                case RoomType.Acrobat: anomaliesList.Add(RoomAnomalies.Acrobat);
                    break;
                case RoomType.Bats: anomaliesList.Add(RoomAnomalies.Bats);
                    break;
                case RoomType.Rats: anomaliesList.Add(RoomAnomalies.Rat);
                    break;
                case RoomType.Vats: anomaliesList.Add(RoomAnomalies.Vat);
                    break;
            }

            return anomaliesList;
        }
        

        /// <summary>
        /// This is a debug method.
        /// </summary>
        /// <returns>The room number the wumpus is in.</returns>
        public int GetWumpusLocation()
        {
            return gameLocations.GetWumpus().location;
        }

        public void StartTrivia()
        {
            ValidateState(new []{ ControllerState.VatRoom });

            trivia.StartRound(3,2);
        }

        public bool hasNextTriviaQuestion()
        {
            if (trivia.reportResult() == GameResult.InProgress)
            {
                return true;
            }

            return false;
        }


        public Trivia.AskableQuestion GetTriviaQuestion()
        {
            return trivia.GetQuestion();
        }

        public bool SubmitTriviaAnswer(int choice)
        {
            bool correctness = trivia.SubmitAnswer(choice);

            if (trivia.reportResult() == GameResult.Win)
            {
                state = InRoom;
            }
            else if (trivia.reportResult() == GameResult.Loss)
            {
                state = GameOver;
            }

            return correctness;
        }

        public void StartGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not.
            // being prepared to handle that controller state.
            ValidateState(new[] { StartScreen });
            this.state = InRoom;
        }

        public void EndGame()
        {
            // TODO! This will need to be rewritten
            this.state = StartScreen;
        }
        
        

        /// <summary>
        /// Meant to be used as validation for methods to prevent UI from getting any funny ideas. Throws an invalid operations exception if the current state is not in the valid states.
        /// </summary>
        /// <param name="validStates">The list of states that you are allowed to be in to use the method.</param>
        /// <exception cref="InvalidOperationException">Thrown if you are not in the valid states to call the function.</exception>
        private void ValidateState(ControllerState[] validStates)
        {
            if (debug)
            {
                return;
            }
            if (!validStates.Contains(state))
            {
                throw new InvalidOperationException(
                    $"You cannot do that operation from {state}. The only valid options are {validStates}");
            }
        }

        /// <summary>
        /// Called when the player successfully earns a secret. Generate a new secret and give it to the player.
        /// </summary>
        public void GenerateSecret()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows you to exit the bat state. Puts the player in a random room and changes the controller state to whatever is in that room.
        /// </summary>
        public void ExitBat()
        {
            ValidateState(new []{BatTransition});

            gameLocations.GetPlayer().location = gameLocations.GetEmptyRoom();

            SetCorrectStateForRoom(gameLocations.GetPlayer().location);
        }

        public void ExitAcrobat(bool success)
        {
            if (success)
            {
                state = InRoom;
            }
            else
            {
                state = GameOver;
            }
        }
    }
}