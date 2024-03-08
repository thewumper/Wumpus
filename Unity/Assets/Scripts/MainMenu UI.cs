using UnityEngine;
using WumpusUnity;

public class MainMenuUI : MonoBehaviour
{
    private SceneController sceneController;

    void Start()
    {
        sceneController = SceneController.GlobalSceneController;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sceneController.GotoCorrectScene();
        }
    }
}
