using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WumpusCore.Controller;
using WumpusUnity;
using Random = UnityEngine.Random;

public class GameOverUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;
    
    private void Awake()
    {
        ushort randomMap = (ushort)Random.Range(0, 4);

        Debug.Log("Loading map #" + randomMap);
        
        controller = new Controller
            (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", randomMap);
        sceneController = SceneController.GlobalSceneController;
        sceneController.Reinitialize(controller);
    }

    private void Start()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        controller.StartGame();
        sceneController.GotoCorrectScene();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
