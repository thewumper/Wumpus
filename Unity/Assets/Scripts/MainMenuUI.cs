using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using WumpusCore.Controller;
using WumpusUnity;

public class MainMenuUI : MonoBehaviour
{
    /// <summary>
    /// The global SceneController object.
    /// </summary>
    private SceneController sceneController;
    /// <summary>
    /// The global Controller object.
    /// </summary>
    private Controller controller;

    private string triviaNormal;
    private string triviaAdvanced;
    private string topologyDir;
    
    void Awake()
    {
        // Gets the path to the trivia questions.
        triviaNormal = Application.dataPath + "/Trivia/Questions.json";
        triviaAdvanced = Application.dataPath + "/Trivia/AdvancedQuestions.json";

        // Gets the path to the Map.
        topologyDir = Application.dataPath + "/Maps";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        ushort randomMap = (ushort)Random.Range(0, 4);

        Debug.Log("Loading map #" + randomMap);

        // Initializes the Controller.
        controller = new Controller(triviaNormal, topologyDir, randomMap, 0, 0, 20, 0, 0, 0, 20);
        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;
        sceneController.Reinitialize(controller);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller.StartGame();
        sceneController.GotoCorrectScene();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit(0);
#endif
    }
}
