// Chief - Davin


using UnityEngine;
using WumpusCore.Topology;

public class Door : MonoBehaviour
{
    private Directions dir;
    
    /// <summary>
    /// Initializes the door with a <see cref="Directions"/> direction.
    /// </summary>
    /// <param name="dir">The <see cref="Directions"/> direction to initialize this door to.</param>
    public void Init(Directions dir)
    {
        this.dir = dir;
    }
    
    /// <summary>
    /// Gets the direction of this door.
    /// </summary>
    /// <returns>The direction of this door.</returns>
    public Directions GetDir()
    {
        return this.dir;
    }
}
