using UnityEngine;

/// <summary>
/// This object is destroyed when it touches the edge of the room.
/// </summary>
public class EdgeDestruct : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Room")
        {
            Destroy(this.gameObject);
        }
    }
}
