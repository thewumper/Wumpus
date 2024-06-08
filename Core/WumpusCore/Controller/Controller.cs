using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WumpusCore.Controller.Stopwatch;
using WumpusCore.Entity;
using WumpusCore.GameLocations;
using WumpusCore.Topology;
using static WumpusCore.Controller.ControllerState;
using WumpusCore.LuckyCat;
using WumpusCore.Trivia;
using WumpusCore.HighScoreNS;


namespace WumpusCore.Controller
{
    /// <summary>
    /// The overall controller for the Wumpus game. This should be your main interaction point for any UI implementation
    /// </summary>
    public class Controller
    {
        private static Controller _controllerReference;
        private IRoom nextRoom;
        internal IStopwatch ratTimeStopwatch = new RealStopwatch();
        private ControllerState state = StartScreen;
        private ITopology topology;
        private List<RoomAnomaly> currentRoomHandledAmomalies = new List<RoomAnomaly>();
        private GameLocations.GameLocations gameLocations;
        internal Trivia.Trivia trivia;
        internal WinLossConditions gameEndCause;
        internal static SaveFile headFile = new SaveFile("", false);

        /// <summary>
        /// The <c>RoomAnomaly</c> that causes the game to end.
        /// The property is only valid in the <c>GameOver</c> state.
        /// </summary>
        public WinLossConditions GameEndCause
        {
            get
            {
                ValidateState(new [] {GameOver});
                return gameEndCause;
            }
            private set
            {
                gameEndCause = value;
            }
        }

        /// <summary>
        /// The random object that should be used by all
        /// classes and tests related to Wumpus
        /// </summary>
        public static Random Random = new Random();

        /// <summary>
        /// A debug flag allows you to bypass room travel restrictions.
        /// Useful for testing different parts of a frontend without having
        /// to start from the main menu every time.
        /// </summary>
        public bool Debug = false;

        private int turnCounter;
        private int mapID;

        /// <summary>
        /// Gets the reference to the global controller
        /// uses the internal <c>_controllerReference</c> variable
        ///
        /// You must use one of the <c>Controller</c> constructors
        /// before there will be a global controller to reference.
        /// </summary>
        /// <exception cref="NullReferenceException">You must construct a controller once before the global controller is set</exception>
        public static Controller GlobalController
        {
            get
            {
                if (_controllerReference == null)
                {
                    throw new NullReferenceException(
                        "You have to initialize a controller before you can grab a global controller");
                }

                return _controllerReference;
            }
        }

        /// <summary>
        /// Instantiates a controller and setup the required stuff for global controller.
        /// </summary>
        /// <param name="triviaFile">The path to the file you want to load trivia from. See Triva/Questions.json for format.</param>
        /// <param name="topologyDirectory">The directory to load map files from.</param>
        /// <param name="mapId">The mapid to load from the topologyDirectory. Format is map{n}.wmp where n is the mapId.</param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId):this(triviaFile,topologyDirectory,mapId,2,1,1,2,2,1,0)
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
        /// <param name="numGunRooms"></param>
        /// <param name="numAmmoRooms"></param>
        /// <param name="startingCoins"></param>
        public Controller(string triviaFile, string topologyDirectory, ushort mapId, int numVats, int numBats, int numRats,
            int numAcrobats, int numAmmoRooms, int numGunRooms, int startingCoins)
        {
            mapID = mapId;
            _controllerReference = this;
            trivia = new Trivia.Trivia(triviaFile);
            topology = new Topology.Topology(topologyDirectory, mapId);
            gameLocations = new GameLocations.GameLocations(topology.RoomCount,numVats,numBats,numRats,numAcrobats,numAmmoRooms,numGunRooms,topology,Controller.Random,trivia);
            gameLocations.AddEntity(new Cat(topology, gameLocations, gameLocations.GetEmptyRoom()));
            gameLocations.AddEntity(new Wumpus.Wumpus(topology, gameLocations,gameLocations.GetEmptyRoom()));
            gameLocations.AddEntity(new Player.Player(topology, gameLocations, gameLocations.GetEmptyRoom()));
            gameLocations.GetPlayer().GainCoins((uint) startingCoins);
        }

