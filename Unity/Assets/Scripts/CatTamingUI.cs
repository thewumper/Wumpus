using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusUnity;

public class CatTamingUI : MonoBehaviour
{
    [SerializeField] private Button upArrow;
    [SerializeField] private Button downArrow;
    [FormerlySerializedAs("submit")] [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text selectorCoinsText;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject catIcon;

    private Controller controller;
    private int coinsSubmit;

    void OnEnable()
    {
        Debug.Log("Initiated taming UI");
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

        mainCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        coinsSubmit = 0;

        coinText.SetText($"You have {controller.GetCoins()} coins");

        upArrow.onClick.AddListener(UpArrowClick);
        downArrow.onClick.AddListener(DownArrowClick);
        submitButton.onClick.AddListener(SubmitButtonClick);
        mainUI.GetComponent<MainUI>().pLock = true;
        selectorCoinsText.SetText(coinsSubmit.ToString());
    }

    private void OnDisable()
    {
        mainCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        if (controller.GetState() == ControllerState.AmmoRoom || controller.GetState() == ControllerState.GunRoom)
        {
            mainUI.GetComponent<StoresUI>().pLock = false;
        }
        else
        {
            mainUI.GetComponent<MainUI>().pLock = false;
        }

    }

    private void SubmitButtonClick()
    {
        bool success = controller.AttemptToTameCat(coinsSubmit);
        if (!success)
        {
            PersistentData.Instance.IsCatMadAtPlayer = true;
        }
        catIcon.SetActive(controller.hasPlayerTamedCat());

        // Disable the taming UI again
        gameObject.SetActive(false);
    }

    private void UpArrowClick()
    {
        if (coinsSubmit < controller.GetCoins())
        {
            coinsSubmit++;
        }
        selectorCoinsText.SetText(coinsSubmit.ToString());
    }

    private void DownArrowClick()
    {
        if (coinsSubmit > 0)
        {
            coinsSubmit--;
        }
        selectorCoinsText.SetText(coinsSubmit.ToString());
    }
}
