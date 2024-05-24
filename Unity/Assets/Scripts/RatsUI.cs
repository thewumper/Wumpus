using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class RatsUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;

    private RatRoomStats stats;

    [SerializeField] private TMP_Text coins;
    private string coinsV
    {
        get => coins.text;
        set => coins.text = "Coins: " + value;
    }
    
    [SerializeField] private TMP_Text room;

    private float interval;
    
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject canvas;
    private List<GameObject> damageObjects = new();
    private float dmgSpeed = 2f;
    
    private void Awake()
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
        
        // Initializes the SceneController.
        sceneController = SceneController.GlobalSceneController;
    }

    private void Start()
    {
        stats = controller.GetRatRoomStats();
        coinsV = stats.StartingCoins.ToString();
        room.text = "Rats";

        interval = Time.time + 1;
    }

    private void Update()
    {
        coinsV = stats.RemainingCoins.ToString();
        if (interval <= Time.time)
        {
            interval = Time.time + 1;
            GameObject dmg = Instantiate(damageText, canvas.transform);
            dmg.GetComponent<TMP_Text>().text = stats.DamageDelt.ToString();
            damageObjects.Add(dmg);
        }

        foreach (GameObject obj in damageObjects)
        {
            Transform objTransform = obj.transform;
            Vector3 objPos = objTransform.position;
            if (objPos.y >= 30)
            {
                damageObjects.Remove(obj);
                Destroy(obj);
                continue;
            }
            Vector3 targetPos = new Vector3(
                objPos.x,
                objPos.y + dmgSpeed * Time.deltaTime, 
                objPos.z);
            objTransform.SetPositionAndRotation(targetPos, objTransform.rotation);
        }
    }
}
