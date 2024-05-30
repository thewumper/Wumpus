using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace WumpusTesting
{
    [TestClass]
    public class ControllerTest
    {
        public ControllerTest()
        {
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter("./map0.wmp"))
            {

                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }

            using (StreamWriter outputFile = new StreamWriter("./questions.json"))
            {
                outputFile.WriteLine("[{\"question\": \"0\", choices : [\"0\",\"1\",\"2\",\"3\"],\"answer\": 0},{\"question\": \"1\", choices : [\"0\",\"1\",\"2\",\"3\"],\"answer\": 1},{\"question\": \"2\", choices : [\"0\",\"1\",\"2\",\"3\"],\"answer\": 2},{\"question\": \"3\", choices : [\"0\",\"1\",\"2\",\"3\"],\"answer\": 3}]");
            }

            CreateNewController();
        }

        private static Controller CreateNewController()
        {
            // This will just create it at global controller which is what we want. Resharper doesn't like this, but it's fine
            // ReSharper disable once ObjectCreationAsStatement
            return new Controller("./questions.json", "./",0);
        }

        [TestMethod]
        public void TestGlobalController()
        {
            Assert.AreEqual(Controller.GlobalController, Controller.GlobalController);
        }

        [TestMethod]
        public void SimulateGames()
        {
            for (int i = 0; i < 10000; i++)
            {
                // Setup
                Controller controller = CreateNewController();
                Controller.Random = new Random(i);

                // Start the game

                Assert.AreEqual(controller.GetState(), ControllerState.StartScreen);
                controller.StartGame();

                // Verify that the player starts in an empty room
                Assert.AreEqual(controller.GetState(), ControllerState.InRoom);
                Assert.AreEqual(controller.GetAnomaliesInRoom(controller.GetPlayerLocation()).Count, 0);

                while (!(controller.GetState() == ControllerState.GameOver || controller.GetState() == ControllerState.WonGame))
                {
                    HandleRoomInARandomDirection(controller);
                }
            }
        }

        private static void HandleRoomInARandomDirection(Controller controller)
        {
            // Go one room north
            Directions[] dirs = controller.GetCurrentRoom().ExitDirections;
            Directions dir = dirs[Controller.Random.Next(0, dirs.Length)];
            controller.MoveInADirection(dir);
            controller.MoveFromHallway();


            Assert.AreNotEqual(ControllerState.InBetweenRooms,controller.GetState());

            switch (controller.GetState())
            {
                case ControllerState.Acrobat:
                {
                    if (Controller.Random.Next(0,2) == 1)
                    {
                        controller.ExitAcrobat(true);
                        Assert.AreEqual(controller.GetState(),ControllerState.InRoom);
                    }
                    else
                    {
                        controller.ExitAcrobat(false);
                        Assert.AreEqual(controller.GetState(),ControllerState.GameOver);
                    }
                    break;
                }
                case ControllerState.Rats:
                {
                    int timeInRoom = Controller.Random.Next(0, 11);
                    IStopwatch realStopwatch = controller.ratTimeStopwatch;
                    controller.ratTimeStopwatch = new FakeStopwatch(new TimeSpan(0, 0, timeInRoom));

                    RatRoomStats stats = controller.GetRatRoomStats();

                    controller.ExitRat();
                    if (stats.RemainingCoins <0)
                    {
                        Assert.AreEqual(controller.GetState(),ControllerState.GameOver);
                    }
                    else
                    {
                        Assert.IsTrue(controller.GetState() != ControllerState.GameOver && controller.GetState() != ControllerState.Rats);
                        Assert.IsTrue(controller.GetCoins() < stats.StartingCoins);
                    }

                    controller.ratTimeStopwatch = realStopwatch;

                    break;
                }
                case ControllerState.BatTransition:
                {
                    int initialLoc = controller.GetPlayerLocation();
                    controller.ExitBat();
                    Assert.AreNotEqual(initialLoc, controller.GetPlayerLocation());
                    break;
                }
                case ControllerState.InRoom:
                {
                    break;
                }
                case ControllerState.CatDialouge:
                {
                    int choice = Controller.Random.Next(0, 2);


                    if (choice == 0)
                    {
                        int previousCoins = controller.GetCoins();
                        controller.AttemptToTameCat(controller.GetCoins());

                        Assert.IsTrue(controller.GetCoins() <= previousCoins);
                    }

                    break;
                }
                case ControllerState.WumpusFight:
                {
                    int result = Controller.Random.Next(0, 2);
                    controller.ExitWumpusFight(result == 0);

                    break;
                }
                case ControllerState.VatRoom:
                {
                    controller.StartTrivia();

                    List<AskableQuestion> previousQuestions = new List<AskableQuestion>();

                    int successCount = 0;

                    while (controller.GetState() == ControllerState.VatRoom)
                    {
                        AskableQuestion question = controller.GetTriviaQuestion();
                        Assert.IsFalse(previousQuestions.Contains(question));
                        previousQuestions.Add(question);

                        int questionNum = Int32.Parse(question.questionText);

                        int choice = Controller.Random.Next(0, 4);
                        if (choice == questionNum)
                        {
                            // This should succeed
                            Assert.IsTrue(controller.SubmitTriviaAnswer(choice));
                            successCount += 1;
                        }
                        else
                        {
                            Assert.IsFalse(controller.SubmitTriviaAnswer(choice));
                        }
                    }

                    if (successCount >= 2)
                    {
                        Assert.IsTrue(controller.GetState() == ControllerState.InRoom);
                    }
                    else
                    {
                        Assert.IsTrue(controller.GetState()==ControllerState.GameOver);
                    }

                    // This resets the trivia questions back to the original array
                    controller.trivia.questions = new Questions("./questions.json");

                    break;

                }
                default:
                    throw new Exception($"{controller.GetState()} was not handled in the test (this is bad)");
            }
        }
    }
}