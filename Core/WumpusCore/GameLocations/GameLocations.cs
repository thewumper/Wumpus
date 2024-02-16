using System;
using System.Collections.Generic;

namespace WumpusCore.GameLocations
{
    public class GameLocations
    {
        /// <summary>
        /// All possible types of rooms.
        /// </summary>
        public enum RoomType
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
        private Room[] rooms;

        public GameLocations(int numRooms)
        {
            rooms = new Room[numRooms];
        }

        /// <summary>
        /// Gets a random empty room.
        /// </summary>
        /// <returns>A random room of <c>RoomType</c> type <c>Flats</c> from the <c>rooms</c> list.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ushort GetEmptyRoom()
        {
            List<ushort> positions = new List<ushort>();
            for (ushort i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].type == RoomType.Flats)
                {
                    positions.Add(i);
                }
            }
            if (positions.Count <= 0)
            {
                throw new InvalidOperationException("There are no empty rooms.");
            }
            return positions[Controller.Controller.Random.Next(0, positions.Count + 1)];
        }

        /// <summary>
        /// Adds a room to the <c>rooms</c> list.
        /// </summary>
        /// <param name="type">The <c>RoomType</c> type of room to add.</param>
        /// <param name="pos">The position of the room to add.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddRoom(RoomType type, ushort pos)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (pos == rooms[i].pos)
                {
                    throw new ArgumentException("A room already exists at that position.");
                }
            }
            rooms.Add(new Room(type, pos));
        }

        /// <summary>
        /// Sets the room at <c>index</c> to another <c>RoomType</c> type.
        /// </summary>
        /// <param name="type">The <c>RoomType</c> type to set the room to.</param>
        /// <param name="index">The index of the room on the <c>rooms</c> list to change the <c>RoomType</c> type of.</param>
        public void SetRoom(RoomType type, ushort index)
        {
            rooms[index] = new Room(type, index);
        }

        /// <summary>
        /// Gets the <c>RoomType</c> type of a room at <c>index</c> in the <c>rooms</c> list.
        /// </summary>
        /// <param name="index">The index of the room on the <c>rooms</c> list to check the type of.</param>
        /// <returns>The <c>RoomType</c> type of room at the given location.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RoomType GetRoomAt(int index)
        {
            return rooms[index].type;
        }

        public RoomType GetRoomAtPos(int pos)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].pos == pos)
                {
                    return rooms[i].type;
                }
            }
            throw new ArgumentException("There is no room with that position.");
        }

        public int GetIndexFromPos(int pos)
        {

        }

        public int GetPosFromIndex()
        {

        }

        /// <summary>
        /// A type that stores a <c>RoomType</c> type and its position.
        /// </summary>
        private struct Room
        {
            public RoomType type { get; private set; }
            public ushort pos { get; private set; }

            public Room(RoomType type, ushort pos)
            {
                this.type = type;
                this.pos = pos;
            }
        }
    }
}
