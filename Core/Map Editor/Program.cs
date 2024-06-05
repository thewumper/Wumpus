using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WumpusCore.Topology;

namespace Map_Editor
{
    /// <summary>
    /// Quick tool used to quickly create maps.
    /// </summary>
    internal class MapEditor
    {
        public static void Main(string[] args)
        {
            using (StreamWriter outputFile = new StreamWriter("navigation.map"))
            {
                for (int i = 0; i < 30; i++)
                {
                    outputFile.WriteLine("N,NE,SE,S,SW,NW");
                }
            }

            ITopology topology = new Topology("navigation.map");
            IRoom currentRoom = topology.GetRoom(0);
            HashSet<Directions>[] rooms = new HashSet<Directions>[30];
            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i] = new HashSet<Directions>();
            }
            
            bool running = true;
            while (running)
            {
                Console.Write("wumpus@map-editor $ ");
                string[] command = Console.ReadLine()?.Split(' ');
                
                switch (command[0])
                {
                    case "quit":
                    case "q":
                        running = false;
                        break;
                    case "help":
                    case "h":
                        Console.WriteLine("quit: quits the game");
                        Console.WriteLine("help: displays this message");
                        Console.WriteLine("create: creates a new map");
                        Console.WriteLine("save [filename]: saves the current map");
                        Console.WriteLine("move [Direction]: move in a direction");
                        Console.WriteLine("carve [Direction]: create a door from the current room to another room in a given direction");
                        Console.WriteLine("teleport [Room#]: teleport to the given room id");
                        Console.WriteLine("whereami: prints your room");
                        break;
                    case "create":
                        WriteWarning("Warning! This will override current room. Proceed? [Y/n]");
                        if (Console.ReadLine()?.ToLower() == "n")
                        {
                            WriteError("Aborting");
                        }
                        else
                        {
                            rooms = new HashSet<Directions>[30];
                            for (int i = 0; i < rooms.Length; i++)            
                            {                                                 
                                rooms[i] = new HashSet<Directions>();            
                            }                                                 
                            currentRoom = topology.GetRoom(1);
                            Console.WriteLine("New map created");
                        }
                        break;
                    case "save":
                    case "s":
                        if (command.Length <= 1)
                        {
                            WriteError("Expected more arguments. Your dumb");
                            break;
                        }
                        using (StreamWriter outFile = new StreamWriter(command[1]))
                        {
                            foreach (HashSet<Directions> room in rooms)
                            {
                                string data = "";
                                Directions[] directionsArray = room.ToArray();
                                for (var i = 0; i < room.Count; i++)
                                {
                                    var direction = directionsArray[i];
                                    data += DirectionHelper.GetShortNameFromDirection(direction);
                                    if (i != room.Count)
                                    {
                                        data += ",";
                                    }
                                }
                                outFile.WriteLine(data);
                            }
                            Console.WriteLine($"Saved map to: {command[1]}");
                        }  
                        break;   
                    case "move":
                    case "m":
                        if (command.Length <= 1)                                 
                        {                                                       
                            WriteError("Expected more arguments. Your dumb");   
                            break;                                              
                        }
                        try
                        {
                            Directions direction = DirectionHelper.GetDirectionFromShortName(command[1]);
                            IRoom previousRoom = currentRoom;
                            currentRoom = currentRoom.ExitRooms[direction];
                            Console.WriteLine($"Moved {direction} from {previousRoom.Id}, now in {currentRoom.Id}");
                        }
                        catch (KeyNotFoundException e)
                        {
                            WriteError($"{command[1]} is not a valid direction please use N,NE,SE,S,SW,NW");
                        }
                        break;
                    case "carve":                                                                                        
                    case "c":                                                                                           
                        if (command.Length <= 1)                                                                         
                        {                                                                                               
                            WriteError("Expected more arguments. Your dumb");                                           
                            break;                                                                                      
                        }                                                                                               
                        try                                                                                             
                        {                                                                                               
                            Directions direction = DirectionHelper.GetDirectionFromShortName(command[1]);               
                            rooms[currentRoom.Id].Add(direction);
                            
                            IRoom previousRoom = currentRoom;
                            currentRoom = currentRoom.ExitRooms[direction];
                            rooms[currentRoom.Id].Add(direction.GetInverse());
                            Console.WriteLine($"Carved {direction} from {previousRoom.Id}, now in {currentRoom.Id}");     
                        }                                                                                               
                        catch (KeyNotFoundException e)                                                                  
                        {                                                                                               
                            WriteError($"{command[1]} is not a valid direction please use N,NE,SE,S,SW,NW");            
                        }                                                                                               
                        break;
                    case "teleport":
                    case "tp":
                        if (command.Length <= 1)                              
                        {                                                    
                            WriteError("Expected more arguments. Your dumb");
                            break;                                   
                        }

                        try
                        {
                            ushort roomNum = ushort.Parse(command[1]);
                            if (roomNum < 29)
                            {
                                currentRoom = topology.GetRoom(roomNum);
                                Console.WriteLine($"Teleported to {roomNum}");
                            }
                            else
                            {
                                WriteError("That's not a valid room number");
                            }
                        }
                        catch (FormatException)
                        {
                            WriteError("That's not a number silly");
                        }
                        break;
                    case "whereami":
                        Console.WriteLine($"I am in {currentRoom.Id}");
                        break;
                    default:
                         WriteError("Invalid command");
                         break;
                }
            }
        }

        private static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        private static void WriteWarning(string warning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(warning);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}