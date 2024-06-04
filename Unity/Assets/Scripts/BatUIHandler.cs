using UnityEngine;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusUnity;

public class BatUIHandler : MonoBehaviour
{
    /// <summary>
    /// The game object for the camera
    /// </summary>
    [SerializeField]
    private GameObject cam;

    /// <summary>
    /// The Animator that handles the player moving.
    /// </summary>
    [SerializeField]
    private Animator movingAnimator;

    /// <summary>
    /// The image used to fade in and out.
    /// </summary>
    [SerializeField]
    private Image black;

    /// <summary>
    /// The ID of the moving variable in <see cref="movingAnimator"/>.
    /// </summary>
    private int fadingID;

    /// <summary>
    /// Reference to the global Controller.
    /// </summary>
    private Controller controller;

    [SerializeField] private int numBats;

    /// <summary>
    /// The speed at which the camera rotates with the player's mouse.
    /// </summary>
    private const float camSens = 5f;

    // Update is called once per frame
    void Update()
    {
        // Look around using the mouse.
        float mouseX = Input.GetAxis("Mouse X");

        cam.transform.eulerAngles += new Vector3(0, mouseX * camSens, 0);
        // PersistentData.Instance.EulerAngle = cam.transform.eulerAngles;

        movingAnimator.SetBool(fadingID, true);
    }
}
