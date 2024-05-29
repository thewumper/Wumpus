using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WumpusCore.Topology
{
    /// <summary>
    /// Represents a map which contains 30 rooms
    /// </summary>
    public class Topology : ITopology
    {
        /// <summary>
        /// Array of rooms
        /// </summary>
        private readonly Room[] rooms;

        /// <summary>
        /// The number of rooms in the map
        /// </summary>
        public ushort RoomCount
        {
            get
            {
                return (ushort)rooms.Length;
            }
        }

        /// <summary>
        /// Consturct a topology based on a Filestream object
        /// </summary>
        /// <param name="stream">The filestream to load from</param>
        /// <exception cref="NullReferenceException">The number of rooms is not 30</exception>
        public Topology(FileStream stream)
        {

            // Initialize the arrays to be size 30
            List<Directions>[] roomExits = new List<Directions>[30];
            rooms = new Room[30];

            // Read the map data until there is no more lines, It will throw if the map has to many directions
            using (StreamReader mapData = new StreamReader(stream))
            {
                string line;
                ushort room = 0;
                // Keep reading until the file is done
                while ((line = mapData.ReadLine()) != null)
                {
                    // Split the directions
                    string[] tokens = line.Split(',');
                    Directions[] directions = new Directions[tokens.Length];
                    // Parse the directions
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        directions[i] = DirectionHelper.GetDirectionFromShortName(tokens[i]);
                    }
                    // Create a room
                    rooms[room] = new Room(directions, room);
                    room++;
                }
            }
            // Connect all of the rooms
            foreach (Room room in rooms)
            {
                if (room == null)
                {
                    throw new NullReferenceException(
                        "Room wasn't initialized properly, make sure your map has 30 rooms.");
                }

                room.InitializeConnections(this);
            }
        }

        /// <summary>
        /// Creates topology from filepath to map data
        /// </summary>
        /// <param name="filePath">path</param>
        public Topology(string filePath): this(LoadFilepath(filePath))
        {
        }

        private static FileStream LoadFilepath(string filePath)
        {

            // Check if the file exists
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Could not find map data @{filePath}");
            // Open the file
            return File.Open(filePath, FileMode.Open);
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

        /// <summary>
        /// Get the room adjacent to another room in a direction
        /// </summary>
        /// <param name="currentRoom">The room to start in</param>
        /// <param name="direction">The direction to go in</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If an invalid direction is provided</exception>
        private  IRoom RoomFromDirection(ushort currentRoom, Directions direction)
        {
            // Cave size
            int width = 6;
            int height = 5;
            
            // Hexagon representing input room
            int row = currentRoom / width;
            int column = currentRoom % width;
            Hexagon room = new Hexagon(row, column);

            // Find the next room
            Hexagon nextRoom = room.GetFromDirection(direction);
            row = nextRoom.row;
            column = nextRoom.column;
            
            // Wrap around
            if (row < 0)
            {
                row += height;
            }
            else if (row >= height)
            {
                row -= height;
            }

            if (column < 0)
            {
                column += width;
            } 
            else if (column >= width)
            {
                column -= width;
            }
            
            // Find the room in array
            return rooms[row * width + column];
        }
        
        /// <summary>
        /// Function for DistanceBetweenRooms. Pass into getConnections to navigate ignoring all obstacles.
        /// </summary>
        public static Func<IRoom, IRoom[]> NavigateBoundless = room => room.AdjacentRooms.Values.ToArray();

        /// <summary>
        /// Function for DistanceBetweenRooms. Pass into getConnections to navigate only through accessible doors.
        /// </summary>
        public static Func<IRoom, IRoom[]> NavigateDoors = room => room.ExitRooms.Values.ToArray();

        /// <summary>
        /// Finds the distance in room movements between two given room indices, ignoring walls, doors, and obstacles.
        /// Uses Dijkstra's algorithm
        /// </summary>
        /// <param name="roomIndexA">The first room, the start of the search</param>
        /// <param name="roomIndexB">The second room, the end point of the search</param>
        /// <param name="getConnections">Takes in an IRoom and returns an array of all rooms that should be navigable to. Use to define which paths are considered to route through.</param>
        /// <returns>The distance in movements between the two rooms</returns>
        public int DistanceBetweenRooms(ushort startRoomIndex, ushort endRoomIndex, Func<IRoom, IRoom[]> getConnections)
        {

            if (startRoomIndex == endRoomIndex)
            {
                return 0;
            }

            int[] distance = new int[RoomCount];
            bool[] visited = new bool[RoomCount];
            for (int i = 0; i < RoomCount; i++)
            {
                distance[i] = int.MaxValue;
                visited[i] = false;
            }

            distance[startRoomIndex] = 0;
            ushort currentRoomIndex = startRoomIndex;

            while (true)
            {
                IRoom currentRoom = rooms[currentRoomIndex];

                for (int i = 0; i < getConnections(currentRoom).Length; i++)
                {
                    // Distance between each adjacent room is 1
                    int index = getConnections(currentRoom)[i].Id;
                    int newDistance = distance[currentRoomIndex] + 1;
                    if (newDistance < distance[index])
                    {
                        distance[index] = newDistance;
                    }
                }

                visited[currentRoomIndex] = true;

                int minimum = Int32.MaxValue;
                int minimumIndex = -1;

                // Find the node with the smallest distance
                for (ushort i = 0; i < visited.Length; i++)
                {
                    // Skip if we've already seen this one
                    if (visited[i]) {continue;}

                    // Skip if bigger than others
                    if (distance[i] >= minimum) {continue;}

                    minimum = distance[i];
                    minimumIndex = i;
                }

                if (minimum == Int32.MaxValue)
                {
                    // No more reachable nodes.
                    return distance[endRoomIndex];
                }

                if (minimum == -1)
                {
                    // How?
                    throw new InvalidOperationException();
                }

                currentRoomIndex = (ushort)minimumIndex;
            }
        }

        /// <summary>
        /// Internally used to keep track of rooms
        /// </summary>
        private class Room : IRoom
        {
            /// <summary>
            /// Create a room
            /// </summary>
            /// <param name="exitDirections">Ways you can leave the room</param>
            /// <param name="id">The room id</param>
            public Room(Directions[] exitDirections, ushort id)
            {
                ExitDirections = exitDirections;
                Id = id;
            }
            /// <summary>
            /// Setup the connnections in this room, please only call once
            /// </summary>
            /// <param name="topology"></param>
            public void InitializeConnections(Topology topology)
            {
                ExitRooms = new Dictionary<Directions, IRoom>();
                foreach (var direction in ExitDirections)
                {
                    ExitRooms[direction] = topology.RoomFromDirection(Id, direction);
                }

                AdjacentRooms = new Dictionary<Directions, IRoom>();
                for (int i = 0; i < 6; i++)
                {
                    Directions direction = (Directions)i;
                    AdjacentRooms[direction] = topology.RoomFromDirection(Id, direction);
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
            /// All six adjacent rooms, in same order as ExitDirections
            /// </summary>
            public Dictionary<Directions, IRoom> AdjacentRooms { get; private set; }
            /// <summary>
            /// This rooms ID
            /// </summary>
            public ushort Id { get; }
        }

        public IRoom[] GetRooms()
        {
            return rooms;
        }
    }


}