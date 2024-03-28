using System;
using System.Collections.Generic;
using System.Linq;

namespace WumpusCore.Topology
{
    public class Graph
    {
        private List<IRoom> nodes;

        public Graph(List<IRoom> nodes)
        {
            this.nodes = nodes;
        }


        public bool IsNodeRemovalValid(IRoom room)
        {
            return IsNodeRemovalValid(new HashSet<IRoom>() {room});
        }
        
        /// <summary>
        /// Checks if removing a node disconnects graph
        /// </summary>
        /// <param name="rooms">Nodes to remove</param>>
        /// <returns>True if the node can be removed without disconnecting the graph</returns>
        public bool IsNodeRemovalValid(HashSet<IRoom> rooms)
        {
            // If we are to small it just doesn't make any sense
            if (nodes.Count <= rooms.Count)
            {
                return false;
            }
            HashSet<IRoom> visited = new HashSet<IRoom>(rooms);
            Queue<IRoom> toExplore = new Queue<IRoom>();

            List<IRoom> possibleStarts = new List<IRoom>(nodes);
            foreach (IRoom room in rooms)
            {
                possibleStarts.Remove(room);
            }
            
            toExplore.Enqueue(possibleStarts[0]);
            while (toExplore.Count > 0)
            {
                var currentRoom = toExplore.Dequeue();
                if (visited.Contains(currentRoom))
                {
                    continue;
                }
                visited.Add(currentRoom);
                foreach (var exit in currentRoom.ExitRooms.Values)
                {
                    if (!(visited.Contains(exit) || toExplore.Contains(exit)))
                    {
                        toExplore.Enqueue(exit);
                    }
                }
            }
            return visited.Count == nodes.Count;
        }

        public HashSet<IRoom> GetRandomPossibleSolutions(ushort numRemoved, ushort panicExit=10000)
        {
            Random random = new Random();
            HashSet<IRoom> solution = new HashSet<IRoom>();
            ushort tries = 0;
            while (tries < panicExit)
            {
                ushort hasRemoved = 0;
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