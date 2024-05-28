using UnityEngine;

/// <summary>
/// This object is destroyed when it touches the edge of the room.
/// </summary>
public class EdgeDestruct : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Room")
        {
            Destroy(this.gameObject);
        }
    }
}
