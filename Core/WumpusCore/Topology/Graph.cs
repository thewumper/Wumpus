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
        
        /// <summary>
        /// Checks if removing a node disconnects graph
        /// </summary>
        /// <param name="room">Node to remove</param>>
        /// <returns>True if the node can be removed without disconnecting the graph</returns>
        public bool IsNodeRemovalValid(IRoom room)
        {
            // If we are to small it just doesn't make any sense
            if (nodes.Count <= 1)
            {
                return false;
            }
            HashSet<IRoom> visited = new HashSet<IRoom> { room };
            Queue<IRoom> toExplore = new Queue<IRoom>();
            toExplore.Enqueue(nodes[0] != room ? nodes[0] : nodes[1]);
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
    }
}