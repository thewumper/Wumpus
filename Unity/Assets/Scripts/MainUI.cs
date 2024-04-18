using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

public class MainUI : MonoBehaviour
{
    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;

    /// <summary>
    /// Reference to the global SceneController.
    /// </summary>
    private SceneController sceneController;
    
    private SoundManager soundManager;
    
    /// <summary>
    /// The GameObject of the Camera.
    /// </summary>
    [SerializeField]
    private GameObject cam;
    /// <summary>
    /// The Rotation for the Movement of the Player.
    /// </summary>
    private GameObject movementRotation;
    
    /// <summary>
    /// The speed at which the camera rotates with the player's mouse.
    /// </summary>
    private const float camSens = 5f;
    /// <summary>
    /// The speed the camera moves.
    /// </summary>
    private const float camSpeed = 4f;
    /// <summary>
    /// Whether or not the player can move or look around.
    /// </summary>
    private bool pLock;
    
    /// <summary>
    /// The room that the player is currently in.
    /// </summary>
    private ushort roomNum;
    /// <summary>
    /// The room that the player is currently in.
    /// </summary>
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
    
    /// <summary>
    /// Reference to the door that is north of the player.
    /// </summary>
    [SerializeField]
    private GameObject northDoor;
    /// <summary>
    /// Reference to the door that is northeast of the player.
    /// </summary>
    [SerializeField]
    private GameObject northEastDoor;
    /// <summary>
    /// Reference to the door that is southeast of the player.
    /// </summary>
    [SerializeField]
    private GameObject southEastDoor;
    /// <summary>
    /// Reference to the door that is south of the player.
    /// </summary>
    [SerializeField]
    private GameObject southDoor;
    /// <summary>
    /// Reference to the door that is southwest of the player.
    /// </summary>
    [SerializeField]
    private GameObject southWestDoor;
    /// <summary>
    /// Reference to the door that is northwest of the player.
    /// </summary>
    [SerializeField]
    private GameObject northWestDoor;
    
    /// <summary>
    /// The GameObject that shows each interactable icon.
    /// </summary>
    [SerializeField]
    private GameObject interactIcon;

    /// <summary>
    /// The icon that represents being able to move through a door.
    /// </summary>
    [SerializeField] 
    private Sprite doorIcon;
    /// <summary>
    /// The icon that represents not being able to interact with something.
    /// </summary>
    [SerializeField] 
    private Sprite uninteractableIcon;
    
    /// <summary>
    /// The text that shows the amount of coins the player has.
    /// </summary>
    [SerializeField] 
    private TMP_Text coinsText;
    /// <summary>
    /// The text that shows what room the player is currently in.
    /// </summary>
    [SerializeField] 
    private TMP_Text roomText;
    
    /// <summary>
    /// The Animator that handles the player moving.
    /// </summary>
    [SerializeField] 
    private Animator movingAnimator;
    /// <summary>
    /// The ID of the moving variable in <see cref="movingAnimator"/>.
    /// </summary>
    private int movingID;
    
    /// <summary>
    /// The image used to fade in and out.
    /// </summary>
    [SerializeField] 
    private Image black;
    

    [SerializeField]
    private AudioClip wumpusClip;
    private AudioClip luckyCatClip;

    /// <summary>
    /// The <see cref="Directions"/> direction the player is moving in.
    /// </summary>
    private Directions moveDir;

    private void Awake()
    {
        // Instantiates the Controller, if there isn't one already.
        try
        {
            controller = Controller.GlobalController;
        }
        catch (NullReferenceException)
        {
            controller = new Controller
                (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", 0);
        }
        
        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;
    }

    void Start()
    {
        // Locks the cursor and makes it invisible.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initializes the roomText text.
        roomText.text = $"Room: {RoomNum}";
        
        // Makes it so you can't normally see the interactIcon.
        HideInteract();
        
        // Initializes the roomNum to the player's starting location.
        controller = Controller.GlobalController;
        soundManager = new SoundManager(wumpusClip);
        
        RoomNum = controller.GetPlayerLocation();
        
        // Initializes the movingID.
        movingID = Animator.StringToHash("moving");
        
        // Initializes the pLock.
        pLock = false;
        
        // Adds the Door script to all doors.
        northDoor.AddComponent<Door>().Init(Directions.North);
        northEastDoor.AddComponent<Door>().Init(Directions.NorthEast);
        southEastDoor.AddComponent<Door>().Init(Directions.SouthEast);
        southDoor.AddComponent<Door>().Init(Directions.South);
        southWestDoor.AddComponent<Door>().Init(Directions.SouthWest);
        northWestDoor.AddComponent<Door>().Init(Directions.NorthWest);

        soundManager.PlaySound(SoundManager.SoundType.Wumpus, northDoor);
    }

    void LateUpdate()
    {
        // Makes an IRoom which is the room that the player is currently in.
        IRoom room = controller.GetRoom(RoomNum);
        // Makes the roomText show which room the player is actually in.
        roomText.text = $"Room: {RoomNum}";
        // Makes only the doors that are in the room visible.
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
        // If the player isn't locked.
        if (!pLock)
        {
            // Look around using the mouse.
            float mouseX = Input.GetAxis("Mouse X");
            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        }
        // Used for checking what the player is currently looking at.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        // If the player is looking at something.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the player is looking at a door.
            if (hit.transform.CompareTag("door") && !pLock)
            {
                ShowInteract(doorIcon);
                if (Input.GetMouseButtonDown(0))
                {
                    movementRotation.transform.eulerAngles = cam.transform.eulerAngles;
                    moveDir = hit.transform.GetComponent<Door>().GetDir();
                    movingAnimator.SetBool(movingID, true);
                    pLock = true;
                }
            }
            // If the player is looking at a door that they can't move through.
            else if (hit.transform.CompareTag("unmoveableDoor") && !pLock)
            {
                ShowInteract(uninteractableIcon);
            }
            // If the player is looking at something that is none of these.
            else
            {
                HideInteract();
            }
        } 
        // If the player isn't looking at anything.
        else
        {
            HideInteract();
        }
        
        // Makes the coinsText show the actual amount of coins that the player currently has.
        coinsText.text = controller.GetCoins().ToString();
        
        // If the player is moving.
        if (movingAnimator.GetBool(movingID))
        {
            // If the screen has fully faded to black.
            if (black.color.a.Equals(1))
            {
                // Move rooms.
                controller.MoveInADirection(moveDir);
                movingAnimator.SetBool(movingID, false);
                // Unlock the player.
                pLock = false;
                // If we are in a hallway.
                if (controller.GetState() == ControllerState.InBetweenRooms)
                {
                    // Go to the hallway, and do nothing else.
                    sceneController.GotoCorrectScene();
                    return;
                }
                // If not in a hallway, move to the next room.
                RoomNum = controller.MoveFromHallway(HallwayDir.Forward);
                // Reset camera position.
                cam.transform.position = new Vector3(0, cam.transform.position.y, 0);
            }
            // If the screen has not fully faded to black.
            else
            {
                // Move the camera toward the doorway.
                cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
            }
        }
    }
    /// <summary>
    /// Shows the <see cref="interactIcon"/> with the given sprite.
    /// </summary>
    /// <param name="sprite">The sprite that the interactIcon shows.</param>
    private void ShowInteract(Sprite sprite)
    {
        interactIcon.GetComponent<Image>().sprite = sprite;
        interactIcon.SetActive(true);
    }

    /// <summary>
    /// Hides the <see cref="interactIcon"/>.
    /// </summary>
    private void HideInteract()
    {
        interactIcon.SetActive(false);
    }
}
