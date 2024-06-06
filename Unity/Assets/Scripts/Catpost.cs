using System;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.LuckyCat;
using WumpusUnity;

public class Catpost : MonoBehaviour
{
    private Controller controller;


    [SerializeField] private GameObject catModel;
    [SerializeField] private Camera cam;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private GameObject uiHandler;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiates the Controller, if there isn't one already.
        try
        {
            controller = Controller.GlobalController;
        }
        catch (NullReferenceException)
        {
            controller = new Controller
                (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentData.Instance.IsCatMadAtPlayer)
        {
            catModel.transform.eulerAngles = new Vector3(0,-170,0);
        }
        // If we're not in the cat room it's really easy
        if (!controller.GetAnomaliesInRoom(controller.GetPlayerLocation()).Contains(RoomAnomaly.Cat))
        {
            gameObject.SetActive(controller.hasPlayerTamedCat());
        }


        if (Physics.Raycast(new Ray(), out RaycastHit hit)){
            if (hit.transform.CompareTag("CatPost"))
            {
                hintText.SetText("Click to tame the cat");
                if (Input.GetMouseButtonDown(0))
                {
                    uiHandler.SetActive(true);
                }
            }
        }
    }
}
