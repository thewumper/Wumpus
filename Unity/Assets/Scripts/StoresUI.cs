using System;
using UnityEngine;
using TMPro;
using WumpusCore.Controller;
using WumpusCore.Topology;
using UnityEngine.UI;
using WumpusUnity;

public class StoresUI : MonoBehaviour

{
    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;

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
    /// The icon that represents not being able to interact with something.
    /// </summary>
    [SerializeField]
    private Sprite useIcon;

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
    /// The hint for what sounds are near you
    /// </summary>
    [SerializeField]
    private TMP_Text roomHintText;

    /// <summary>
    /// Rext that displays your current room type
    /// </summary>
    [SerializeField]
    private TMP_Text roomTypeText;

    /// <summary>
    /// The text that shows which direction the player is looking in.
    /// </summary>
    [SerializeField]
    private TMP_Text directionText;

    /// <summary>
    /// The Animator that handles the player moving.
    /// </summary>
    [SerializeField]
    private Animator movingAnimator;
    /// <summary>
    /// The ID of the moving variable in <see cref="movingAnimator"/>.
    /// </summary>
    private int fadingID;

    /// <summary>
    /// The image used to fade in and out.
    /// </summary>
    [SerializeField]
    private Image black;

    /// <summary>
    /// The <see cref="Directions"/> direction the player is moving in.
    /// </summary>
    private Directions moveDir;

    [SerializeField]
    private GameObject mmNorth;
    [SerializeField]
    private GameObject mmNorthEast;
    [SerializeField]
    private GameObject mmSouthEast;
    [SerializeField]
    private GameObject mmSouth;
    [SerializeField]
    private GameObject mmSouthWest;
    [SerializeField]
    private GameObject mmNorthWest;

    [SerializeField]
    private GameObject mmDirection;
    // Start is called before the first frame update

    /// <summary>
    /// The GameObject of the Camera.
    /// </summary>
    [SerializeField]
    private GameObject cam;
    /// <summary>
    /// The Rotation for the Movement of the Player.
    /// </summary>
    [SerializeField]
    private GameObject movementRotation;

    [SerializeField]
    public GameObject gun;

    [SerializeField]
    public GameObject ammo;

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

            mmNorth.SetActive(false);
            mmNorthEast.SetActive(false);
            mmSouthEast.SetActive(false);
            mmSouth.SetActive(false);
            mmSouthWest.SetActive(false);
            mmNorthWest.SetActive(false);

