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
    
    void Start()
    {
        sceneController = SceneController.GlobalSceneController;
        controller = Controller.GlobalController;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sceneController.GotoCorrectScene();
        }
    }
}
