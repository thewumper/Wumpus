using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class CrossBowOnScreen : MonoBehaviour
{
    [SerializeField] private GameObject crossbow;
    [SerializeField] private GameObject arrow;

    private Controller controller;

    private void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.DoesPlayerHaveGun())
        {
            crossbow.SetActive(true);
            if (controller.GetArrowCount() > 0)
            {
                arrow.SetActive(true);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
        else
        {
            crossbow.SetActive(false);
            arrow.SetActive(false);
        }
    }
}
