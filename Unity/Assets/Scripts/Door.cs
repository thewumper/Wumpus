using UnityEngine;
using WumpusCore.Topology;

public class Door : MonoBehaviour
{
    private Directions dir;
    public void Init(Directions dir)
    {
        this.dir = dir;
    }

    public Directions GetDir()
    {
        return this.dir;
    }
}
