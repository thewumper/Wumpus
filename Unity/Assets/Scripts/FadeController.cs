using System;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;

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
        UI.GetComponent<UI>().MoveRooms();
        UI.GetComponent<UI>().cam.transform.position =
            new Vector3(0, UI.GetComponent<UI>().cam.transform.position.y, 0);
    }

    public void AbleToMove()
    {
        UI.GetComponent<UI>().ableToMove = true;
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetBool("moving"))
        {
            UI.GetComponent<UI>().camLock = true;
            UI.GetComponent<UI>().ableToMove = false;
            UI.GetComponent<UI>().cam.transform.position += UI.GetComponent<UI>().cam.transform.forward * Time.deltaTime * camSpeed;
        }
    }
}
