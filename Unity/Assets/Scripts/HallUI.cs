using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Trivia;
using WumpusUnity;

public class HallUI : MonoBehaviour
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
    
    [SerializeField] 
    private GameObject backDoor;
    [SerializeField] 
    private GameObject forwardDoor;

    private HallwayDir moveDir;
    
    [SerializeField]
    private GameObject interactIcon;
    [SerializeField] 
    private Sprite doorIcon;
    [SerializeField] 
    private Sprite uninteractableIcon;
    
    [SerializeField] 
    private Animator movingAnimator;
    private int movingID;

    [SerializeField] 
    private Image black;

    [SerializeField] 
    private TMP_Text hint;
    
    // Start is called before the first frame update
    void Start()
    {
        movingID = Animator.StringToHash("moving");
        
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
        
        backDoor.AddComponent<HallDoor>().Init(HallwayDir.Previous);
        forwardDoor.AddComponent<HallDoor>().Init(HallwayDir.Forward);

        AnsweredQuestion q = controller.GetUnaskedQuestion();
        hint.text = $"The answer to the question \"{q.QuestionText}\" is {q.choices[q.answer]}.";

        interactIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pLock)
        {
            float mouseX = Input.GetAxis("Mouse X");
            cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        }
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("hallDoor"))
            {
                if (!pLock)
                {
                    ShowInteract(doorIcon);
                    if (Input.GetMouseButtonDown(0))
                    {
                        movementRotation.transform.eulerAngles = cam.transform.eulerAngles;
                        moveDir = hit.transform.GetComponent<HallDoor>().GetDir();
                        movingAnimator.SetBool(movingID, true);
                        pLock = true;
                    }
                }
            } else if (hit.transform.CompareTag("unmoveableDoor"))
            {
                if (!pLock)
                {
                    ShowInteract(uninteractableIcon);
                }
            }
            else
            {
                HideInteract();
            }
        }
        
        if (movingAnimator.GetBool(movingID))
        {
            if (black.color.a.Equals(1))
            {
                controller.MoveFromHallway(moveDir);
                Debug.Log(moveDir);
                Debug.Log(controller.GetPlayerLocation());
                cam.transform.position = new Vector3(0, cam.transform.position.y, 0);
                movingAnimator.SetBool(movingID, false);
                pLock = false;
                sceneController.GotoCorrectScene();
            }
            else
            {
                cam.transform.position += movementRotation.transform.forward * (Time.deltaTime * camSpeed);
            }
        }
    }

    private void ShowInteract(Sprite sprite)
    {
        interactIcon.GetComponent<Image>().sprite = sprite;
        interactIcon.SetActive(true);
    }

    private void HideInteract()
    {
        interactIcon.SetActive(false);
    }
}
