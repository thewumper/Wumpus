using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;

public class UI : MonoBehaviour
{
    private Controller controller;
    
    public GameObject cam;

    private const float CamSens = 5f;
    public bool camLock = false;

    public bool ableToMove = true;
    
    private ushort roomNum;
    private ushort RoomNum
    {
        get
        {
            return roomNum;
        }
        set
        {
            northDoor.SetActive(false);
            northEastDoor.SetActive(false);
            southEastDoor.SetActive(false);
            southDoor.SetActive(false);
            southWestDoor.SetActive(false);
            northWestDoor.SetActive(false);

            roomNum = value;
        }
    }
    
    // doors
    [SerializeField]
    private GameObject northDoor;
    [SerializeField]
    private GameObject northEastDoor;
    [SerializeField]
    private GameObject southEastDoor;
    [SerializeField]
    private GameObject southDoor;
    [SerializeField]
    private GameObject southWestDoor;
    [SerializeField]
    private GameObject northWestDoor;

    [SerializeField]
    private GameObject interactIcon;
    
    [SerializeField] 
    private TMP_Text coinsText;
    [SerializeField] 
    private TMP_Text roomText;

    [SerializeField] 
    private Animator movingAnimator;
    
    /// <summary>
    /// The <see cref="Directions"/> direction the player is moving in.
    /// </summary>
    private Directions moveDir;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        roomText.text = "Room: 1";

        interactIcon.SetActive(false);
        
        controller = Controller.GlobalController;
        RoomNum = 1;
        
        northDoor.AddComponent<Door>().Init(Directions.North);
        northEastDoor.AddComponent<Door>().Init(Directions.NorthEast);
        southEastDoor.AddComponent<Door>().Init(Directions.SouthEast);
        southDoor.AddComponent<Door>().Init(Directions.South);
        southWestDoor.AddComponent<Door>().Init(Directions.SouthWest);
        northWestDoor.AddComponent<Door>().Init(Directions.NorthWest);
    }

    void LateUpdate()
    {
        IRoom room = controller.GetRoom(RoomNum);
        roomText.text = "Room: " + RoomNum;
        foreach (Directions dir in room.ExitDirections)
        {
            string name = DirectionHelper.GetShortNameFromDirection(dir);
            switch (name)
            {
                case "N":
                    northDoor.SetActive(true);
                    break;
                case "NE":
                    northEastDoor.SetActive(true);
                    break;
                case "SE":
                    southEastDoor.SetActive(true);
                    break;
                case "S":
                    southDoor.SetActive(true);
                    break;
                case "SW":
                    southWestDoor.SetActive(true);
                    break;
                case "NW":
                    northWestDoor.SetActive(true);
                    break;
            }
        }
    }

    private void Update()
    {
        if (!camLock)
        {
            float mouseX = Input.GetAxis("Mouse X");
            cam.transform.eulerAngles += new Vector3(0, mouseX * CamSens, 0);
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("door") && ableToMove)
            {
                interactIcon.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    movingAnimator.SetBool("moving", true);
                    moveDir = hit.transform.GetComponent<Door>().GetDir();
                }
            }
        }
        else
        {
            interactIcon.SetActive(false);
        }

        coinsText.text = "" + controller.GetCoins();
    }
    
    /// <summary>
    /// Actually changes which room the player is currently in.
    /// </summary>
    public void MoveRooms()
    {
        RoomNum = controller.MoveInADirection(moveDir);
        camLock = false;
        movingAnimator.SetBool("moving", false);
    }
}
