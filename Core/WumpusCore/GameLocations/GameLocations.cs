using System;
using System.Collections.Generic;

namespace WumpusCore.GameLocations
{
    public class GameLocations
    {
        /// <summary>
        /// All possible types of rooms.
        /// </summary>
        public enum RoomTypes
        {
            Flats,
            Vats,
            Bats,
            Rats,
            Acrobat
        }

        /// <summary>
        /// The list of rooms and their positions.
        /// </summary>
        private List<Room> rooms;

        /// <summary>
        /// The random object from the controller class.
        /// </summary>
        Random random;

        public GameLocations()
        {
            rooms = new List<Room>();
            random = Controller.Controller.Random;
        }

        /// <summary>
        /// Gets a random empty room.
        /// </summary>
        /// <returns>A random empty room.</returns>
        public int GetEmptyRoom()
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].type == RoomTypes.Flats)
                {
                    positions.Add(i);
                }
            }
            return positions[random.Next(0, positions.Count + 1)];
        }

        /// <summary>
        /// Adds a room to the List.
        /// </summary>
        /// <param name="type">The type of room to add.</param>
        /// <param name="pos">The position of the room to add.</param>
        public void AddRoom(RoomTypes type, uint pos)
        {
            rooms.Add(new Room(type, pos));
        }

        /// <summary>
        /// Gets the type of room at a certain position.
        /// </summary>
        /// <param name="pos">The position to check the type of.</param>
        /// <returns>The type of room at the given location.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RoomTypes GetRoomAt(int pos)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].pos == pos)
                {
                    return rooms[i].type;
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// A type that stores a RoomType and its position.
        /// </summary>
        private struct Room
        {
            public RoomTypes type { get; private set; }
            public uint pos { get; private set; }

            public Room(RoomTypes type, uint pos)
            {
                this.type = type;
                this.pos = pos;
            }
        }
    }
}
