using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;

public class HallDoor : MonoBehaviour
{
    private HallwayDir dir;
    
    /// <summary>
    /// Initializes the door with a <see cref="Directions"/> direction.
    /// </summary>
    /// <param name="dir">The <see cref="Directions"/> direction to initialize this door to.</param>
    public void Init(HallwayDir dir)
    {
        this.dir = dir;
    }
    
    /// <summary>
    /// Gets the direction of this door.
    /// </summary>
    /// <returns>The direction of this door.</returns>
    public HallwayDir GetDir()
    {
        return this.dir;
    }
}