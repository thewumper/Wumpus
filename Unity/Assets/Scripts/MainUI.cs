using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;

[RequireComponent(typeof(SoundManager))]
public class MainUI : MonoBehaviour
{
    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;
    
    /// <summary>
    /// Reference to the SoundManager.
    /// </summary>
    private SoundManager soundManager;

    /// <summary>
    /// The room that the player is currently in.
    /// </summary>
    private ushort RoomNum
    {
        get
        {
            return (ushort) controller.GetPlayerLocation();
        }
        set
        {
            northDoor.SetActive(false);
            northEastDoor.SetActive(false);
            southEastDoor.SetActive(false);
            southDoor.SetActive(false);
            southWestDoor.SetActive(false);
            northWestDoor.SetActive(false);
            
            mmNorth.SetActive(false);
            mmNorthEast.SetActive(false);
            mmSouthEast.SetActive(false);
            mmSouth.SetActive(false);
            mmSouthWest.SetActive(false);
            mmNorthWest.SetActive(false);
        }
    }
    
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
    
    /// <summary>
    /// The wumpus GameObject in the room.
    /// </summary>
    [SerializeField]
    private GameObject wumpus;
    
    /// <summary>
    /// The text that shows the amount of coins the player has.
    /// </summary>
    [SerializeField] 
    private TMP_Text coinsText;
    /// <summary>
    /// The text that shows what room the player is currently in.
    /// </summary>
    [SerializeField] 
    private TMP_Text roomText;

    /// <summary>
    /// The hint for what sounds are near you
    /// </summary>
    [SerializeField]
    private TMP_Text roomHintText;
    /// <summary>
    /// Rext that displays your current room type
    /// </summary>
    [SerializeField]
    private TMP_Text roomTypeText;

    [SerializeField]
    private AudioClip wumpusClip;
    private AudioClip luckyCatClip;

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
    private GameObject mmDirection;

    [SerializeField] private TMP_Text ArrowText;
    [SerializeField] private GameObject CrossBowNotFound;
    [SerializeField] private GameObject CrossBowFound;

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
        // Initializes the SoundManager
        soundManager = GetComponent<SoundManager>();
        soundManager.Init(northDoor, northEastDoor, southEastDoor, southDoor, southWestDoor, northWestDoor);
    }

    void Start()
    {
        mmDirection.transform.eulerAngles = new Vector3(
            mmDirection.transform.eulerAngles.x, 
            mmDirection.transform.eulerAngles.y, 
            PersistentData.Instance.EulerAngle.y);
        
        // Makes it so you can't normally see the wumpus.
        wumpus.SetActive(false);
        
        // Initializes the roomText text.
        roomText.SetText($"Room: {RoomNum}");
        
        // Initializes the RoomNum with the Player's location.
        RoomNum = (ushort) controller.GetPlayerLocation();
        
        // Adds the Door script to all doors.
        northDoor.AddComponent<Door>().Init(Directions.North);
        northEastDoor.AddComponent<Door>().Init(Directions.NorthEast);
        southEastDoor.AddComponent<Door>().Init(Directions.SouthEast);
        southDoor.AddComponent<Door>().Init(Directions.South);
        southWestDoor.AddComponent<Door>().Init(Directions.SouthWest);
        northWestDoor.AddComponent<Door>().Init(Directions.NorthWest);

        // Get the sounds properly working
        soundManager.UpdateSoundState();

        Debug.Log(controller.GetWumpusLocation());
    }

    void LateUpdate()
    {
        if (controller.DoesPlayerHaveGun())
        {
            CrossBowFound.SetActive(true);
            CrossBowNotFound.SetActive(false);
        }
        else
        {
            CrossBowFound.SetActive(false);
            CrossBowNotFound.SetActive(true);
        }

        ArrowText.SetText(controller.GetArrowCount().ToString());

        // Makes an IRoom which is the room that the player is currently in.
        IRoom room = controller.GetCurrentRoom();
        // Makes the roomText show which room the player is actually in.
        roomText.SetText($"Room: {RoomNum}");
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
        
        // All hazards in the player's current room.
        List<RoomAnomaly> hazards = controller.GetAnomaliesInRoom(RoomNum);
        // For each hazard in the room.
        foreach (RoomAnomaly hazard in hazards)
        {
            // Make the hazard visible.
            switch (hazard)
            {
                case RoomAnomaly.Wumpus:
                    wumpus.SetActive(true);
                    break;
            }
        }
        List<Controller.DirectionalHint> hints = controller.GetHazardHints();
        List<string> hintString = new List<string>();
        foreach (Controller.DirectionalHint hint in hints)
        {
            foreach (RoomAnomaly anomaly in hint.Hazards)
            {
                hintString.Add("You hear " + anomaly);
            }
        }
        if (!(hintString.Count <= 0)) roomHintText.SetText(string.Join('\n', hintString));
        else roomHintText.SetText("You hear nothing.");
        roomTypeText.SetText(controller.GetCurrentRoomType().ToString());
    }

    private void Update()
    {
        mmDirection.transform.eulerAngles = new Vector3(
            mmDirection.transform.eulerAngles.x, 
            mmDirection.transform.eulerAngles.y, 
                180 - PersistentData.Instance.EulerAngle.y);
        
        // Makes the coinsText show the actual amount of coins that the player currently has.
        coinsText.SetText(controller.GetCoins().ToString());
    }
}