using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AcrobatView : MonoBehaviour
{
    [SerializeField] private float speed;

    private float rotY = 0;

    private float rotX = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Look around using the mouse.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * speed * -1;
        rotY += mouseX * speed;

        rotX = Mathf.Clamp(rotX, -90f,90f);
        
        transform.eulerAngles = new Vector3(rotX, rotY, 0);
    }
}
