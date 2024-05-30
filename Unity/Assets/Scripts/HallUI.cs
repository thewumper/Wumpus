using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Trivia;
using WumpusUnity;

public class HallUI : MonoBehaviour
{
    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;
    /// <summary>
    /// Reference to the global SceneController.
    /// </summary>
    private SceneController sceneController;
    
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


    
    /// <summary>
    /// The speed at which the camera rotates with the player's mouse.
    /// </summary>
    private const float camSens = 5f;
    /// <summary>
    /// The speed the camera moves.
    /// </summary>
    private const float camSpeed = 4f;
    /// <summary>
    /// The position the camera is supposed to stop moving at in the scene.
    /// </summary>
    private const int camStartPos = 0;
    /// <summary>
    /// Whether or not the player can move or look around.
    /// </summary>
    private bool pLock;
    
    /// <summary>
    /// The door facing behind the player.
    /// </summary>
    [SerializeField] 
    private GameObject backDoor;
    /// <summary>
    /// The door facing in front of the player.
    /// </summary>
    [SerializeField] 
    private GameObject forwardDoor;
    
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
    
    // <summary>
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
    /// The text of the hint that is shown in the hallway.
    /// </summary>
    [SerializeField] 
    private TMP_Text hint;

    private bool isCutscene;
    
    [SerializeField] private ShaderApplication shader;
    [SerializeField] private TMP_Text[] wumpusMessages;
    [SerializeField] private Light[] ceilingLights;
    [SerializeField] private GameObject[] automove;
    
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
        // Initializes the movingID.
        fadingID = Animator.StringToHash("fading");

        isCutscene = Controller.GlobalController.isNextRoomAWumpus();
        
        AnsweredQuestion q = controller.GetUnaskedQuestion();
        hint.text = $"The answer to the question \"{q.QuestionText}\" is {q.choices[q.answer]}.";
        isCutscene = true;
        if (isCutscene)
        {
            SetupCutscene();
        }
        interactIcon.SetActive(false);
    }



    void Update()
    {
        
        // If the player isn't locked.
        if (!pLock)
        {
            // Look around using the mouse.
            float mouseX = Input.GetAxis("Mouse X");
            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        }

        if (isCutscene)
        {
            DoCutscene();
            return;
        }
        
        
        // Used for checking what the player is looking at.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        // If the player is looking at something.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the player is looking at a door in a hallway.
            if (hit.transform.CompareTag("hallDoor") && !pLock)
            {
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
            }
        }
        // If the player isn't looking at anything.
        else
        {
            HideInteract();
        }
        
        // move forward when just starting in hallway.
        if (cam.transform.position.z < camStartPos)
        {
            cam.transform.position += Vector3.forward * (Time.deltaTime * camSpeed);
        }

        // If the player is moving.
        if (movingAnimator.GetBool(fadingID) && cam.transform.position.z >= camStartPos)
        {
            // If the screen has fully faded to black.
            if (black.color.a.Equals(1))
            {
                // Move from the Hallway.
                controller.MoveFromHallway();
                movingAnimator.SetBool(fadingID, false);
                // Unlock the player.
                pLock = false;
                // Reset camera position.
                cam.transform.position = new Vector3(0, cam.transform.position.y, 0);
                sceneController.GotoCorrectScene();
            }
            // If the screen has not fully faded to black.
            else
            {
                // Move the camera toward the doorway.
                cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
            }
        }
    }
    private void SetupCutscene()
    {
        shader.PosterzationBands1 = 20;
        shader.OverallDistortionFreq = 10000.7f;
        shader.OverallDistortionMag = 2;
        shader.OverallDistortionSpeed = 0.002f;
        foreach (TMP_Text message in wumpusMessages)
        {
            message.gameObject.SetActive(true);
        }
        hint.gameObject.SetActive(false);
        for (int i = 0; i < automove.Length; i++)
        {
            StartCoroutine(LightGoesOut(i));
        }
        
    }
    private void DoCutscene()
    {
        cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
        if (cam.transform.position.z >= automove[^1].transform.position.z)
        {
            isCutscene = false;
        }
    }

    private IEnumerator LightGoesOut(int light)
    {
        yield return new WaitForSeconds(light * 7);
        ceilingLights[light].gameObject.AddComponent<LightFlicker>();
        yield return new WaitForSeconds(5);
        ceilingLights[light].enabled = false;
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
