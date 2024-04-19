using System;
using System.Collections.Generic;
using System.Linq;
using WumpusCore.Entity;
using WumpusCore.Topology;

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
        
        // Whether there is a coin in the hallway out from a given room
        public Dictionary<Directions, bool>[] hallwayCoins;



        private bool[] triviaRemaining;

        public bool GetTriviaAvailable(int index)
        {
            // Trivia is not available in hazard rooms
            if (GetRoomAt((ushort)index) == RoomType.Flats)
            {
                return triviaRemaining[index];
            } 
            return false;
        }

        public void SetTriviaRemaining(int index, bool remaining)
        {
            triviaRemaining[index] = remaining;
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
        /// <param name="numVats">The number of vat rooms to generate</param>
        /// <param name="numBats">The number of bat rooms to generate</param>
        /// <param name="numRats">The number of rat rooms to generate</param>
        /// <param name="numAcrobats">The number of acrobat rooms to generate</param>
        /// <param name="topology">The topology structure</param>
        /// <param name="random">A random object</param>
        public GameLocations(int numRooms,int numVats, int numBats, int numRats, int numAcrobats, Topology.Topology topology, Random random)
        {
            if (numVats + numRats + numAcrobats + numBats >= numRooms)
            {
                throw new ArgumentException("Too many hazards!");
            }
            
            rooms = new RoomType[numRooms];
            int hardHazards = (numVats + numBats);
            Graph graph = new Graph(new List<IRoom>(topology.GetRooms()));
            
            List<IRoom> solutions = new List<IRoom>(graph.GetRandomPossibleSolutions(hardHazards)).OrderBy( (_) => random.Next()).ToList();
            List<IRoom> validRooms = new List<IRoom>(topology.GetRooms()).Except(solutions).OrderBy( (_) => random.Next()).ToList();

            UseListPopulateHazards(solutions, RoomType.Vats, numVats);
            UseListPopulateHazards(solutions, RoomType.Bats, numBats);
            UseListPopulateHazards(validRooms, RoomType.Rats, numRats);
            UseListPopulateHazards(validRooms, RoomType.Acrobat, numAcrobats);
        }

        private void UseListPopulateHazards(List<IRoom> list, RoomType type, int num)
        {
            int listSize = list.Count;
            for (int i = listSize - 1; i >= listSize - num; i--)            {
                var location = list[i];
                list.Remove(location);
                rooms[location.Id] = type;
            }
            entities = new Dictionary<EntityType, Entity.Entity>();
            
            hallwayCoins = new Dictionary<Directions, bool>[numRooms];
            triviaRemaining = new bool[numRooms];
            for (int i = 0; i < rooms.Length; i++)
            {
                hallwayCoins[i] = new Dictionary<Directions, bool>();
                for (int j = 0; j < 6; j++)
                {
                    hallwayCoins[i][(Directions)j] = true;
                }

                triviaRemaining[i] = true;
            }
        }

        /// <summary>
        /// Creates an entity in the game
        /// </summary>
        /// <param name="e">Places the entity into entities</param>
        /// <exception cref="ArgumentException">If the given entity already is present</exception>
        public void AddEntity(Entity.Entity e)
        {
            if (entities.ContainsKey(e.Type))
            {
                throw new ArgumentException("Entity type already created in GameLocations!");
            }

            entities[e.Type] = e;
        }

        /// <summary>
        /// Gets an entity of a selected type.
        /// Result must be cast to the desired type.
        /// </summary>
        /// <param name="type">Type of entity to get</param>
        /// <returns>The entity of the given type</returns>
        /// <exception cref="ArgumentException">If the requested entity does not exist</exception>
        public Entity.Entity GetEntity(EntityType type)
        {
            if (!entities.ContainsKey(type))
            {
                throw new ArgumentException("Cannot retrieve entity. Entity type not present in GameLocations!");
            }

            return entities[type];
        }
        
        /// <summary>
        /// Get the player entity
        /// </summary>
        /// <returns>The Player</returns>
        public Player.Player GetPlayer()
        {
            return (Player.Player)GetEntity(EntityType.Player);
        }

        /// <summary>
        /// Get the cat entity
        /// </summary>
        /// <returns>The Cat</returns>
        public Cat GetCat()
        {
            return (Cat)GetEntity(EntityType.Cat);
        }

        public Wumpus.Wumpus GetWumpus()
        {
            return (Wumpus.Wumpus)GetEntity(EntityType.Wumpus);
        }

        /// <summary>
        /// Gets a random empty room from the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <returns>A random room of <see cref="RoomType">RoomType</see> type <c>Flats</c> from the <see cref="rooms">rooms</see> array.</returns>
        /// <exception cref="InvalidOperationException">When there are no empty rooms.</exception>
        public ushort GetEmptyRoom()
        {
            return GetRoomOfType(RoomType.Flats);
        }

        /// <summary>
        /// Gets a random room of the given type from the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <returns>A random room of <see cref="RoomType">RoomType</see> type <c>type</c> from the <see cref="rooms">rooms</see> array.</returns>
        /// <exception cref="InvalidOperationException">When there are no rooms of the given type.</exception>
        public ushort GetRoomOfType(RoomType type)
        {
            List<ushort> positions = new List<ushort>();
            for (ushort i = 0; i < rooms.Length; i++)
            {
                if (rooms[i] == type)
                {
                    positions.Add(i);
                }
            }
            if (positions.Count <= 0)
            {
                throw new InvalidOperationException("There are no empty rooms.");
            }
            return positions[Controller.Controller.Random.Next(0, positions.Count)];
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
