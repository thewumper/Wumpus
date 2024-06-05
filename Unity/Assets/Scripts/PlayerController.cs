using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

public class PlayerController : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;

    private bool pLock;
    [SerializeField]
    private GameObject cam;
    private const float camSens = 5f;
    private const float camSpeed = 4f;
    [SerializeField]
    private GameObject movementRotation;

    private Directions moveDir;

    private int fadingID;
    [SerializeField]
    private Animator movingAnimator;
    [SerializeField]
    private Image black;

    [SerializeField]
    private GameObject interactIcon;
    [SerializeField]
    private Sprite doorIcon;
    [SerializeField]
    private Sprite uninteractableIcon;

    [SerializeField]
    private TMP_Text directionText;
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

    private void Start()
    {
        pLock = false;

        fadingID = Animator.StringToHash("fading");
    }

    private void Update()
    {
        // If the player isn't locked.
        if (!pLock)
        {
            // Look around using the mouse.
            float mouseX = Input.GetAxis("Mouse X");

            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
            PersistentData.Instance.EulerAngle = cam.transform.eulerAngles;
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
    private void MoveRooms()
    {
        // Move rooms.
        controller.MoveInADirection(moveDir);
        movingAnimator.SetBool(fadingID, false);
        sceneController.GotoCorrectScene();
    }
}