        /// <summary>
        /// Gets the room type for the current room
        /// </summary>
        /// <returns>A RoomType enum with the current room type</returns>
        public RoomType GetCurrentRoomType()
        {
            // You can only get the room type if you're in the room

            return gameLocations.GetRoomAt(gameLocations.GetPlayer().location);
        }
        
        /// <summary>
        /// Gets the IRoom the current room
        /// </summary>
        /// <returns>An IRoom for the current room</returns>
        public IRoom GetCurrentRoom()
        {
            return topology.GetRoom(gameLocations.GetPlayer().location);
        }


        /// <summary>
        /// Moves the player in a given direction.
        /// </summary>
        /// <param name="direction">The direction to move the player in.</param>
        public void MoveInADirection(Directions direction)
        {
            turnCounter++;

            ValidateState(new [] {InRoom, Rats, CatDialouge, AmmoRoom, GunRoom});

            // if (state == CatDialouge)
            // {
            //     gameLocations.GetCat().location = gameLocations.GetEmptyRoom();
            // }


            state = InBetweenRooms;
            currentRoomHandledAmomalies.Clear();
            gameLocations.GetPlayer().MoveInDirection(direction);
            nextRoom = GetCurrentRoom();
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

            //if (hasPlayerTamedCat())
            //{
            //    // This protects the player from the effects of the rats
            //    currentRoomHandledAmomalies.Add(RoomAnomaly.Rat);
            //}

            SetCorrectStateForRoom(nextRoom.Id);

            return player.location;
        }

