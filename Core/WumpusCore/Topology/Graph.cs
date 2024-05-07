using System;
using System.Collections.Generic;
using System.Linq;

namespace WumpusCore.Topology
{
    public class Graph
    {
        private List<IRoom> nodes;

        // The random number generator
        private readonly Random random;

        /// <summary>
        /// Creates a graph repersenation of a set of nodes
        /// </summary>
        /// <param name="nodes">The nodes in the graph</param>
        /// <param name="random">The random number generator to be used by the graph</param>
        public Graph(List<IRoom> nodes, Random random)
        {
            this.nodes = nodes;
            this.random = random;
        }

        /// <summary>
        /// Creates a graph repersenation of a set of nodes
        /// </summary>
        /// <param name="nodes">The nodes in the graph</param>
        public Graph(List<IRoom> nodes) : this(nodes, new Random())
        {
        }

        /// <summary>
        /// Checks if removing a set of nodes disconnects the graph
        /// </summary>
        /// <param name="room">The node to try to remove</param>
        /// <returns>True if the node can be removed without disconnecting the graph</returns>
        public bool IsNodeRemovalValid(IRoom room)
        {
            return IsNodeRemovalValid(new HashSet<IRoom>() {room});
        }
        
        /// <summary>
        /// Checks if removing a set of nodes disconnects the graph
        /// (If it would split the graph into multiple graphs)
        /// </summary>
        /// <param name="rooms">Nodes to remove</param>>
        /// <returns>True if the nodes can be removed without disconnecting the graph</returns>
        public bool IsNodeRemovalValid(HashSet<IRoom> rooms)
        {
            // If we are to small it just doesn't make any sense
            if (nodes.Count <= rooms.Count)
            {
                return false;
            }
            HashSet<IRoom> visited = new HashSet<IRoom>(rooms);
            Queue<IRoom> toExplore = new Queue<IRoom>();
            // Figure out which nodes are fine to start in 
            List<IRoom> possibleStarts = new List<IRoom>(nodes);
            foreach (IRoom room in rooms)
            {
                possibleStarts.Remove(room);
            }
            // Start somewhere
            toExplore.Enqueue(possibleStarts[0]);
            while (toExplore.Count > 0) // As long as there are still rooms to go to keep exploring
            {
                var currentRoom = toExplore.Dequeue(); // We have just visited this room
                if (visited.Contains(currentRoom))// If we have already been in this room we dont need to go back
                {
                    continue;
                }
                // We have found a new room
                visited.Add(currentRoom);
                // Add all of the exit rooms as possible exits to check
                foreach (var exit in currentRoom.ExitRooms.Values)
                {
                    // If we are already going to check this we don't need to
                    if (!(visited.Contains(exit) || toExplore.Contains(exit)))
                    {
                        toExplore.Enqueue(exit);
                    }
                }
            }
            return visited.Count == nodes.Count;// If we reached every room that isn't blocked it is good! 
        }
        /// <summary>
        /// Finds nodes that can be removed while keeping the graph connected
        /// </summary>
        /// <param name="numRemoved">Number of nodes to remove</param>
        /// <param name="panicExit">When to stop checking</param>
        /// <returns>Set of nodes that solve the graph</returns>
        /// <exception cref="TimeoutException">If the panic exit is reached</exception>
        public HashSet<IRoom> GetRandomPossibleSolutions(int numRemoved, ushort panicExit=10000)
        {
            // This code is BAD however it is just fine for our small maps
            // It just tries random things until something works basically
            // In theory it could try the same thing over and over again
            // Maybe later I could make it not do the same thing over and over again
            HashSet<IRoom> solution = new HashSet<IRoom>();
            int tries = 0;
            while (tries < panicExit)
            {
                int hasRemoved = 0;
                List<IRoom> nodesInSolution = new List<IRoom>(nodes);
                solution.Clear();
                bool validSolution = true;
                while (numRemoved > hasRemoved)
                {
                    int index = random.Next() % nodesInSolution.Count;
                    IRoom node = nodesInSolution[index];
                    solution.Add(node);
                    nodesInSolution.RemoveAt(index);
                    if (!IsNodeRemovalValid(solution))
                    {
                        validSolution = false;
                        break;
                    }
                    hasRemoved++;
                }
                if (validSolution)
                {
                    return solution;
                }
                tries++;
            }

            throw new TimeoutException("Could not find a valid solution to the rooms ");
        }
        
    }
}