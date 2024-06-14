// Chief - 
// Contributors:
// Davin


using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class VatUI : MonoBehaviour
{
    [SerializeField] private string triviaScene;
    
    private Controller controller;
    private SceneController sceneController;
    
    void Awake()
    {
        controller = Controller.GlobalController;
        sceneController = SceneController.GlobalSceneController;
        
        // Start trivia and load the scene
        controller.StartTrivia();
        sceneController.LoadTrivia();
    }
}

