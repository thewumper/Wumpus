using UnityEngine;
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

    [SerializeField] 
    private string triviaNormal;
    private string topologyDir;
    
    
    void Awake()
    {
        // Gets the path to the trivia questions.
        triviaNormal = Application.dataPath + "/Trivia/Questions.json";
        // Gets the path to the Map.
        topologyDir = Application.dataPath + "/Maps";
        
        // Initializes the Controller.
        controller = new Controller(triviaNormal, topologyDir, 0, 0, 0, 20, 0);
        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;
    }

    void Update()
    {
        // If the player is pressing the lmb.
        if (Input.GetMouseButtonDown(0))
        {
            // Start the game.
            controller.StartGame();
            sceneController.GotoCorrectScene();
        }
    }
}
