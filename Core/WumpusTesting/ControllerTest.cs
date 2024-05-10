using System;
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

                outputFile.WriteLine("N");
                for (int i = 0; i < 29; i++)
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

        private static void CreateNewController()
        {
            // This will just create it at global controller which is what we want. Resharper doesn't like this, but it's fine
            // ReSharper disable once ObjectCreationAsStatement
            new Controller("./questions.json","./",0);
        }

        [TestMethod]
        public void TestGlobalController()
        {
            Assert.AreEqual(Controller.GlobalController, Controller.GlobalController);
        }

        [TestMethod]
        public void SimulateGames()
        {
            // Run through it 1000 times to make sure that stuff doesn't happen randomly
            for (int i = 0; i < 1000; i++)
            {
                // Setup
                Controller.Random = new Random(i);

                // Start the game
                Assert.AreEqual(Controller.GlobalController.GetState(), ControllerState.StartScreen);
                Controller.GlobalController.StartGame();

                // Verify that the player starts in an empty room
                Assert.AreEqual(Controller.GlobalController.GetState(), ControllerState.InRoom);
                Assert.AreEqual(Controller.GlobalController.GetAnomaliesInRoom(Controller.GlobalController.GetPlayerLocation()).Count, 0);

                // Go one room north
                Controller.GlobalController.MoveInADirection(Directions.North);
                Controller.GlobalController.MoveFromHallway();


                Assert.AreNotEqual(ControllerState.InBetweenRooms,Controller.GlobalController.GetState( ));

                switch (Controller.GlobalController.GetState())
                {
                        case ControllerState.Acrobat:
                        {
                            Console.WriteLine("Acrobat");
                            break;
                        }
                        case ControllerState.Rats:
                        {
                            Console.WriteLine("Rats");
                            break;
                        }
                        case ControllerState.BatTransition:
                        {
                            Console.WriteLine("Bats");
                            break;
                        }
                        case ControllerState.InRoom:
                        {
                            Console.WriteLine("InRoom");
                            break;
                        }
                        case ControllerState.VatRoom:
                        {
                            Controller.GlobalController.StartTrivia();
                            AskableQuestion question = Controller.GlobalController.GetTriviaQuestion();

                            int questionNum = Int32.Parse(question.questionText);

                            int choice = Controller.Random.Next(0, 4);
                            if (choice==questionNum)
                            {
                                // This should succeed
                                Assert.IsTrue(Controller.GlobalController.SubmitTriviaAnswer(choice));
                            }
                            else
                            {
                                Assert.IsFalse(Controller.GlobalController.SubmitTriviaAnswer(choice));
                            }

                            break;

                        }
                        default:
                            throw new Exception($"{Controller.GlobalController.GetState()} was not handled in the test (this is bad)");
                }

                CreateNewController();
            }
        }
    }
}