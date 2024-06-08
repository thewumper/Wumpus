using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;
using WumpusUnity;
using Random = System.Random;

[RequireComponent(typeof(SoundManager))]
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
    /// Reference to the SoundManager.
    /// </summary>
    private SoundManager soundManager;

    [SerializeField] private GameObject cam;

    /// <summary>
    /// The stats of what is happening in the rat room.
    /// </summary>
    private RatRoomStats stats;

    /// <summary>
    /// The TMP text that contains how many coins you have left.
    /// </summary>
    [SerializeField] private TMP_Text coins;
    
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
    private const float dmgSpeed = 20f;

    [SerializeField] private GameObject rat;
    private List<GameObject> rats = new();
    private float ratSpeed = 5f;

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
    private GameObject mmDirection;

    [SerializeField]
    ShaderApplication camShaders;

    Random rand = new();

    private IEnumerator oneSec() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            GameObject dmg = Instantiate(damageText, canvas.transform);
            dmg.GetComponent<TMP_Text>().text = $"-{stats.DamageDelt}";
            damageObjects.Add(dmg);
        }
    }

    private IEnumerator tenthSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);

            camShaders.PosterzationBands1 -= 1;
            camShaders.PosterzationBands1 = Math.Clamp(camShaders.PosterzationBands1, 10, 75);

            camShaders.OverallDistortionFreq += .2f;
            camShaders.OverallDistortionMag += .2f;
            camShaders.OverallDistortionSpeed += .1f;
        }
    }

    private IEnumerator twentiethSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(.05f);

            GameObject aRat = Instantiate(rat);
            int randDir = rand.Next(0, 4);
            if (randDir == 0)
            {
                aRat.transform.SetPositionAndRotation(new Vector3(rand.Next(-15, 16), .2f, 15), aRat.transform.rotation);
            }
            else if (randDir == 1)
            {
                aRat.transform.SetPositionAndRotation(new Vector3(rand.Next(-15, 16), .2f, -15), aRat.transform.rotation);
            }
            else if (randDir == 2)
            {
                aRat.transform.SetPositionAndRotation(new Vector3(15, .2f, rand.Next(-15, 16)), aRat.transform.rotation);
            }
            else if (randDir == 3)
            {
                aRat.transform.SetPositionAndRotation(new Vector3(-15, .2f, rand.Next(-15, 16)), aRat.transform.rotation);
            }
            aRat.transform.LookAt(cam.transform);
            aRat.transform.eulerAngles = new Vector3(
                -90,
                aRat.transform.eulerAngles.y,
                aRat.transform.eulerAngles.z);
            rats.Add(aRat);
            ratSpeed += .5f;
        }
    }

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
        
        // Initializes the SoundManager
        soundManager = GetComponent<SoundManager>();
        soundManager.Init(northDoor, northEastDoor, southEastDoor, southDoor, southWestDoor, northWestDoor);
    }

    private void Start()
    {
        mmDirection.transform.eulerAngles = new Vector3(
            mmDirection.transform.eulerAngles.x,
            mmDirection.transform.eulerAngles.y,
            PersistentData.Instance.EulerAngle.y);

        stats = controller.GetRatRoomStats();
        coinsV = stats.StartingCoins.ToString();
        roomTypeText.text = "Rats";
        camShaders = Camera.main.GetComponent<ShaderApplication>();
        camShaders.PosterzationBands1 = 75;
        camShaders.MaxTime = 1;

        camShaders.OverallDistortionFreq = 1;

        // Adds the Door script to all doors.
        northDoor.AddComponent<Door>().Init(Directions.North);
        northEastDoor.AddComponent<Door>().Init(Directions.NorthEast);
        southEastDoor.AddComponent<Door>().Init(Directions.SouthEast);
        southDoor.AddComponent<Door>().Init(Directions.South);
        southWestDoor.AddComponent<Door>().Init(Directions.SouthWest);
        northWestDoor.AddComponent<Door>().Init(Directions.NorthWest);
        
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
        room.SetText("Room: " + controller.GetPlayerLocation().ToString());
        
        // Get the sounds properly working
        soundManager.UpdateSoundState();

        StartCoroutine(oneSec());
        StartCoroutine(tenthSecond());
        StartCoroutine(twentiethSecond());
    }

    private void Update()
    {
        stats = controller.GetRatRoomStats();
        coinsV = stats.RemainingCoins.ToString();

        mmDirection.transform.eulerAngles = new Vector3(
            mmDirection.transform.eulerAngles.x,
            mmDirection.transform.eulerAngles.y,
                180 - PersistentData.Instance.EulerAngle.y);

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

        for (int i = 0; i < rats.Count; i++)
        {
            rats[i].transform.position += -rats[i].transform.up * ratSpeed * Time.deltaTime;
        }

        if (stats.RemainingCoins <= 0)
        {
            sceneController.GotoCorrectScene();
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