        private void SetCorrectStateForRoom(int roomId)
        {
            List<RoomAnomaly> anomaliesInRoom =  GetAnomaliesInRoom(roomId);


            // This if else block will determine the ordering that you deal with anomalies
            // This includes if you get sent away by the bats before fighting the wumpus
            // Or in which order the cat gets handled
            if (anomaliesInRoom.Contains(RoomAnomaly.Wumpus) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Wumpus))
            {
                state = WumpusFight;
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Bats) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Bats))
            {
                state = BatTransition;
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Acrobat) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Acrobat))
            {
                state = Acrobat;
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Vat) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Vat))
            {
                state = VatRoom;
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Rat) &&
                       !currentRoomHandledAmomalies.Contains(RoomAnomaly.Rat))
            {
                state = Rats;
                ratTimeStopwatch.Restart();
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Ammo) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Ammo))
            {
                state = AmmoRoom;
            } else if (anomaliesInRoom.Contains(RoomAnomaly.Gun) && !currentRoomHandledAmomalies.Contains(RoomAnomaly.Gun))
            {
                state = GunRoom;
            } else if ((anomaliesInRoom.Count == currentRoomHandledAmomalies.Count) || anomaliesInRoom.Count == 0 || (anomaliesInRoom.Count + 1 == currentRoomHandledAmomalies.Count || anomaliesInRoom.Count == 1) && anomaliesInRoom.Contains(RoomAnomaly.Cat))

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
            return gameLocations.GetPlayer().Bullets;
        }


        public struct DirectionalHint
        {
            public Directions Direction;
            public List<RoomAnomaly> Hazards;
            public DirectionalHint(List<RoomAnomaly> hazards, Directions direction)
            {
                Hazards = hazards;
                Direction = direction;
            }
        }

        /// <summary>
        /// Returns the audible/hinted hazards currently around the player.
        /// </summary>
        /// <returns>List containing the hazards that are around the player that they should be hinted about/hear</returns>
        public List<DirectionalHint> GetHazardHints()
        {

            Dictionary<Directions, IRoom> exitRooms = topology.GetRoom((ushort)GetPlayerLocation()).ExitRooms;

            List<DirectionalHint> hints = new List<DirectionalHint>();

            // Loop over all the keys
            foreach (Directions directions in exitRooms.Keys)
            {
                hints.Add(new DirectionalHint(
                    GetAudibleAnomaliesInRom(exitRooms[directions].Id),directions));

            }

            return hints;
        }


        public List<RoomAnomaly> GetAnomaliesInRoom(int roomnum)
        {
            List<RoomAnomaly> anomaliesList = new List<RoomAnomaly>();

            if (gameLocations.GetCat().location == roomnum)
            {
                anomaliesList.Add(RoomAnomaly.Cat);
            }

            if (gameLocations.GetWumpus().location == roomnum)
            {
                anomaliesList.Add(RoomAnomaly.Wumpus);
            }

            RoomType room = gameLocations.GetRoomAt((ushort) roomnum);

            switch (room)
            {
                case RoomType.Acrobat: anomaliesList.Add(RoomAnomaly.Acrobat);
                    break;
                case RoomType.Bats: anomaliesList.Add(RoomAnomaly.Bats);
                    break;
                case RoomType.Rats: anomaliesList.Add(RoomAnomaly.Rat);
                    break;
                case RoomType.Vats: anomaliesList.Add(RoomAnomaly.Vat);
                    break;
                case RoomType.GunRoom: anomaliesList.Add(RoomAnomaly.Gun);
                    break;
                case RoomType.AmmoRoom: anomaliesList.Add(RoomAnomaly.Ammo);
                    break;
            }

            return anomaliesList;
        }

        private List<RoomAnomaly> GetAudibleAnomaliesInRom(int roomnum)
        {
            List<RoomAnomaly> allAnomalies = GetAnomaliesInRoom(roomnum);
            // We don't want the player to get a hint for the gun or ammo

            allAnomalies.Remove(RoomAnomaly.Gun);
            allAnomalies.Remove(RoomAnomaly.Ammo);
            // Remove the cat if it's tamed and the rats
            // since they don't matter anymore
            if (hasPlayerTamedCat())
            {
                allAnomalies.Remove(RoomAnomaly.Cat);
                allAnomalies.Remove(RoomAnomaly.Rat);
            }
            return allAnomalies;
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
            ValidateState(new []{ VatRoom, GunRoom, AmmoRoom });

            state = ControllerState.Trivia;

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
            GameResult triviaState = trivia.reportResult();
            RoomType currentRoomType = GetCurrentRoomType();

            if (triviaState == GameResult.Win)
            {
                if (currentRoomType == RoomType.Vats)
                {
                    state = InRoom;
                }
                else if (currentRoomType == RoomType.GunRoom)
                {
                    state = GunRoom;
                    CollectItemsInRoom();
                } else if (currentRoomType == RoomType.AmmoRoom)
                {
                    state = AmmoRoom;
                    CollectItemsInRoom();
                }
                else
                {
                    throw new InvalidOperationException(
                        "The room you are in doesn't have trivia despite you being in trivia");
                }
            }
            if (trivia.reportResult() == GameResult.Loss)
            {
                if (GetCurrentRoomType() == RoomType.GunRoom) {
                    state = GunRoom;
                    gameLocations.MarkRoomAsCollected((ushort) GetPlayerLocation());
                } else if (GetCurrentRoomType() == RoomType.AmmoRoom) {
                    state = AmmoRoom;
                    gameLocations.MarkRoomAsCollected((ushort) GetPlayerLocation());
                }
                else if (GetCurrentRoomType() == RoomType.Vats){
                    EndGame(false,WinLossConditions.Vat);
                }
                else
                {
                    throw new InvalidOperationException(
                        "The room you are in doesn't have trivia despite you being in trivia");
                }
            }

            return correctness;
        }

        private void CollectItemsInRoom()
        {
            ValidateState(new []{GunRoom, AmmoRoom});

            if (!CanRoomBeCollectedFrom())
            {
                throw new InvalidOperationException("The items in this room have already bee");
            }

            gameLocations.MarkRoomAsCollected((ushort) GetPlayerLocation());

            if (GetCurrentRoomType() == RoomType.AmmoRoom)
            {
                gameLocations.GetPlayer().GainArrows(3);
                return;
            }

            if (GetCurrentRoomType() == RoomType.GunRoom)
            {
                gameLocations.GetPlayer().GainGun();
                return;
            }
        }

        public void StartGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not.
            // being prepared to handle that controller state.
            ValidateState(new[] { StartScreen });
            state = InRoom;
        }

        public void EndGame(bool success, WinLossConditions gameEndCause)
        {
            if (success)
            {
                state = WonGame;

            }
            else
            {
                state = GameOver;
            }
            GameEndCause = gameEndCause;
        }

        public HighScore SaveHighScore(String name)
        {
            ValidateState(new []{WonGame,GameOver});

            bool success = state == WonGame;

            return new HighScore(headFile.path, name, turnCounter, GetCoins(), GetArrowCount(), success, mapID);
        }



        /// <summary>
        /// Meant to be used as validation for methods to prevent UI from getting any funny ideas. Throws an invalid operations exception if the current state is not in the valid states.
        /// </summary>
        /// <param name="validStates">The list of states that you are allowed to be in to use the method.</param>
        /// <exception cref="InvalidOperationException">Thrown if you are not in the valid states to call the function.</exception>
        private void ValidateState(ControllerState[] validStates)
        {
            if (Debug)
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
                EndGame(false,WinLossConditions.Acrobat);
            }
        }

        public void ExitRat()
        {
            RatRoomStats stats = GetRatRoomStats();
            if (stats.RemainingCoins < 0)
            {
                EndGame(false, WinLossConditions.Rats);
                return;
            }
            gameLocations.GetPlayer().LoseCoins((uint) stats.DamageDelt);

            currentRoomHandledAmomalies.Add(RoomAnomaly.Rat);
            SetCorrectStateForRoom(gameLocations.GetPlayer().location);
        }

        public void ExitCat()
        {
            currentRoomHandledAmomalies.Add(RoomAnomaly.Cat);
            SetCorrectStateForRoom(gameLocations.GetPlayer().location);
        }

        public void ExitWumpusFight(bool won)
        {
            EndGame(won,WinLossConditions.WumpusFight);
        }


        /// <summary>
        /// Gets the information that needs to be displayed in a rat room
        /// Required you to be in a Rat Room
        /// </summary>
        /// <returns>A <c>RatRoomStats</c> object containing the correct info</returns>
        public RatRoomStats GetRatRoomStats()
        {
            int timeDiff = ratTimeStopwatch.GetElapsed().Seconds;

            int ratDamage = CalculateRatDamage(timeDiff);

            if (hasPlayerTamedCat())
            {
                return new RatRoomStats(timeDiff, gameLocations.GetPlayer().Coins, gameLocations.GetPlayer().Coins, 0);
            }
            
            RatRoomStats stats = new RatRoomStats(timeDiff, gameLocations.GetPlayer().Coins,
                gameLocations.GetPlayer().Coins - ratDamage, ratDamage);

            if (stats.RemainingCoins < 0)
            {
                EndGame(false, WinLossConditions.Rats);
            }

            return stats;

        }

        /// <summary>
        /// Calculate the amount of damage that a rat does to the player after some number of seconds
        /// Currently uses a 2^x function, where x is the number of seconds.
        /// </summary>
        /// <param name="timeDiffSeconds">The number of seconds since the player has entered the rat room</param>
        /// <returns>The damage that the rat has done after being run through some function.</returns>
        private int CalculateRatDamage(int timeDiffSeconds)
        {
            return (int) Math.Pow(2,timeDiffSeconds);
        }

        public bool AttemptToTameCat(int coinInput)
        {
            return gameLocations.GetCat().Tame(coinInput);
        }

        public bool ShootGun(Directions shootingDir)
        {
            gameLocations.GetPlayer().LoseArrow();

            if (GetAnomaliesInRoom(topology.GetRoom(gameLocations.GetPlayer().location).AdjacentRooms[shootingDir].Id).Contains(RoomAnomaly.Wumpus) && topology.GetRoom(gameLocations.GetPlayer().location).ExitDirections.Contains(shootingDir))
            {
                EndGame(true,WinLossConditions.ShotWumpus);
                return true;
            }

            gameLocations.GetWumpus().move(Random);
            return false;
        }

        public bool isNextRoomAWumpus()
        {
            ValidateState(new []{InBetweenRooms});
            return gameLocations.GetWumpus().location==nextRoom.Id;
        }

        public bool DoesPlayerHaveGun()
        {
            return gameLocations.GetPlayer().HasGun;
        }

        public bool CanRoomBeCollectedFrom()
        {
            return !gameLocations.HasRoomBeenCollected((ushort) GetPlayerLocation());
        }

        public bool hasPlayerTamedCat()
        {
            return gameLocations.GetCat().tamed;
        }

        public int getCatPosition()
        {
            return gameLocations.GetCat().location;
        }
    }
}
