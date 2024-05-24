using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
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
            for (int i = 0; i < 100; i++)
            {
                // Setup
                Controller controller = CreateNewController();

                // Start the game

                Assert.AreEqual(controller.GetState(), ControllerState.StartScreen);
                controller.StartGame();

                // Verify that the player starts in an empty room
                Assert.AreEqual(controller.GetState(), ControllerState.InRoom);
                Assert.AreEqual(controller.GetAnomaliesInRoom(controller.GetPlayerLocation()).Count, 0);

                // while (Controller.GlobalController.GetState() != ControllerState.GameOver)
                // {
                HandleRoomInARandomDirection(controller);
                // }
            }
        }

        private static void HandleRoomInARandomDirection(Controller controller)
        {
            // Go one room north
            controller.MoveInADirection(Directions.North);
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

                    break;
                }
                case ControllerState.BatTransition:
                {
                    controller.ExitBat();
                    break;
                }
                case ControllerState.InRoom:
                {
                    break;
                }
                case ControllerState.CatDialouge:
                {
                    controller.ExitCat();

                    break;
                }
                case ControllerState.WumpusFight:
                {
                    controller.ExitWumpus();

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
                        if (choice==questionNum)
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

                    break;

                }
                default:
                    throw new Exception($"{controller.GetState()} was not handled in the test (this is bad)");
            }
        }
    }
}