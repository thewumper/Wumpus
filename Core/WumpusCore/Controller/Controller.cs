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
        internal static Controller controllerReference;
        public static Controller GlobalController
        {
            get
            {
                if (controllerReference == null)
                {
                    controllerReference = new Controller();
                }

                return controllerReference;
            }
        }
        public static Random Random = new Random();
        private ControllerState state = StartScreen;
        //private Player.Player player = new Player.Player();
        private ITopology topology;


        private GameLocations.GameLocations gameLocations;
        // TODO! This likely won't construct properly
        private Trivia.Trivia trivia = new Trivia.Trivia("../Trivia/");

        // TODO! This won't work

        private Controller()
        {
            topology = new Topology.Topology("./Assets/Maps", 0);
        }

        /// <summary>
        /// Win the game.
        /// </summary>
        public void WinGame()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lose the game.
        /// </summary>
        public void LoseGame()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executed each turn the player takes.
        /// </summary>
        public void GameTick()
        {
            throw new NotImplementedException();
            if (GetCoins() < 0)
            {
                LoseGame();
            }
            // TODO: Entity motion here
        }

        /// <summary>
        /// Returns the room at the room number from topology (1 indexed)
        /// </summary>
        /// <param name="roomNumber">The 1 indexed room number</param>
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
        
        /// <summary>
        /// Play trivia to earn arrows for free. Can happen in any room.
        /// Gain two arrows if won, encounter danger if lost.
        /// Call this after trivia (3, 2) was already played
        /// </summary>
        /// <param name="triviaOutcome">Whether you won or lost the trivia game</param>
        public void EarnArrows(GameResult triviaOutcome)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Play trivia to learn a secret. Can happen in any room.
        /// Learn something if won, encounter danger if lost.
        /// Call this after trivia (3, 2) was already played.
        /// </summary>
        /// <param name="triviaOutcome">Whether you won or lost the trivia game</param>
        public void EarnSecret(GameResult triviaOutcome)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the player successfully earns a secret. Generate a new secret and give it to the player.
        /// </summary>
        public void GenerateSecret()
        {
            throw new NotImplementedException();
        }

        public GameLocations.GameLocations.RoomType GetRoomType(ushort roomNumber)
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
            return gameLocations.GetPlayer().Coins;
        }

        public int GetArrowCount()
        {
            return (int)gameLocations.GetPlayer().Arrows;
        }

        public bool SubmitTriviaAnswer(int guess)
        {
            return trivia.SubmitAnswer(guess);
        }

        public Trivia.AskableQuestion GetTriviaQuestion()
        {
            return trivia.GetQuestion();
        }

        public void StartGame()
        {
            // Make sure you're on the start screen so that we don't run into weird issues with the internal state not
            // being prepared to handle that controller state
            ValidateScene(new []{StartScreen},InRoom);
            this.state = InRoom;
        }

        /// <summary>
        /// Meant to be used as validation for methods to prevent UI from getting any funny ideas. Throws an invalid operations exception if the current state is not in the valid states
        /// </summary>
        /// <param name="validStates">The list of states that you are allowed to be in to use the method</param>
        /// <param name="attemptedState">The state that the called is attempting to change to</param>
        /// <exception cref="InvalidOperationException">Thrown if you are not in the valid states to call the function</exception>
        private void ValidateScene(ControllerState[] validStates, ControllerState attemptedState)
        {
            if (!validStates.Contains(state))
            {
                throw new InvalidOperationException(
                    $"You cannot go to {attemptedState} from {state}. The only valid options are {validStates}");
            }
        }
    }
}