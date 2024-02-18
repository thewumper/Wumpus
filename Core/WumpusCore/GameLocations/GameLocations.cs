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
        /// The array of rooms.
        /// </summary>
        private RoomType[] rooms;
        
        /// <summary>
        /// The array of rooms.
        /// </summary>
        public RoomType[] Rooms
        {
            get { return rooms; }
        }
        
        /// <summary>
        /// Contains most methods and data to do with rooms.
        /// </summary>
        /// <param name="numRooms">The total amount of rooms.</param>
        public GameLocations(ushort numRooms)
        {
            rooms = new RoomType[numRooms];
        }

        /// <summary>
        /// Gets a random empty room from the <see cref="rooms">rooms</see> array.
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
        /// Sets the room at <c>index</c> to another <see cref="RoomType">RoomType</see> type.
        /// </summary>
        /// <param name="type">The <see cref="RoomType">RoomType</see> type to set the room to.</param>
        /// <param name="index">The index of the room on the <see cref="rooms">rooms</see> array to change the <see cref="RoomType">RoomType</see> type of.</param>
        public void SetRoom(RoomType type, ushort index)
        {
            rooms[index] = type;
        }

        /// <summary>
        /// Gets the <see cref="RoomType">RoomType</see> type of a room at <c>index</c> in the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <param name="index">The index of the room on the <see cref="rooms">rooms</see> array to check the type of.</param>
        /// <returns>The <see cref="RoomType">RoomType</see> type of room at the given location.</returns>
        public RoomType GetRoomAt(ushort index)
        {
            return rooms[index];
        }
    }
}
