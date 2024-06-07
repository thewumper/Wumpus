using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
    [SerializeField] private GameObject catIcon;
    [SerializeField] private AudioClip meowClip;
    [FormerlySerializedAs("UIInteractIcon")] [SerializeField] private GameObject uiInteractIcon;
    [SerializeField] private Sprite catInteractIcon;
    
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

            catIcon.SetActive(controller.hasPlayerTamedCat());
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

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit)){
            if (hit.transform.CompareTag("CatPost"))
            {
                uiInteractIcon.GetComponent<Image>().sprite = catInteractIcon;
                uiInteractIcon.SetActive(true);
                if (controller.hasPlayerTamedCat())
                {
                    hintText.SetText("Click to pet the cat");
                    if (Input.GetMouseButtonDown(0))
                    {
                        AudioSource soundSource = catModel.AddComponent<AudioSource>();
                        soundSource.clip = meowClip;
                        soundSource.loop = false;
                        soundSource.Play();
                    }
                }
                else
                {
                    hintText.SetText("Click to tame the cat");
                    if (Input.GetMouseButtonDown(0))
                    {
                        uiHandler.SetActive(true);
                    }
                }
            }
        }
        else
        {
            uiInteractIcon.SetActive(false);
        }
    }
}
