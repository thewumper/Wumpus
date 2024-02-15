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
        private Random random;

        public GameLocations()
        {
            rooms = new List<Room>();
            random = Controller.Controller.Random;
        }

        /// <summary>
        /// Initializes the <c>rooms</c> list with rooms of <c>RoomTypes</c> type <c>Flats</c>.
        /// </summary>
        /// <param name="numRooms">The total amount of rooms.</param>
        public void InitRooms(ushort numRooms)
        {
            for (ushort i  = 0; i < numRooms; i++)
            {
                AddRoom(RoomTypes.Flats, (ushort)(i+1));
            }
        }

        /// <summary>
        /// Gets a random empty room.
        /// </summary>
        /// <returns>A random room of <c>RoomTypes</c> type <c>Flats</c> from the <c>rooms</c> list.</returns>
        public ushort GetEmptyRoom()
        {
            List<ushort> positions = new List<ushort>();
            for (ushort i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].type == RoomTypes.Flats)
                {
                    positions.Add(i);
                }
            }
            return positions[random.Next(0, positions.Count + 1)];
        }

        /// <summary>
        /// Adds a room to the <c>rooms</c> list.
        /// </summary>
        /// <param name="type">The <c>RoomTypes</c> type of room to add.</param>
        /// <param name="pos">The position of the room to add.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddRoom(RoomTypes type, ushort pos)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (pos == rooms[i].pos && rooms[i].type != RoomTypes.Flats)
                {
                    throw new ArgumentException();
                }
            }
            rooms.Add(new Room(type, pos));
        }

        /// <summary>
        /// Sets the room at <c>index</c> to another <c>RoomTypes</c> type.
        /// </summary>
        /// <param name="type">The <c>RoomTypes</c> type to set the room to.</param>
        /// <param name="index">The index of the room on the <c>rooms</c> list to change the <c>RoomType</c> type of.</param>
        public void SetRoom(RoomTypes type, ushort index)
        {
            rooms.RemoveAt(index);
            rooms.Insert(index, new Room(type, (ushort)(index+1)));
        }

        /// <summary>
        /// Gets the <c>RoomTypes</c> type of a room at <c>index</c> in the <c>rooms</c> list.
        /// </summary>
        /// <param name="index">The index of the room on the <c>rooms</c> list to check the type of.</param>
        /// <returns>The <c>RoomTypes</c> type of room at the given location.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RoomTypes GetRoomAt(int index)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].pos == index)
                {
                    return rooms[i].type;
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// A type that stores a <c>RoomTypes</c> type and its position.
        /// </summary>
        private struct Room
        {
            public RoomTypes type { get; private set; }

            public ushort pos { get; private set; }

            public Room(RoomTypes type, uint pos)
            {
                this.type = type;
                this.pos = pos;
            }
        }
    }
}
