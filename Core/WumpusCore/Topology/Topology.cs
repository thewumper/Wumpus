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
                ushort room = 0;
                while ((line = mapData.ReadLine()) != null)
                {
                    string[] tokens = line.Split(',');
                    Directions[] directions = new Directions[tokens.Length];
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        directions[i] = DirectionHelper.GetDirectionFromShortName(tokens[i]);
                    }
                    rooms[room] = new Room(directions,room);
                    rooms[room].InitializeConnections(this);
                    room++;
                }
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
            return rooms[id];
        }


        private  IRoom RoomFromDirection(ushort currentRoom, Directions direction)
        {
            ushort result = currentRoom;

            switch (direction)
            {
                case Directions.North:
                    result -= 6;
                    break;
                case Directions.NorthEast:
                    if (currentRoom % 2 == 0)
                        result += 1;
                    else
                        result -= 5;
                    break;
                case Directions.SouthEast:
                    if (currentRoom % 2 == 0)
                        result += 7;
                    else
                        result += 1;
                    break;
                case Directions.South:
                    result += 6;
                    break;
                case Directions.SouthWest:
                    if (currentRoom % 2 == 0)
                        result += 5;
                    else
                        result -= 1;
                    break;
                case Directions.NorthWest:
                    if (currentRoom % 2 == 0)
                        result -= 1;
                    else
                        result -= 7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Invalid Direction");
            }
            
            return rooms[(result - 1) % 30];
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
                ExitRooms = new IRoom[ExitDirections.Length];
                for (int i = 0; i < ExitRooms.Length; i++)
                {
                    ExitRooms[i] = topology.RoomFromDirection(Id, ExitDirections[i]);
                }
            }
            
            /// <summary>
            /// Directions of exits in the room
            /// </summary>
            public Directions[] ExitDirections { get; }
            /// <summary>
            /// Connected rooms, in same order as ExitDirections
            /// </summary>
            public IRoom[] ExitRooms { get; private set; }
            /// <summary>
            /// This rooms ID
            /// </summary>
            public ushort Id { get; }
        }
    }


}