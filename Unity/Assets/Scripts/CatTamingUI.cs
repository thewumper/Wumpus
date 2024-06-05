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

    private Controller controller;
    private int coinsSubmit;


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

        mainCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        coinText.SetText($"You have {controller.GetCoins()} coins");

        upArrow.onClick.AddListener(UpArrowClick);
        downArrow.onClick.AddListener(DownArrowClick);
        submitButton.onClick.AddListener(SubmitButtonClick);
    }

    private void OnDisable()
    {
        mainCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void SubmitButtonClick()
    {
        bool success = controller.AttemptToTameCat(coinsSubmit);
        if (!success)
        {
            PersistentData.Instance.IsCatMadAtPlayer = true;
        }

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
