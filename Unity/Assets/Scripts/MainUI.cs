using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

public class MainUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;
    
    [SerializeField]
    private GameObject cam;
    [SerializeField] 
    private GameObject movementRotation;
    private const float camSens = 5f;
    private const float camSpeed = 4f;
    private bool pLock;
    
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
    private int movingID;

    [SerializeField] 
    private Image black;
    
    /// <summary>
    /// The <see cref="Directions"/> direction the player is moving in.
    /// </summary>
    private Directions moveDir;

    private void Awake()
    {
        try
        {
            controller = Controller.GlobalController;
        }
        catch (NullReferenceException)
        {
            controller = new Controller
                (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", 0);
        }

        sceneController = SceneController.GlobalSceneController;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        roomText.text = "Room: 1";

        interactIcon.SetActive(false);
        
        RoomNum = controller.GetPlayerLocation();

        movingID = Animator.StringToHash("moving");
        pLock = false;
        
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
        if (!pLock)
        {
            float mouseX = Input.GetAxis("Mouse X");
            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("door") && !pLock)
            {
                interactIcon.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    movementRotation.transform.eulerAngles = cam.transform.eulerAngles;
                    moveDir = hit.transform.GetComponent<Door>().GetDir();
                    movingAnimator.SetBool(movingID, true);
                    pLock = true;
                }
            }
        }
        else
        {
            interactIcon.SetActive(false);
        }

        coinsText.text = controller.GetCoins().ToString();
        
        if (movingAnimator.GetBool(movingID))
        {
            if (black.color.a.Equals(1))
            {
                controller.MoveInADirection(moveDir);
                movingAnimator.SetBool(movingID, false);
                pLock = false;
                if (controller.GetState() == ControllerState.InBetweenRooms)
                {
                    sceneController.GotoCorrectScene();
                    return;
                }
                RoomNum = controller.MoveFromHallway(HallwayDir.Forward);
                cam.transform.position = new Vector3(0, cam.transform.position.y, 0);
            }
            else
            {
                cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
            }
        }
    }
}
