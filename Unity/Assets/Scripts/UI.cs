using System;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;

public class UI : MonoBehaviour
{
    private Controller controller;

    [SerializeField] 
    private GameObject cam;

    private const float camSens = 5f;

    private ushort roomNum;
    private ushort RoomNum
    {
        get
        {
            return roomNum;
        }
        set
        {
            northDoor.SetActive(false);
            northEastDoor.SetActive(false);
            southEastDoor.SetActive(false);
            southDoor.SetActive(false);
            southWestDoor.SetActive(false);
            northWestDoor.SetActive(false);

            roomNum = value;
        }
    }
    
    // doors
    [SerializeField]
    private GameObject northDoor;
    [SerializeField]
    private GameObject northEastDoor;
    [SerializeField]
    private GameObject southEastDoor;
    [SerializeField]
    private GameObject southDoor;
    [SerializeField]
    private GameObject southWestDoor;
    [SerializeField]
    private GameObject northWestDoor;

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
        RoomNum = 1;
    }

    void FixedUpdate()
    {
        IRoom room = controller.GetRoom(RoomNum);
        foreach (Directions dir in room.ExitDirections)
        {
            string name = DirectionHelper.GetShortNameFromDirection(dir);
            switch (name)
            {
                case "N":
                    northDoor.SetActive(true);
                    break;
                case "NE":
                    northEastDoor.SetActive(true);
                    break;
                case "SE":
                    southEastDoor.SetActive(true);
                    break;
                case "S":
                    southDoor.SetActive(true);
                    break;
                case "SW":
                    southWestDoor.SetActive(true);
                    break;
                case "NW":
                    northWestDoor.SetActive(true);
                    break;
            }
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);

        if (Input.GetMouseButtonDown(0))
        {
            RoomNum = (ushort)((RoomNum + 1) % 30);
        }
    }
}
