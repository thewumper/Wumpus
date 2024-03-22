using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private UI ui;

    private float camSpeed;

    public void Start()
    {
        camSpeed = 4f;
    }
    
    /// <summary>
    /// Is called when we are changing rooms.
    /// </summary>
    public void MoveRooms()
    {
        ui.MoveRooms();
        ui.cam.transform.position =
            new Vector3(0, ui.cam.transform.position.y, 0);
    }
    
    /// <summary>
    /// Called when the player is able to move rooms again.
    /// </summary>
    public void AbleToMove()
    {
        ui.ableToMove = true;
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetBool("moving"))
        {
            ui.camLock = true;
            ui.ableToMove = false;
            ui.cam.transform.position += ui.cam.transform.forward * Time.deltaTime * camSpeed;
        }
    }
}
