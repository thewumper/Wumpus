using System;
using System.Collections.Generic;
using System.Linq;
using WumpusCore.Entity;

namespace WumpusCore.GameLocations
{
    public class GameLocations
    {
        /// <summary>
        /// All entities in the game
        /// </summary>
        private Dictionary<EntityType, Entity.Entity> entities;
        
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
        /// Creates an entity in the game
        /// </summary>
        /// <param name="e">Places the entity into entities</param>
        public void AddEntity(Entity.Entity e)
        {
            if (entities.ContainsKey(e.Type))
            {
                throw new ArgumentException("Entity type already created in GameLocations!");
            }

            entities[e.Type] = e;
        }

        public Entity.Entity GetEntity(EntityType type)
        {
            if (!entities.ContainsKey(type))
            {
                throw new ArgumentException("Cannot retrieve entity. Entity type not present in GameLocations!");
            }

            return entities[type];
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
        /// <param name="index">The index of the room on the <see cref="rooms">rooms</see> array to change the <see cref="RoomType">RoomType</see> type of.</param>
        /// <param name="type">The <see cref="RoomType">RoomType</see> type to set the room to.</param>
        public void SetRoom(ushort index, RoomType type)
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
