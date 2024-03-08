using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class MainMenuUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = Controller.GlobalController;
        sceneController = SceneController.GlobalSceneController;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sceneController.SetScene();
        }
    }
}
