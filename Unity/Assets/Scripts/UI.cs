using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;

public class UI : MonoBehaviour
{
    private Controller controller;

    [SerializeField] 
    private GameObject cam;

    private const float camSens = 5f;
    
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IRoom room = controller.GetRoom(0);
        foreach (Directions dir in room.ExitDirections)
        {
            string name = DirectionHelper.GetShortNameFromDirection(dir);
            Debug.Log(name);
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
    }
}
