using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WumpusCutscene : MonoBehaviour
{
    [SerializeField] GameObject tv;
    [SerializeField] private string battleScene;
    [SerializeField] private Transform targetPosition;
    bool isDropping;
    private bool isStarted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropping)
        {
            tv.transform.position = Vector3.Lerp(tv.transform.position,targetPosition.position,0.1f);
            if (Vector3.Distance(tv.transform.position, targetPosition.position) <= 0.01)
            {
                isStarted = true;
                SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);
            }
        }
    }

    public void No()
    {
        
    }

    public void Yes()
    {
        
    }
}
