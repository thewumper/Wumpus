using System;
using System.Collections.Generic;
using System.IO;

namespace WumpusCore.Topology
{
    public class Topology : ITopology
    {
        /// <summary>
        /// Array of rooms
        /// </summary>
        private readonly Room[] rooms;
        
        /// <summary>
        /// Creates topology from filepath to map data
        /// </summary>
        /// <param name="filePath">path</param>
        public Topology(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Could not find map data @{filePath}");
            FileStream stream = File.Open(filePath, FileMode.Open);
            

            List<Directions>[] roomExits = new List<Directions>[30];

            rooms = new Room[30];
            using (StreamReader mapData = new StreamReader(stream))
            {
                string line;
                ushort room = 1;
                while ((line = mapData.ReadLine()) != null)
                {
                    string[] tokens = line.Split(',');
                    Directions[] directions = new Directions[tokens.Length];
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        directions[i] = DirectionHelper.GetDirectionFromShortName(tokens[i]);
                    }
                    rooms[room - 1] = new Room(directions, room);
                    room++;
                }
            }

            foreach (Room room in rooms)
            {
                room.InitializeConnections(this);
            }
        }
        /// <summary>
        /// Creates topology from a folder containing maps and the id of the map
        /// </summary>
        /// <param name="folder">Path to folder containing maps</param>
        /// <param name="mapId">ID of the map</param>
        public Topology(string folder, ushort mapId) : this($"{folder}/map{mapId}.wmp")
        {
            
        }
        /// <summary>
        /// Returns a room in the cave given and ID
        /// </summary>
        /// <param name="id">ID of the room</param>
        /// <returns></returns>
        public IRoom GetRoom(ushort id)
        {
            return rooms[id - 1];
        }


        private  IRoom RoomFromDirection(ushort currentRoom, Directions direction)
        {
            ushort result = currentRoom;

            bool isEdge = currentRoom % 6 <= 1;
            bool isEven = currentRoom % 2 == 0;
            switch (direction)
            {
                case Directions.North:
                    result -= 6;
                    break;
                case Directions.South:
                    result += 6;
                    break;
                case Directions.NorthWest:
                    if (isEdge || isEven)
                    {
                        result -= 1;
                    }
                    else
                    {
                        result -= 7;
                    }
                    break;
                case Directions.SouthEast:
                    if (isEdge || !isEven)
                    {
                        result += 1;
                    }
                    else
                    {
                        result += 7;
                    }
                    break;
                case Directions.NorthEast:
                    if (isEdge || !isEven)
                    {
                        result -= 5;
                    }
                    else
                    {
                        result += 1;
                    }

                    break;
                case Directions.SouthWest:
                    if (isEdge || isEven)
                    {
                        result += 5;
                    }
                    else
                    {
                        result -= 1;
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid Direction");
            }

            int roomNum = (result - 1) % 30; // Probably could us unsafe and wrap around the ushort but I am not fully sure exactly how that owuld work
            return rooms[roomNum >= 0 ? roomNum : roomNum + 30];
        }
        
        
        
        /// <summary>
        /// Internally used to keep track of rooms
        /// </summary>
        private class Room : IRoom
        {
            public Room(Directions[] exitDirections, ushort id)
            {
                ExitDirections = exitDirections;
                Id = id;
            }

            public void InitializeConnections(Topology topology)
            {
                ExitRooms = new Dictionary<Directions, IRoom>();
                foreach (var direction in ExitDirections)
                {
                    ExitRooms[direction] = topology.RoomFromDirection(Id, direction);
                }
            }
            
            /// <summary>
            /// Directions of exits in the room
            /// </summary>
            public Directions[] ExitDirections { get; }
            /// <summary>
            /// Connected rooms, in same order as ExitDirections
            /// </summary>
            public Dictionary<Directions, IRoom> ExitRooms { get; private set; }
            /// <summary>
            /// This rooms ID
            /// </summary>
            public ushort Id { get; }
        }
    }


}