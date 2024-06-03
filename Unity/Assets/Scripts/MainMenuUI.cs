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
    }

    public void PlayGame()
    {
        ushort randomMap = (ushort)Random.Range(0, 4);

        Debug.Log("Loading map #" + randomMap);

        // Initializes the Controller.
        controller = new Controller(triviaNormal, topologyDir, randomMap);
        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;

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
