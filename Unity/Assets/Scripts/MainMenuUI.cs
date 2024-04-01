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
        triviaNormal = Application.dataPath + "/Trivia/Questions.json";
        topologyDir = Application.dataPath + "/Maps";
        
        controller = new Controller(triviaNormal, topologyDir, 0);
        sceneController = SceneController.GlobalSceneController;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            controller.StartGame();
            sceneController.GotoCorrectScene();
        }
    }
}