            roomNum = value;
        }
    }

    /// <summary>
    /// Reference to the global SceneController.
    /// </summary>
    private SceneController sceneController;

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
        controller.Debug = true;

        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;
    }

    void Start()
    {
        // Locks the cursor and makes it invisible.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam.transform.eulerAngles = PersistentData.Instance.EulerAngle;
        Debug.Log(PersistentData.Instance.EulerAngle);
        mmDirection.transform.eulerAngles = new Vector3(
            mmDirection.transform.eulerAngles.x,
            mmDirection.transform.eulerAngles.y,
            PersistentData.Instance.EulerAngle.y);


        // Makes it so you can't normally see the interactIcon.
        HideInteract();

        // Initializes the movingID.
        fadingID = Animator.StringToHash("fading");

        // Initializes the roomText text.
        roomText.SetText($"Room: {RoomNum}");

        // Initializes the RoomNum with the Player's location.
        RoomNum = (ushort) controller.GetPlayerLocation();

        // Initializes the pLock.
        pLock = false;

        // Adds the Door script to all doors.
        northDoor.AddComponent<Door>().Init(Directions.North);
        northEastDoor.AddComponent<Door>().Init(Directions.NorthEast);
        southEastDoor.AddComponent<Door>().Init(Directions.SouthEast);
        southDoor.AddComponent<Door>().Init(Directions.South);
        southWestDoor.AddComponent<Door>().Init(Directions.SouthWest);
        northWestDoor.AddComponent<Door>().Init(Directions.NorthWest);

        ControllerState state = controller.GetState();
        if (state==ControllerState.GunRoom)
        {
            gun.SetActive(true);
            ammo.SetActive(false);
        }
        else if (state == ControllerState.AmmoRoom)
        {
            ammo.SetActive(true);
            gun.SetActive(false);
        }
        else
        {
            Debug.LogError("This room doesn't have anything stored. This shouldn't happen");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // If the player isn't locked.
        if (!pLock)
        {
            // Look around using the mouse.
            float mouseX = Input.GetAxis("Mouse X");

            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
            PersistentData.Instance.EulerAngle = cam.transform.eulerAngles;


            mmDirection.transform.eulerAngles = new Vector3(
                mmDirection.transform.eulerAngles.x,
                mmDirection.transform.eulerAngles.y,
                180 - PersistentData.Instance.EulerAngle.y);
        }


        IRoom room = controller.GetCurrentRoom();
        // Makes only the doors that are in the room visible.
        foreach (Directions dir in room.ExitDirections)
        {
            string name = DirectionHelper.GetShortNameFromDirection(dir);
            switch (name)
            {
                case "N":
                    northDoor.SetActive(true);
                    mmNorth.SetActive(true);
                    break;
                case "NE":
                    northEastDoor.SetActive(true);
                    mmNorthEast.SetActive(true);
                    break;
                case "SE":
                    southEastDoor.SetActive(true);
                    mmSouthEast.SetActive(true);
                    break;
                case "S":
                    southDoor.SetActive(true);
                    mmSouth.SetActive(true);
                    break;
                case "SW":
                    southWestDoor.SetActive(true);
                    mmSouthWest.SetActive(true);
                    break;
                case "NW":
                    northWestDoor.SetActive(true);
                    mmNorthWest.SetActive(true);
                    break;
            }
        }

        // Used for checking what the player is currently looking at.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        // If the player is looking at something.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            // If the player is looking at a door.
            if (hit.transform.CompareTag("door") && !pLock)
            {
                moveDir = hit.transform.GetComponent<Door>().GetDir();
                directionText.SetText(moveDir.ToString());
                ShowInteract(doorIcon);
                if (Input.GetMouseButtonDown(0))
                {
                    movementRotation.transform.eulerAngles = cam.transform.eulerAngles;
                    movingAnimator.SetBool(fadingID, true);
                    pLock = true;
                }
            }
            // If the player is looking at a door that they can't move through.
            else if (hit.transform.CompareTag("unmoveableDoor") && !pLock)
            {
                ShowInteract(uninteractableIcon);
            }
            // If the player is looking at the gun or ammo they can intereact
            else if (hit.transform.CompareTag("Gun") && !pLock)
            {
                // TODO! Stinky code duplication
                bool canCollect = controller.CanRoomBeCollectedFrom();
                if (canCollect)
                {
                    ShowInteract(useIcon);
                    directionText.SetText("Collect the gun");
                }
                else
                {
                    directionText.SetText("You have already collected the gun");
                    ShowInteract(uninteractableIcon);
                }

                if (Input.GetMouseButtonDown(0) && canCollect)
                {
                    controller.StartTrivia();
                    sceneController.GotoCorrectScene();
                }
            }
            else if (hit.transform.CompareTag("Ammo") && !pLock)
            {
                bool canCollect = controller.CanRoomBeCollectedFrom();
                if (canCollect)
                {
                    ShowInteract(useIcon);
                    directionText.SetText("Collect the ammo");
                }
                else
                {
                    directionText.SetText("You have already collected this ammo");
                    ShowInteract(uninteractableIcon);
                }

                if (Input.GetMouseButtonDown(0) && canCollect)
                {
                    controller.StartTrivia();
                    sceneController.GotoCorrectScene();
                }
            }
            // If the player is looking at something that is none of these.
            else
            {
                HideInteract();
                directionText.SetText("");
            }
        }
        // If the player isn't looking at anything.
        else
        {
            HideInteract();
            directionText.SetText("");
        }

        // Makes the coinsText show the actual amount of coins that the player currently has.
        coinsText.SetText(controller.GetCoins().ToString());

        // If we are fading.
        if (movingAnimator.GetBool(fadingID))
        {
            // If the screen has fully faded to black.
            if (black.color.a.Equals(1))
            {
               MoveRooms();
            }
            // If the screen has not fully faded to black.
            else
            {
                if (controller.GetState() == ControllerState.BatTransition) return;
                // Move the camera toward the doorway.
                cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
            }
        }
    }

    private void HideInteract()
    {
        interactIcon.SetActive(false);
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

    private void MoveRooms()
    {
        // Move rooms.
        controller.MoveInADirection(moveDir);
        movingAnimator.SetBool(fadingID, false);
        sceneController.GotoCorrectScene();
    }
}