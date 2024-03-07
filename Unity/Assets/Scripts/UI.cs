using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Topology;

public class UI : MonoBehaviour
{
    private Controller controller;

    [SerializeField] 
    private GameObject cam;

    private const float camSens = 5f;

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IRoom room = controller.GetRoom(0);
        for (int i = 0; i < room.ExitRooms.Count; i++)
        {
            //room.ExitRooms[];
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
    }
}
