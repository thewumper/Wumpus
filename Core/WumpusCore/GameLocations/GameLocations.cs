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
        private RoomType[] rooms;
        
        /// <summary>
        /// Contains most methods and data to do with rooms.
        /// </summary>
        /// <param name="numRooms">The total amount of rooms.</param>
        public GameLocations(ushort numRooms)
        {
            rooms = new RoomType[numRooms];
        }

        /// <summary>
        /// Gets a random empty room from the <see cref="rooms">rooms</see>.
        /// </summary>
        /// <returns>A random room of <see cref="RoomType">RoomType</see> type <c>Flats</c> from the <see cref="rooms">rooms</see> array.</returns>
        /// <exception cref="InvalidOperationException">When there are no empty rooms.</exception>
        public ushort GetEmptyRoom()
        {
            List<ushort> positions = new List<ushort>();
            for (ushort i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] == RoomType.Flats)
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
        /// Adds a room to the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <param name="type">The <see cref="RoomType">RoomType</see> type of room to add.</param>
        /// <param name="index">The index of the room to be added on the <see cref="rooms">rooms</see> array.</param>
        /// <exception cref="ArgumentException">When a room already exists at <c>index</c>.</exception>
        public void AddRoom(RoomType type, ushort index)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (index == i)
                {
                    throw new ArgumentException("A room already exists at that position.");
                }
            }

            rooms[index] = type;
        }

        /// <summary>
        /// Sets the room at <c>index</c> to another <see cref="RoomType">RoomType</see> type.
        /// </summary>
        /// <param name="type">The <see cref="RoomType">RoomType</see> type to set the room to.</param>
        /// <param name="index">The index of the room on the <see cref="rooms">rooms</see> list to change the <see cref="RoomType">RoomType</see> type of.</param>
        public void SetRoom(RoomType type, ushort index)
        {
            rooms[index] = type;
        }

        /// <summary>
        /// Gets the <see cref="RoomType">RoomType</see> type of a room at <c>index</c> in the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <param name="index">The index of the room on the <see cref="rooms">rooms</see> list to check the type of.</param>
        /// <returns>The <see cref="RoomType">RoomType</see> type of room at the given location.</returns>
        public RoomType GetRoomAt(ushort index)
        {
            return rooms[index];
        }
    }
}
