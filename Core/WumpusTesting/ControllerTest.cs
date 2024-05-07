using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpusCore.Controller;
using WumpusCore.Topology;

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
                outputFile.WriteLine("[{\"question\": \"Which is right\", choices : [\"correct\",\"wrong\",\"wrong\",\"wrong\"],\"answer\": 0}]");
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
                try
                {
                    Assert.AreNotEqual(ControllerState.InBetweenRooms,Controller.GlobalController.GetState( ));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

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
                            Console.WriteLine("Vatroom");
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