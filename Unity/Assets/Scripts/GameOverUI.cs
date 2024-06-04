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
    
    public void Awake()
    {
        ushort randomMap = (ushort)Random.Range(0, 4);

        Debug.Log("Loading map #" + randomMap);
        
        controller = new Controller
            (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", randomMap);
        sceneController = SceneController.GlobalSceneController;
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
