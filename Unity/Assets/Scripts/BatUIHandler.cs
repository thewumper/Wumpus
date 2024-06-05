using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusUnity;
using Random = UnityEngine.Random;

public class BatUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject batObject;


    [SerializeField] private float size = 0.05f;

    [SerializeField] private double batMultiplier = 1.1;

    /// <summary>
    /// The game object for the camera
    /// </summary>
    [SerializeField]
    private GameObject cam;

    /// <summary>
    /// The Animator that handles the player moving.
    /// </summary>
    [SerializeField]
    private Animator movingAnimator;

    /// <summary>
    /// The image used to fade in and out.
    /// </summary>
    [SerializeField]
    private Image black;

    /// <summary>
    /// The ID of the moving variable in <see cref="movingAnimator"/>.
    /// </summary>
    private int fadingID;

    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;

    [SerializeField] private int numBats;

    private List<GameObject> bats;

    /// <summary>
    /// The speed at which the camera rotates with the player's mouse.
    /// </summary>
    private const float camSens = 5f;

    private bool canActivateMoreBats = true;

    private void Start()
    {
        bats = new List<GameObject>();
        // Initializes the movingID.
        fadingID = Animator.StringToHash("fading");
        movingAnimator.SetBool(fadingID, false);

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
        InstantiateBats(200);
    }

    // Update is called once per frame
    void Update()
    {
        // Look around using the mouse.
        float mouseX = Input.GetAxis("Mouse X");

        cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        // PersistentData.Instance.EulerAngle = cam.transform.eulerAngles;

        if (numBats > 100)
        {
            movingAnimator.SetBool(fadingID, true);
        }


        if (movingAnimator.GetBool(fadingID))
        {
            // If the screen has fully faded to black.
            if (black.color.a.Equals(1))
            {
                controller.ExitBat();
                SceneController.GlobalSceneController.GotoCorrectScene();
            }
        }
    }

    private void FixedUpdate()
    {
        ActivateABat();
        // if (numBats < 300)
        // {
        //
        // // This could use instanced rendering for performance if needed
        // Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
        // GameObject instance =  Instantiate(batModel, randomSpawnPosition, Quaternion.identity);
        // instance.gameObject.transform.localScale = new Vector3(size,size,size);
        // numBats++;
        // }
    }

    private void InstantiateBats(int numBats)
    {
        for (int i = 0; i < numBats; i++)
        {
            GameObject newBat = Instantiate(batObject);
            newBat.transform.position += new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f,10f));
            bats.Add(newBat);
        }
    }

    private void ActivateABat()
    {
        if (bats.Count <= 0)
        {
            movingAnimator.SetBool(fadingID,true);
            canActivateMoreBats = false;
            return;
        }
        int selectedBat = Random.Range(0, bats.Count);
        bats[selectedBat].GetComponent<BatMover>().enabled = true;
        bats.RemoveAt(selectedBat);
    }
}
