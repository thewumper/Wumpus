using System;
using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class GameOverUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;
    
    public void Awake()
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
        sceneController = SceneController.GlobalSceneController;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            controller.StartGame();
            sceneController.GotoCorrectScene();
        }
    }
}
