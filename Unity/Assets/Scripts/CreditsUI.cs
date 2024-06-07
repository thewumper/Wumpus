using TMPro;
using UnityEngine;
using WumpusUnity;

public class CreditsUI : MonoBehaviour
{
    SceneController sceneController;

    [SerializeField] private TMP_Text credits;
    private const float creditsSpeed = 5f;

    private void Awake()
    {
        sceneController = SceneController.GlobalSceneController;
    }

    // Update is called once per frame
    void Update()
    {
        if (credits.transform.position.y >= 360)
        {
            sceneController.GotoCorrectScene();
        }
        credits.transform.position += new Vector3(0, creditsSpeed * Time.deltaTime, 0);
    }
}
