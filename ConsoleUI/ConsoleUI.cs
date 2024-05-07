using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace ConsoleUI
{
    public class ConsoleUI
    {
        // fields
        private bool running;
        private Controller controller;
        private char[] special = { ',', '.', '?', '(', ')', ':' };

        // constructor

        // methods
        // move code from Program.cs to here, as non-static (i.e. instance) methods.
        public void StartGame(Controller controller)
        {
            this.controller = controller;
            running = true;
            while (running)
            {
                switch (controller.GetState())
                {
                    case ControllerState.StartScreen:
                        MainMenu();
                        break;
                    case ControllerState.InRoom:
                        this.InRoom();
                        break;
                    case ControllerState.InBetweenRooms:
                        this.InHallway();
                        break;
                }
            }
        }

        public void SlowWrite(string text)
        {
            Random rnd = new Random();
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(rnd.Next(20, 40));
                if (special.Contains(c))
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void SlowWriteLine(string text)
        {
            SlowWrite(text);
            Thread.Sleep(400);
            Console.WriteLine("");
        }

        public void MainMenu()
        {
            SlowWriteLine("You find a fun hole in the ground. Where could it lead?!");
            SlowWriteLine("Do you wish to enter and begin an incredible adventure? (yes) (no)");
            String start = Console.ReadLine();
            if (start.ToLower().StartsWith("y"))
            {
                SlowWriteLine("You jump into the hole without a second thought.");
                SlowWriteLine("Probably should've set up a rope so you could leave...");
            }
            else
            {
                SlowWriteLine("Oopsie! looks like you slipped into the hole.");
                SlowWriteLine("That is very unfortunate.");
                SlowWriteLine("Welp, good Luck!");
            }
            SlowWriteLine("");
            SlowWriteLine("");
            SlowWriteLine("");
            controller.StartGame();
        }

        public void InRoom()
        {
            IRoom location = controller.GetCurrentRoom();
            switch (controller.GetState())
            {
                case ControllerState.InRoom:
                    SlowWriteLine("You find yourselve in a gloomy stone cavern with a dim light coming from a torch on the wall.");
                    ReportHazards();
                    SlowWrite("The rooms has 3 passages exiting it: ");
                    ReportDoors(location);

                    bool cancontinue = false;
                    String response = "";
                    while (!cancontinue)
                    {
                        SlowWriteLine("");
                        SlowWriteLine("Where do you wanna go? ");
                        response = Console.ReadLine();
                        // test rather than relying on exceptions
                        try
                        {
                            cancontinue = location.ExitDirections.Contains(DirectionHelper.GetDirectionFromShortName(response.ToUpper()));
                        }
                        catch
                        {
                            cancontinue = false;
                            SlowWrite("You can't go that way.");
                        }
                    }
                    controller.MoveInADirection(DirectionHelper.GetDirectionFromShortName(response.ToUpper()));
                    break;
                
            }
        }

        public void InHallway()
        {
            SlowWriteLine("You see something written on the wall as you traverse the passage. ");
            AnsweredQuestion AQ = controller.GetUnaskedQuestion();
            ReportHint();
            SlowWrite("You see a dim light ahead, do you wish to continue into the next room? (yes) (no)");
            
            String move = Console.ReadLine();

            if (move.ToLower().StartsWith("y"))
            {
                controller.MoveFromHallway();
            }
            /*
            String[] denied = { "You take a closer look at the writing on the wall", "You ponder the words carefully, making to sure to fully take in every word as it is written", "You have entered a state of madness, completely obsessed over the writing before you." };
            String[] question = { "Has your curiosity been satisfied? (yes) (no)", "With your mind filled with the poetry written before you, are you ready to continue your adventure? (yes) (no)", "You must break out of your trance! Say something! Anything!" };

            int timesdenied = 0;
            while ((!move.ToLower().StartsWith("y")) || timesdenied == 3);
            {

                SlowWriteLine(denied[timesdenied]);
                ReportHint();
                SlowWriteLine(question[timesdenied]);

                move = Console.ReadLine();

                timesdenied++;
            }
            if (timesdenied == 3)
            {
                SlowWriteLine("I'm glad to see you've come back to your senses.");
            }

            controller.MoveFromHallway();
            */
            SlowWriteLine("");
        }

        public void ReportHazards()
        {
            List<Controller.DirectionalHint> hazards = controller.GetHazardHints();
            for (int i = 0; i < hazards.Count; i++)
            {
                SlowWriteLine(hazards[i].ToString());
            }
        }

        public void ReportDoors(IRoom location)
        {
            foreach (Directions direction in location.ExitDirections)
            {
                SlowWrite(DirectionHelper.GetShortNameFromDirection(direction));
                Console.Write(" ");
            }
        }

        public void ReportHint()
        {
            AnsweredQuestion AQ = controller.GetUnaskedQuestion();
            SlowWriteLine("\"" + AQ.QuestionText + "\"");
            SlowWriteLine("\"Answer: " + AQ.choices[AQ.answer] + "\"");
        }
    }
}