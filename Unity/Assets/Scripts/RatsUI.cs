using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

public class RatsUI : MonoBehaviour
{
    /// <summary>
    /// The global Controller object.
    /// </summary>
    private Controller controller;
    /// <summary>
    /// The global SceneController object.
    /// </summary>
    private SceneController sceneController;

    /// <summary>
    /// The stats of what is happening in the rat room.
    /// </summary>
    private RatRoomStats stats;

    /// <summary>
    /// The TMP text that contains how many coins you have left.
    /// </summary>
    [SerializeField] private TMP_Text coins;
    /// <summary>
    /// Returns the actual text of <see cref="coins"/>.
    /// </summary>
    private string coinsV
    {
        get => coins.text;
        set => coins.text = "Coins: " + value;
    }
    
    /// <summary>
    /// What the RoomType text shows.
    /// </summary>
    [SerializeField] private TMP_Text room;

    /// <summary>
    /// The interval at which the coin damage should appear at.
    /// </summary>
    private float interval;
    
    /// <summary>
    /// The prefab for the coin damage text.
    /// </summary>
    [SerializeField] private GameObject damageText;
    /// <summary>
    /// The canvas to add the coin damage text to.
    /// </summary>
    [SerializeField] private GameObject canvas;
    /// <summary>
    /// Contains all of the GameObjects that are coin damage text.
    /// </summary>
    private List<GameObject> damageObjects = new();
    /// <summary>
    /// The speed at which the coin damage moves upwards.
    /// </summary>
    private float dmgSpeed = 20f;

    /// <summary>
    /// Reference to the door that is north of the player.
    /// </summary>
    [SerializeField]
    private GameObject northDoor;
    /// <summary>
    /// Reference to the door that is northeast of the player.
    /// </summary>
    [SerializeField]
    private GameObject northEastDoor;
    /// <summary>
    /// Reference to the door that is southeast of the player.
    /// </summary>
    [SerializeField]
    private GameObject southEastDoor;
    /// <summary>
    /// Reference to the door that is south of the player.
    /// </summary>
    [SerializeField]
    private GameObject southDoor;
    /// <summary>
    /// Reference to the door that is southwest of the player.
    /// </summary>
    [SerializeField]
    private GameObject southWestDoor;
    /// <summary>
    /// Reference to the door that is northwest of the player.
    /// </summary>
    [SerializeField]
    private GameObject northWestDoor;

    [SerializeField]
    private GameObject mmNorth;
    [SerializeField]
    private GameObject mmNorthEast;
    [SerializeField]
    private GameObject mmSouthEast;
    [SerializeField]
    private GameObject mmSouth;
    [SerializeField]
    private GameObject mmSouthWest;
    [SerializeField]
    private GameObject mmNorthWest;

    [SerializeField]
    ShaderApplication camShaders;

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
        camShaders = Camera.main.GetComponent<ShaderApplication>();
        camShaders.PosterzationBands1 = 100;

        interval = Time.time;
    }

    private void Update()
    {
        stats = controller.GetRatRoomStats();
        coinsV = stats.RemainingCoins.ToString();
        if (interval + 1 <= Time.time)
        {
            interval = Time.time;
            GameObject dmg = Instantiate(damageText, canvas.transform);
            dmg.GetComponent<TMP_Text>().text = stats.DamageDelt.ToString();
            damageObjects.Add(dmg);
        }
        
        for (int i = 0; i < damageObjects.Count; i++)
        {
            GameObject obj = damageObjects[i];
            RectTransform objTransform = obj.GetComponent<RectTransform>();
            if (objTransform.anchoredPosition.y >= 30)
            {
                damageObjects.Remove(obj);
                Destroy(obj);
                continue;
            }
            Vector3 targetPos = new Vector3(
                objTransform.position.x,
                objTransform.position.y + dmgSpeed * Time.deltaTime, 
                objTransform.position.z);
            objTransform.SetPositionAndRotation(targetPos, objTransform.rotation);
        }
    }

    private void LateUpdate()
    {
        // Makes an IRoom which is the room that the player is currently in.
        IRoom room = controller.GetCurrentRoom();
        // Makes the roomText show which room the player is actually in.
        //roomText.SetText($"Room: {RoomNum}");
        // Makes only the doors that are in the room visible.
        foreach (Directions dir in room.ExitDirections)
        {
            string name = DirectionHelper.GetShortNameFromDirection(dir);
            switch (name)
            {
                case "N":
                    northDoor.SetActive(true);
                    mmNorth.SetActive(true);
                    break;
                case "NE":
                    northEastDoor.SetActive(true);
                    mmNorthEast.SetActive(true);
                    break;
                case "SE":
                    southEastDoor.SetActive(true);
                    mmSouthEast.SetActive(true);
                    break;
                case "S":
                    southDoor.SetActive(true);
                    mmSouth.SetActive(true);
                    break;
                case "SW":
                    southWestDoor.SetActive(true);
                    mmSouthWest.SetActive(true);
                    break;
                case "NW":
                    northWestDoor.SetActive(true);
                    mmNorthWest.SetActive(true);
                    break;
            }
        }
    }
}
