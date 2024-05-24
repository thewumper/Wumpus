using System;
using System.Collections.Generic;
using System.Linq;
using WumpusCore.Controller;
using WumpusCore.Entity;
using WumpusCore.LuckyCat;
using WumpusCore.Topology;
using WumpusCore.Trivia;

namespace WumpusCore.GameLocations
{
    public class GameLocations
    {
        /// <summary>
        /// All entities in the game
        /// </summary>
        private Dictionary<EntityType, Entity.Entity> entities;
        /// <summary>
        /// How all the rooms connect to each other
        /// </summary>
        private ITopology topology;
        
        /// <summary>
        /// Whether there is a coin in the hallway out from a given room
        /// </summary>
        public Dictionary<Directions, bool>[] hallwayCoins;

        /// <summary>
        /// The trivia question engraved onto the wall in a given hallway
        /// </summary>
        public readonly Dictionary<Directions, AnsweredQuestion>[] hallwayTrivia;

        // Whether the player can do trivia in a room to earn secrets, arrows, etc
        private bool[] triviaRemaining;

        /// <summary>
        /// Gets whether or not the player can earn arrows or secrets in a given room
        /// </summary>
        /// <param name="index">The index of the room</param>
        /// <returns>Whether the player can earn trivia-based rewards in the room</returns>
        public bool GetTriviaAvailable(int index)
        {
            // Trivia is not available in hazard rooms
            if (GetRoomAt((ushort)index) == RoomType.Flats)
            {
                return triviaRemaining[index];
            } 
            return false;
        }

        /// <summary>
        /// Sets whether the player can earn trivia-based rewards in the room
        /// </summary>
        /// <param name="index">The room to change</param>
        /// <param name="remaining">The new value of whether or not trivia can still be done</param>
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
        public GameLocations(int numRooms,int numVats, int numBats, int numRats, int numAcrobats, ITopology topology, Random random, Trivia.Trivia trivia)

        {
            this.topology = topology;
            if (numVats + numRats + numAcrobats + numBats >= numRooms)
            {
                throw new ArgumentException("Too many hazards!");
            }
            
            rooms = new RoomType[numRooms];
            int hardHazards = (numVats + numBats);
            Graph graph = new Graph(new List<IRoom>(topology.GetRooms()),random);
            List<IRoom> solutions = new List<IRoom>(graph.GetRandomPossibleSolutions(hardHazards)).OrderBy( (_) => random.Next()).ToList();
            List<IRoom> validRooms = new List<IRoom>(topology.GetRooms()).Except(solutions).OrderBy( (_) => random.Next()).ToList();

            UseListPopulateHazards(solutions, RoomType.Vats, numVats);
            UseListPopulateHazards(solutions, RoomType.Bats, numBats);
            UseListPopulateHazards(validRooms, RoomType.Rats, numRats);
            UseListPopulateHazards(validRooms, RoomType.Acrobat, numAcrobats);
            
            // Populate hallways with coins
            // Populate rooms with trivia options
            // Populate hallways with trivia answers
            hallwayCoins = new Dictionary<Directions, bool>[numRooms];
            triviaRemaining = new bool[numRooms];
            hallwayTrivia = new Dictionary<Directions, AnsweredQuestion>[numRooms];
            for (int i = 0; i < rooms.Length; i++)
            {
                hallwayCoins[i] = new Dictionary<Directions, bool>();
                hallwayTrivia[i] = new Dictionary<Directions, AnsweredQuestion>();
            }
            for (int i = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Directions direction = (Directions)j;

                    hallwayCoins[i][direction] = true;
                    hallwayCoins[i][direction.GetInverse()] = true;

                    AnsweredQuestion knowledge = trivia.PeekRandomQuestion();
                    hallwayTrivia[i][direction] = knowledge;
                    // Set the opposite direction hallway from the room on the other side
                    int oppositeRoom = topology.GetRoom((ushort)i).AdjacentRooms[direction].Id;
                    hallwayTrivia[oppositeRoom][direction.GetInverse()] = knowledge;
                }

                triviaRemaining[i] = true;
            }
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

        /// <summary>
        /// Get the wumpus entity
        /// </summary>
        /// <returns>The Wumpus</returns>
        public Wumpus.Wumpus GetWumpus()
        {
            return (Wumpus.Wumpus)GetEntity(EntityType.Wumpus);
        }

        /// <summary>
        /// Gets a random empty room from the <see cref="rooms">rooms</see> array.
        /// </summary>
        /// <returns>A random room of <see cref="RoomType">RoomType</see> type <c>Flats</c> from the <see cref="rooms">rooms</see> array where the are no <see cref="Entity">entities</see></returns>
        /// <exception cref="InvalidOperationException">When there are no empty rooms.</exception>
        public ushort GetEmptyRoom()
        {
            List<ushort> positions = new List<ushort>();
            for (ushort i = 0; i < rooms.Length; i++)
            {

                if (rooms[i] == RoomType.Flats && GetEntityInRoom(i).Count == 0)
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
        /// Gets the entity(s) in a room if there are any
        /// </summary>
        /// <param name="roomunm">The room number that you want to check</param>
        /// <returns>A List of <see cref="Entity">entity(s)</see> or any empty list if there are no entities</returns>
        public List<Entity.Entity> GetEntityInRoom(ushort roomunm)
        {
            List<Entity.Entity> list = new List<Entity.Entity>();
            foreach (EntityType entity in entities.Keys)
            {
                if (entities[entity].location == roomunm)
                {
                    list.Add(entities[entity]);
                }
            }

            return list;
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

        public Dictionary<Directions, RoomType> GetAdjacentRoomTypes(int position)
        {
            IRoom room = topology.GetRoom((ushort)position);
            Dictionary<Directions, RoomType> adjacentRooms = new Dictionary<Directions, RoomType>();
            foreach (Directions direction in room.ExitDirections)
            {
                adjacentRooms[direction] = rooms[room.AdjacentRooms[direction].Id];
            }

            return adjacentRooms;
        }
        
        
    }
}
